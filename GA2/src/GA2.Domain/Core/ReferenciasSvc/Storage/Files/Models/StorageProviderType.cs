using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Storage.Files
{
    public enum StorageProviderType
    {
        Local = 1,
        AzureBlob = 2,
        ExternalApi = 3
    }
}
