using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcClient.State.Accounts;

namespace GrpcClient.Services.Auth
{
    internal interface ITokenProvider
    {
        Task<string> GetTokenAsync();
    }
    public class AppTokenProvider : ITokenProvider
    {
        private string _token;
        private IAccountStore _accountStore;
        public AppTokenProvider(IAccountStore accountStore)
        {
            _accountStore = accountStore;
        }
        public async Task<string> GetTokenAsync()
        {
            if (_token == null)
            {
                //_token = _accountStore.CurrentAccount.token;
            }
            return _token;
        }
    }
}
