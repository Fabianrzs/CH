namespace GA2.Transversals.Commons
{
    /// <summary>
    /// Constantes cargue de nomina
    /// <author>Oscar Julian Rojas</author>
    /// <date>13/05/2021</date>
    /// </summary>
    public static class CargueNominaConstans
    {
        /// <summary>
        /// Blobname blobstorage
        /// </summary>
        public const string BlobName = "carguenomina";

        /// <summary>
        /// Formato de archivo json web
        /// </summary>
        public const string ApplicacionJson = "application/json";

        /// <summary>
        /// Extension de archivo .txt
        /// </summary>
        public const string ExtensionTxt = ".txt";

        /// <summary>
        /// Extension de archivo .json
        /// </summary>
        public const string ExtensionJson = ".json";

        /// <summary>
        /// Formato de Time string
        /// </summary>
        public const string FormatTimeStringDocumento = "dd/MM/yyyy HH:mm:ss";

        public const string FormatTimeStringPeriodoAportes = "yyyyMM";

        public const string FormatTimeStringFechaEnvio = "ddMMyyyy";

        public const string ArchivoCargado = "Archivo cargado con exito";

        public const string ErrorCargaArchivo = "Se carga el archivo con errores";

        public const string ErrorCargaArchivoExistente = "El archivo ya existe";
        public const string ErrorCargaArchivoExistenteStorage = "Error, ya existe un archivo con el mismo nombre en el Storage";
        public const string ErrorCargaArchivoSumaAportes = "Error, la sumatoria de aportes no coincide con el valor en el Fin";
        public const string ErrorCargaArchivoIndicadorControl = "Error, la formula del indicador control no coincide valor del indicador de control en el Fin";

        // Mensaje de errror de Enzabezado
        public const string ErrorCorregirArchivoConsecutivoEncabezado = "Error, el valor del Consecutivo no corresponde";
        public const string ErrorCorregirArchivoUnidadEjecutora = "Error, el valor de Unidad Ejecutora no corresponde";
        public const string ErrorCorregirArchivoPeriodo = "Error, el valor del Periodo no corresponde";
        public const string ErrorCorregirArchivoTipoo = "Error, el Tipo del Archivo  no corresponde";

        //Mensajes de error de Cuerpo
        public const string ErrorCorregirArchivoNombreVacio = "Error, el primer nombre se encuentra vacio";
        public const string ErrorCorregirArchivoApellidoVacio = "Error, el primer apellido se encuentra vacio";
        public const string ErrorCorregirArchivoMinusculasPrimerNombre = "Error, el primer nombre contiene caracteres en minúscula";
        public const string ErrorCorregirArchivoMinusculasSegundoNombre = "Error, el segundo nombre contiene caracteres en minúscula";
        public const string ErrorCorregirArchivoMinusculasPrimerApellido = "Error, el primer apellido contiene caracteres en minúscula";
        public const string ErrorCorregirArchivoMinusculasSegundoApellido = "Error, el segundo apellido contiene caracteres en minúscula";
        public const string ErrorCorregirArchivoCaracterEspecialPrimerNombre = "Error, el primer nombre contiene caracteres especiales";
        public const string ErrorCorregirArchivoCaracterEspecialSegundoNombre = "Error, el segundo nombre contiene caracteres especiales";
        public const string ErrorCorregirArchivoCaracterEspecialPrimerApellido = "Error, el primer apellido contiene caracteres especiales";
        public const string ErrorCorregirArchivoCaracterEspecialSegundoApellido = "Error, el segundo apellido contiene caracteres especiales";

        // Mensaje de errror de Fin
        public const string ErrorCorregirArchivoConsecutivoFin = "Error, el valor del Consecutivo no es correcto";
        public const string ErrorCorregirArchivoCodigoFin = "Error, el valor del Código fin del archivo no es correcto";
        public const string ErrorCorregirArchivoIndicadorControl = "Error, el valor del indicador de control no es correcto";
    }
}
