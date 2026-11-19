using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito.Simulador.Comite
{
    public class SolicitudComiteInfInmuebleDto
    {
        public SolicitudComiteInfInmuebleUbicacionDto Ubicacion { get; set; }
        public SolicitudComiteInfInmuebleAvaComercialDto AvaluoComercial { get; set; }
        public SolicitudComiteInfInmueblePolGarantiasDto PoliticasGarantias { get; set; }
        public Guid Solicitud_id { get; set; }
    }

    public class SolicitudComiteInfInmuebleUbicacionDto
    {
        public string Ciudad { get; set; }
        public string Municipio { get; set; }
        public string NombrePropiedad { get; set; }
        public string Direccion { get; set; }
    }

    public class SolicitudComiteInfInmuebleAvaComercialDto
    {
        public decimal AreaPrivada { get; set; }
        public decimal ValorM2Privada { get; set; }
        public decimal AreaGaraje { get; set; }
        public decimal ValorM2Garaje { get; set; }
        public decimal AreaDeposito { get; set; }
        public decimal ValorM2Deposito { get; set; }
        public decimal ValorM2Sector { get; set; }
    }

    public class SolicitudComiteInfInmueblePolGarantiasDto
    {
        public string EstadoVivienda { get; set; }
        public string Estrato { get; set; }
        public int AñosConstruccion { get; set; }
        public int EdadPrimeraTradicion { get; set; }
        public string TipoParquadero { get; set; }
        public string Avaluo { get; set; }
        public string EstudioTitulos { get; set; }
    }
}
