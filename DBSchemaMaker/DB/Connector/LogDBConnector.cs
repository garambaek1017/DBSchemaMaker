﻿using System;

namespace DBSchemaMaker.DB.Connector
{
    public sealed class LogDBConnector : MySqlDBConnector, IDisposable
    {
        public static string Name { get; set; }
        public static string ConnectionString { get; set; }

        public LogDBConnector(bool useTransaction = false)
            : base(ConnectionString, useTransaction)
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}