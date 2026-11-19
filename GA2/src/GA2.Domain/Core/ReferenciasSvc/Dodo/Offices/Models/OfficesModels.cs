using GA2.Domain.Core.Dodo.Common;
using System.Collections.Generic;

namespace GA2.Domain.Core.Dodo.Offices.Models
{
    public class GetOfficesRequest
    {
        public DodoHeader Header { get; set; }
    }

    public class OfficeItem
    {
        public int OfficeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class GetOfficesResponse
    {
        public List<OfficeItem> OfficeList { get; set; } = new();
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
