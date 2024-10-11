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
    public class ParticipantController
    {
        private readonly MongoDBClient _client;
        private readonly ParticipantDao _participantDao;
        private readonly AccountDao _accountDao;

        public ParticipantController()
        {
            _client = new MongoDBClient(new DBConfig());
            _participantDao = new ParticipantDao(_client);
            _accountDao = new AccountDao(_client);
        }

        // TODO
        public async void CreateNewParticipant(int conversation_id, int user_id, int friend_id)
        {
            var friendAccount = await _accountDao.GetAccountByUID(friend_id);

            var newParticipant = new ParticipantModel
            {
                conversation_id = conversation_id,
                user_id = user_id,
                chatname = friendAccount.lastName + " " + friendAccount.firstName,
                joinDate = DateTime.Now
            };

            _participantDao.InsertNewParticipant(newParticipant);
        }
    }
}
