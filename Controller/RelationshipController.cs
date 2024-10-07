using MessApp.DB.Model;
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
    public class RelationshipController
    {
        private readonly RelationshipDao _relationshipDao;
        public RelationshipController()
        {
            _relationshipDao = new RelationshipDao(new MongoDBClient(new DBConfig()));
        }

        /// <summary>
        /// Check 2 user be friend together
        /// </summary>
        /// <param name="uid">current user id</param>
        /// <param name="fid">friend id</param>
        /// <returns>true or false</returns>
        public async Task<bool> CheckRelationship(int uid, int fid)
        {
            List<RelationshipModel> relationships = await _relationshipDao.GetRelationships(uid);
            foreach (RelationshipModel relationship in relationships)
            {
                if (relationship.friend_id == fid)
                    return relationship.status == "accept";
            }
            return false;
        }
    }
}
