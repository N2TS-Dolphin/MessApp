using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using MessApp.DB;
using MessApp.DB.Dao;
using MessApp.DB.Model;
using MessApp.Config;
using Newtonsoft.Json;

namespace MessApp.Controller
{
    public class MessageController
    {
        private readonly MessageDao _messageDao;

        public MessageController()
        {
            _messageDao = new MessageDao(new MongoDBClient(new DBConfig()));
        }

        // TODO

        public MessageModel ParseMessage(string messageJSON)
        {
            return JsonConvert.DeserializeObject<MessageModel>(messageJSON);
        }

        public void HandleIncomingMessage(string messageJson)
        {
            var message = ParseMessage(messageJson);
            // Logic xử lý tin nhắn
        }
    }
}
