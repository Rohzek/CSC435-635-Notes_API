namespace API.Classes.Settings
{
    public class ConnectionSetting
    {
        public string Ip { get; set; }
        public string Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }

        public ConnectionSetting()
        {
            Ip = "127.0.0.1";
            Port = "3306";
            User = "root";
            Password = "password";
            Database = "name";
        }
    }
}
