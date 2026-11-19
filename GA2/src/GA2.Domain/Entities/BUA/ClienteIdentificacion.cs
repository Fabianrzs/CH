using System;

namespace GA2.Domain.Entities
{
    public class ClienteIdentificacion
    {
        #region Cliente
        public int CLI_ID { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public string CLI_PRIMER_NOMBRE { get; set; }
        public string CLI_SEGUNDO_NOMBRE { get; set; }
        public string CLI_PRIMER_APELLIDO { get; set; }
        public string CLI_SEGUNDO_APELLIDO { get; set; }
        #endregion

    }
}
