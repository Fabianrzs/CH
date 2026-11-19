using GA2.Domain.Core.Dodo.Common;
using System.Collections.Generic;

namespace GA2.Domain.Core.Dodo.Field.Models
{
    // ─── Field_GetValues ───────────────────────────────────────────────────────

    public class FieldGetValuesRequest
    {
        public DodoHeader Header { get; set; }
        public string FormCode { get; set; }
        public string ColumnName { get; set; }
        public int? FatherItemId { get; set; }
    }

    public class FieldValueItem
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public string? ToolTipText { get; set; }
    }

    public class FieldGetValuesResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<FieldValueItem> Items { get; set; } = new();
        public int Rows { get; set; }
    }
}
