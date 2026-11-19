using GA2.Application.Dto.Solicitud;
using System;
using System.Collections.Generic;

namespace GA2.Application.Dto
{
    public class CrearSolicitudTareaDto
    {
        public CrearSolicitudTareaDto() {
            valorRetirar = 0;
        }

        public Guid SolicitudId { get; set; }
        public Guid solicitudTareaId { get; set; }
        public Guid CreadoPor { get; set; }
        public decimal valorRetirar { get; set; }
        public string observacion { get; set; }
        public bool? Posee_Avac { get; set; }
        public int Numero_Folio { get; set; }
        public DateTime? Fecha_Firma_Escritura { get; set; }
        public string Identificacion { get; set; }
        public int TipoIdentificacion { get; set; }
        public bool Opcion { get; set; }
        //public ConyugueDto? Conyugue { get; set; }
        public List<SolicitudMatriculaDto> SolicitudMatriculaDto { get; set; }
        public List<SolicitudPropietarioDto> SolicitudPropietarioDto { get; set; }
        public List<SolicitudTerceroApoderadoDto> SolicitudTerceroApoderadoDto { get; set; }
        public List<SolicitudBeneficiarioDto> SolicitudBeneficiarioDto { get; set; }
        public InformacionAdjudicacionDto InformacionAdjudicacionDto { get; set; }
        public InformacionEscrituracionDto InformacionEscrituracionDto { get; set; }
        public List<SolicitudTerceroBeneficiarioDto> SolicitudTerceroBeneficiarioDto { get; set; }
        public List<SolicitudTerceroVendedorDto> SolicitudTerceroVendedorDto { get; set; }
        public SolicitudTerceroConstructorDto SolicitudTerceroConstructorDto { get; set; }
        public SolicitudTerceroAutorizadoDto SolicitudTerceroAutorizadoDto { get; set; }
        public List<SolicitudTerceroEntidadEducativaDto> SolicitudTerceroEntidadEducativaDto { get; set; }
        public List<SolicitudTerceroBeneficiarioEstudioDto> SolicitudTerceroBeneficiarioEstudioDto { get; set; }
        public List<SolicitudTerceroBeneficiarioEstudioEntidadEducativaDto> SolicitudTerceroBeneficiarioEstudioEntidadEducativaDto { get; set; }
        public SolicitudTerceroEntidadSeguroEducativoDto SolicitudTerceroEntidadSeguroEducativoDto { get; set; }
        public SolicitudTerceroTenedorAccionesDto SolicitudTerceroTenedorAccionesDto { get; set; }
        public InformacionTurnoDto InformacionTurnoDto { get; set; }

        public string? CumpleRequisito { get; set; }
        public string? Causal { get; set; }
        public string? AccionRealizar { get; set; }
        public string? Acuerdo { get; set; }
        public MisCuentasDto? SolicitudMiscuentasDto { get; set; }
    }
}
