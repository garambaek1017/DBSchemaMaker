namespace DB_Schema_Maker
{
    /// <summary>
    /// DB 접속을 위한 config 파일 
    /// </summary>
    public class DBConfig
    {
        #region Instance
        static DBConfig m_Instance = new DBConfig();
        public static DBConfig Instance
        {
            get { return m_Instance; }
        }
        #endregion
        public string ConnectionString { get; set; }
        public void Init(string _serverIP, string _port, string _dataBaseName, string _uid, string _password)
        {
            this.ConnectionString = $"Server={_serverIP};Port={_port};Database={_dataBaseName};Uid={_uid};Password={_password};";
        }
    }
}
