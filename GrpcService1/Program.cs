using GrpcService1.Data;
using GrpcService1.Services;

namespace GrpcService1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddGrpc();

            builder.Services.AddTransient<ISqlService,SqlService>();

           

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<CustomerDataService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}