using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class AfiliacionDto
    {

        public int idCliente { get; set; }
        public int idAfiliacion { get; set; }
        public int idTipoAfiliacionExtraordinaria { get; set; }
        public string numeroRadicado { get; set; }
        public DateTime fechaRadicado { get; set; }
        public DateTime fechaElaboracion { get; set; }
        public int campoCalidad { get; set; }
        public int IdTipoAfiliacionProcedente { get; set; }
        public bool tramiteAfiliacion { get; set; }
        public bool conservaAntiguedad { get; set; }
        public int ratificacionCuotas { get; set; }
        public string campoObservacionesJustificacion { get; set; }
        public string numeroActoAdministrativo { get; set; }
        public DateTime fechaNotificacion { get; set; }
        public bool campoCesantias { get; set; }
        public bool activacionDescuento { get; set; }
        public string campoObservacionesConceptualizacion { get; set; }
        public int idPorcentajeDescuento { get; set; }
        public int idModalidadAfiliacionVeterano { get; set; }
        public bool ahorroConsignacionBancaria { get; set; }
        public bool solicitudTrasladoCesantias { get; set; }
        public int idCiudad { get; set; }
        public int idDepartamento { get; set; }
        public int oficioSolicitudCesantias { get; set; }
        public int oficioSolicitudTrasladoCesantias { get; set; }
        public DateTime mesDeInicioActivacionDescuento { get; set; }
        public DateTime mesDeFinActivacionDescuento { get; set; }
        public int ahorroConsignacionBancariaOpcion { get; set; }

        //informacion cliente

        public int tipoIdentificacion { get; set; }
        public string cedula { get; set; }
        public string primerNombre { get; set; }
        public string segundoNombre { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public int idCategoria { get; set; }
        public int idFuerza { get; set; }
        public int idUnidadEjecutora { get; set; }
        public int idGrado { get; set; }
        public DateTime fechaAlta { get; set; }
        public decimal asignacionBasicaMensual { get; set; }
        public string regimen { get; set; }
        public List<BeneficiariosAfiliacionesDto> Beneficiarios { get; set; }
        public List<AportesCategoriaAfiliacionesDto> Aportes { get; set; }
        public List<TramiteDesafiliacionDto> TramitesDesafiliacion { get; set; }

    }

    public class InsertarAfiliacionGestorDocumental
    {
        public string NumeroRadicado { get; set; }

        public int TipoAfiliacion { get; set; }

        public int TipoDocumento { get; set; }

        public int NumeroDocumento { get; set; }
    }
    public class InsertarAfiliacionGestorDocumentalDto
    {
        public string NumeroRadicado { get; set; }

        public int TipoAfiliacion { get; set; }

        public int TipoDocumento { get; set; }

        public int NumeroDocumento { get; set; }
    }

}
