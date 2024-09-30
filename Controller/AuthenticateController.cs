using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows;

using MessApp.DB.Model;
using MessApp.DB.Dao;
using MessApp.DB;
using MessApp.Config;

namespace MessApp.Controller
{
    internal class AuthenticateController
    {
        private readonly MongoDBClient _client;

        public AuthenticateController(MongoDBClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Xử lí đăng ký tài khoản mới
        /// </summary>
        /// <param name="username">tên người dùng</param>
        /// <param name="password">mật khẩu</param>
        /// <param name="firstName">tên</param>
        /// <param name="lastName">họ và tên đệm</param>
        /// <param name="phone">số điện thoại</param>
        /// <param name="birthDate">ngày sinh</param>
        public void SignUp(string username, string password, string phone, string firstName, string lastName, DateTime birthDate)
        {
            // Khởi tạo Dao
            AccountDao accountDao = new AccountDao(_client);
            // Đọc danh sách tài khoản trong db
            List<AccountModel> accounts = accountDao.GetAllAccounts();
            
            // Xử lý mã hóa mật khẩu
            var passwordInBytes = Encoding.UTF8.GetBytes(password);
            var entropy = new byte[32];

            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(entropy);
            }

            var cypherText = ProtectedData.Protect(passwordInBytes, entropy, DataProtectionScope.CurrentUser);

            int uid = accounts.Count() + 1;

            AccountModel newAccount = new AccountModel
            {
                user_id = uid,
                username = username,
                password = Convert.ToBase64String(cypherText),
                entropy = Convert.ToBase64String(entropy),
                firstName = firstName,
                lastName = lastName,
                phone = phone,
                birthDate = birthDate,
                registerDate = DateTime.UtcNow
            };

            accountDao.InsertNewAccount(newAccount);
        }

        public bool LogIn(string username, string password)
        {
            // Khởi tạo Dao
            AccountDao accountDao = new AccountDao(_client);
            // Đọc danh sách tài khoản từ db
            List<AccountModel> accounts = accountDao.GetAllAccounts();

            int index = accounts.FindIndex(x => x.username == username);

            if(index != -1)
            {
                var passwordInBytes = Convert.FromBase64String(accounts[index].password);
                var entropy = Convert.FromBase64String(accounts[index].entropy);
                var decryptedPassword = ProtectedData.Unprotect(passwordInBytes, entropy, DataProtectionScope.CurrentUser);

                var accountPassword = Encoding.UTF8.GetString(decryptedPassword);
                if(accountPassword == password)
                {
                    LoginState.Instance.Set(accounts[index].user_id);
                    return true;
                }
                else
                    return false;

            }
            else
                return false;
        }
    }
}
