using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessApp.DB.Dao;
using MessApp.DB.Model;
using MessApp.Config;
using MessApp.DB;

namespace MessApp.Controller
{
    public class AccountController
    {
        private readonly MongoDBClient _client;
        private readonly AccountDao _accountDao;
        private readonly FriendController _friendController;

        // Constructor
        public AccountController() 
        {
            _client = new MongoDBClient(new DBConfig());
            _accountDao = new AccountDao(_client);
            _friendController = new FriendController();
        }
        
        /// <summary>
        /// Get Infomation of Account Loged In
        /// </summary>
        /// <param name="uid">uid of this Account</param>
        /// <returns></returns>
        public async Task<AccountModel> GetInfoAccount(int user_id)
        {
            return await _accountDao.GetAccountByUID(user_id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task<List<AccountModel>> GetAllFriendRequest(int user_id)
        {
            List<AccountModel> result = new List<AccountModel>();
            var friend_ids = await _friendController.GetIDFriendRequestList(user_id);

            foreach (var friend in friend_ids)
            {
                result.Add(await _accountDao.GetAccountByUID(friend));
            }

            return result;
        }
    }
}
