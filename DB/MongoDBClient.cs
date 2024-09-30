using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessApp.Config;
using MongoDB.Driver;

namespace MessApp.DB
{
    public class MongoDBClient
    {
        private readonly IMongoDatabase _database;

        public MongoDBClient(DBConfig config)
        {
            var client = new MongoClient(config.connectionString);
            _database = client.GetDatabase(config.databaseName);
        }

        // Return DB
        public IMongoDatabase GetDatabase() { return _database; }
    }
}
