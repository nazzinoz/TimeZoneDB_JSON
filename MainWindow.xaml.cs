using System;
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
            if (Worker.MAIN_WINDOW == null)
                Worker.MAIN_WINDOW = this;
        }

        private void Button_CheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            var currentTitle = this.Title;
            this.IsEnabled = false;
            this.Title = currentTitle + Worker.TITLE_CHECKING;
            this.InvalidateVisual();
            try
            {
                var updateInfo = Worker.CheckUpdate();
                if (updateInfo == null)
                {
                    MessageBox.Show(Worker.MESSAGE_NO_DB);
                    return;
                }

                var localVersion = updateInfo.Item1;
                var currentVersion = updateInfo.Item2;

                var message = Worker.MESSAGE_LOCAL_VERSION + localVersion + Environment.NewLine
                              + Worker.MESSAGE_CURRENT_VERSION + currentVersion + Environment.NewLine
                              + (currentVersion.CompareTo(localVersion) > 0 ? Worker.MESSAGE_REQUIRE_REDOWNLOADING : Worker.MESSAGE_UP_TO_DATE);

                MessageBox.Show(message);
            }
            catch
            {
                MessageBox.Show(Worker.MESSAGE_FAILED);
            }
            this.IsEnabled = true;
            this.Title = currentTitle;
        }

        private void Button_Download_Click(object sender, RoutedEventArgs e)
        {
            var currentTitle = this.Title;
            this.IsEnabled = false;
            this.Title = currentTitle + Worker.TITLE_DOWNLOADING;
            try
            {
                var message = Worker.MESSAGE_CURRENT_VERSION + Worker.Download() + Environment.NewLine
                              + Worker.MESSAGE_DOWNLOADED;

                MessageBox.Show(message);
            }
            catch
            {
                MessageBox.Show(Worker.MESSAGE_FAILED);
            }
            this.IsEnabled = true;
            this.Title = currentTitle;
        }

        private void Button_Extract_Click(object sender, RoutedEventArgs e)
        {
            var currentTitle = this.Title;
            this.IsEnabled = false;
            this.Title = currentTitle + Worker.TITLE_EXTRACTING;
            try
            {
                Worker.Extract();
                MessageBox.Show(Worker.MESSAGE_EXTRACTED);
            }
            catch
            {
                MessageBox.Show(Worker.MESSAGE_FAILED);
            }
            this.IsEnabled = true;
            this.Title = currentTitle;
        }

        private void Button_Generate_Click(object sender, RoutedEventArgs e)
        {
            var currentTitle = this.Title;
            this.IsEnabled = false;
            this.Title = currentTitle + Worker.TITLE_GENERATING;
            try
            {
                Worker.Generate();
                MessageBox.Show(Worker.MESSAGE_GENERATED);
            }
            catch
            {
                MessageBox.Show(Worker.MESSAGE_FAILED);
            }
            this.IsEnabled = true;
            this.Title = currentTitle;
        }

        private void Button_Open_Click(object sender, RoutedEventArgs e)
        {
            var currentTitle = this.Title;
            this.IsEnabled = false;
            this.Title = currentTitle + Worker.TITLE_OPENING;
            try
            {
                Worker.OpenResult();
            }
            catch
            {
                MessageBox.Show(Worker.MESSAGE_FAILED);
            }
            this.IsEnabled = true;
            this.Title = currentTitle;
        }

        private void Button_AllInOne_Click(object sender, RoutedEventArgs e)
        {
            var currentTitle = this.Title;
            this.IsEnabled = false;
            try
            {
                this.Title = currentTitle + Worker.TITLE_DOWNLOADING;
                Worker.Download();
                this.Title = currentTitle + Worker.TITLE_EXTRACTING;
                Worker.Extract();
                this.Title = currentTitle + Worker.TITLE_GENERATING;
                Worker.Generate();
                this.Title = currentTitle + Worker.TITLE_OPENING;
                Worker.OpenResult();
            }
            catch
            {
                MessageBox.Show(Worker.MESSAGE_FAILED);
            }
            this.IsEnabled = true;
            this.Title = currentTitle;
        }

        private void Button_ClearCache_Click(object sender, RoutedEventArgs e)
        {
            var currentTitle = this.Title;
            this.IsEnabled = false;
            this.Title = currentTitle + Worker.TITLE_DELETING;
            try
            {
                Worker.ClearCache();
                MessageBox.Show(Worker.MESSAGE_DELETED);
            }
            catch
            {
                MessageBox.Show(Worker.MESSAGE_FAILED);
            }
            this.IsEnabled = true;
            this.Title = currentTitle;
        }
    }
}