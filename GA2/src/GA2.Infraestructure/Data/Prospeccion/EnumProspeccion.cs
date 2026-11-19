using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Prospeccion
{
    public class EnumProspeccion
    {
        public enum EnumConsultarPropeccion
        {
            [Description("@TIPO_DOCUMENTO")]
            tipoDocumento,
            [Description("@NUMERO_DOCUMENTO")]
            NumeroDocumento,
        }


        public enum EnumGuardarValoresSimulacion
        {
            [Description("@VSA_RIESGO_MINIMO")]
            RiesgoMinimo,
            [Description("@VSA_VALOR_A_FINANCIAR")]
            ValorFinanciar,
            [Description("@SMS_ID")]
            Sms_id,
            [Description("@NUMERO_DOCUMENTO")]
            NumeroDocumento,
            [Description("@VSA_FECHA")]
            FechaRegistro,
            [Description("@VALOR_SECTOR_FINANCIERO")]
            ValorSectorFinanciero,
            [Description("@VALOR_SECTOR_REAL")]
            ValorSectorReal,
            [Description("@FACTOR")]
            Factor
        }

        public enum EnumInsertarLineasCredito
        {
            [Description("@OLN_LINEA_CREDITO")]
            LineaCredito,
            [Description("@OLN_SALDO_OBLIGACION")]
            SaldoObligacion,
            [Description("@OLN_VALOR_CUOTA")]
            ValorCuota,
            [Description("@OLN_ESTADO")]
            EstadoLineaCredito,
            [Description("@OLN_CUOTAS_PACTADAS")]
            CuotasPactadas,
            [Description("@OLN_CUOTAS_CANCELADAS")]
            CuotasCanceladas,
            [Description("@OLN_CUOTAS_RESTANTES")]
            CuotasRestantes,
            [Description("@OLN_NUMERO_DOCUMENTO")]
            NumeroDocumento,
            [Description("@OLN_ACCION")]
            Accion,
            [Description("@OLN_CALIDAD")]
            Calidad,
            [Description("@OLN_NUMERO_OBLIGACION")]
            NumeroObligacion,
            [Description("@OLN_TIPO_OBLIGACION")]
            TipoObligacion
        }


        public enum EnumConsultarSimulacionAfiliado
        {
            [Description("@AFL_ID")]
            Afl_id,
            [Description("@NUMERO_DOCUMENTO")]
            NumeroDocumento
        }

        public enum EnumLineasCreditoRestaurar
        {
            [Description("@OLN_ID")]
            LlineaCreditoId,

            [Description("@OLN_ESTADO")]
            LineaEstado
        }

        public enum EnumPeritoDetalle 
        {
            [Description("@PER_OBSERVACION")]
            Observacion,

            [Description("@SOC_ID")]
            SolicitudCredito,

            [Description("@PER_ID")]
            PeritoId

        }

        public enum EnumDatosEndeudamientoProspeccion
        {
            [Description("@EDP_NUMERO_DOCUMENTO")]
            NumeroDocumento,

            [Description("@EDP_SALARIO_TOTAL")]
            SalarioTotal,

            [Description("@EDP_DEDUCCIONES")]
            Deducciones,

            [Description("@EDP_SALARIO_NETO")]
            SalarioNeto,

            [Description("@EDP_CAPACIDAD_PAGO")]
            CapacidadPago,

            [Description("@EDP_CANON_MENSUAL")]
            CanonMensual,

            [Description("@EDP_SECTOR_FINANCIERO")]
            SectorFinanciero,

            [Description("@EDP_SECTOR_REAL")]
            SectorReal,

            [Description("@EDP_SCORE")]
            Score,

            [Description("@EDP_DISPONIBLE_LIBRANZA")]
            DisponibleLibranza,

            [Description("@EDP_ENDEUDAMIENTO_GLOBAL_CUOTA_PORC")]
            EndeudamientoGlobalCuota,

            [Description("@EDP_CONTROL_NIVEL_ENDUEDAMIENTO_CUOTA_PORC")]
            ControlNivelEndeudamientoCuota,

            [Description("@EDP_ENDEUDAMIENTO_GLOBAL_SIN_CUOTA_PORC")]
            EndeudamientoGlobalSinCuota,

            [Description("@EDP_CONTROL_NIVEL_ENDUEDAMIENTO_SIN_CUOTA_PORC")]
            ControlNivelEndeudamientoSinCuota,

            [Description("@EDP_ENDUEDAMIENTO_ACTUAL_CUOTA_PORC")]
            PorcentajeEndeudamientoCuota,

            [Description("@EDP_ENDUEDAMIENTO_ACTUAL_SIN_CUOTA_PORC")]
            PorcentajeEndeudamientoSinCuota,

            [Description("@EDP_TOTAL_CANON_DEDUCCIONES")]
            TotalCanonDeducciones,

            [Description("@EDP_DECISION_HOMOLOGADA")]
            DecisionHomologada
        }

        public enum DecripcionSegurosSolicitudCredito
        {

            [Description("@SSC_CONVENIO_VIDA")]
            ConvenioSeguroVida,

            [Description("@SSC_SEGURO_VIDA")]
            SeguroVida,

            [Description("@SSC_CONVENIO_HOGAR")]
            ConvenioSeguroHogar,

            [Description("@SSC_SEGURO_HOGAR")]
            SeguroHogar,

            [Description("@SCC_NUMERO_DOCUMENTO")]
            NumeroDocumento,

            [Description("@SOC_ID")]
            SolicitudCreditoId,

        }

        public enum DecripcionTipoDeAfiliacion
        {

            [Description("@DESCRIPCION")]
            DescripcionAfiliacion,

            [Description("@TIPO")]
            TipoCredito   

        }

        public enum CongelamientoRecursos
        {
            [Description("@CGR_DESCRIPCION")]
            Descripcion,

            [Description("@CGR_VALOR")]
            Valor,

            [Description("@CGR_SIMULACION_ID")]
            SimulacionId,

            [Description("@CGR_ORDEN")]
            Orden,

            [Description("@CGR_NUMERO_DOCUMENTO")]
            NumeroDocumento,

            [Description("@CGR_TIPO_CREDITO")]
            TipoCredito

        }

        public enum ConsultarUsuarios
        {
            [Description("@USR_ID")]
            UsuarioID,

        }

        public enum EnumNotificacionesSolicitudes
        {

            [Description("@NOT_CDP")]
            CDP,

            [Description("@SLS_ID")]
            SimulacionId,

            [Description("@NOT_FECHA")]
            Fecha,

            [Description("@TRS_ID")]
            Trs_id,

            [Description("@SOC_ID")]
            Soc_Id
        }

        public enum EnumConceptosCongeladosProspeccion
        {

            [Description("@CON_DESCRIPCION")]
            Descripcion,

            [Description("@CON_VALOR")]
            Valor,

            [Description("@SLS_ID")]
            SimulacionId,

            [Description("@CON_NUMERO_DOCUMENTO")]
            NumeroDocumento,

        }

        public enum EnumTipoVinculacionCredito
        {

            [Description("@DTV_DESCRIPCION")]
            Descripcion,

            [Description("@DTV_TIPO_CREDITO")]
            TipoCredito
        }

        public enum EnumInsertarPrimerPago
        {

            [Description("@PPC_CDP")]
            Cdp,

            [Description("@SLS_ID")]
            SimulacionId
        }

        public enum ReporteCreditos
        {
            [Description("@FECHA_INICIO")]
            FECHA_INICIO,

            [Description("@FECHA_FINAL")]
            FECHA_FINAL,

            [Description("@CONSECUTIVO")]
            CONSECUTIVO,

            [Description("@TAC_ID")]
            TAC_ID,
            [Description("@NUMERO_DOCUMENTO")]
            NUMERO_DOCUMENTO,
            [Description("@ESTADO")]
            EstadoCredito,

            [Description("@USUARIO_ID")]
            UsuarioId
        }
    }
}
