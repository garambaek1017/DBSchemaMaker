using DB_Schema_Maker.DB;
using System;

namespace DB_Schema_Maker
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                // 실행시 바로 뽑음 
                DBConfig.Instance.Init("localhost","3306","test_db","root","1234");
                Console.WriteLine("DB config Init Done");

                var testDB = new TestDB("test_db");
                testDB.GetTableList("testdb_list");
                testDB.GetTableInfoFromDB();
                testDB.WriteExcel();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                Console.WriteLine(e.StackTrace.ToString());
            }

            Run();
        }
        static void Run()
        {
            // 실행용 
            while (true)
            {

            }
        }
    }
}
