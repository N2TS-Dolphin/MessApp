using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MessApp.DB.Model
{
    public class AccountModel : INotifyPropertyChanged
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("uid")]
        public int user_id { get; set; }

        [BsonElement("username")]
        public string username { get; set; }

        [BsonElement("password")]
        public string password { get; set; }

        [BsonElement("entropy")]
        public string entropy { get; set; }

        [BsonElement("firstName")]
        public string firstName { get; set; }

        [BsonElement("lastName")]
        public string lastName { get; set; }

        [BsonElement("phone")]
        public string phone { get; set; }

        [BsonElement("birthDate")]
        public DateTime birthDate { get; set; }

        [BsonElement("registerDate")]
        public DateTime registerDate { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
