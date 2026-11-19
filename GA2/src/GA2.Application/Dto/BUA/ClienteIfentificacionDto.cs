using System;

namespace GA2.Application.Dto
{
    public class ClienteIdentificacionDto
    {
        #region Cliente
        public int IdCliente { get; set; }
        public string Identificacion { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        #endregion
    }
}
