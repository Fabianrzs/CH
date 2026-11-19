using GA2.Domain.Core.Dodo.Common;
using System;
using System.Collections.Generic;

namespace GA2.Domain.Core.Dodo.Lists.Models
{
    // ─── List_GetItems ─────────────────────────────────────────────────────────

    public class ListGetItemsRequest
    {
        public int ListId { get; set; }
        public int? FatherItemId { get; set; }
        public DodoHeader Header { get; set; }
    }

    public class ListItem
    {
        public int IdItem { get; set; }
        public int IdLista { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int? IdItemPadre { get; set; }
        public bool Activo { get; set; }
        public string? Usuario { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Host { get; set; }
        public string? ToolTipText { get; set; }
    }

    public class ListGetItemsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ListItem> Data { get; set; } = new();
        public int Rows { get; set; }
    }
}
