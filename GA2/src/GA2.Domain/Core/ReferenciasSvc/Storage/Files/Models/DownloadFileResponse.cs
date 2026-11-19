using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Storage.Files.Models
{
    public class DownloadFileResponse
    {
        public string FileName { get; set; }

        public string ContentBase64 { get; set; }
        public string Base64Content { get; set; }
    }
}
