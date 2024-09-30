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
    internal class ParticipantModel : INotifyPropertyChanged
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("cid")]
        public int conversation_id { get; set; }

        [BsonElement("uid")]
        public int user_id { get; set; }

        [BsonElement("join_At")]
        public DateTime joinDate { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
