using ProtoBuf.Grpc;
using SharedContracts.Contracts;
using System.ServiceModel;

namespace SharedContracts.Services
{
    [ServiceContract]
    public interface IAuthenticationService
    {
        [OperationContract]
        AuthenticationResponse Authenticate(AuthenticationRequest request, CallContext context = default);

        [OperationContract]
        RegiterResponse Register(RegisterRequest request, CallContext context = default);
    }
}
