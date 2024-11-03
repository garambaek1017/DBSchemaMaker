using MySql.Data.MySqlClient;
using System.Data;

namespace DBSchemaMaker
{
    public class MySqlDBConnector
    {
        private bool _rollbackFlag;
        private IDbTransaction _transaction = null;
        public IDbConnection Connection { get; set; }

        public MySqlDBConnector(string connectionString, bool useTransaction = false)
        {
            Connection = new MySqlConnection
            {
                ConnectionString = connectionString
            };

            Connection.Open();

            if (useTransaction == true)
            {
                _transaction = Connection.BeginTransaction();
                _rollbackFlag = true;
            }
        }

        public virtual void Dispose()
        {
            if (_transaction != null)
            {
                if (_rollbackFlag == true)
                    _transaction.Rollback();
                else
                    _transaction.Commit();
            }

            Connection.Close();
        }
    }
}
