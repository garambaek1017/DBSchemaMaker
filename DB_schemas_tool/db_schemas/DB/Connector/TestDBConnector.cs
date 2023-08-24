using System;

namespace DB_Schema_Maker.DB.Connector
{
    public sealed class TestDBConnector : MySqlDBConnector, IDisposable
    {
        public static string ConnectionString { get; set; }

        public TestDBConnector(bool useTransaction = false)
            : base(ConnectionString, useTransaction)
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
