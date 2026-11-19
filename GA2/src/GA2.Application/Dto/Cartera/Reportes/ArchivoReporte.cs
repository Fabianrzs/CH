using System;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class ArchivoReporte
    {
        public string Nombre { get; set; }
        public byte[] Bytes { get; set; }
        public string Message { get; set; }
        public bool Error { get; set; }
        public Exception Exception { get; set; }
    }
}