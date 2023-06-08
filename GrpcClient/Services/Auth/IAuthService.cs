using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using GrpcModels;
using SharedContracts.Services;
using SharedContracts.Contracts;

namespace GrpcClient.Services.Auth
{
    public interface IAuthService
    {
        Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword);
        TaiKhoan Login(string username, string password, out string msg);
    }

    public class AuthService : IAuthService
    {


        public AuthService()
        {

        }

        public TaiKhoan Login(string username, string password, out string msg)
        {
            msg = string.Empty;
            var channel = GrpcChannel.ForAddress("http://localhost:5206");
            var client = channel.CreateGrpcService<IAuthenticationService>();
            var x = client.Authenticate(new AuthenticationRequest
            {

            });

            //try
            //{

            //   // var authenticationClient = new Authentication.AuthenticationClient(channel);
            //    var client = channel.CreateGrpcService<IAuthenticationService>();
            //    var authenticationResponse = client.Authenticate(new Grpc.SharedContracts.Contracts.AuthenticationRequest
            //    {
            //        Username = username,
            //        Password = password
            //    });


            //    if (authenticationResponse != null)
            //    {
            //        if (authenticationResponse.Success)
            //        {
            //            TaiKhoan account = new TaiKhoan()
            //            {
            //                username = authenticationResponse.Account.Username,
            //                //token = authenticationResponse.AccessToken,
            //            };
            //            msg = "Đăng nhập thành công";
            //            return account;
            //        }
            //        else
            //        {
            //            msg = authenticationResponse.Message;
            //            return null;
            //        }
            //    }
            //    else
            //    {
            //        throw new UserNotFoundException(username);
            //    }
            //}
            //catch (RpcException ex)
            //{
            //    msg = string.Format("StatusCode:{0} | Error: {1}", ex.StatusCode, ex.Message);
            //}
            return null;
        }

        public async Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword)
        {

            var channel = GrpcChannel.ForAddress("http://localhost:5206");
            RegistrationResult result = RegistrationResult.Success;

            if (password != confirmPassword)
            {
                result = RegistrationResult.PasswordsDoNotMatch;
            }
            try
            {

            }
            catch (Exception)
            {
            }


            if (result == RegistrationResult.Success)
            {


            }
            return result;
        }
    }
}
