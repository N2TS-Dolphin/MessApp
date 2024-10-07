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
        private readonly RelationshipDao _relationshipDao;

        // Constructor
        public AccountController() 
        {
            _client = new MongoDBClient(new DBConfig());
            _accountDao = new AccountDao(_client);
            _relationshipDao = new RelationshipDao(_client);
        }
        
        /// <summary>
        /// Get Infomation of Account Loged In
        /// </summary>
        /// <param name="uid">uid of this Account</param>
        /// <returns></returns>
        public async Task<AccountModel> GetInfoAccount(int uid)
        {
            return await _accountDao.GetAccountByUID(uid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<AccountModel> GetAllFriendAccount(string uid)
        {
            List<AccountModel> result = new List<AccountModel>();



            return result;
        }
    }
}
