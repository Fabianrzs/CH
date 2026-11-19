using System;

namespace GA2.Application.Dto
{
    public class BeneficiariosPagoEmbargoDto
    {

		public Guid Id { get; set; }
        public Guid EmbargoId { get; set; }
		public int TipoDocumentoId { get; set; }
		public string TipoDocumento { get; set; }
		public string NumeroIdentificacion { get; set; }
		public string PrimerNombre { get; set; }
		public string SegundoNombre { get; set; }
		public string PrimerApellido { get; set; }
		public string SegundoApellido { get; set; }
		public bool Demandante { get; set; }
		public bool? BPE_BENEFICIARIO { get; set; }
		public DateTime FechaRegistro { get; set; }
		public DateTime FechaActualizacion { get; set; }
	    public Guid Creadopor { get; set; }
		public Guid ActualizadoPor { get; set; }
		public bool Estado { get; set; }

		public bool insert { get; set; }

		// public string RadicadoWorkManager { get; set; }

		// public string TipoIdentificacion { get; set; }


	}
}
