using GA2.Application.Dto.BUA;
using GA2.Application.Dto.Catalogs;
using GA2.Application.Dto.Solicitud;
using System;
using System.Collections.Generic;

namespace GA2.Application.Dto
{
    /// <summary>
    /// Dto Cliente
    /// <author>Nicolas Florez Sarrazola</author>
	/// <date>12/04/2021</date>
    /// </summary>
    public class ClienteDto
    {
        #region Cliente
       
        public int IdCliente { get; set; }
        public int? CLV_ID { get; set; }
        public List<AnotacionIntegracion> Anotaciones { get; set; }
        public int IdTipoActor { get; set; }
        public int IdTipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public DateTime FechaExpedicionDocumento { get; set; }
        public int IdPaisExpedicionDocumento { get; set; }
        public int IdDepartamentoExpedicionDocumento { get; set; }
        public int IdCiudadExpedicionDocumento { get; set; }
        public string LugarExpedicionExtranjero { get; set; }
        public string CodigoMilitar { get; set; }
        public string SituacionActual { get; set; }
        public string TipoVinculacion { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int IdPaisNacimiento { get; set; }
        public int IdDepartamentoNacimiento { get; set; }
        public int IdCiudadNacimiento { get; set; }
        public string LugarNacimientoExtranjero { get; set; }
        public string Sexo { get; set; }
        public string EstadoCivil { get; set; }
        public string IdEstadoCivil { get; set; }
        public int IdProfesion { get; set; }
        public string Unidad { get; set; }
        public int IdCategoria { get; set; }
        public string? NombreTipoIdentificacion { get; set; }
        public int IdFuerza { get; set; }
        public int IdGrado { get; set; }
        public int IdUnidadEjecutora { get; set; }
        public DateTime? FechaActualizaDatos { get; set; }
        public DateTime? FechaActualizaDatosFinancieros { get; set; }
        public DateTime? FechaActualizaDatosContacto { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaPrimeraCuota { get; set; }
        public DateTime FechaUltimaCuota { get; set; }
        public int Regimen { get; set; }
        public DateTime FechaAltaCalculada { get; set; }
        public DateTime FechaReintegro { get; set; }
        public string? EstadoCliente { get; set; }
        public int? IdEstadoCliente { get; set; }
        public string NumeroValidacion { get; set; }
        public int IdPatrocinador { get; set; }
        public decimal Participacion { get; set; }
        public decimal SaldoDisponible { get; set; }
        public string NumeroResolucion { get; set; }
        public bool ValidacionBiometrica { get; set; }
        public bool EsBeneficiario { get; set; }
        public int EstadoCargueNomina { get; set; }
        public int CuentasHabilitadas { get; set; }
        public decimal TasaAportes { get; set; }
        public decimal TasaVoluntariaAportes { get; set; }
        public decimal TasaSalarioBase { get; set; }
        public int NumeroCuotas { get; set; }
        public string Payload { get; set; }
        public decimal ValorSubsidio { get; set; }
        public int Actualiza { get; set; }
        public string Usuario { get; set; }
        #endregion

        #region Información económica
        public int IdInformacionEconomica { get; set; }
        public int IdActividadEconomica { get; set; }
        public DateTime FechaCorte { get; set; }
        public decimal IngresoMensual { get; set; }
        public decimal EgresoMensual { get; set; }
        public decimal TotalActivos { get; set; }
        public decimal TotalPasivos { get; set; }
        public decimal TotalPatrimonio { get; set; }
        public decimal IngresoAdicional { get; set; }
        public string ConceptoIngresosAdicionales { get; set; }
        public bool TransaccionesMonedaExtranjera { get; set; }
        public int IdMoneda { get; set; }
        public decimal MontoTransaccionesExtranjero { get; set; }
        #endregion

        #region Direccion
        public int IdDireccion { get; set; }
        public int ConsecutivoDireccion { get; set; }
        public int IdPaisResidencia { get; set; }
        public int IdDepartamentoResidencia { get; set; }
        public int IdCiudadResidencia { get; set; }
        public string CiudadResidenciaExtranjero { get; set; }
        public int IdTipoCalle { get; set; }
        public int NumeroCalle1 { get; set; }
        public int IdLetra1 { get; set; }
        public int IdBis1 { get; set; }
        public int IdCardinal1 { get; set; }
        public int NumeroCalle2 { get; set; }
        public int IdLetra2 { get; set; }
        public int IdBis2 { get; set; }
        public int IdCardinal2 { get; set; }
        public int NumeroCalle3 { get; set; }
        public int IdLetra3 { get; set; }
        public int IdCardinal3 { get; set; }
        public string ComplementoDireccion { get; set; }
        public string Direccion { get; set; }
        public string Barrio { get; set; }
        public int IdTipoDireccion { get; set; }
        public int IdSistemaActualiza { get; set; }
        #endregion

        #region Correos
        public int IdCorreo { get; set; }
        public string Correo { get; set; }
        public string Correo2 { get; set; }
        public int IdTipoCorreo { get; set; }
        public bool EnvioInformacionCorreo { get; set; }
        public bool CorreoActivo { get; set; }
        #endregion

        #region Telefonos
        public int IdTelefono { get; set; }
        public int IndicativoPaisTelefono { get; set; }
        public int IndicativoCiudadTelefono { get; set; }
        public string Telefono { get; set; }
        public string Telefono2 { get; set; }
        public int IdTipoTelefono { get; set; }
        public bool EnvioInformacionSMS { get; set; }
        public bool TelefonoActivo { get; set; }
        #endregion

        #region Conyuge
        public int IdConyuge { get; set; }
        public int IdTipoIdentificacionConyuge { get; set; }
        public string IdentificacionConyuge { get; set; }
        public string PrimerNombreConyuge { get; set; }
        public string SegundoNombreConyuge { get; set; }
        public string PrimerApellidoConyuge { get; set; }
        public string SegundoApellidoConyuge { get; set; }
        public bool ConyugeActivo { get; set; }
        #endregion

        public List<CuentaDto> Cuentas { get; set; }
        public List<SaldoDto> Saldos { get; set; }
        public List<AportesClienteDto> AportesCategoria { get; set; }
        public List<DependienteDto> Dependientes { get; set; }
        public List<SolicitudBUADto> Solicitudes { get; set; }
        public List<TipoActorDto> TiposVinculacion { get; set; }
        public List<EstadosClienteDto> EstadosCliente { get; set; }
        public List<EstadoCausalClienteDto> CausalesEstadoCliente { get; set; }
        public List<CausalEstadoClienteDto> EstadosCausaleslCliente { get; set; }
        public List<EmbargosDto> Embargos { get; set; }
        public List<CausalesBloqueoAfiliados> CausalesBloqueoAfiliado { get; set; }
        public List<EmbargosAfiliados> EmbargosAfiliado { get; set; }
        public List<TipoSolicitudDto> TiposSolicitud { get; set; }
        public ClienteDto() { }

    }


    public class ReporteAfiliadosResponseDto
    {
        public string IdTipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string Afiliado { get; set; }
        public string IdFuerza { get; set; }
        public string IdUnidadEjecutora { get; set; }
        public string IdCategoria { get; set; }
        public string IdGrado { get; set; }
        public string Regimen { get; set; }
        public string FechaInscripcion { get; set; }
        public string FechaActualizacion { get; set; }
        public string EstadoCivil { get; set; }
        public string Conyuge { get; set; }
        public string TipoVinculacion { get; set; }
        public string EstadoCliente { get; set; }

    }


    public class InfoClienteDto
    {

        public int ClienteId { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Identificacion { get; set; }
        public int TipoIdentificacionId { get; set; }
        public int CategoriaId { get; set; }
        public int FuerzaId { get; set; }
        public int GradoId { get; set; }
        public int UnidadEjecutoraId { get; set; }
        public string Categoria { get; set; }
        public string Fuerza { get; set; }
        public string Grado { get; set; }
        public string UnidadEjecutora { get; set; }
        public string SiglaUnidadEjecutora { get; set; }
        public string TipoIdentificacion { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public DateTime FechaAlta { get; set; }
        public int TipoAfiliacionId { get; set; }
        public string TipoAfiliacion { get; set; }
        public List<AportesClienteDto> Aportes { get; set; }
        public List<CuentaClienteDto> Cuentas { get; set; }
        public List<CausalEstadoCuentaDto> causalEstadoCuenta { get; set; }
        public List<TipoCuentaCDto> tipoCuentaC { get; set; }
        public List<Catalogs.EstadoCuentaDto> estadosCuenta { get; set; }
        public List<PorcentajeDescuentoDto> porcentajeDescuentos { get; set; }
        public List<TipoAfiliacionDto> modalidadesAfiliacion { get; set; }
        public decimal? ValorDevolverEmbargo { get; set; }


    }

    public class AfiliadoDto
    {
        public int IdCliente { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreCompletoInvertido { get; set; }
        public int TipoDocumento { get; set; }
        public string Documento { get; set; }
        public string Sexo { get; set; }
        public int Departamento { get; set; }
        public int Ciudad { get; set; }
        public int UnidadEjecutora { get; set; }

    }

    public class SolicitudBUADto
    {
        public int IdSolicitud { get; set; }
        public string DocumentoCausante { get; set; }
        public string Actividad { get; set; }
        public string NombreSolicitante { get; set; }
        public string NumeroSolicitud { get; set; }
        public string Radicado { get; set; }
        public string TipoSolicitud { get; set; }
        public string Modalidad { get; set; }
        public string EstadoSolicitud { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorPagado { get; set; }
        public DateTime FechaInicioSolicitud { get; set; }
        public DateTime FechaFinSolicitud { get; set; }
    }
}
