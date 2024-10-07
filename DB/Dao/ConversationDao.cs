using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessApp.DB.Model;
using MongoDB.Driver;

namespace MessApp.DB.Dao
{
    public class ConversationDao
    {
        private readonly IMongoCollection<ConversationModel> _conversationCollection;

        private readonly ParticipantDao _participantDao;
        private readonly MessageDao _messageDao;

        public ConversationDao(MongoDBClient client)
        {
            _conversationCollection = client.GetDatabase().GetCollection<ConversationModel>("conversations");

            _participantDao = new ParticipantDao(client);
            _messageDao = new MessageDao(client);
        }

        /// <summary>
        /// Get All Conversations of Account Loged In via UID
        /// </summary>
        /// <param name="user_id">User ID</param>
        /// <returns>All Conversations</returns>
        public async Task<List<ConversationModel>> GetAllConversationsByUID(int user_id)
        {
            var participants = await _participantDao.FindParticipantByUID(user_id);
            var conversationIds = participants.Select(p => p.conversation_id);

            var filter = Builders<ConversationModel>.Filter.In(c => c.conversation_id, conversationIds);
            return await _conversationCollection.Find(filter).ToListAsync();
        }
    }
}
