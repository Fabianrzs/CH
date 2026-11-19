using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Connected_Services.ServiceSeven
{
    public static class CuentaFiscalNifMapper
    {
        /// <summary>
        /// Diccionario de mapeo: CUENTA_CONTABLE_SEVEN → CUENTAS_NIF
        /// Ambas cuentas tienen 8 dígitos
        /// Solo se incluyen las cuentas que tienen equivalencia (CUENTAS_NIF != NULL)
        /// </summary>
        private static readonly Dictionary<string, string> MapeoLocalANif = new Dictionary<string, string>
        {
            // LEASING - Cuentas por cobrar y seguros
            { "1636050401", "1636050201" },  // CXCobrar seguro todo riesgo
            { "1636050402", "1636050202" },  // CXCobrar seguro de vida
            { "1636050302", "1636050302" },  // CxCobrar Seguro Hogar Crédito Hipotecario
            { "1636050301", "1636050301" },  // CxCobrar Seguro Vida Crédito Hipotecario
            { "1636050201", "1636050201" },  // CXCobrar seguro de vida (variante)
            
            // LEASING - Provisiones
            { "51701103", "51701003" },  // Provisión Intereses Leasing (remanentes)
            { "51701102", "51701003" },  // Provisión Intereses Leasing (variante)
            { "51701101", "51701001" },  // Provisión Cartera Leasing
            
            // HIPOTECARIO - Provisiones
            { "5170100203", "5170050203" },  // Provisión Intereses Categoría A Riesgo Normal
            { "5170100202", "5170050202" },  // Crédito Hipotecario Acuerdo 02/2021
            { "5170100201", "5170050201" },  // Provisión General Crédito Hipotecario
            
            // HIPOTECARIO - Cuentas por cobrar
            { "16050602", "16051002" },  // Categoría A Riesgo Normal Crédito Hipotecario
            { "16970502", "16970502" },  // Provisión Intereses Crédito Hipotecario (mismo)
            { "16975202", "16970502" },  // Provisión Intereses Crédito Hipotecario
            
            // LEASING - Intereses categorías
            { "16080202", "16081002" },  // Categoría A intereses remanentes
            { "16080201", "16081001" },  // ⚠️ Categoría A intereses reducidos (TU CUENTA PROBLEMÁTICA)
            { "16948602", "16975502" },  // Provisión Categoría A Comp. Finan. Leasing
            { "16948601", "16975501" },  // Provisión Categoría A Componente Financiero Leasing
            
            // HIPOTECARIO - Provisión cuenta contable
            { "14890502", "14890503" },  // Crédito Hipotecario Acuerdo 02/2021
            { "14980502", "14980503" },  // Provisión General Crédito Hipotecario
            
            // LEASING - Provisión general
            { "14981001", "14980502" },  // Provisión General Cartera Leasing Habitacional
            { "14950901", "14891001" },  // Provisión Categoría A Leasing Habitacional
            
            // LEASING - Ingresos
            { "41962601", "41964001" },  // Ingreso: intereses leasing habitacional
            
            // HIPOTECARIO - Ingresos
            { "41020802", "41021002" },  // Intereses y rendimientos Crédito Hipotecario
            
            // LEASING - Créditos aprobados
            { "62200005", "62200001" },  // Créditos Leasing Aprobados 
            { "61200501", "61050001" },  // Créditos Leasing Aprobados 
            
            // HIPOTECARIO - Créditos aprobados
            { "62200006", "62200002" },  // Créditos Hipotecarios Aprobados 
            { "61200502", "61050003" },  // Créditos Hipotecarios Aprobados 
            
            // HIPOTECARIO - Capital
            { "14040504", "14040503" },  // Créditos Hipotecarios 
            { "14598901", "14041001" },  // Capital: Categoría A leasing habitacional
            
            // Recaudos
            { "25957008", "25909524" },  // Recaudo cuota Crédito Hipotecario 
            { "25957005", "25909522" },  // Recaudos Leasing Habitacional
            { "25959519", "25909515" },  // Dev. Recursos Afiliado - Liquidación Contrato
            { "25957004", "25909525" },  // Leasing seguro vida deudor
            { "25957010", "25909528" },  // Crédito Hipotecario Seguro Hogar
            { "25957009", "25909527" },  // Crédito Hipotecario Seguro Vida
            { "25957003", "25909526" },  // Leasing seguro todo hogar
            
            // Garantías e inmuebles
            { "82131502", "82131502" },  // Garantías Leasing (mismo)
            { "82131504", "82131504" },  // Inmuebles Crédito Hipotecario
            { "82131503", "82131503" },  // Actualización Valor Inmuebles Leasing (mismo)
            { "82131505", "82131505" },  // Actualización Valor Inmuebles Crédito Hipotecario (mismo)
            { "84050303", "84050505" },  // Garantías Leasing (Co)
            { "84050302", "84050005" },  // Capital desembolso crédito (co)
            { "84050304", "84050506" },  // Actualización Valor Inmuebles Leasing (co)
            { "84050306", "84050507" },  // Inmuebles Crédito Hipotecario (Co)
            { "84050307", "84050508" },  // Actualización Valor Inmuebles Crédito Hipotecario
            { "84050305", "84050007" },  // Crédito Hipotecario (capital)
            
            // Capital
            { "82810202", "82810202" },  // Crédito Hipotecario (capital) (mismo)
            { "82810201", "82810201" },  // Capital desembolso crédito (mismo)
            
            // Otros
            { "1687950301", "1687950301" },  // Constitución por CXC por opción anticipada (mismo)
            { "25959541", "25909519" },
            { "25959509", "25909503" },
        };

        /// <summary>
        /// Convierte una cuenta del plan LOCAL a su equivalente NIF
        /// </summary>
        /// <param name="cuentaLocal">Cuenta del plan local/actual (8 dígitos)</param>
        /// <returns>Cuenta del plan NIF equivalente</returns>
        /// <exception cref="InvalidOperationException">Si la cuenta no tiene mapeo</exception>
        public static string ConvertirLocalANif(string cuentaLocal)
        {
            if (string.IsNullOrWhiteSpace(cuentaLocal))
                throw new ArgumentException("La cuenta no puede ser vacía", nameof(cuentaLocal));

            if (MapeoLocalANif.TryGetValue(cuentaLocal, out var cuentaNif))
            {
                return cuentaNif;
            }

            throw new InvalidOperationException(
                $"No existe mapeo NIF para la cuenta '{cuentaLocal}'. " +
                $"Verifica la tabla CTA_EQUIVALENCIA o consulta con el equipo contable."
            );
        }

        /// <summary>
        /// Intenta convertir una cuenta local a NIF, retorna null si no existe
        /// </summary>
        public static string TryConvertirLocalANif(string cuentaLocal)
        {
            if (string.IsNullOrWhiteSpace(cuentaLocal))
                return null;

            return MapeoLocalANif.TryGetValue(cuentaLocal, out var cuentaNif) ? cuentaNif : null;
        }

        /// <summary>
        /// Verifica si una cuenta tiene mapeo a NIF
        /// </summary>
        public static bool TieneMapeoCuentaNif(string cuentaLocal)
        {
            return !string.IsNullOrWhiteSpace(cuentaLocal) && MapeoLocalANif.ContainsKey(cuentaLocal);
        }

        /// <summary>
        /// Valida que una cuenta sea válida (8 dígitos numéricos)
        /// </summary>
        public static bool ValidarCuenta(string cuenta, out string mensajeError)
        {
            mensajeError = null;

            if (string.IsNullOrWhiteSpace(cuenta))
            {
                mensajeError = "La cuenta no puede ser vacía";
                return false;
            }

            if (!cuenta.All(char.IsDigit))
            {
                mensajeError = $"La cuenta '{cuenta}' debe contener solo dígitos";
                return false;
            }

            if (cuenta.Length != 8)
            {
                mensajeError = $"La cuenta '{cuenta}' debe tener 8 dígitos";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Obtiene todas las cuentas locales mapeadas
        /// </summary>
        public static IEnumerable<string> ObtenerCuentasLocalesMapeadas()
        {
            return MapeoLocalANif.Keys;
        }

        /// <summary>
        /// Obtiene todas las cuentas NIF disponibles
        /// </summary>
        public static IEnumerable<string> ObtenerCuentasNifDisponibles()
        {
            return MapeoLocalANif.Values.Distinct();
        }

        /// <summary>
        /// Obtiene estadísticas del mapeo
        /// </summary>
        public static string ObtenerEstadisticas()
        {
            var cuentasNifUnicas = MapeoLocalANif.Values.Distinct().Count();

            return $"📊 Estadísticas de mapeo:\n" +
                   $"  - Cuentas LOCAL mapeadas: {MapeoLocalANif.Count}\n" +
                   $"  - Cuentas NIF únicas: {cuentasNifUnicas}";
        }

        /// <summary>
        /// Obtiene el diccionario completo de mapeos (para diagnóstico)
        /// </summary>
        public static Dictionary<string, string> ObtenerTodosMapeos()
        {
            return new Dictionary<string, string>(MapeoLocalANif);
        }
    }
}

