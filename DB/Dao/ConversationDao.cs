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
        private readonly IMongoCollection<MessageModel> _messageCollection;
        private IChangeStreamCursor<ChangeStreamDocument<MessageModel>> _messageChangeStream;

        public ConversationDao(MongoDBClient client)
        {
            _conversationCollection = client.GetDatabase().GetCollection<ConversationModel>("conversations");
            _participantCollection = client.GetDatabase().GetCollection<ParticipantModel>("participants");
            _messageCollection = client.GetDatabase().GetCollection<MessageModel>("messages");
        }

        public int CountMessages()
        {
            return _messageCollection.Find(message => true).ToList().Count();
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

        /// <summary>
        /// Get All Messages in Conversation via CID
        /// </summary>
        /// <param name="conversation_id">Conversation ID</param>
        /// <returns>All Messages in Conversation</returns>
        public List<MessageModel> GetAllMessagesByCID(int conversation_id)
        {
            var sortDefinition = Builders<MessageModel>.Sort.Ascending(m => m.send_At);
            List<MessageModel> messages = _messageCollection.Find(message => message.conversation_id == conversation_id).Sort(sortDefinition).ToList();

            return messages;
        }

        public void InsertNewMessage(MessageModel newMessage)
        {
            _messageCollection.InsertOne(newMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conversation_id"></param>
        /// <param name="onMessageReceived"></param>
        public void StartMessageStream(int conversation_id, Action<MessageModel> onMessageReceived)
        {
            // Define the pipeline for change stream (you can filter based on conversation_id
            var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<MessageModel>>()
                .Match(change => change.FullDocument.conversation_id == conversation_id);

            // Start watching the messages collection for changes
            Task.Run(() =>
            {
                _messageChangeStream = _messageCollection.Watch(pipeline);

                foreach (var change in _messageChangeStream.ToEnumerable())
                {
                    if(change.OperationType == ChangeStreamOperationType.Insert)
                    {
                        onMessageReceived(change.FullDocument);
                    }    
                }    
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopMessageStream()
        {
            _messageChangeStream?.Dispose();
        }
    }
}
