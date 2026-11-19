using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Postulacion
{
    public class ActualizarPostulacionDto
    {
        public string Radicado { get; set; }
        public Guid CreadoPor { get; set; }
        public int Numero_Folio { get; set; }
    }
    public class InsertarComunicacionDto
    {
        public string identificacion { get; set; }
        public string nombreCompleto { get; set; }
        public string tipoDocumento { get; set; }
        public string contacto { get; set; }
        public string cargo { get; set; }
        public string email { get; set; }
        public string departamento { get; set; }
        public string ciudad { get; set; }
        public string direccion { get; set; }
        public string asunto { get; set; }
        public string anexo { get; set; }
        public string barrio { get; set; }
        public string radicadoRelacionado { get; set; }
        public string observaciones { get; set; }
    }
    public class insertarComunicacionDto
    {
        public string Radicado { get; set; }
        public Guid IdReintegro { get; set; }
    }
}
