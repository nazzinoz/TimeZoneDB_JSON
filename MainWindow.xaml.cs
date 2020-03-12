using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Windows;

namespace TimeZoneDB_JSON
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (StaticValues.MAIN_WINDOW == null)
                StaticValues.MAIN_WINDOW = this;
        }

        private void Button_CheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(StaticValues.DOWNLOAD_VERSION_FILE))
            {
                MessageBox.Show(StaticValues.MESSAGE_NO_DB);
                return;
            }

            var localVersion = File.ReadAllText(StaticValues.DOWNLOAD_VERSION_FILE);

            StaticValues.WEB_CLIENT.Headers.Add("User-Agent", StaticValues.DEVELOPER);
            var currentVersion = StaticValues.WEB_CLIENT.DownloadString(StaticValues.VERSION_API);

            var message = StaticValues.MESSAGE_LOCAL_VERSION + localVersion
                + Environment.NewLine + StaticValues.MESSAGE_CURRENT_VERSION + currentVersion
                + Environment.NewLine +
            (currentVersion.CompareTo(localVersion) > 0 ? StaticValues.MESSAGE_REQUIRE_REDOWNLOADING : StaticValues.MESSAGE_UP_TO_DATE);

            MessageBox.Show(message);
        }

        private void Button_Download_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(StaticValues.DOWNLOAD_PATH))
                Directory.CreateDirectory(StaticValues.DOWNLOAD_PATH);

            StaticValues.WEB_CLIENT.Headers.Add("User-Agent", StaticValues.DEVELOPER);
            var currentVersion = StaticValues.WEB_CLIENT.DownloadString(StaticValues.VERSION_API);

            StaticValues.WEB_CLIENT.Headers.Add("User-Agent", StaticValues.DEVELOPER);
            StaticValues.WEB_CLIENT.DownloadFile(StaticValues.DOWNLOAD_API, StaticValues.DOWNLOAD_TIMEZONE_FILE);

            File.WriteAllText(StaticValues.DOWNLOAD_VERSION_FILE, currentVersion);

            var message = StaticValues.MESSAGE_CURRENT_VERSION + currentVersion
                + Environment.NewLine + StaticValues.MESSAGE_DOWNLOADED;

            MessageBox.Show(message);
        }

        private void Button_Extract_Click(object sender, RoutedEventArgs e)
        {
            ZipFile.ExtractToDirectory(StaticValues.DOWNLOAD_TIMEZONE_FILE, StaticValues.EXTRACT_PATH, true);

            MessageBox.Show(StaticValues.MESSAGE_EXTRACTED);
        }

        private void Button_Generate_Click(object sender, RoutedEventArgs e)
        {
            var zoneParser = new TextFieldParser(StaticValues.EXTRACTED_ZONE);
            zoneParser.SetDelimiters(",");

            var zoneParsed = new Dictionary<string, Dictionary<string, dynamic>>();
            while (!zoneParser.EndOfData)
            {
                var lineData = zoneParser.ReadFields();

                var timezoneId = int.Parse(lineData[0]);
                var countryCode = lineData[1];
                var name = lineData[2];

                zoneParsed[timezoneId.ToString()] = new Dictionary<string, dynamic> { { StaticValues.KEY_COUNTRY_CODE, countryCode }, { StaticValues.KEY_NAME, name } };
            }

            var countryParser = new TextFieldParser(StaticValues.EXTRACTED_COUNTRY);
            countryParser.SetDelimiters(",");

            var countryParsed = new Dictionary<string, string>();
            while (!countryParser.EndOfData)
            {
                var lineData = countryParser.ReadFields();

                var countryCode = lineData[0];
                var countryName = lineData[1];

                countryParsed[countryCode] = countryName;
            }
            countryParser.Close();

            var timezoneParser = new TextFieldParser(StaticValues.EXTRACTED_TIMEZONE);
            timezoneParser.SetDelimiters(",");

            var timezoneParsed = new Dictionary<string, List<Dictionary<string, dynamic>>>();
            while (!timezoneParser.EndOfData)
            {
                var lineData = timezoneParser.ReadFields();

                var timezoneId = int.Parse(lineData[0]);
                var abbreviation = lineData[1];
                var unixTimestamp = long.Parse(lineData[2]);
                var offset = int.Parse(lineData[3]);
                var isDaylightSavingTime = int.Parse(lineData[4]) == 1;

                if (!timezoneParsed.ContainsKey(timezoneId.ToString()))
                    timezoneParsed[timezoneId.ToString()] = new List<Dictionary<string, dynamic>>();

                timezoneParsed[timezoneId.ToString()].Add(new Dictionary<string, dynamic> { { StaticValues.KEY_ABBREVIATION, abbreviation }, { StaticValues.KEY_UNIX_TIME_STAMP, unixTimestamp }, { StaticValues.KEY_OFFSET, offset }, { StaticValues.KEY_DST, isDaylightSavingTime } });
            }
            timezoneParser.Close();

            var filledData = new Dictionary<string, Dictionary<string, dynamic>>(zoneParsed);
            timezoneParsed.Keys.ToList().ForEach(key => filledData[key][StaticValues.KEY_CHANGE] = timezoneParsed[key]);

            if (!Directory.Exists(StaticValues.RESULT_PATH))
                Directory.CreateDirectory(StaticValues.RESULT_PATH);

            File.WriteAllText(StaticValues.RESULT_ZONE, JsonSerializer.Serialize(zoneParsed));
            File.WriteAllText(StaticValues.RESULT_COUNTRY, JsonSerializer.Serialize(countryParsed));
            File.WriteAllText(StaticValues.RESULT_TIMEZONE, JsonSerializer.Serialize(timezoneParsed));
            File.WriteAllText(StaticValues.RESULT_FILE, JsonSerializer.Serialize(filledData));

            MessageBox.Show(StaticValues.MESSAGE_GENERATED);
        }

        private void Button_Open_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", StaticValues.RESULT_PATH);
        }
    }
}
