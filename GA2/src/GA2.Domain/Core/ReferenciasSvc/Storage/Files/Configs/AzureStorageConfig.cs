using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Storage.Files.Configs
{
    public class AzureStorageConfig
    {
        public string ConnectionString { get; set; }

        public string ContainerName { get; set; }
    }
}
