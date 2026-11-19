using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.BUA
{
    public class AportesCategorium
    {
        public int cuotasAcumuladas { get; set; }
        public DateTime fechaPrimeraCuota { get; set; }
        public DateTime fechaUltimaCuota { get; set; }
        public int idCategoria { get; set; }
    }

    public class Categoria
    {
        public int idCategoria { get; set; }
    }

    public class CiudadNacimiento
    {
        public Departamento departamento { get; set; }
        public string idCiudad { get; set; }
    }

    public class CiudadResidencia
    {
        public Departamento departamento { get; set; }
        public string idCiudad { get; set; }
    }

    public class ConceptosSaldosBloqueo
    {
        public int cantidadIngresos { get; set; }
        public string concepto { get; set; }
        public int documento { get; set; }
        public string estadoBloqueo { get; set; }
        public DateTime fechaBloqueo { get; set; }
        public DateTime fechaUltimoAporteConcepto { get; set; }
        public int idConcepto { get; set; }
        public int idExpd { get; set; }
        public string nroExpd { get; set; }
        public string origen { get; set; }
        public double saldoConceptoCuenta { get; set; }
        public double saldoInicialAno { get; set; }
        public double valorAcumulado { get; set; }
        public double valorBloqueo { get; set; }
    }

    public class Conyuge
    {
        public string celularConyuge { get; set; }
        public string identificacionConyuge { get; set; }
        public string primerApellidoConyuge { get; set; }
        public string primerNombreConyuge { get; set; }
        public string segundoApellidoConyuge { get; set; }
        public string segundoNombreConyuge { get; set; }
        public string telefonoFijoConyuge { get; set; }
        public TipoIdConyuge tipoIdConyuge { get; set; }
    }

    public class Cuenta
    {
        public int causalEstadoCuenta { get; set; }
        public List<ConceptosSaldosBloqueo> conceptosSaldosBloqueos { get; set; }
        public int cuotas { get; set; }
        public Documento documento { get; set; }
        public int estadoCuenta { get; set; }
        public DateTime fechaCierre { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaPrimerAporte { get; set; }
        public int idCuenta { get; set; }
        public int idTipoProducto { get; set; }
        public List<Movimiento> movimientos { get; set; }
        public string numeroCuenta { get; set; }
        public double saldo { get; set; }
        public double saldoDisponible { get; set; }
        public int saldoEnCanje { get; set; }
    }

    public class Departamento
    {
        public string idDepartamento { get; set; }
        public Pais pais { get; set; }
    }

    public class Direccion
    {
        public string barrioResidencia { get; set; }
        public string celular { get; set; }
        public CiudadResidencia ciudadResidencia { get; set; }
        public string correoInstitucional { get; set; }
        public string correoPersonal { get; set; }
        public string direccionResidencia { get; set; }
        public DateTime fechaActualizacion { get; set; }
        public string telefono { get; set; }
        public int tipoDirCo { get; set; }
        public int tipoDirRes { get; set; }
    }

    public class Documento
    {
        public string alerta { get; set; }
        public int anulacion { get; set; }
        public DateTime fechaDecision { get; set; }
        public DateTime fechaInicial { get; set; }
        public int idCausalEstado { get; set; }
        public int idDocumento { get; set; }
        public int idEstadoDocumento { get; set; }
        public int idTipoDocumento { get; set; }
    }

    public class EstadoActualCli
    {
        public List<object> causalBloqueo { get; set; }
        public int idEstado { get; set; }
    }

    public class Fuerza
    {
        public int IdFuerza { get; set; }
    }

    public class Grado
    {
        public int IdGrado { get; set; }
        public Categoria categoria { get; set; }
        public Fuerza fuerza { get; set; }
    }

    public class InformacionFinanciera
    {
        public string conceptoIngresosAdicionales { get; set; }
        public int egresoMensual { get; set; }
        public int ingresoMensual { get; set; }
        public int ingresosAdicionales { get; set; }
        public int totalActivos { get; set; }
        public int totalPasivos { get; set; }
        public int totalPatrimonio { get; set; }
    }

    public class LugarExpedicion
    {
        public Departamento departamento { get; set; }
        public string idCiudad { get; set; }
    }

    public class Movimiento
    {
        public string anoPeriodo { get; set; }
        public Documento documento { get; set; }
        public DateTime fechaProceso { get; set; }
        public int idConcepto { get; set; }
        public int idMovimiento { get; set; }
        public string tipoMovimiento { get; set; }
        public int valor { get; set; }
    }

    public class Pais
    {
        public string idPais { get; set; }
    }

    public class Root
    {
        public List<AportesCategorium> AportesCategoria { get; set; }
        public List<Cuenta> Cuentas { get; set; }
        public List<object> Dependiente { get; set; }
        public string actividad { get; set; }
        public CiudadNacimiento ciudadNacimiento { get; set; }
        public Conyuge conyuge { get; set; }
        public DateTime derechoAdquirido { get; set; }
        public Direccion direccion { get; set; }
        public bool envioInformacion { get; set; }
        public EstadoActualCli estadoActualCli { get; set; }
        public string estadoCivil { get; set; }
        public DateTime fechaActualizaDatos { get; set; }
        public DateTime fechaAdquisicionVivienda { get; set; }
        public DateTime fechaAlta { get; set; }
        public DateTime fechaAltaCalculada { get; set; }
        public DateTime fechaExpedicionId { get; set; }
        public DateTime fechaInscripcion { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public Grado grado { get; set; }
        public int idPatrocinador { get; set; }
        public int idRegistro { get; set; }
        public string identificacion { get; set; }
        public InformacionFinanciera informacionFinanciera { get; set; }
        public LugarExpedicion lugarExpedicion { get; set; }
        public int modeloSubsidio { get; set; }
        public int participacion { get; set; }
        public int participacionSubsidio { get; set; }
        public string primerApellido { get; set; }
        public string primerNombre { get; set; }
        public string profesion { get; set; }
        public int regimen { get; set; }
        public string resolucion { get; set; }
        public string segundoApellido { get; set; }
        public string segundoNombre { get; set; }
        public string sexo { get; set; }
        public decimal tasaAporte { get; set; }
        public decimal tasaAporteVoluntario { get; set; }
        public int tasaSalarioBase { get; set; }
        public TipoActor tipoActor { get; set; }
        public TipoId tipoId { get; set; }
        public TipoViviendaPosee tipoViviendaPosee { get; set; }
        public string unidad { get; set; }
        public int unidadEjecutora { get; set; }
    }

    public class TipoActor
    {
        public int idTipoActor { get; set; }
    }

    public class TipoId
    {
        public int idTipoId { get; set; }
    }

    public class TipoIdConyuge
    {
        public int idTipoId { get; set; }
    }

    public class TipoViviendaPosee
    {
        public int tipoViviendaId { get; set; }
    }


}
