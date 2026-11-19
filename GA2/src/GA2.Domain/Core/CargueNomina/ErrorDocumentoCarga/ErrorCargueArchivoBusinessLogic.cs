using AutoMapper;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Domain.Entities.CargueNonima.CargaNomina;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class ErrorCargueArchivoBusinessLogic : IErrorCargueArchivoBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IErrorCargueArchivoRepository _errorCargueArchivoRepository;
        private readonly ICatalogosRepository _catalogosRepository;
        public ErrorCargueArchivoBusinessLogic(IMapper mapper,
                                               IErrorCargueArchivoRepository errorCargueArchivoRepository,
                                               ICatalogosRepository catalogosRepository)
        {
            _mapper = mapper;
            _errorCargueArchivoRepository = errorCargueArchivoRepository;
            _catalogosRepository = catalogosRepository;
        }        

        public async Task<List<ErrorCargueArchivo>> CrearErroresCargueArchivo(List<ErrorCargueArchivo> errorCargueArchivo)
        {
            return await this._errorCargueArchivoRepository.CrearErroresCargueArchivo(errorCargueArchivo);
        }

        public async Task<List<ErrorCargueArchivo>> ConsultarErroresCargueArchivo()
        {
            return await _errorCargueArchivoRepository.ConsultarErroresCargueArchivo();
        }

        public async Task<List<ErrorCargueArchivo>> ConsultarErroresPorCargueArchivo(int documentoId)
        {
            return await _errorCargueArchivoRepository.ConsultarErroresPorCargueArchivo(documentoId);
        }

        public async Task<NombreArchivoDto> ObtenerNombreArchivoPorId(int documentoId)
        {
            return this._mapper.Map<NombreArchivoDto>(
                   await this._errorCargueArchivoRepository.ObtenerNombreArchivoPorId(documentoId));
        }

        public async Task<ErrorCargueArchivo> ValidarCargueArchivo(GuardarArchivoCorregirDto guardarArchivoCorregir, NombreArchivoDto nombre, Response<List<ArchivoConErrorDto>> consultaArchivoConError)
        {
            // aqui se valida si el guardando desde el corregir archivo es correcto
            if (guardarArchivoCorregir.Corregir == 2)
            {   
                //Encabezado
                if (guardarArchivoCorregir.Encabezado_Consecutivo != 1) throw new Exception(CargueNominaConstans.ErrorCorregirArchivoConsecutivoEncabezado);
                if (guardarArchivoCorregir.Uej_Id != nombre.FILE_UNIDAD_EJECUTORA) throw new Exception(CargueNominaConstans.ErrorCorregirArchivoUnidadEjecutora);
                if (guardarArchivoCorregir.Dct_FechaCreacion != nombre.FILE_PERIODOS_APORTES.ToString()) throw new Exception(CargueNominaConstans.ErrorCorregirArchivoPeriodo);

                var TiposArchivos = await this._catalogosRepository.ObtenerTiposArchivos();
                bool tipoArchivoCoincide = TiposArchivos.Any(x => x.TAR_TIPO == nombre.FILE_TIPO && x.TAR_ID == guardarArchivoCorregir.Tipo_Archivo);
                if (!tipoArchivoCoincide) throw new Exception(CargueNominaConstans.ErrorCorregirArchivoTipoo);
            }
            else if (guardarArchivoCorregir.Corregir == 3)
            {
                //Cuerpo
                // Verificar si las cadena contiene minisculas
                if(string.IsNullOrEmpty(guardarArchivoCorregir.PrimerNombreCuerpo) || string.IsNullOrWhiteSpace(guardarArchivoCorregir.PrimerNombreCuerpo)) throw new Exception(CargueNominaConstans.ErrorCorregirArchivoNombreVacio);
                if (string.IsNullOrEmpty(guardarArchivoCorregir.PrimerApellidoCuerpo) || string.IsNullOrWhiteSpace(guardarArchivoCorregir.PrimerApellidoCuerpo)) throw new Exception(CargueNominaConstans.ErrorCorregirArchivoApellidoVacio);

                //valida minusculas
                if (Regex.IsMatch(guardarArchivoCorregir.PrimerNombreCuerpo, "[a-z]")) throw new Exception(CargueNominaConstans.ErrorCorregirArchivoMinusculasPrimerNombre);
                if (Regex.IsMatch(guardarArchivoCorregir.SegundoNombreCuerpo, "[a-z]")) throw new Exception(CargueNominaConstans.ErrorCorregirArchivoMinusculasSegundoNombre);
                if (Regex.IsMatch(guardarArchivoCorregir.PrimerApellidoCuerpo, "[a-z]")) throw new Exception(CargueNominaConstans.ErrorCorregirArchivoMinusculasPrimerApellido);
                if (Regex.IsMatch(guardarArchivoCorregir.SegundoApellidoCuerpo, "[a-z]")) throw new Exception(CargueNominaConstans.ErrorCorregirArchivoMinusculasSegundoApellido);
                //valida caracteres especiales
                if (Regex.IsMatch(guardarArchivoCorregir.PrimerNombreCuerpo, @"[^a-zA-ZñÑÁÉÍÓÚáéíóú\s]")) throw new Exception(CargueNominaConstans.ErrorCorregirArchivoCaracterEspecialPrimerNombre);
                if (Regex.IsMatch(guardarArchivoCorregir.SegundoNombreCuerpo, @"[^a-zA-ZñÑÁÉÍÓÚáéíóú\s]")) throw new Exception(CargueNominaConstans.ErrorCorregirArchivoCaracterEspecialSegundoNombre);
                if (Regex.IsMatch(guardarArchivoCorregir.PrimerApellidoCuerpo, @"[^a-zA-ZñÑÁÉÍÓÚáéíóú\s]")) throw new Exception(CargueNominaConstans.ErrorCorregirArchivoCaracterEspecialPrimerApellido);
                if (Regex.IsMatch(guardarArchivoCorregir.SegundoApellidoCuerpo, @"[^a-zA-ZñÑÁÉÍÓÚáéíóú\s]")) throw new Exception(CargueNominaConstans.ErrorCorregirArchivoCaracterEspecialSegundoApellido);

            }
            else if (guardarArchivoCorregir.Corregir == 4) 
            {   
                //Fin
                var consecutivoFin = this._mapper.Map<CuerpoArchivoDto>(await this._errorCargueArchivoRepository.ObtenerUltimoConsecutivoCuerpo((int)guardarArchivoCorregir.Dct_id));
                if (guardarArchivoCorregir.Fin_Consecutivo != (consecutivoFin.CONSECUTIVE + 1)) throw new Exception(CargueNominaConstans.ErrorCorregirArchivoConsecutivoFin);
                if (guardarArchivoCorregir.Codigo_fin_archivo != 999) throw new Exception(CargueNominaConstans.ErrorCorregirArchivoCodigoFin);

                int numeroComoEntero = Convert.ToInt32(guardarArchivoCorregir.IndicadorControl);
                var Calculo = ((decimal)nombre.FILE_PERIODOS_APORTES * (decimal)guardarArchivoCorregir.Fin_Consecutivo)
                               / (int)nombre.FILE_UNIDAD_EJECUTORA + (int)consultaArchivoConError.Data[0].IdTipoArchivo;
                var valorFormulaIndicadorControl = Math.Round(Calculo, 0, MidpointRounding.AwayFromZero);

                if (numeroComoEntero != valorFormulaIndicadorControl) throw new Exception(CargueNominaConstans.ErrorCorregirArchivoIndicadorControl);

            }

            ErrorCargueArchivo resultado = new ErrorCargueArchivo
            {
                // Configura las propiedades de resultado
            };
            return resultado;
        }

        public async Task<GuardarArchivoCorregirDto> AsignarDatosCorreccionMasiva(CorreccionMasivaArchivoDto CorreccionMasiva, NombreArchivoDto nombre)
        {
            //Correccion de consecutivo en todas las tablas
            ResponseCorreccionMasiva responseCorreccion = await this._errorCargueArchivoRepository.ActualizarConsecutivos((int)CorreccionMasiva.Dct_id);
            
            GuardarArchivoCorregirDto datos = new GuardarArchivoCorregirDto();

            // Asigna de datos para encabezado de correccion masiva
            datos.Dct_id = CorreccionMasiva.Dct_id;
            datos.Uej_Id = nombre.FILE_UNIDAD_EJECUTORA;
            datos.Dct_FechaCreacion = nombre.FILE_PERIODOS_APORTES.ToString();
            datos.Codigo_fin_archivo = 999;

            return datos;
        }

        public async Task GuardarDatosCorreccionMasiva(CorreccionMasivaArchivoDto CorreccionMasiva, NombreArchivoDto nombre, Response<List<ArchivoConErrorDto>> consultaArchivoConError)
        {
            if (CorreccionMasiva.Valor == 2)
            {
                await this._errorCargueArchivoRepository.GuardarEncabezadoMasivo((int)CorreccionMasiva.Dct_id, (int)nombre.FILE_UNIDAD_EJECUTORA, nombre.FILE_PERIODOS_APORTES.ToString(), nombre.FILE_TIPO, nombre.FILE_CATEGORIA );
            }
            else if (CorreccionMasiva.Valor == 3)
            {
                await this._errorCargueArchivoRepository.GuardarCuerpoMasivo((int)CorreccionMasiva.Dct_id, (int)nombre.FILE_UNIDAD_EJECUTORA, nombre.FILE_PERIODOS_APORTES.ToString());
            }
            else if (CorreccionMasiva.Valor == 4)
            {

                var consecutivoFin = this._mapper.Map<CuerpoArchivoDto>(await this._errorCargueArchivoRepository.ObtenerUltimoConsecutivoCuerpo((int)CorreccionMasiva.Dct_id));

                var Calculo = ((decimal)nombre.FILE_PERIODOS_APORTES * ((decimal)consecutivoFin.CONSECUTIVE + 1))
                               / (int)nombre.FILE_UNIDAD_EJECUTORA + (int)consultaArchivoConError.Data[0].IdTipoArchivo;
                var valorFormulaIndicadorControl = Math.Round(Calculo, 0, MidpointRounding.AwayFromZero);

                await this._errorCargueArchivoRepository.GuardarFinMasivo((int)CorreccionMasiva.Dct_id, (int)valorFormulaIndicadorControl);
            }
        }

        public async Task<List<ErrorCargueArchivoDto>> ActualizarEstadoErrorArchivo(GuardarArchivoCorregirDto guardarArchivoCorregir)
        {
            return await _errorCargueArchivoRepository.ActualizarEstadoErrorArchivo(guardarArchivoCorregir);
        }

        public async Task<ArchivoDto> ObtenerTotalErroresPorArchivo(int documentoId)
        {
            return this._mapper.Map<ArchivoDto>(
                   await this._errorCargueArchivoRepository.ObtenerTotalErroresPorArchivo(documentoId));
        }

        public async Task<ArchivoDto> ActualizarEstadoDocumento(int documentoId, int estadoCargueDocumentoId, int estadoCargueArchivoId)
        {
            return this._mapper.Map<ArchivoDto>(
                   await this._errorCargueArchivoRepository.ActualizarEstadoDocumento(documentoId, estadoCargueDocumentoId, estadoCargueArchivoId));
        }

    }
}
