using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Estore.Test.Helpers
{
    class WebHostHelper
    {
        public IConfigurationRoot GetConfig()
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        public IWebHostBuilder GetWebHostBuilder()
        {
            WebHostBuilder builder = new WebHostBuilder();
            return builder.UseConfiguration(GetConfig()).UseStartup<Estore.Startup>();
        }
    }
}
