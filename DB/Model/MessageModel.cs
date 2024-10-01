using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MessApp.DB.Model
{
    public class MessageModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("mid")]
        public int message_id { get; set; }

        [BsonElement("cid")]
        public int conversation_id { get; set; }

        [BsonElement("sender_id")]
        public int sender_id { get; set; }

        [BsonElement("message")]
        public string message { get; set; }

        [BsonElement("send_At")]
        public DateTime send_At { get; set; }
    }
}
