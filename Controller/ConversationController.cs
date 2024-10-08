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
    public class ConversationController
    {
        private readonly ConversationDao _conversationDao;

        public ConversationController()
        {
            _conversationDao = new ConversationDao(new MongoDBClient(new DBConfig()));
        }

        // TODO
    }
}
