using Grpc.Core;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace SharedContracts.Contracts
{
    [DataContract]
    public class AuthenticationRequest
    {
        [DataMember(Order = 1)]
        public string Username { get; set; }
        [DataMember(Order = 2)]
        public string Password { get; set; }
    }


    [DataContract]
    public class AuthenticationResponse : BaseResponse
    {
        [DataMember]
        public string AccessToken { get; set; }
        [DataMember]
        public int ExpiresIn { get; set; }
        [DataMember]
        public AccountContract Account { get; set; }
    }


    [DataContract]
    public class RegisterRequest
    {
        [DataMember(Order = 1)]
        public string Username { get; set; }
        [DataMember(Order = 2)]
        public string Password { get; set; }
        [DataMember(Order = 3)]
        public string ConfirmPassword { get; set; }
        [DataMember(Order = 4)]
        public string Email { get; set; }
    }


    [DataContract]
    public class RegiterResponse : BaseResponse
    {

    }



}
