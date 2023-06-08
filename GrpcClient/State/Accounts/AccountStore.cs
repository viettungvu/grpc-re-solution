using GrpcModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcClient.State.Accounts
{
    public class AccountStore : IAccountStore
    {
        private TaiKhoan _currentAccount;
        public TaiKhoan CurrentAccount
        {
            get
            {
                return _currentAccount;
            }
            set
            {
                _currentAccount = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;

    }
}
