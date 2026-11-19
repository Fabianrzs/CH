using GA2.Domain.Core.ReferenciasSvc.Storage.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Storage
{
    public interface IStorageFactory
    {
        Task<IStorageProvider> GetProviderAsync();
    }
}
