using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessApp.DB;
using MessApp.DB.Dao;
using MessApp.DB.Model;
using MessApp.Config;

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
    }
}
