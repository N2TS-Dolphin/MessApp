using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessApp.DB.Model;
using MongoDB.Driver;

namespace MessApp.DB.Dao
{
    public class FriendDao
    {
        private readonly IMongoCollection<FriendModel> _friendCollection;

        public FriendDao(MongoDBClient client)
        {
            _friendCollection = client.GetDatabase().GetCollection<FriendModel>("friends");
        }

        public async Task<FriendModel> GetFriendRequest(int user_id, int friend_id)
        {
            return await _friendCollection.Find(f => f.user_id == user_id && f.friend_id == friend_id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get Friend Request list of Account via UID
        /// </summary>
        /// <param name="uid">User ID</param>
        /// <returns>All Friend Request list of Account</returns>
        public async Task<List<FriendModel>> GetFriendRequestList(int user_id)
        {
            return await _friendCollection.Find(friend => friend.friend_id == user_id && friend.status == "Pending").ToListAsync();
        }

        public async Task DeleteFriendRequest (FriendModel friend)
        {
            await _friendCollection.DeleteOneAsync(f => f.user_id == friend.user_id && f.friend_id == friend.friend_id && f.status == friend.status);
        }

        public void StartFriendStream(int user_id, Action<FriendModel> onFriendResponse)
        {

        }
    }
}
