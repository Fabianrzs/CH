using System;
using System.ComponentModel.DataAnnotations;

namespace GA2.Application.Dto
{
    public class SimulacionClienteDto
    {
        [Key]
        public string numeroDocumento { get; set; }
        public string numeroPreaprobado { get; set; }
        public Guid smc_id { get; set; }
        public string smcTipoVivienda { get; set; }
        public decimal valorVivienda { get; set; }
        public decimal valorViviendaSeguro { get; set; }
        public decimal valorPrestamo { get; set; }
        public string tipoAlivio { get; set; }
        public string smcTipoAbono { get; set; }
        public decimal valorTasaEA { get; set; }
        public decimal valorTasaMV { get; set; }
        public decimal valorTasaMVFresh { get; set; }
        public string viviendaVIS { get; set; }
        public int valorCuota { get; set; }
        public int valorCuotaSinSeg { get; set; }
        public int plazo { get; set; }
        public DateTime fechaSimulacion { get; set; }
        public DateTime fechaAprobacion { get; set; }
        public string nombreCliente { get; set; }
        public string departamento { get; set; }
        public string ciudad { get; set; }
        public string direccion { get; set; }
        public string correoElectronico { get; set; }
        public string telefonoFijo { get; set; }
        public string telefonoCelular { get; set; }
        public string fuerza { get; set; }
        public string regimen { get; set; }
        public string categoria { get; set; }
        public string grado { get; set; }
        public int tipoCredito { get; set; }
        public double años { get; set; }
        public string tipoAbono { get; set; }
        public string tipoVivienda { get; set; }
        public int dpc_inmueble { get; set; }
        public int dpd_inmueble { get; set; }
        public int tivviviendaid  { get; set; }
        public decimal ValorCuotaMod { get; set; }
        public int UEJ_Id { get; set; }
        public decimal PorcentajeTasaFres { get; set; }

    }

    public class PreaprobadoClienteDto
    {
        public decimal SMD_CUOTA_TOTAL { get; set; }
        public string SMC_NUMERO_PREAPROBADO { get; set; }
        public string SMC_NUMERO_DOCUMENTO { get; set; }
        public decimal SMC_VALOR_CUOTA { get; set; }
        public decimal SMC_VALOR_PRESTAMO { get; set; }
        public int SMC_PLAZO { get; set; }
        public decimal VALOR_CUOTA_MOD { get; set; }
        public string NUMERO_RADICADO { get; set; }

        public DateTime FECHA_ACTUALIZA { get; set; }

        public String MODALIDAD { get; set; }

        public String PUNTOATENCION { get; set; }
        public string SLS_NUMERO_RADICADO_83 { get; set; }
        public string SLS_SIMULADOR_DESC { get; set; }
    }
}
