using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessApp.DB.Model;
using MongoDB.Driver;

namespace MessApp.DB.Dao
{
    public class AccountDao
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
        public async Task<List<AccountModel>> GetAllAccounts()
        {
            return await _accountCollection.Find(account => true).ToListAsync();
        }

        /// <summary>
        /// Get Info Account by UID
        /// </summary>
        /// <param name="uid">UID</param>
        /// <returns>Info Account</returns>
        public async Task<AccountModel> GetAccountByUID(int uid)
        {
            return await _accountCollection.Find(account => account.user_id == uid).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get Account by Phone Number
        /// </summary>
        /// <param name="phone">Phone Number</param>
        /// <returns>Info Account</returns>
        public async Task<AccountModel> GetAccountByPhone(string phone)
        {
            return await _accountCollection.Find(account => account.phone == phone).FirstOrDefaultAsync();
        }

        public async Task<List<AccountModel>> GetAllAccountByName(string name)
        {
            var accounts = await GetAllAccounts();
            return accounts.Where(account => (account.firstName + " " + account.lastName).Contains(name)).ToList();
        }

        /// <summary>
        /// Insert New Account
        /// </summary>
        /// <param name="account">Infomation of Account</param>
        public async Task InsertNewAccount(AccountModel account) 
        {
            await _accountCollection.InsertOneAsync(account);
        }
    }
}
