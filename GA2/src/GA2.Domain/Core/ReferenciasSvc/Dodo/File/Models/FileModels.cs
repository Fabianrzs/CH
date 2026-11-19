using GA2.Domain.Core.Dodo.Common;

namespace GA2.Domain.Core.Dodo.File.Models
{
    // ─── File_CopyToEP ─────────────────────────────────────────────────────────

    public class FileCopyToEPRequest
    {
        public string FileCode { get; set; }
        public string DirectoryCodeEP { get; set; }
        public DodoHeader Header { get; set; }
    }

    // ─── File_CopyToFather ─────────────────────────────────────────────────────

    public class FileCopyToFatherRequest
    {
        public string BarCodeSon { get; set; }
        public string FileCode { get; set; }
        public string DirectoryCodeFather { get; set; }
        public DodoHeader Header { get; set; }
    }

    // ─── File_Delete ───────────────────────────────────────────────────────────

    public class FileDeleteRequest
    {
        public string FileCode { get; set; }
        public DodoHeader Header { get; set; }
    }

    // ─── Response simple compartida ────────────────────────────────────────────

    public class FileSimpleResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
