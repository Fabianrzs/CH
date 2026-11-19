using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Storage.Files
{
    public sealed class StorageResult
    {
        public string FileName { get; set; } = string.Empty;

        public string Path { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public long Size { get; set; }

        public string ContentType { get; set; } = string.Empty;
    }
}
