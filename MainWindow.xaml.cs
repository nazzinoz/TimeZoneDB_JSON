using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Web;

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
            if (!File.Exists(StaticValues.VERSION_FILE))
            {
                MessageBox.Show(StaticValues.MESSAGE_NO_DB);
                return;
            }

            var localVersion = File.ReadAllText(StaticValues.VERSION_FILE);

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
            if (!Directory.Exists(StaticValues.EXECUTE_PATH))
                Directory.CreateDirectory(StaticValues.EXECUTE_PATH);

            StaticValues.WEB_CLIENT.Headers.Add("User-Agent", StaticValues.DEVELOPER);
            var currentVersion = StaticValues.WEB_CLIENT.DownloadString(StaticValues.VERSION_API);

            StaticValues.WEB_CLIENT.Headers.Add("User-Agent", StaticValues.DEVELOPER);
            StaticValues.WEB_CLIENT.DownloadFile(StaticValues.DOWNLOAD_API, StaticValues.DOWNLOAD_FILE);

            File.WriteAllText(StaticValues.VERSION_FILE, currentVersion);

            var message = StaticValues.MESSAGE_CURRENT_VERSION + currentVersion
                + Environment.NewLine + StaticValues.MESSAGE_DOWNLOADED;

            MessageBox.Show(message);
        }

        private void Button_Extract_Click(object sender, RoutedEventArgs e)
        {
            ZipFile.ExtractToDirectory(StaticValues.DOWNLOAD_FILE, StaticValues.EXECUTE_PATH, true);

            MessageBox.Show(StaticValues.MESSAGE_EXTRACTED);
        }

        private void Button_Generate_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
