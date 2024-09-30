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
    public class ConversationModel : INotifyPropertyChanged
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("cid")]
        public int conversation_id { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("create_At")]
        public DateTime createDate { get; set; }

        [BsonElement("create_By")]
        public int user_id { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
