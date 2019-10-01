using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;

namespace CloseGroup.Api
{
    internal class Settings : ISettings
    {
        private readonly IConfiguration configuration;

        public Settings(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public T Get<T>(string key)
        {
            return configuration.GetValue<T>(key);
        }
    }
}
