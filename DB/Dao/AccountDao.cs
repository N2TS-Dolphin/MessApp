using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessApp.DB.Model;
using MongoDB.Driver;

namespace MessApp.DB.Dao
{
    class AccountDao
    {
        private readonly IMongoCollection<AccountModel> _accountCollection;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">MongoDBClient</param>
        public AccountDao(MongoDBClient client)
        {
            _accountCollection = client.GetDatabase().GetCollection<AccountModel>("accounts");
        }

        /// <summary>
        /// Get All Accounts in database
        /// </summary>
        /// <returns>All Accounts</returns>
        public List<AccountModel> GetAllAccounts()
        {
            return _accountCollection.Find(account => true).ToList();
        }

        /// <summary>
        /// Get Info Account by UID
        /// </summary>
        /// <param name="uid">UID</param>
        /// <returns>Info Account</returns>
        public AccountModel GetAccountByUID(string uid)
        {
            return _accountCollection.Find(uid).FirstOrDefault();
        }

        /// <summary>
        /// Insert New Account
        /// </summary>
        /// <param name="account">Infomation of Account</param>
        public void InsertNewAccount(AccountModel account) 
        {
            _accountCollection.InsertOne(account);
        }
    }
}
