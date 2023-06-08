using Microsoft.IdentityModel.Tokens;
using SharedContracts.Contracts;
using SharedContracts.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GrpcServer.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public const string JWT_TOKEN_KEY = "tungvv@2023";
        private const int JWT_TOKEN_VALIDITY = 30;

        public AuthenticationResponse Authenticate(AuthenticationRequest request, ProtoBuf.Grpc.CallContext context = default)
        {
            // -- Implement User Credentials Validation
            if (request.Username == "admin" && request.Password == "admin")
            {
                //GrpcModels.TaiKhoan acc = TaiKhoanRepository.Instance.GetById(authenticationRequest.Username);
                GrpcModels.TaiKhoan acc = new GrpcModels.TaiKhoan();
                acc.username = "admin";
                acc.thuoc_tinh = new List<int>() { 1 };
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.ASCII.GetBytes(JWT_TOKEN_KEY);
                var tokenExpiryDateTime = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY);
                var securityTokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new List<Claim>
                {
                    new Claim("username", request.Username),
                    new Claim(ClaimTypes.Role, acc.role.ToString())
                }),
                    Expires = tokenExpiryDateTime,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };

                var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
                var token = jwtSecurityTokenHandler.WriteToken(securityToken);
                AccountContract responseAccount = new AccountContract()
                {
                    Username = acc.username,
                    ThuocTinh = acc.thuoc_tinh,
                };
                return new AuthenticationResponse
                {
                    Account = responseAccount,
                    AccessToken = token,
                    ExpiresIn = (int)tokenExpiryDateTime.Subtract(DateTime.Now).TotalSeconds,
                    Success = true,
                    Message = "Đăng nhập thành công",
                };
            }
            else
            {
                return new AuthenticationResponse
                {
                    Account = null,
                    AccessToken = null,
                    ExpiresIn = -1,
                    Success = false,
                    Message = "Không đúng",
                };
            }
        }


        public RegiterResponse Register(RegisterRequest request, ProtoBuf.Grpc.CallContext context = default)
        {
            throw new NotImplementedException();
        }
    }
}
