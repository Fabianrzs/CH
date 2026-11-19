using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class SolicCreditoReferernciasDto
    {

        public Guid Id { get; set; }
        public Guid SolicitudId { get; set; }
        public string TipoReferencia { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Nombres { get; set; }
        public string Parentesco { get; set; }
        public string Relacion { get; set; }
        public string Direccion { get; set; }
        public int DepartamentoId { get; set; }
        public int Ciudadid { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public Guid CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Guid ActualizadoPor { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string referencia { get; set; }


    }

    public class SolicCreditoFinancierosDto {

        public Guid Id { get; set; }
        public Guid SolicitudId{ get; set; }
        public string DireccionVivienda { get; set; }
        public string MatriculaVivienda { get; set; }
        public decimal ValorComercialVivienda { get; set; }
        public string MarcaVehiculo { get; set; }
        public string PlacaVehiculo { get; set; }
        public decimal ValorComercial { get; set; }
        public Guid ActualizadoPor { get; set; }
        public DateTime FechaActualizado { get; set; }

    }

    public class SolicitudCreditoReferernciasDto { 
    
     public SolicCreditoFinancierosDto activos { get; set; }

     public List<SolicCreditoReferernciasDto> referencias { get; set; }


    }
}
