using System.Windows;
using GrpcClient.HostBuilders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GrpcClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            _host = CreateHostBuilder().Build();
        }


        public static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder()
                .AddConfiguration()
                .AddServices()
                .AddStores()
                .AddViewModels()
                .AddViews();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host!.StartAsync();
            //var channel = GrpcChannel.ForAddress("http://localhost:5137");
            //try
            //{
            //    var authenticationClient = new Authentication.AuthenticationClient(channel);
            //    var authenticationResponse = authenticationClient.Authenticate(new AuthenticationRequest
            //    {
            //        UserName = "admin",
            //        Password = "admin"
            //    });

            //    Console.WriteLine($"Received Auth Response | Token: {authenticationResponse.AccessToken} | Expires In: {authenticationResponse.ExpiresIn}");


            //    var calculationClient = new Calculation.CalculationClient(channel);
            //    var headers = new Metadata();
            //    headers.Add("Authorization", $"Bearer {authenticationResponse.AccessToken}");
            //    //var sumResult = calculationClient.Add(new InputNumbers { Number1 = 5, Number2 = 10 }, headers);
            //    //Console.WriteLine($"Sum Result: 5+10={sumResult.Result}");

            //    //var subtractResult = calculationClient.Subtract(new InputNumbers { Number1 = 20, Number2 = 5 }, headers);
            //    //Console.WriteLine($"Subtract result: 20-5={subtractResult.Result}");


            //    var multiplyResult = calculationClient.Multiply(new InputNumbers { Number1 = 5, Number2 = 6 });
            //    Console.WriteLine($"Multiply Result: 5*6={multiplyResult.Result}");

            //}
            //catch (RpcException ex)
            //{
            //    Console.WriteLine($"Status Code: {ex.StatusCode} | Error: {ex.Message}");
            //    return;
            //}
            Window window = _host.Services.GetRequiredService<MainWindow>();
            window.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host!.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
