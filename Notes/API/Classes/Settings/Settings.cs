using Newtonsoft.Json;
using System.IO;

namespace API.Classes.Settings
{
    /**
     * Holds all information required to connect to database, and to read the JSON formatted files that hold the information.
     */
    public class Settings
    {
        public static bool IsLoaded { get; set; }
        public static string Connection { get; set; }
        

        static string _file = "ConnectionSettings.json";
        static ConnectionSetting _settings = new ConnectionSetting();

        public static void Load()
        {
            using (StreamReader reader = new StreamReader(_file))
            {
                string json = reader.ReadToEnd();
                _settings = JsonConvert.DeserializeObject<ConnectionSetting>(json);

                IsLoaded = true;
                Connection = $"server={_settings.Ip};uid={_settings.User};pwd={_settings.Password};database={_settings.Database};" ;
            }
        }

        public static void Create()
        {
            string json = JsonConvert.SerializeObject(_settings, Formatting.Indented);
            File.WriteAllText(_file, json);
        }

        public static string GetFileName()
        {
            return _file;
        }
    }
}
