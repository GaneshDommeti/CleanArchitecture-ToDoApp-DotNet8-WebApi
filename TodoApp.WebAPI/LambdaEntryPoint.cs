using Amazon.Lambda.AspNetCoreServer;
using Microsoft.AspNetCore.Hosting;

namespace TodoApp.WebAPI
{
    public class LambdaEntryPoint : APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder.UseSetting(WebHostDefaults.ApplicationKey, typeof(Program).Assembly.FullName); 
        }
    }
}