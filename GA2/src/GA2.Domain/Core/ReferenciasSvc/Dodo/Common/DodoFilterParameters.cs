namespace GA2.Domain.Core.Dodo.Common
{
    /// <summary>
    /// Filtro simple de búsqueda. Operadores válidos: =, &lt;&gt;, &gt;=, &lt;=, Like
    /// </summary>
    public class DodoFilterParameters
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }
}
