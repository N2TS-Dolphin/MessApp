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
        private readonly ParticipantDao _participantDao;

        public ParticipantController()
        {
            _participantDao = new ParticipantDao(new MongoDBClient(new DBConfig()));
        }

        // TODO
    }
}
