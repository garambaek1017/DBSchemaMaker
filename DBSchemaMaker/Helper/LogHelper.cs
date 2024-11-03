using System;

namespace DBSchemaMaker
{
    public class LogHelper
    {
        public static void Debug(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:{message}");
        }
    }
}
