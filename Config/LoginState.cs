using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessApp.Config
{
    /// <summary>
    /// Save UID Current User Loged In
    /// </summary>
    internal class LoginState
    {
        private static LoginState? _instance;
        private int _index;

        public static LoginState Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LoginState();
                return _instance;
            }
        }

        private LoginState() { }

        /// <summary>
        /// Get UID
        /// </summary>
        /// <returns></returns>
        public int Get()
        {
            return _index;
        }

        /// <summary>
        /// Set UID
        /// </summary>
        /// <param name="index">UID</param>
        public void Set(int index)
        {
            _index = index;
        }
    }
}
