using DBSchemaMaker.DB.Connector;

namespace DBSchemaMaker.DB
{
    public class LogDB : BaseDB
    {
        public LogDB() : base()
        {
        }

        public LogDB(string name) : base(name)
        {
        }

        public override void GetTableInfoFromDB()
        {
            using (var db = new LogDBConnector())
            {
                GetAllTableListFromDB(db.Connection);
                GetTableInfoFromDB(db.Connection);
            }
        }

        public override void TestConnection()
        {
            using (var db = new LogDBConnector())
            {
                DoTestQuery(db.Connection);
            }
        }
    }
}
