using System;

namespace GA2.Domain.Entities
{
    public class CargeArchivosPostulacion
    {
        public Guid Asp_Id { get; set; }
        public string Asp_Nombre_Archivo { get; set; }
        public string Asp_Ruta_Storage { get; set; }
        public string Asp_Extension { get; set; }
        public DateTime Asp_Fecha_Cargue { get; set; }
        public Guid Asp_Solicitud_Id { get; set; }
        public int Asp_Estado { get; set; }
        public string Asp_Numero_Folio { get; set; }
        public DateTime Asp_Fecha_Creacion { get; set; }
        public Guid Asp_Id_Archivo_Fk { get; set; }
        public Guid UserId { get; set; }

    }
}
