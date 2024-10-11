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
        public async Task<int> CreateNewConversation(int created_id)
        {
            var conversationId = (int)await _conversationDao.CountConversation() + 1;

            var newConversation = new ConversationModel
            {
                conversation_id = conversationId,
                createDate = DateTime.Now,
                name = "",
                user_id = created_id
            };

            int conversation_id = await _conversationDao.InsertNewConversation(newConversation);

            return conversation_id;
        }
    }
}
