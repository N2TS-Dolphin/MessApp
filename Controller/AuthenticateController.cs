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
    public class AuthenticateController
    {
        private readonly MongoDBClient _client;

        public AuthenticateController(MongoDBClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Hàm băm mật khẩu
        /// </summary>
        /// <param name="password">Mật khẩu</param>
        /// <returns>Mật khẩu đã băm</returns>
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
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
        public async Task SignUp(string username, string password, string phone, string firstName, string lastName, DateTime birthDate)
        {
            // Khởi tạo Dao
            AccountDao accountDao = new AccountDao(_client);

            // Xử lý mã hóa mật khẩu
            var hashedPassword = HashPassword(password);

            int uid = (await accountDao.GetAllAccounts()).Count() + 1;

            AccountModel newAccount = new AccountModel
            {
                user_id = uid,
                username = username,
                password = hashedPassword,
                firstName = firstName,
                lastName = lastName,
                phone = phone,
                birthDate = birthDate,
                registerDate = DateTime.UtcNow
            };

            await accountDao.InsertNewAccount(newAccount);
        }

        /// <summary>
        /// Xử lý so sánh thông tin tài khoản
        /// </summary>
        /// <param name="username">Tên người dùng</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns></returns>
        public async Task<bool> LogIn(string username, string password)
        {
            // Khởi tạo Dao
            AccountDao accountDao = new AccountDao(_client);
            // Đọc danh sách tài khoản từ db
            var accounts = await accountDao.GetAllAccounts();

            var account = accounts.Find(x => x.username == username);
            
            if(account != null)
            {
                var hashedPassword = account.password;
                var enteredHashedPassword = HashPassword(password);

                if (hashedPassword == enteredHashedPassword)
                {
                    LoginState.Instance.Set(account.user_id);
                    return true;
                }
            }
            return false; 
        }
    }
}
