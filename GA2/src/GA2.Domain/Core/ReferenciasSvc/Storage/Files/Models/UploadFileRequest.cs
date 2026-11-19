using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Storage.Files.Models
{
    public class UploadFileRequest
    {
        public string Path { get; internal set; }
        public string FileName { get; internal set; }
        public string ContentType { get; internal set; }
        public string Base64Content { get; internal set; }
    }
}
