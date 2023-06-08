using Grpc.Core;
using GrpcServer;
using ProtoBuf.Grpc;
using SharedContracts;

namespace GrpcServer.Services
{
    public class GreeterService : IGreeterService
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public Task<HelloReply> SayHelloAsync(HelloRequest request, CallContext context = default)
        {
            throw new NotImplementedException();
        }
    }
}