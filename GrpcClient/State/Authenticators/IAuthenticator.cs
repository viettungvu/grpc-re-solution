using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GrpcModels;
using GrpcClient.Services.Auth;
using GrpcClient.State.Accounts;

namespace GrpcClient.State.Authenticators
{
    public interface IAuthenticator
    {
        TaiKhoan CurrentAccount { get; }
        bool IsLoggedIn { get; }

        event Action StateChanged;

        Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword);
        bool Login(string username, string password, out string msg);

        void Logout();
    }

    public class Authenticator : IAuthenticator
    {
        private readonly IAuthService _authenticationService;
        private readonly IAccountStore _accountStore;

        public Authenticator(IAuthService authenticationService, IAccountStore accountStore)
        {
            _authenticationService = authenticationService;
            _accountStore = accountStore;
        }

        public TaiKhoan CurrentAccount
        {
            get
            {
                return _accountStore.CurrentAccount;
            }
            private set
            {
                _accountStore.CurrentAccount = value;
                StateChanged?.Invoke();
            }
        }

        public bool IsLoggedIn => CurrentAccount != null;

        public event Action StateChanged;

        public bool Login(string username, string password, out string msg)
        {
            msg = string.Empty;
            TaiKhoan acc = _authenticationService.Login(username, password, out msg);
            CurrentAccount = acc;
            if (acc != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Logout()
        {
            CurrentAccount = null;
        }

        public async Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword)
        {
            return await _authenticationService.Register(email, username, password, confirmPassword);
        }
    }
}
