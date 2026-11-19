using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class TipoListaCargarArchivoPostulacionDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Requerido { get; set; }
        public int Orden { get; set; }
        public int IdModalidad { get; set; }
        public int Estado { get; set; }
        public string Formato { get; set; }
        public Guid SolicitudId { get; set; }
        public int ArchivoCargado { get; set; }
        public int NumeroFolio { get; set; }
    }
}
