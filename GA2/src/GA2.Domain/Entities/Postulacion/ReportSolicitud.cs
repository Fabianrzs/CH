using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Postulacion
{
    public class ReportSolicitud
    {
        public Guid IdSolicitud { get; set; }
        public string NoRadicado { get; set; }
        public string NombreAfiliado { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Fuerza { get; set; }
        public string UnidadEjecutora { get; set; }
        public string Categoria { get; set; }
        public string Ciudad1 { get; set; }
        public string Ciudad2 { get; set; }
        public string Ciudad3 { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string DisfrutePension { get; set; }
        public int TipoDocumentoId { get; set; }
        public int ConsecutivoSolicitud { get; set; }
        public string ModalidadSolucionVivienda { get; set; }
        public int NumeroBeneficiario { get; set; }
        public string ModalidadSolicitud { get; set; }
    }
}
