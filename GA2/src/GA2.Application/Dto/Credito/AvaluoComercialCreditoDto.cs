using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito
{
    public class AvaluoComercialCreditoDto
    {
        public Guid SocId { get; set; }

        public ResumenEjecutivoDto ResumenEjecutivo { get; set; } = new();

        public AntecedentesDto Antecedentes { get; set; } = new();

        public AspectosJuridicosDto AspectosJuridicos { get; set; } = new();

        public ObservacionesGeneralesDto ObservacionesGenerales { get; set; } = new();

        public DependenciasDto Dependencias { get; set; } = new();

        public ConstruccionDto Construccion { get; set; } = new();

        public CuadroValoracionDto CuadroValoracion { get; set; } = new();
    }

    public class ResumenEjecutivoDto
    {
        public string Cliente { get; set; }

        public string NumeroDocumento { get; set; }

        public string Direccion { get; set; }

        public string Sector { get; set; }

        public string Barrio { get; set; }

        public int? DepartamentoId { get; set; }

        public int? CiudadId { get; set; }
    }

    public class AntecedentesDto
    {
        public DateTime? FechaInforme { get; set; }

        public string Vetustez { get; set; }

        public DateTime? FechaPagoAvaluo { get; set; }

        public DateTime? FechaVisita { get; set; }
    }

    public class AspectosJuridicosDto
    {
        public string Propietario { get; set; }

        public string Escritura { get; set; }

        public string Notaria { get; set; }

        public DateTime? FechaEscritura { get; set; }

        public int? DepartamentoEscrituraId { get; set; }

        public int? CiudadEscrituraId { get; set; }

        public string CedulaCatastral { get; set; }

        public string LicenciaConstruccion { get; set; }

        public string NombreConjunto { get; set; }

        public string CoeficienteCopropiedad { get; set; }
    }

    public class ObservacionesGeneralesDto
    {
        public string DistribucionInmueble { get; set; }

        public string CaracteristicasAdicionales { get; set; }
    }

    public class DependenciasDto
    {
        public string Descripcion { get; set; }

        public string Garaje { get; set; }

        public string DescripcionGaraje { get; set; }
    }

    public class ConstruccionDto
    {
        public string SalaAreaConstruida { get; set; }

        public string AreaPrivada { get; set; }
    }

    public class CuadroValoracionDto
    {
        public string Inmueble { get; set; }
    }
}
