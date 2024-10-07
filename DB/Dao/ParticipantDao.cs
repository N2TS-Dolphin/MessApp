using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessApp.DB.Model;
using MongoDB.Driver;

namespace MessApp.DB.Dao
{
    public class ParticipantDao
    {
        private readonly IMongoCollection<ParticipantModel> _participantCollection;

        public ParticipantDao(MongoDBClient client)
        {
            _participantCollection = client.GetDatabase().GetCollection<ParticipantModel>("participants");
        }

        public async Task<ParticipantModel> GetParticipant(int user_id, int conversation_id)
        {
            return await _participantCollection.Find(participant => participant.conversation_id == conversation_id && participant.user_id == user_id).FirstOrDefaultAsync();
        }

        public async Task<List<ParticipantModel>> GetAll()
        {
            return await _participantCollection.Find(participant => true).ToListAsync();
        }

        public async Task<List<ParticipantModel>> FindParticipantByUID(int user_id)
        {
            return await _participantCollection.Find(participant => participant.user_id == user_id).ToListAsync();
        }
    }
}
