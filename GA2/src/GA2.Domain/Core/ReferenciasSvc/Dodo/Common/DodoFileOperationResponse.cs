using System.Collections.Generic;

namespace GA2.Domain.Core.Dodo.Common
{
    public class DodoFileOperationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string? BarCode { get; set; }
        public int RecordId { get; set; }
        public int WorkflowId { get; set; }
        public List<string> FileCodes { get; set; } = new();
        public List<int> FileIdList { get; set; } = new();
        public List<string> DestinationPath { get; set; } = new();
    }
}
