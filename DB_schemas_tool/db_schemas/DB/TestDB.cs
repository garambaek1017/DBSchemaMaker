using DB_Schema_Maker.DB.Connector;
using System;

namespace DB_Schema_Maker.DB
{
    public class TestDB : BaseDB
    {
        public TestDB() : base()
        {
        }

        public TestDB(string _name) : base(_name)
        {
        }

        /// <summary>
        /// 테이블 정보 받아옴 
        /// </summary>
        public override void GetTableInfoFromDB()
        {
            using (var db = new TestDBConnector())
            {
                GetDataFromDB(db.Connection);
            }

            Console.WriteLine($"Get {DBName} TableInfos is Done");
        }

    }
}
