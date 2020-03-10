using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace TimeZoneDB_JSON
{
    public static class StaticValues
    {
        public const string MESSAGE_NO_DB = "No databases downloaded.";
        public const string MESSAGE_LOCAL_VERSION = "Local version: ";
        public const string MESSAGE_CURRENT_VERSION = "Current version: ";
        public const string MESSAGE_UP_TO_DATE = "Up to date.";
        public const string MESSAGE_REQUIRE_REDOWNLOADING = "Require redownloading.";
        public const string MESSAGE_DOWNLOADED = "Downloaded.";
        public const string MESSAGE_EXTRACTED = "Extracted.";


        public const string DEVELOPER = @"nazzinoz";
        public const string APP_NAME = @"TimeZoneDB_JSON";
        public const string VERSION_API = @"https://timezonedb.com/date.txt";
        public const string DOWNLOAD_API = @"https://timezonedb.com/files/timezonedb.csv.zip";

        public static readonly string EXECUTE_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),DEVELOPER,APP_NAME);
        public static readonly string VERSION_FILE = Path.Combine(EXECUTE_PATH, @"version.txt");
        public static readonly string DOWNLOAD_FILE = Path.Combine(EXECUTE_PATH, @"timezonedb.csv.zip");

        public static readonly string EXTRACTED_ZONE = Path.Combine(EXECUTE_PATH, @"zone.csv");
        public static readonly string EXTRACTED_COUNTRY = Path.Combine(EXECUTE_PATH, @"country.csv");
        public static readonly string EXTRACTED_TIMEZONE = Path.Combine(EXECUTE_PATH, @"timezone.csv");

        public static readonly WebClient WEB_CLIENT = new WebClient();
        public static MainWindow MAIN_WINDOW;
    }
}
