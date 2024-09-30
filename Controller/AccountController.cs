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
    internal class AccountController
    {
        private readonly AccountDao _accountDao;
        private readonly MongoDBClient _client;

        // Constructor
        public AccountController() 
        {
            _client = new MongoDBClient(new DBConfig());
            _accountDao = new AccountDao(_client);
        }
        
        /// <summary>
        /// Get Infomation of Account Loged In
        /// </summary>
        /// <param name="uid">uid of this Account</param>
        /// <returns></returns>
        public AccountModel GetInfoAccount(string uid)
        {
            return _accountDao.GetAccountByUID(uid);
        }

        public List<AccountModel> GetAllFriendAccount(string uid)
        {
            List<AccountModel> result = new List<AccountModel>();



            return result;
        }
    }
}
