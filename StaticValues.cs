using System;
using System.IO;
using System.Net;

namespace TimeZoneDB_JSON
{
    public static class StaticValues
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

        public static readonly string DOWNLOAD_VERSION_FILE = Path.Combine(DOWNLOAD_PATH,"version.txt");
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
    }
}
