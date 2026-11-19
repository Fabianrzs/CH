using System;

namespace GA2.Application.Dto.Proyecto
{
    public class SupervisorProyecoDto
    {
        public int SP_TIPO_DOCUMENTO { get; set; }
        public string SP_NUMERO_DOCUMENTO { get; set; }
        public string SP_NOMBRE_APELLIDO { get; set; }
        public string SP_CARGO { get; set; }
        public string SP_EMAIL { get; set; }
        public double SP_TELEFONO { get; set; }
        public Guid? PC_PROYECTO_ID { get; set; } = Guid.Empty;
    }
}
