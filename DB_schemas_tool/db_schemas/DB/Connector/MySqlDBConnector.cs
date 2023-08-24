using MySql.Data.MySqlClient;
using System.Data;

namespace DB_Schema_Maker
{
    public class MySqlDBConnector
    {
        private bool _rollbackFlag;
        private IDbTransaction _Transaction = null;
        public IDbConnection Connection { get; set; }

        public MySqlDBConnector(string connectionString, bool useTransaction = false)
        {
            Connection = new MySqlConnection();
            // connection string 사용 
            Connection.ConnectionString = connectionString;
            // connection open 
            Connection.Open();

            if (useTransaction == true)
            {
                _Transaction = Connection.BeginTransaction();
                _rollbackFlag = true;
            }
        }

        public virtual void Dispose()
        {
            if (_Transaction != null)
            {
                if (_rollbackFlag == true)
                    _Transaction.Rollback();
                else
                    _Transaction.Commit();
            }

            Connection.Close();
        }
    }
}
