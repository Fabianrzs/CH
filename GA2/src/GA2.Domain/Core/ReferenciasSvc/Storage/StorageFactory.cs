
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Storage.Files
{
    public class StorageFactory : IStorageFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public StorageFactory(
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public async Task<IStorageProvider> GetProviderAsync()
        {
            var provider = _configuration["ProviderStorage"];

            return provider switch
            {
                "Azure" => _serviceProvider.GetRequiredService<AzureStorageProvider>(),
                "Api" => _serviceProvider.GetRequiredService<ApiStorageProvider>(),
                _ => _serviceProvider.GetRequiredService<LocalStorageProvider>()
            };
        }
    }
}
