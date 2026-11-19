using System;

namespace GA2.Domain.Entities.Proyecto
{
    public class BeneficiarioResponse
    {
        public int Codigo { get; set; }
        public int TipoDocumentoId { get; set; }
        public string TipoDocumento { get; set; }
        public string NumerDocumento { get; set; }
        public int DigitoVerificacion { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public int TipoCuentaId { get; set; }
        public string TipoCuenta { get; set; }
        public string TipoEntidaId { get; set; }
        public string Banco { get; set; }
        public string NumeroCuenta { get; set; }
        public string ActividadCuenta { get; set; }
        public Guid? ProyectoID { get; set; }
    }
}
