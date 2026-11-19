using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class Pais
    {
        public string IdPais { get; set; }
        public int IdPaisEntero { get; set; }
        public Pais() { }
    }

    public class Departamento
    {
        public string IdDepartamento { get; set; }
        public int IdDepartamentoEntero { get; set; }
        public Pais Pais { get; set; }
        public Departamento() 
        {
            Pais = new Pais();
        }
    }

    public class CiudadNacimiento
    {
        public Departamento Departamento { get; set; }
        public string IdCiudad { get; set; }
        public int IdCiudadEntero { get; set; }
        public CiudadNacimiento() 
        {
            Departamento = new Departamento();
        }

    }

    public class TipoIdConyuge
    {
        public int IdTipoId { get; set; }
    }

    public class Conyuge
    {
        public string CelularConyuge { get; set; }
        public string IdentificacionConyuge { get; set; }
        public string PrimerApellidoConyuge { get; set; }
        public string PrimerNombreConyuge { get; set; }
        public string SegundoApellidoConyuge { get; set; }
        public string SegundoNombreConyuge { get; set; }
        public string TelefonoFijoConyuge { get; set; }
        public TipoIdConyuge TipoIdConyuge { get; set; }
    }

    public class CiudadResidencia
    {
        public Departamento Departamento { get; set; }
        public string IdCiudad { get; set; }
        public int IdCiudadEntero { get; set; }
        public CiudadResidencia() 
        {
            Departamento = new Departamento();
        }
    }

    public class Direccion
    {
        public string BarrioResidencia { get; set; }
        public string Celular { get; set; }
        public CiudadResidencia CiudadResidencia { get; set; }
        public string CorreoInstitucional { get; set; }
        public string CorreoPersonal { get; set; }
        public string DireccionResidencia { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string Telefono { get; set; }
        public int TipoDirCo { get; set; }
        public int TipoDirRes { get; set; }
        public string TelefonoMostrar { get; set; }
        public int TipoTelefono { get; set; }
        public string Correo { get; set; }
        public int TipoCorreo { get; set; }
        public Direccion() 
        {
            CiudadResidencia = new CiudadResidencia();
        }
    }

    public class EstadoActualCli
    {
        public List<object> CausalBloqueo { get; set; }
        public int IdEstado { get; set; }
    }

    public class Categoria
    {
        public int IdCategoria { get; set; }
    }

    public class Fuerza
    {
        public int IdFuerza { get; set; }
    }

    public class Grado
    {
        public int IdGrado { get; set; }
        public Categoria Categoria { get; set; }
        public Fuerza Fuerza { get; set; }
    }

    public class InformacionFinanciera
    {
        public string ConceptoIngresosAdicionales { get; set; }
        public int EgresoMensual { get; set; }
        public int IngresoMensual { get; set; }
        public int IngresosAdicionales { get; set; }
        public int TotalActivos { get; set; }
        public int TotalPasivos { get; set; }
        public int TotalPatrimonio { get; set; }
    }

    public class LugarExpedicion
    {
        public Departamento Departamento { get; set; }
        public string IdCiudad { get; set; }
        public int IdCiudadEntero { get; set; }
        public LugarExpedicion() 
        {
            Departamento = new Departamento();
        }
    }

    public class TipoActorIntegracion
    {
        public int IdTipoActor { get; set; }
    }

    public class TipoId
    {
        public int IdTipoId { get; set; }
    }

    public class TipoViviendaPosee
    {
        public int TipoViviendaId { get; set; }
    }

    public class CuentaIntegracion
    {
        public int IdCuenta { get; set; }
        public string NumeroCuenta { get; set; }
        public int IdCliente { get; set; }
        public int IdTipoProducto { get; set; }
        public int EstadoCuenta { get; set; }
        public int CausalEstadoCuenta { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaCierre { get; set; }
        public DateTime FechaPrimerAporte { get; set; }
        public decimal Saldo { get; set; }
        public decimal SaldoEnCanje { get; set; }
        public decimal SaldoDisponible { get; set; }
        public int Cuotas { get; set; }
        //public List<DocumentoIntegracion> Documento { get; set; }
        public List<Movimiento> Movimientos { get; set; }
        public List<ConceptosSaldosBloqueos> ConceptosSaldosBloqueos { get; set; }
    }

    public class ConceptosSaldosBloqueos
    {
        public int CantidadIngresos { get; set; }
        public string Concepto { get; set; }
        public string Razon { get; set; }
        public string Demandante { get; set; }
        public int Documento { get; set; }
        public string EstadoBloqueo { get; set; }
        public DateTime FechaBloqueo { get; set; }
        public DateTime FechaUltimoAporteConcepto { get; set; }
        public int IdConcepto { get; set; }
        public int IdExpd { get; set; }
        public string Tipo_Cuenta { get; set; }
        public string Solicitud { get; set; }
        public string NroExpd { get; set; }
        public string Origen { get; set; }
        public decimal SaldoConceptoCuenta { get; set; }
        public decimal SaldoInicialAno { get; set; }
        public decimal ValorAcumulado { get; set; }
        public decimal ValorBloqueo { get; set; }
    }

    public class DocumentoIntegracion
    {
        public int IdDocumento { get; set; }
        public int IdTipoDocumento { get; set; }
        public DateTime FechaInicial { get; set; }
        public int IdEstadoDocumento { get; set; }
        public int IdCausalEstado { get; set; }
        public DateTime FechaDecision { get; set; }
        public int Anulacion { get; set; }
        public string Alerta { get; set; }
    }

    public class Movimiento
    {
        public int IdMovimiento { get; set; }
        public int IdConcepto { get; set; }
        public string Valor { get; set; }
        public string TipoMovimiento { get; set; }
        public DateTime FechaProceso { get; set; }
        public string AnoPeriodo { get; set; }
        public DocumentoIntegracion Documento { get; set; }
    }

    public class AporteCategoria
    {
        public int IdCategoria { get; set; }
        public int CuotasAcumuladas { get; set; }
        public DateTime FechaUltimaCuota { get; set; }
        public DateTime FechaPrimeraCuota { get; set; }
    }

    public class DependienteIntegracion
    {
        public int TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string NombreDependiente { get; set; }
        public decimal ParticDependiente { get; set; }
    }

    public class CausalesBloqueoAfiliados
    {
        public string CAUSAL { get; set; }
        public int? CEC_ID { get; set; }
        public int? CEL_ID { get; set; }
        public bool COLUMNA_SELECCION { get; set; }
        public int? ECL_ID { get; set; }
        public string ESTADO { get; set; }
        public string ESTADO_BLOQUEO { get; set; }
        public DateTime? FECHA_REGISTRO { get; set; }
        public DateTime? FECHA_RETIRO { get; set; }
        public string OBSERVACIONES { get; set; }
    }

    public class EmbargosAfiliados
    {
        public string CODIGO_JUZGADO { get; set; }
        public int? CTA_ID { get; set; }
        public string CUENTA { get; set; }
        public string ESTADO_BLOQUEO { get; set; }
        public string ESTADO_SOLICITUD { get; set; }
        public decimal? MONTOPORCENTAJE { get; set; }
        public string NOMBRE_DEMANDANTE { get; set; }
        public string RADICADO { get; set; }
        public string RAZON { get; set; }
        public int? SLC_ID { get; set; }
        public int? SOLICITUD { get; set; }
    }

    public class SolicitudIntegracion
    {
        public string BENEFICIARIO { get; set; }
        public string CDP { get; set; }
        public int CLI_ID { get; set; }
        public int COD_CONCEPTO_APLICA { get; set; }
        public int COD_ESTADO_SOLICITUD { get; set; }
        public int COD_MODALIDAD_SOLICITUD { get; set; }
        public int COP_TIPO_DOCUMENTO { get; set; }
        public int COP_TIPO_SOLICITUD { get; set; }
        public int COP_USUARIO_APROBADOR { get; set; }
        public int COP_USUARIO_CREA { get; set; }
        public string CONCEPTO_APLICA { get; set; }
        public string DOCUMENTO_CAUSANTE { get; set; }
        public int DOCUMENTO_ID { get; set; }
        public string ESTADO_DOCUMENTO { get; set; }
        public string ESTADO_SOLICITUD { get; set; }
        public string FACTURA_PAGO { get; set; }
        public DateTime FECHA_FINAL_SOLICITUD { get; set; }
        public DateTime FECHA_INICIAL_DOCUMENTO { get; set; }
        public DateTime FECHA_INICIO_SOLICITUD { get; set; }
        public string IDENTIFICACION_BENEFICIARIO { get; set; }
        public decimal INTERES { get; set; }
        public string MODALIDAD_SOLICITUD { get; set; }
        public decimal MONTO { get; set; }
        public decimal MONTO_TOTAL_PAGO { get; set; }
        public int NRO_CUENTA { get; set; }
        public int NRO_SOLICITUD { get; set; }
        public string OBSERVACION_SOLICITUD { get; set; }
        public string RADICADO { get; set; }
        public string SOLICITANTE { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string TIPO_SOLICITUD { get; set; }
        public string USUARIO_APROBADOR { get; set; }
        public string TRA_NOMBRE { get; set; }
        public string USUARIO_CREA { get; set; }
        public decimal VALOR_DISPONIBLE { get; set; }
        public decimal VALOR_TOTAL_PAGADO_BENEFICIARIO { get; set; }
    }

    public class ClienteIntegracionGA2DTO
    {
        public string Actividad { get; set; }
        public IEnumerable<AnotacionIntegracion> Anotaciones { get; set; }
        public CiudadNacimiento CiudadNacimiento { get; set; }
        public Conyuge Conyuge { get; set; }
        public DateTime DerechoAdquirido { get; set; }
        public Direccion Direccion { get; set; }
        public bool EnvioInformacion { get; set; }
        public EstadoActualCli EstadoActualCli { get; set; }
        public string EstadoCivil { get; set; }
        public DateTime FechaActualizaDatos { get; set; }
        public DateTime FechaAdquisicionVivienda { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaAltaCalculada { get; set; }
        public DateTime FechaExpedicionId { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaActualizaDatosContacto { get; set; }
        public Grado Grado { get; set; }
        public int IdPatrocinador { get; set; }
        public int IdRegistro { get; set; }
        public string Identificacion { get; set; }
        public InformacionFinanciera InformacionFinanciera { get; set; }
        public LugarExpedicion LugarExpedicion { get; set; }
        public int ModeloSubsidio { get; set; }
        public int Participacion { get; set; }
        public int ParticipacionSubsidio { get; set; }
        public string PrimerApellido { get; set; }
        public string PrimerNombre { get; set; }
        public string Profesion { get; set; }
        public int Regimen { get; set; }
        public string Resolucion { get; set; }
        public string SegundoApellido { get; set; }
        public object SegundoNombre { get; set; }
        public string Sexo { get; set; }
        public decimal TasaAporte { get; set; }
        public decimal TasaAporteVoluntario { get; set; }
        public int TasaSalarioBase { get; set; }
        public TipoActorIntegracion TipoActor { get; set; }
        public TipoId TipoId { get; set; }
        public TipoViviendaPosee TipoViviendaPosee { get; set; }
        public string Unidad { get; set; }
        public int UnidadEjecutora { get; set; }
        public string Payload { get; set; }
        public List<CuentaIntegracion> Cuentas { get; set; }
        public List<AporteCategoria> AportesCategoria { get; set; }
        public List<DependienteIntegracion> Dependiente { get; set; }
        public List<SolicitudIntegracion> Solicitudes { get; set; }
        public List<CausalesBloqueoAfiliados> CausalesBloqueoAfiliado { get; set; }
        public List<EmbargosAfiliados> EmbargosAfiliado { get; set; }
        public ClienteIntegracionGA2DTO()
        {
            CiudadNacimiento = new CiudadNacimiento();
            Conyuge = new Conyuge();
            Direccion = new Direccion();
            EstadoActualCli = new EstadoActualCli();
            Grado = new Grado();
            InformacionFinanciera = new InformacionFinanciera();
            LugarExpedicion = new LugarExpedicion();
            TipoActor = new TipoActorIntegracion();
            TipoId = new TipoId();
            TipoViviendaPosee = new TipoViviendaPosee();
            Cuentas = new List<CuentaIntegracion>();
            AportesCategoria = new List<AporteCategoria>();
            Dependiente = new List<DependienteIntegracion>();
            Solicitudes = new List<SolicitudIntegracion>();
            CausalesBloqueoAfiliado = new List<CausalesBloqueoAfiliados>();
            EmbargosAfiliado = new List<EmbargosAfiliados>();
        }
    }

    public class AnotacionIntegracion
    {
        public string AFILIADO { get; set; }
        public string ANOTACION { get; set; }
        public int CLI_ID { get; set; }
        public string COD_USUARIO { get; set; }
        public DateTime FECHA_ANOTACION { get; set; }
        public string IDENTIFICACION { get; set; }
        public string USUARIO { get; set; }
        public int ANOTACION_ID { get; set; }
    }

    public class ObtenerBuaResponse
    {
        public ClienteIntegracionGA2DTO ObtenerBUAResult { get; set; }
        public ObtenerBuaResponse()
        {
            ObtenerBUAResult = new ClienteIntegracionGA2DTO();
        }
    }
}
