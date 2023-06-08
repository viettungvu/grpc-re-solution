using GrpcModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using GrpcClient.Services.Auth;
using SharedContracts.Services;

namespace GrpcClient.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddScoped<ITokenProvider, AppTokenProvider>();
                services.AddSingleton<IAuthService, AuthService>(); 
                services.AddGrpcClient<IAuthenticationService>(o =>
                {
                    o.Address = new Uri("http://localhost:5137");
                })
                .ConfigureChannel(c =>
                {
                    c.HttpHandler = new SocketsHttpHandler()
                    {
                        EnableMultipleHttp2Connections = true
                    };

                })
                .AddCallCredentials(async (context, metadata, serviceProvider) =>
                {
                    var provider = serviceProvider.GetRequiredService<ITokenProvider>();
                    var token = await provider.GetTokenAsync();
                    if (!string.IsNullOrEmpty(token))
                    {
                        metadata.Add("Authorization", $"Bearer {token}");
                    }
                });
            });

            return host;
        }
    }
}
