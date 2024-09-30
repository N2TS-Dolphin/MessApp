using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessApp.DB.Model;
using MongoDB.Driver;

namespace MessApp.DB.Dao
{
    public class RelationshipDao
    {
        private readonly IMongoCollection<RelationshipModel> _relationshipCollection;

        public RelationshipDao(MongoDBClient client)
        {
            _relationshipCollection = client.GetDatabase().GetCollection<RelationshipModel>("relationships");
        }

        /// <summary>
        /// Get All Friends of Account via UID
        /// </summary>
        /// <param name="uid">User ID</param>
        /// <returns>All Friends of Account</returns>
        public List<RelationshipModel> GetRelationships(int uid)
        {
            return _relationshipCollection.Find(relationship => relationship.user_id == uid).ToList();
        }

        /// <summary>
        /// Get All Friend Requests of Account via UID
        /// </summary>
        /// <param name="uid">User ID</param>
        /// <returns>All Friend Requests of Account</returns>
        public List<RelationshipModel> GetFriendRequests(int uid)
        {
            return _relationshipCollection.Find(relationship => relationship.friend_id == uid && relationship.status == "Pending").ToList();
        }
    }
}
