using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessApp.Config
{
    class DBConfig
    {
        public string connectionString {  get; set; }
        public string databaseName { get; set; }

        public DBConfig()
        {
            connectionString = "mongodb+srv://admin:123admin@mess.lyqs9.mongodb.net/";
            databaseName = "users";
        }
    }
}
