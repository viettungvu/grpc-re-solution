using System.Security.Principal;

namespace GrpcModels
{
    public class TaiKhoan : BaseModel
    {
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public Role role { get; set; }
    }
}