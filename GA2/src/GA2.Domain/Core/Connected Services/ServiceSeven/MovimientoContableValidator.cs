using ServiceSeven;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GA2.Domain.Core.Connected_Services.ServiceSeven
{

    /// <summary>
    /// Validador de movimientos contables antes de enviar a SEVEN ERP
    /// Verifica estructura, campos requeridos, y reglas de negocio
    /// </summary>
    public static class MovimientoContableValidator
    {
        /// <summary>
        /// Valida un movimiento contable completo
        /// </summary>
        /// <param name="movimiento">Movimiento a validar</param>
        /// <returns>Lista de errores encontrados (vacía si todo está OK)</returns>
        public static List<string> ValidarMovimiento(TSCnMcont movimiento)
        {
            var errores = new List<string>();

            if (movimiento == null)
            {
                errores.Add("El movimiento no puede ser null");
                return errores;
            }

            // Validar encabezado
            ValidarEncabezado(movimiento, errores);

            // Validar detalle
            if (movimiento.vDetalle == null || movimiento.vDetalle.Length == 0)
            {
                errores.Add("El movimiento debe tener al menos una línea de detalle (vDetalle)");
                return errores;
            }

            ValidarDetalle(movimiento.vDetalle, errores);

            // Validar que cuadre
            ValidarCuadre(movimiento.vDetalle, errores);

            return errores;
        }

        /// <summary>
        /// Valida el encabezado del movimiento
        /// </summary>
        private static void ValidarEncabezado(TSCnMcont movimiento, List<string> errores)
        {
            if (movimiento.Emp_codi <= 0)
                errores.Add("Emp_codi (código de empresa) debe ser mayor a 0");

            if (movimiento.Top_codi <= 0)
                errores.Add("Top_codi (tipo de operación) debe ser mayor a 0");

            if (movimiento.Mco_fech == default)
                errores.Add("Mco_fech (fecha) es requerida");

            if (string.IsNullOrWhiteSpace(movimiento.Mco_desc))
                errores.Add("Mco_desc (descripción) es requerida");

            if (movimiento.Mco_desc?.Length > 255)
                errores.Add("Mco_desc (descripción) no puede exceder 255 caracteres");
        }

        /// <summary>
        /// Valida cada línea del detalle
        /// </summary>
        private static void ValidarDetalle(TSCnDmcon[] detalle, List<string> errores)
        {
            for (int i = 0; i < detalle.Length; i++)
            {
                var linea = detalle[i];
                var lineaNum = i + 1;

                // Validar cuenta
                if (string.IsNullOrWhiteSpace(linea.Cue_codi))
                {
                    errores.Add($"Línea {lineaNum}: Cue_codi (cuenta contable) es requerida");
                }
                else
                {
                    // Validar formato básico de cuenta (solo números)
                    if (!linea.Cue_codi.All(char.IsDigit))
                        errores.Add($"Línea {lineaNum}: Cue_codi '{linea.Cue_codi}' debe contener solo números");

                    

                    //   Verificar si la cuenta tiene mapeo a NIF
                    if (!CuentaFiscalNifMapper.TieneMapeoCuentaNif(linea.Cue_codi))
                    {
                        errores.Add($"Línea {lineaNum}: La cuenta '{linea.Cue_codi}' NO tiene equivalencia NIF en el mapeo. " +
                                   $"SEVEN podría rechazarla. Verifica CTA_EQUIVALENCIA.");
                        if (string.IsNullOrWhiteSpace(linea.Dmc_desc))
                            errores.Add($"Línea {lineaNum}: Dmc_desc (descripción) es requerida");

                        // Validar débito/crédito
                        ValidarDebitoCredito(linea, lineaNum, errores);

                        // Validar tercero (recomendado pero no obligatorio)
                        if (!string.IsNullOrWhiteSpace(linea.Ter_coda))
                        {
                            if (!linea.Ter_coda.All(char.IsDigit))
                                errores.Add($"Línea {lineaNum}: Ter_coda '{linea.Ter_coda}' debe contener solo números");
                        }

                        // Validar campos opcionales si están presentes
                        ValidarCamposOpcionales(linea, lineaNum, errores);
                    }
                }
            }
        }

        /// <summary>
        /// Valida débito y crédito
        /// </summary>
        private static void ValidarDebitoCredito(TSCnDmcon linea, int lineaNum, List<string> errores)
        {
            // Al menos uno debe tener valor
            if (linea.Dmc_vadb == 0 && linea.Dmc_vacr == 0)
            {
                errores.Add($"Línea {lineaNum}: Debe tener Dmc_vadb (débito) O Dmc_vacr (crédito) con valor");
            }

            // No pueden ser ambos al mismo tiempo
            if (linea.Dmc_vadb > 0 && linea.Dmc_vacr > 0)
            {
                errores.Add($"Línea {lineaNum}: No puede tener Dmc_vadb Y Dmc_vacr al mismo tiempo. Use débito O crédito, no ambos.");
            }

            // No pueden ser negativos
            if (linea.Dmc_vadb < 0)
                errores.Add($"Línea {lineaNum}: Dmc_vadb (débito) no puede ser negativo: {linea.Dmc_vadb}");

            if (linea.Dmc_vacr < 0)
                errores.Add($"Línea {lineaNum}: Dmc_vacr (crédito) no puede ser negativo: {linea.Dmc_vacr}");
        }

        /// <summary>
        /// Valida campos opcionales si tienen valor
        /// </summary>
        private static void ValidarCamposOpcionales(TSCnDmcon linea, int lineaNum, List<string> errores)
        {
            // Validar centros de costo, administrativo, proyecto, sucursal si están presentes
            if (!string.IsNullOrWhiteSpace(linea.Arb_codc) && linea.Arb_codc.Length > 20)
                errores.Add($"Línea {lineaNum}: Arb_codc (centro de costo) excede longitud máxima");

            if (!string.IsNullOrWhiteSpace(linea.Arb_coda) && linea.Arb_coda.Length > 20)
                errores.Add($"Línea {lineaNum}: Arb_coda (centro administrativo) excede longitud máxima");

            if (!string.IsNullOrWhiteSpace(linea.Arb_codp) && linea.Arb_codp.Length > 20)
                errores.Add($"Línea {lineaNum}: Arb_codp (proyecto) excede longitud máxima");

            if (!string.IsNullOrWhiteSpace(linea.Dmc_refe) && linea.Dmc_refe.Length > 50)
                errores.Add($"Línea {lineaNum}: Dmc_refe (referencia) excede longitud máxima");
        }

        /// <summary>
        /// Valida que el movimiento cuadre (débitos = créditos)
        /// </summary>
        private static void ValidarCuadre(TSCnDmcon[] detalle, List<string> errores)
        {
            decimal totalDebitos = detalle.Sum(d => d.Dmc_vadb);
            decimal totalCreditos = detalle.Sum(d => d.Dmc_vacr);

            if (totalDebitos != totalCreditos)
            {
                var diferencia = Math.Abs(totalDebitos - totalCreditos);
                errores.Add($"El movimiento no cuadra. Total Débitos: {totalDebitos:N2}, Total Créditos: {totalCreditos:N2}, Diferencia: {diferencia:N2}");
            }

            // Validar que el total no sea cero
            if (totalDebitos == 0 && totalCreditos == 0)
            {
                errores.Add("El movimiento no puede tener todos los valores en cero");
            }
        }

        /// <summary>
        /// Método rápido para verificar si un movimiento es válido
        /// </summary>
        public static bool EsValido(TSCnMcont movimiento, out List<string> errores)
        {
            errores = ValidarMovimiento(movimiento);
            return errores.Count == 0;
        }

        /// <summary>
        /// Valida y lanza excepción si hay errores
        /// </summary>
        public static void ValidarYLanzarExcepcion(TSCnMcont movimiento)
        {
            var errores = ValidarMovimiento(movimiento);
            if (errores.Any())
            {
                var mensajeCompleto = string.Join(Environment.NewLine, errores);
                throw new ArgumentException($"Movimiento contable inválido:{Environment.NewLine}{mensajeCompleto}");
            }
        }

        /// <summary>
        /// Obtiene un resumen del movimiento para logging
        /// </summary>
        public static string ObtenerResumen(TSCnMcont movimiento)
        {
            if (movimiento == null)
                return "Movimiento null";

            var totalDebitos = movimiento.vDetalle?.Sum(d => d.Dmc_vadb) ?? 0;
            var totalCreditos = movimiento.vDetalle?.Sum(d => d.Dmc_vacr) ?? 0;
            var numLineas = movimiento.vDetalle?.Length ?? 0;

            return $"Movimiento - Empresa: {movimiento.Emp_codi}, " +
                   $"Tipo Op: {movimiento.Top_codi}, " +
                   $"Fecha: {movimiento.Mco_fech:yyyy-MM-dd}, " +
                   $"Líneas: {numLineas}, " +
                   $"Débitos: {totalDebitos:N2}, " +
                   $"Créditos: {totalCreditos:N2}, " +
                   $"Cuadra: {totalDebitos == totalCreditos}";
        }

        /// <summary>
        /// Convierte automáticamente todas las cuentas LOCAL a NIF en un movimiento
        /// </summary>
        public static TSCnMcont ConvertirCuentasLocalANif(TSCnMcont movimiento)
        {
            if (movimiento?.vDetalle == null)
                return movimiento;

            foreach (var linea in movimiento.vDetalle)
            {
                var cuentaNif = CuentaFiscalNifMapper.TryConvertirLocalANif(linea.Cue_codi);
                if (cuentaNif != null)
                {
                    linea.Cue_codi = cuentaNif;
                }
            }

            return movimiento;
        }

        /// <summary>
        /// Detecta si se están usando nombres de campos incorrectos
        /// (esto requiere reflexión ya que los campos incorrectos no existirían en TSCnDmcon)
        /// </summary>
        public static List<string> DetectarCamposIncorrectos()
        {
            var advertencias = new List<string>();
            var tipo = typeof(TSCnDmcon);

            // Verificar que existan los campos correctos según el SOAP
            var camposRequeridos = new[]
            {
                "Cue_codi",  // Cuenta
                "Dmc_vadb",  // Débito
                "Dmc_vacr",  // Crédito
                "Ter_coda",  // Tercero
                "Dmc_desc"   // Descripción
            };

            foreach (var campo in camposRequeridos)
            {
                var propiedad = tipo.GetProperty(campo);
                if (propiedad == null)
                {
                    advertencias.Add($"⚠️ ADVERTENCIA: La clase TSCnDmcon NO tiene el campo '{campo}' requerido por SEVEN");
                }
            }

            // Advertir sobre campos comunes pero incorrectos
            var camposIncorrectos = new[] { "Dmc_cta", "Dmc_ter", "Dmc_vdb", "Dmc_vcr" };
            foreach (var campo in camposIncorrectos)
            {
                var propiedad = tipo.GetProperty(campo);
                if (propiedad != null)
                {
                    advertencias.Add($"⚠️ ADVERTENCIA: La clase TSCnDmcon tiene el campo '{campo}' que NO existe en el SOAP de SEVEN");
                }
            }

            return advertencias;
        }
    }

}
