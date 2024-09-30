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
    internal class ConversationDao
    {
        private readonly IMongoCollection<ConversationModel> _conversationCollection;
        private readonly IMongoCollection<ParticipantModel> _participantCollection;

        public ConversationDao(MongoDBClient client)
        {
            _conversationCollection = client.GetDatabase().GetCollection<ConversationModel>("conversations");
            _participantCollection = client.GetDatabase().GetCollection<ParticipantModel>("participants");
        }

        /// <summary>
        /// Get All Conversations of Account Loged In via UID
        /// </summary>
        /// <param name="user_id">User ID</param>
        /// <returns>All Conversations</returns>
        public List<ConversationModel> GetAllConversationsByUID(int user_id)
        {
            List<ParticipantModel> conversation_ids = _participantCollection.Find(participant => participant.user_id == user_id).ToList();
            List<ConversationModel> conversations = new List<ConversationModel>();

            foreach (var conversation in conversation_ids)
            {
                conversations.Add(_conversationCollection.Find(c=>c.conversation_id==conversation.conversation_id).FirstOrDefault());
            }

            return conversations;
        }
    }
}
