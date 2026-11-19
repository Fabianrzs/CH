using System;

namespace GA2.Domain.Core.Dodo.Common
{
    public class DodoFileProperty
    {
        public int FileId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Ext { get; set; }
        public long Size { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public string? DocumentTypeCode { get; set; }
        public string? Version { get; set; }
    }
}
