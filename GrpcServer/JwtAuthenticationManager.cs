namespace GrpcServer
{
    public static class JwtAuthenticationManager
    {
        public const string JWT_TOKEN_KEY = "tungvv@2023";
        private const int JWT_TOKEN_VALIDITY = 30;

        //public static AuthenticationResponse Authenticate(AuthenticationRequest authenticationRequest)
        //{
        //    // -- Implement User Credentials Validation
        //    if (authenticationRequest.Username == "admin" && authenticationRequest.Password == "admin")
        //    {
        //        Grpc.Models.TaiKhoan acc = TaiKhoanRepository.Instance.GetById(authenticationRequest.Username);
        //        acc = new Grpc.Models.TaiKhoan();
        //        acc.username = "admin";
        //        acc.thuoc_tinh = new List<int>() { 1 };
        //        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        //        var tokenKey = Encoding.ASCII.GetBytes(JWT_TOKEN_KEY);
        //        var tokenExpiryDateTime = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY);
        //        var securityTokenDescriptor = new SecurityTokenDescriptor
        //        {
        //            Subject = new System.Security.Claims.ClaimsIdentity(new List<Claim>
        //        {
        //            new Claim("username", authenticationRequest.Username),
        //            new Claim(ClaimTypes.Role, acc.role.ToString())
        //        }),
        //            Expires = tokenExpiryDateTime,
        //            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        //        };

        //        var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        //        var token = jwtSecurityTokenHandler.WriteToken(securityToken);
        //        Account responseAccount = new )
        //        {
        //            UserName = acc.username,
        //            ThuocTinh = { acc.thuoc_tinh },
        //        };
        //        return new AuthenticationResponse
        //        {
        //            Account = responseAccount,
        //            AccessToken = token,
        //            ExpiresIn = (int)tokenExpiryDateTime.Subtract(DateTime.Now).TotalSeconds,
        //            Success = true,
        //            Message = "Đăng nhập thành công",
        //        };
        //    }
        //    else
        //    {
        //        return new AuthenticationResponse
        //        {
        //            Account = new AuthenticationResponse.Types.Account(),
        //            AccessToken = null,
        //            ExpiresIn = -1,
        //            Success = false,
        //            Msg = "Không đúng",
        //        };
        //    }
        //}
    }
}
