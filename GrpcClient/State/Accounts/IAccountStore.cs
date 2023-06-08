using System;
using System.Collections.Generic;
using System.Text;
using GrpcModels;

namespace GrpcClient.State.Accounts
{
    public interface IAccountStore
    {
        TaiKhoan CurrentAccount { get; set; }
        event Action StateChanged;
    }
}
