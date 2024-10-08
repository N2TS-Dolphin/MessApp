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
    public class FriendModel : INotifyPropertyChanged
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("uid")]
        public int user_id { get; set; }

        [BsonElement("fid")]
        public int friend_id { get; set; }

        [BsonElement("status")]
        public string status { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
