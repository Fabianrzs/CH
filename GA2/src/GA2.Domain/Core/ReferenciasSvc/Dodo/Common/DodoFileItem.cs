using System;

namespace GA2.Domain.Core.Dodo.Common
{
    public class DodoFileItem
    {
        public string? Base64String { get; set; }
        public string Ext { get; set; }
        public string Description { get; set; }
        public string DocumentTypeCode { get; set; } = "00";
        public string DirectoryCode { get; set; } = "00";
        public string? GUID { get; set; } = new Guid().ToString();
        public string? Version { get; set; } = "1";
        public int? Length { get; set; }
        public int? MainFileId { get; set; }
        public DateTime? PublishDate { get; set; } = DateTime.MinValue;
    }
}
