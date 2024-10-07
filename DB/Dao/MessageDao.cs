using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessApp.DB.Model;
using MongoDB.Driver;

namespace MessApp.DB.Dao
{
    public class MessageDao
    {
        private readonly IMongoCollection<MessageModel> _messageCollection;
        private IChangeStreamCursor<ChangeStreamDocument<MessageModel>> _messageChangeStream;

        public MessageDao(MongoDBClient client)
        {
            _messageCollection = client.GetDatabase().GetCollection<MessageModel>("messages");
        }

        public async Task<List<MessageModel>> GetAllMessagesByCID(int conversation_id)
        {
            var sortDefinition = Builders<MessageModel>.Sort.Ascending(m => m.send_At);
            
            return await _messageCollection.Find(message => message.conversation_id == conversation_id).Sort(sortDefinition).ToListAsync();
        }

        public async Task AddMessage(MessageModel newMessage)
        {
            await _messageCollection.InsertOneAsync(newMessage);
        }

        public async Task<long> CountMessages()
        {
            return await _messageCollection.CountDocumentsAsync(message => true);
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
                    if (change.OperationType == ChangeStreamOperationType.Insert)
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
