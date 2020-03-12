using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace TimeZoneDB_JSON
{
    public static class Worker
    {

        public const string DEVELOPER = "nazzinoz";
        public const string APP_NAME = "TimeZoneDB_JSON";
        public const string VERSION_API = "https://timezonedb.com/date.txt";
        public const string DOWNLOAD_API = "https://timezonedb.com/files/timezonedb.csv.zip";

        public const string MESSAGE_NO_DB = "No databases downloaded.";
        public const string MESSAGE_LOCAL_VERSION = "Local version: ";
        public const string MESSAGE_CURRENT_VERSION = "Current version: ";
        public const string MESSAGE_UP_TO_DATE = "Up to date.";
        public const string MESSAGE_REQUIRE_REDOWNLOADING = "Require redownloading.";
        public const string MESSAGE_DOWNLOADED = "Downloaded.";
        public const string MESSAGE_EXTRACTED = "Extracted.";
        public const string MESSAGE_GENERATED = "Generated.";
        public const string MESSAGE_DELETED = "Deleted.";
        public const string MESSAGE_FAILED = "Failed.";

        public const string TITLE_CHECKING = " - Checking...";
        public const string TITLE_DOWNLOADING = " - Downloading...";
        public const string TITLE_EXTRACTING = " - Extracting...";
        public const string TITLE_GENERATING = " - Generating...";
        public const string TITLE_OPENING = " - Opening...";
        public const string TITLE_DELETING = " - Deleting...";

        public const string KEY_COUNTRY_CODE = "countryCode";
        public const string KEY_NAME = "name";
        public const string KEY_ABBREVIATION = "abbreviation";
        public const string KEY_UNIX_TIME_STAMP = "unixTimestamp";
        public const string KEY_OFFSET = "offset";
        public const string KEY_DST = "dst";

        public const string KEY_ZONE = "zone";
        public const string KEY_COUNTRY = "country";
        public const string KEY_TIMEZONE = "timezone";

        public const string KEY_CHANGE = "change";

        public static readonly string EXECUTE_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DEVELOPER, APP_NAME);

        public static readonly string DOWNLOAD_PATH = Path.Combine(EXECUTE_PATH, "download");
        public static readonly string EXTRACT_PATH = Path.Combine(EXECUTE_PATH, "extract");
        public static readonly string RESULT_PATH = Path.Combine(EXECUTE_PATH, "result");

        public static readonly string DOWNLOAD_VERSION_FILE = Path.Combine(DOWNLOAD_PATH, "version.txt");
        public static readonly string DOWNLOAD_TIMEZONE_FILE = Path.Combine(DOWNLOAD_PATH, "timezonedb.csv.zip");

        public static readonly string EXTRACTED_ZONE = Path.Combine(EXTRACT_PATH, KEY_ZONE + ".csv");
        public static readonly string EXTRACTED_COUNTRY = Path.Combine(EXTRACT_PATH, KEY_COUNTRY + ".csv");
        public static readonly string EXTRACTED_TIMEZONE = Path.Combine(EXTRACT_PATH, KEY_TIMEZONE + ".csv");

        public static readonly string RESULT_ZONE = Path.Combine(RESULT_PATH, KEY_ZONE + ".json");
        public static readonly string RESULT_COUNTRY = Path.Combine(RESULT_PATH, KEY_COUNTRY + ".json");
        public static readonly string RESULT_TIMEZONE = Path.Combine(RESULT_PATH, KEY_TIMEZONE + ".json");

        public static readonly string RESULT_FILE = Path.Combine(RESULT_PATH, "timezoneList.json");

        public static readonly WebClient WEB_CLIENT = new WebClient();
        public static MainWindow MAIN_WINDOW;

        public static Tuple<string, string> CheckUpdate()
        {
            if (!File.Exists(DOWNLOAD_VERSION_FILE))
                return null;

            var localVersion = File.ReadAllText(DOWNLOAD_VERSION_FILE);

            WEB_CLIENT.Headers.Add("User-Agent", DEVELOPER);
            var currentVersion = WEB_CLIENT.DownloadString(VERSION_API);

            return new Tuple<string, string>(localVersion, currentVersion);
        }

        public static string Download()
        {
            if (!Directory.Exists(DOWNLOAD_PATH))
                Directory.CreateDirectory(DOWNLOAD_PATH);

            WEB_CLIENT.Headers.Add("User-Agent", DEVELOPER);
            var currentVersion = WEB_CLIENT.DownloadString(VERSION_API);

            WEB_CLIENT.Headers.Add("User-Agent", DEVELOPER);
            WEB_CLIENT.DownloadFile(DOWNLOAD_API, DOWNLOAD_TIMEZONE_FILE);

            File.WriteAllText(DOWNLOAD_VERSION_FILE, currentVersion);

            return currentVersion;
        }

        public static void Extract()
        {
            ZipFile.ExtractToDirectory(DOWNLOAD_TIMEZONE_FILE, EXTRACT_PATH, true);
        }

        public static void Generate()
        {
            var zoneParser = new TextFieldParser(EXTRACTED_ZONE);
            zoneParser.SetDelimiters(",");

            var zoneParsed = new Dictionary<string, Dictionary<string, dynamic>>();
            while (!zoneParser.EndOfData)
            {
                var lineData = zoneParser.ReadFields();

                var timezoneId = int.Parse(lineData[0]);
                var countryCode = lineData[1];
                var name = lineData[2];

                zoneParsed[timezoneId.ToString()] = new Dictionary<string, dynamic> { { KEY_COUNTRY_CODE, countryCode }, { KEY_NAME, name } };
            }
            zoneParser.Close();

            var countryParser = new TextFieldParser(EXTRACTED_COUNTRY);
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

            var timezoneParser = new TextFieldParser(EXTRACTED_TIMEZONE);
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

                timezoneParsed[timezoneId.ToString()].Add(new Dictionary<string, dynamic> { { KEY_ABBREVIATION, abbreviation }, { KEY_UNIX_TIME_STAMP, unixTimestamp }, { KEY_OFFSET, offset }, { KEY_DST, isDaylightSavingTime } });
            }
            timezoneParser.Close();

            var filledData = new Dictionary<string, Dictionary<string, dynamic>>(zoneParsed);
            timezoneParsed.Keys.ToList().ForEach(key => filledData[key][KEY_CHANGE] = timezoneParsed[key]);

            if (!Directory.Exists(RESULT_PATH))
                Directory.CreateDirectory(RESULT_PATH);

            File.WriteAllText(RESULT_ZONE, JsonSerializer.Serialize(zoneParsed));
            File.WriteAllText(RESULT_COUNTRY, JsonSerializer.Serialize(countryParsed));
            File.WriteAllText(RESULT_TIMEZONE, JsonSerializer.Serialize(timezoneParsed));
            File.WriteAllText(RESULT_FILE, JsonSerializer.Serialize(filledData));
        }

        public static void OpenResult()
        {
            Process.Start("explorer.exe", Worker.RESULT_PATH);
        }

        public static void ClearCache()
        {
            Directory.Delete(Worker.EXECUTE_PATH, true);
        }
    }
}
