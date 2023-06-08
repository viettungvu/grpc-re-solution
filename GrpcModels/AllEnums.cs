using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcModels
{
    public enum RegistrationResult
    {
        Success,
        PasswordsDoNotMatch,
        EmailAlreadyExists,
        UsernameAlreadyExists
    }

    public enum IOrder
    {
        ALL = 0,
        ASC = 1,
        DESC = 2
    }


    public enum Role
    {
        ALL = -1,
        ADMIN = 1,
        USER = 2
    }

}
