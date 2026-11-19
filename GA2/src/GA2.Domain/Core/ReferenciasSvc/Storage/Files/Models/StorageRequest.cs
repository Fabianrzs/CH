using DocumentFormat.OpenXml.Spreadsheet;

namespace GA2.Domain.Core.ReferenciasSvc.Storage.Files
{
    public sealed class StorageRequest
    {
        public string Path { get; set; } = string.Empty;
        public bool SameName { get; set; }
         
    }
}
