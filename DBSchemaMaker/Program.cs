using DBSchemaMaker.DB;
using DBSchemaMaker.DB.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBSchemaMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                LogHelper.Debug("Start DB Schema Maker !! ");

                // 커넥션 스트링 추가 
                GameDBConnector.ConnectionString = DBHelper.Init("localhost", "3306", "game_db", "admin", "admin1234!");
                LogDBConnector.ConnectionString = DBHelper.Init("localhost", "3306", "log_db", "admin", "admin1234!");

                LogHelper.Debug("All Connection String Setting is Done");

                var dbs = new List<BaseDB>
                {
                    new GameDB("game_db"),
                    new LogDB("log_db")
                };

                foreach (var r in dbs)
                {
                    r.TestConnection();
                    r.GetTableInfoFromDB();
                    r.WriteExcel();
                }

                LogHelper.Debug("DB Schema Maker is Done");

            }
            catch (Exception e)
            {
                LogHelper.Debug(e.Message.ToString());
                LogHelper.Debug(e.StackTrace.ToString());
            }

            // 3초 후에 자동 종료
            Task.Delay(3000).Wait(); // 3000밀리초 = 3초
            LogHelper.Debug("3초 후 자동 종료됩니다.");
        }
    }
}
