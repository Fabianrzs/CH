using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Solicitud
{
    public class IntervinientesActorSolicitudDto
    {
        public string PersonaJuridicaNatural { get; set; }
        public int TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombre { get; set; }
        public string? SegundoNombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string Celular { get; set; }
        public string CorreoElectronico { get; set; }
        public int Departamento { get; set; }
        public int Ciudad { get; set; }
        public string Direccion { get; set; }
        public string Barrio { get; set; }
        public string Telefono { get; set; }
        public Guid TipoActor { get; set; }
        public Guid? SolId { get; set; }
        public string TipoCuenta { get; set; }
        public string NumeroCuentaBancaria { get; set; }
        public int EntidadBancaria { get; set; }
        public DateTime? FechaExpedicionCedula { get; set; }
        public string Constructora { get; set; }
        public string ConvenioEntidad { get; set; }
        public int NumeroDocumentoAfiliado {get; set;}
        public string TipoDocumentoAfiliado { get; set; }

    }
}
