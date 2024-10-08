﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessApp.DB;
using MessApp.DB.Dao;
using MessApp.Config;
using MessApp.DB.Model;

namespace MessApp.Controller
{
    public class FriendController
    {
        private readonly FriendDao _friendDao;
        public FriendController()
        {
            _friendDao = new FriendDao(new MongoDBClient(new DBConfig()));
        }

        public async Task<List<int>> GetIDFriendRequestList(int user_id)
        {
            List<int> friend_ids = new List<int>();
            var friend_list = await _friendDao.GetFriendRequestList(user_id);

            foreach(var friend in friend_list)
            {
                friend_ids.Add(friend.user_id);
            }

            return friend_ids;
        }

        /// <summary>
        /// Accept Friend Request
        /// </summary>
        /// <param name="user_id">sender</param>
        /// <param name="friend_id">receiver</param>
        /// <returns></returns>
        public async Task AcceptFriendRequest(int user_id, int friend_id)
        {
            // Tìm mối quan hệ từ sender tới receiver sửa từ Pending thành Accept


            // Thêm mối quan hệ từ receiver tới sender với status là Accept


            // Tạo cuộc trò chuyện mới giữa 2 người

        }

        /// <summary>
        /// Reject Friend Request
        /// </summary>
        /// <param name="user_id">sender</param>
        /// <param name="friend_id">receiver</param>
        /// <returns></returns>
        public async Task RejectFriendRequest(int user_id, int friend_id)
        {
            // Tìm mối quan hệ từ sender tới receiver để xóa
            await _friendDao.DeleteFriendRequest(await _friendDao.GetFriendRequest(user_id,friend_id));
        }
    }
}
