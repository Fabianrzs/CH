namespace GA2.Domain.Core.Dodo.Common
{
    /// <summary>
    /// Par Field/Value para enviar o recibir datos de formularios.
    /// Key es de solo lectura (respuesta del servidor).
    /// </summary>
    public class DodoDataField
    {
        public string Field { get; set; }
        public string Value { get; set; }
        public string? Key { get; set; }
    }
}
