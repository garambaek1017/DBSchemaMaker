namespace DBSchemaMaker
{
    public class DBHelper
    {
        public static string Init(string _serverIP, string _port, string _dataBaseName, string _uid, string _password)
        {
            var connectionString =  $"Server={_serverIP};Port={_port};Database={_dataBaseName};Uid={_uid};Password={_password};";

            LogHelper.Debug(connectionString);

            return connectionString;
        }
    }
}
