using DBSchemaMaker.DB.Connector;

namespace DBSchemaMaker.DB
{
    public class GameDB : BaseDB
    {
        public GameDB() : base()
        {
        }

        public GameDB(string name) : base(name)
        {
        }

        public override void GetTableInfoFromDB()
        {
            using (var db = new GameDBConnector())
            {
                GetAllTableListFromDB(db.Connection);
                GetTableInfoFromDB(db.Connection);
            }
        }

        public override void TestConnection()
        {
            using (var db = new GameDBConnector())
            {
                DoTestQuery(db.Connection);
            }
        }
    }
}
