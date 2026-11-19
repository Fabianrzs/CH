using AutoMapper;

using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto.CronogramaActividades;
using GA2.Application.Main;
using GA2.Domain.Entities;
using GA2.Domain.Entities.CronogramaNovedades;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace GA2.Domain.Core
{
    public class CronogramaNovedadesBusinessLogic : ICronogramaNovedadesBusinessLogic
    {
        /// <summary>
        /// Propiedades privadas Core Negocio
        /// </summary>
        private readonly IMapper _mapper;
        private readonly ICronogramaRepository _repositoryRepository;
        private readonly IReportesBusinessLogic _reportesBusinessLogic;
        private readonly IClientRequestProvider _iClientRequestProvider;
        private readonly IConverter _converter;        
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICatalogosBusinessLogic _catalogosBusinessLogic;
        private readonly IUsersRepository _usersRepository;

        /// <summary>
        /// Constructor Negocio Clients
        /// </summary>
        /// <param name="mapper">Instancia mapper</param>
        /// <param name="clientsRepository">Instancia repositorio usuarios</param>
        public CronogramaNovedadesBusinessLogic(
            IMapper mapper,
            ICronogramaRepository repositoryRepository,
            IReportesBusinessLogic reportesBusinessLogic,
            IClientRequestProvider clientRequestProvider,
            IConverter converter,            
            IWebHostEnvironment webHostEnvironment,
            ICatalogosBusinessLogic catalogosBusinessLogic,
            IUsersRepository usersRepository   
            )
        {
            _mapper = mapper;
            _repositoryRepository = repositoryRepository;
            _reportesBusinessLogic = reportesBusinessLogic;
            _iClientRequestProvider = clientRequestProvider;
            _converter = converter;            
            _webHostEnvironment = webHostEnvironment;
            _catalogosBusinessLogic = catalogosBusinessLogic;
            _usersRepository = usersRepository;
        }

        public async Task<Response<IEnumerable<CronogramaDto>>> AgregarUnidadEjecutoraCronograma(CronogramaDto cronogramaDTO)
        {
            var cronograma = this._mapper.Map<CronogramaDto, Cronograma>(cronogramaDTO);
            var cronogramaUnidadesEjecutoras = this._mapper.Map<IEnumerable<Cronograma>, IEnumerable<CronogramaDto>>(await _repositoryRepository.AgregarUnidadEjecutoraCronograma(cronograma));
            return new Response<IEnumerable<CronogramaDto>> { Data = cronogramaUnidadesEjecutoras };
        }
        public async Task<Response<IEnumerable<CronogramaDto>>> EditarFechaRegistroCronograma(CronogramaDto editarCronogramaDTO)
        {
            var cronograma = this._mapper.Map<CronogramaDto, Cronograma>(editarCronogramaDTO);
            if(editarCronogramaDTO.Id > 0)
            {
                var cronogramaUnidadesEjecutoras = this._mapper.Map<IEnumerable<Cronograma>, IEnumerable<CronogramaDto>>(await _repositoryRepository.EditarFechaReporteCronograma(cronograma));
                if(editarCronogramaDTO.Novedades != null)
                {
                    foreach (var item in editarCronogramaDTO.Novedades)
                    {
                        if(item.IdNovedad == 0)
                        {
                            await _repositoryRepository.InsertarNovedadCronograma(_mapper.Map<NovedadesCronograma>(item));
                        }
                        else
                        {
                            await _repositoryRepository.EditarNovedadCronograma(_mapper.Map<NovedadesCronograma>(item));
                        }
                    }
                }
                return new Response<IEnumerable<CronogramaDto>> { Data = cronogramaUnidadesEjecutoras };
            } 
            else
            {
                var cronogramaUnidadesEjecutoras = this._mapper.Map<IEnumerable<Cronograma>, IEnumerable<CronogramaDto>>(await _repositoryRepository.AgregarUnidadEjecutoraCronograma(cronograma));
                if (editarCronogramaDTO.Novedades != null)
                {
                    foreach (var item in editarCronogramaDTO.Novedades)
                    {
                        if (item.IdNovedad == 0)
                        {
                            await _repositoryRepository.InsertarNovedadCronograma(_mapper.Map<NovedadesCronograma>(item));
                        }
                        else
                        {
                            await _repositoryRepository.EditarNovedadCronograma(_mapper.Map<NovedadesCronograma>(item));
                        }
                    }
                }
                return new Response<IEnumerable<CronogramaDto>> { Data = cronogramaUnidadesEjecutoras };
            }
        }

        public async Task<Response<NovedadesAfiliadoDto>> EditarNovedadAfiliado(NovedadesAfiliadoDto request)
        {
            return new Response<NovedadesAfiliadoDto> { Data = this._mapper.Map<NovedadesAfiliadoDto>(await _repositoryRepository.EditarNovedadAfiliado(this._mapper.Map<NovedadesAfiliado>(request))) };
        }

        public async Task<Response<NovedadesCronogramaDto>> EliminarNovedad(int idNovedad)
        {
            var cronograma = this._mapper.Map<NovedadesCronogramaDto>(await _repositoryRepository.EliminarNovedadCrucigrama(idNovedad));
            return new Response<NovedadesCronogramaDto> { Data = cronograma };
        }

        public async Task<Response<NovedadesCronogramaDto>> EliminarNovedadAfiliado(int idUnidadEjecutora)
        {
            return new Response<NovedadesCronogramaDto> { Data = this._mapper.Map<NovedadesCronogramaDto>(await _repositoryRepository.EliminarNovedadAfiliado(idUnidadEjecutora)) };
        }

        public async Task<Response<IEnumerable<CronogramaDto>>> EliminarUnidadEjecutoraCronograma(int idCronograma)
        {
            var cronograma = this._mapper.Map<IEnumerable<Cronograma>, IEnumerable<CronogramaDto>>(await _repositoryRepository.EliminarUnidadEjecutoraCronograma(idCronograma));
            return new Response<IEnumerable<CronogramaDto>> { Data = cronograma };
        }

        public async Task<Response<GeneracionArchivosResponse>> GeneracionArchivoNovedades(GenerarArchivoRequest generarArchivoRequest)
        {
            var data = await _repositoryRepository.GeneracionArchivoNovedades(generarArchivoRequest.IdUnidadEjecutora, generarArchivoRequest.IdConceptoNovedad, generarArchivoRequest.FechaInicio, generarArchivoRequest.FechaFin, generarArchivoRequest.EsAlta, string.Format(generarArchivoRequest.IdUnidadEjecutora == 2 ? "0" : generarArchivoRequest.Codigo), generarArchivoRequest.IdUnidadEjecutora == 2?true: generarArchivoRequest.DHorizontal);
            string line = string.Empty;
            string nameFile = string.Empty;
            int consecutivo = 0;
            DateTime now = DateTime.Now;
            string numeroFormateado = string.Empty;
            string identificacion = string.Empty;
            string htmlTemplateGlobal = string.Empty;
            string fileProcess = string.Empty;
            string nameFilePdf = string.Empty;
            byte[] pdf;

            if (data.Count() >0)
            {
                string templateHtml = string.Empty;
               
                if (generarArchivoRequest.Codigo == "9098" || generarArchivoRequest.Codigo == "984Q" && generarArchivoRequest.EsAlta == true)
                {
                    try
                    {
                        templateHtml = File.ReadAllText("Reportes/HTML/Templates/HTML/Novedades/TemplatePdfPorcentaje.html");
                    }
                    catch
                    {
                        templateHtml = string.Empty;
                    }
                    //Tendra una columna adicional
                    #region HTMLTEMPLATEPORCENTAJE
                    htmlTemplateGlobal = templateHtml;

                    #endregion
                }
                else
                {
                    try
                    {
                        templateHtml = File.ReadAllText("Reportes/HTML/Templates/HTML/Novedades/TemplatePdfDefault.html");
                    }
                    catch
                    {
                        templateHtml = string.Empty;
                    }

                    #region HTMLTemplate

                        htmlTemplateGlobal = templateHtml;
                    #endregion
                }
                var dataGroup = data.GroupBy(x => x.ES_ALTA).OrderByDescending(x=>x.Key);
                switch (generarArchivoRequest.IdUnidadEjecutora)
                {
                    //Ejercito Nacional
                    #region Ejercito Nacional
                    case 1:
                        //Ahorro Obligatorio 9103                   
                        if (generarArchivoRequest.IdConceptoNovedad == 1)
                        {
                            //Encabezado
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;
                            nameFile =generarArchivoRequest.NombreArchivo==null? generarArchivoRequest.Codigo + ".txt":generarArchivoRequest.NombreArchivo;
                            foreach(var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {

                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    if (item.COD_MILITAR == null || item.COD_MILITAR == "")
                                    {
                                        identificacion = item.NUMERO_IDENTIFICACION;
                                    }
                                    else
                                    {
                                        identificacion = item.COD_MILITAR;
                                    }

                                    line += ($"{identificacion.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                }
                            }
                            
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 2)
                        {
                            //Ahorro Voluntario 9098
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;

                            nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    if (item.COD_MILITAR == null || item.COD_MILITAR == "")
                                    {
                                        identificacion = item.NUMERO_IDENTIFICACION;
                                    }
                                    else
                                    {
                                        identificacion = item.COD_MILITAR;
                                    }

                                    line += ($"{identificacion.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : item.FECHA_INICIO.ToString("yyyyMM"))}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 3)
                        {
                            //Fondo Solidaridad 9405
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_FIN;
                                    fechaFin = fechaFin.AddYears(14);

                                    if (item.COD_MILITAR == null || item.COD_MILITAR == "")
                                    {
                                        identificacion = item.NUMERO_IDENTIFICACION;
                                    }
                                    else
                                    {
                                        identificacion = item.COD_MILITAR;
                                    }

                                    line += ($"{identificacion.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_INICIO.ToString("yyyyMM")}{item.FECHA_PERIODO.ToString("yyyyMM")}" + System.Environment.NewLine);
                                }
                            }
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 4)
                        {
                            //Ahorro AVAC 984Q
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    if (item.COD_MILITAR == null || item.COD_MILITAR == "")
                                    {
                                        identificacion = item.NUMERO_IDENTIFICACION;
                                    }
                                    else
                                    {
                                        identificacion = item.COD_MILITAR;
                                    }

                                    line += ($"{identificacion.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                }
                            }
                            
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 6)
                        {
                            //Ahorro AVC 982B
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    if (item.COD_MILITAR == null || item.COD_MILITAR == "")
                                    {
                                        identificacion = item.NUMERO_IDENTIFICACION;
                                    }
                                    else
                                    {
                                        identificacion = item.COD_MILITAR;
                                    }

                                    line += ($"{identificacion.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                }
                            }
                            
                        }
                        break;
                    #endregion
                    // Caja Promotora de Vivienda Militar y de Policía
                    #region CajaPromotoradeVivienda
                    case 2:
                        
                        DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("CODIGO_EMPRESA", typeof(int));
                        dataTable.Columns.Add("NUMERO_IDENTIFICACION", typeof(string));
                        dataTable.Columns.Add("NOMBRES", typeof(string));
                        dataTable.Columns.Add("APELLIDOS", typeof(string));
                        dataTable.Columns.Add("CONCEPTO", typeof(string));
                        dataTable.Columns.Add("FECHA_DE_PAGO", typeof(string));
                        dataTable.Columns.Add("FECHA_REAL", typeof(string));
                        dataTable.Columns.Add("TIPO_NOVEDAD", typeof(string));
                        dataTable.Columns.Add("CANTIDAD_NOVEDAD", typeof(string));
                        foreach(var generacionArchivos in dataGroup)
                        {
                            foreach (GeneracionArchivos generacionArchivo in generacionArchivos.ToList())
                            {
                                var newType = string.Empty;
                                if (generacionArchivo.ES_ALTA)
                                {
                                    newType = generacionArchivo.TIPO_NOVEDAD;
                                }
                                else
                                {
                                    newType = "S";
                                }
                                if (generacionArchivo.ID_CONCEPTO_NOVEDAD == 3 && generacionArchivo.ES_ALTA == true && generacionArchivo.NVD_NOVEDADES_ALTA_BAJA == "210")
                                {
                                    newType = "V";
                                }
                                var clienteDto = await _iClientRequestProvider.ObtenerClientesByNumeroIdentificacion(generacionArchivo.NUMERO_IDENTIFICACION);
                                if (clienteDto.Data == null)
                                {
                                    continue;
                                }
                                DataRow newRow = dataTable.NewRow();
                                newRow["CODIGO_EMPRESA"] = generacionArchivo.CODIGO_EMPRESA;
                                newRow["NUMERO_IDENTIFICACION"] = generacionArchivo.NUMERO_IDENTIFICACION;
                                newRow["NOMBRES"] = clienteDto.Data != null ? clienteDto.Data.PrimerNombre + " " + clienteDto.Data.SegundoNombre : "";
                                newRow["APELLIDOS"] = clienteDto.Data != null ? clienteDto.Data.PrimerApellido + " " + clienteDto.Data.SegundoApellido : "";
                                newRow["CONCEPTO"] = generacionArchivo.NVD_NOVEDADES_ALTA_BAJA;
                                newRow["FECHA_DE_PAGO"] = string.Format(25 + data.ToList()[0].FECHA_PERIODO.ToString("/MM/yyyy"));//FECHA_PAGO //Siempre 25 de cada mes
                                newRow["FECHA_REAL"] = generacionArchivo.FECHA_REPORTE.ToString("dd/MM/yyyy");
                                newRow["TIPO_NOVEDAD"] = newType;
                                newRow["CANTIDAD_NOVEDAD"] = Convert.ToInt32(generacionArchivo.PORCENTAJE_DESCUENTO) + " %";
                                dataTable.Rows.Add(newRow);
                            }
                        }
                       
                        return new Response<GeneracionArchivosResponse> { Data = new() { ArchivoB64 = await _reportesBusinessLogic.GenerarExcel(dataTable, "Caja Promotora"), NameFile = "Caja Promotora de Vivienda Militar y de Policía.xlsx" } };
                        
                    #endregion
                    //Policia Nacional
                    #region PoliciaNacional
                    case 3:
                        //Ahorro Onligatorio 03
                        if (generarArchivoRequest.IdConceptoNovedad == 1)
                        {
                            nameFile = string.Format(generarArchivoRequest.EsAlta == true ? "NUEVO_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_CAJAHONOR.txt" : "CANCEL_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_CAJAHONOR.txt");
                            foreach(var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    consecutivo += 1;
                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                    {
                                        numeroFormateado = item.PORCENTAJE_DESCUENTO.ToString();
                                    }
                                    else
                                    {
                                        numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                    }

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                    line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};;;{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{"3"};{item.APORTE};{item.I};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;{string.Format(item.ES_ALTA == false ? string.Empty : numeroFormateado + ";SI;;;")}" + System.Environment.NewLine);
                                }
                            }
                           
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 2)
                        {
                            //Ahorro Voluntario 03              
                            nameFile = string.Format(generarArchivoRequest.EsAlta == true ? nameFile = "NUEVO_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_CAJAHONOR.txt" : nameFile = "CANCEL_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_CAJAHONOR.txt");
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    consecutivo += 1;
                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                    {
                                        numeroFormateado = item.PORCENTAJE_DESCUENTO.ToString();
                                    }
                                    else
                                    {
                                        numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                    }

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                    line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};;;{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{item.NVD_NOVEDADES_ALTA_BAJA};{item.APORTE};{item.I};{item.FECHA_REPORTE.ToString("dd/MM/yyyy")};;;{numeroFormateado};{"SI"};;;" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 3)
                        {
                            //Fondo de Solidaridad  106                   
                            nameFile = string.Format(generarArchivoRequest.EsAlta == true ? nameFile = "NUEVO_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_CAJAHONOR.txt" : nameFile = "CANCEL_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_CAJAHONOR.txt");
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    consecutivo += 1;
                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                    {
                                        numeroFormateado = item.PORCENTAJE_DESCUENTO.ToString();
                                    }
                                    else
                                    {
                                        numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                    }

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                    line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};;;{5};{11};{item.FECHA_REPORTE.ToString("dd/MM/yyyy")};{item.NVD_NOVEDADES_ALTA_BAJA};{item.APORTE};{"D"};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{numeroFormateado};{"SI"};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 4)
                        {
                            //Ahorro Voluntario Cesantias 1637             
                            nameFile = string.Format(generarArchivoRequest.EsAlta == true ? "NUEVO_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_CAJAHONOR.txt" : "CANCEL_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_CAJAHONOR.txt");
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    consecutivo += 1;
                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                    {
                                        numeroFormateado = item.PORCENTAJE_DESCUENTO.ToString();
                                    }
                                    else
                                    {
                                        numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                    }

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                    line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};;;{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{item.NVD_NOVEDADES_ALTA_BAJA};{item.APORTE};{"I"};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;{string.Format(item.ES_ALTA == true ? numeroFormateado + ";SI;" + /*item.FECHA_PERIODO.ToString("dd/MM/yyyy") +*/ ";;;" + item.TOKEN : string.Empty)}" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 6)
                        {
                            //Fondo de Solidaridad  03                      
                            nameFile = string.Format(generarArchivoRequest.EsAlta == true ? nameFile = "NUEVO_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_CAJAHONOR.txt" : nameFile = "CANCEL_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_CAJAHONOR.txt");
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    consecutivo += 1;
                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                    {
                                        numeroFormateado = item.PORCENTAJE_DESCUENTO.ToString();
                                    }
                                    else
                                    {
                                        numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                    }

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                    line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};;;{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{"3"};{item.APORTE};{item.I};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;{string.Format(item.ES_ALTA == false ? string.Empty : numeroFormateado + ";SI;;;")}" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        break;
                    #endregion
                    //Pensionados Policia Nacional
                    #region Pensionados Policia Nacional
                    case 5:
                        //Suspencion de Ahorro

                        if (generarArchivoRequest.IdConceptoNovedad == 1)
                        {
                            nameFile = string.Format(generarArchivoRequest.EsAlta == true ? nameFile = "NUEVO_" + now.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_PENSIONADOSPOLICIANACIONAL.txt" : nameFile = "CANCEL_" + now.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_CAJAHONOR.txt");
                            foreach(var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    consecutivo += 1;
                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                    {
                                        numeroFormateado = Math.Round(item.PORCENTAJE_DESCUENTO,1).ToString();
                                    }
                                    else
                                    {
                                        numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                    }

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                    var cliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(item.TIPO_IDENTIFICACION_N, item.NUMERO_IDENTIFICACION);
                                    if (cliente.Data != null) 
                                    {
                                        item.TIPO_AFILIADO = cliente.Data.IdTipoActor == 3 ? "PP" : item.TIPO_AFILIADO;
                                    }
                                    if (item.ES_ALTA)
                                    {
                                        line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};{12};{item.TIPO_AFILIADO};{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{"3"};{item.APORTE};{item.I};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;{numeroFormateado};{item.ACTIVACION_NOVEDAD};;{string.Format(item.TIPO_AFILIADO == "PP" ? item.TIPO_IDENTIFICACION + ";" + item.NUMERO_IDENTIFICACION : ";")}" + System.Environment.NewLine);
                                    }
                                    else
                                    {
                                        line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};{12};{item.TIPO_AFILIADO};{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{"3"};{item.APORTE};{item.I};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;" + System.Environment.NewLine);
                                    }
                                }

                            }

                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 2)
                        {
                            //Activacion Ahorro                        
                            nameFile = "NUEVO_" + now.ToString("yyyyMM") + "__PENSIONADOSPOLICIANACIONAL_03.txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    consecutivo += 1;
                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                    {
                                        numeroFormateado = Math.Round(item.PORCENTAJE_DESCUENTO, 1).ToString();
                                    }
                                    else
                                    {
                                        numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                    }

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));
                                    var cliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(item.TIPO_IDENTIFICACION_N, item.NUMERO_IDENTIFICACION);
                                    if (cliente.Data != null)
                                    {
                                        item.TIPO_AFILIADO = cliente.Data.IdTipoActor == 3 ? "PP" : item.TIPO_AFILIADO;
                                    }
                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                    line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};{12};{item.TIPO_AFILIADO};{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{item.NVD_NOVEDADES_ALTA_BAJA};{item.APORTE};{item.I};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;{numeroFormateado};{item.ACTIVACION_NOVEDAD};;{string.Format(item.TIPO_AFILIADO == "PP" ? item.TIPO_IDENTIFICACION + ";" + item.NUMERO_IDENTIFICACION : ";")}" + System.Environment.NewLine);
                                }
                            }    
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 3)
                        {
                            //Fondo Solidaridad                        
                            nameFile = "NUEVO_" + now.ToString("yyyyMM") + "__PENSIONADOSPOLICIANACIONAL_614.txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    consecutivo += 1;
                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                    {
                                        numeroFormateado = Math.Round(item.PORCENTAJE_DESCUENTO, 1).ToString();
                                    }
                                    else
                                    {
                                        numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                    }

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));
                                    var cliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(item.TIPO_IDENTIFICACION_N, item.NUMERO_IDENTIFICACION);
                                    if (cliente.Data != null)
                                    {
                                        item.TIPO_AFILIADO = cliente.Data.IdTipoActor == 3 ? "PP" : item.TIPO_AFILIADO;
                                    }
                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                    line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};{12};{item.TIPO_AFILIADO};{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};614;{item.APORTE};{"D"};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;{numeroFormateado};{item.ACTIVACION_NOVEDAD};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;{string.Format(item.TIPO_AFILIADO == "PP" ? item.TIPO_IDENTIFICACION + ";" + item.NUMERO_IDENTIFICACION : ";")}" + System.Environment.NewLine);
                                }
                            } 
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 4)
                        {
                            //Suspensión Ahorro Voluntario Afiliado Veterano                        
                            nameFile = "CANCEL_" + now.ToString("yyyyMM") + "__PENSIONADOSPOLICIANACIONAL_225.txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    consecutivo += 1;
                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                    {
                                        numeroFormateado = Math.Round(item.PORCENTAJE_DESCUENTO, 1).ToString();
                                    }
                                    else
                                    {
                                        numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                    }

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));
                                    var cliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(item.TIPO_IDENTIFICACION_N, item.NUMERO_IDENTIFICACION);
                                    if (cliente.Data != null)
                                    {
                                        item.TIPO_AFILIADO = cliente.Data.IdTipoActor == 3 ? "PP" : item.TIPO_AFILIADO;
                                    }
                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                    if (item.ES_ALTA)
                                    {
                                        line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};{12};{item.TIPO_AFILIADO};{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};225;{item.APORTE};{item.I};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;{numeroFormateado};{item.ACTIVACION_NOVEDAD};;{string.Format(item.TIPO_AFILIADO == "PP" ? item.TIPO_IDENTIFICACION + ";" + item.NUMERO_IDENTIFICACION : ";")}" + System.Environment.NewLine);
                                    }
                                    else
                                    {
                                        line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};{12};{item.TIPO_AFILIADO};{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};225;{item.APORTE};{item.I};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;{numeroFormateado};;;;{string.Format(item.TIPO_AFILIADO == "PP" ? item.TIPO_IDENTIFICACION + ";" + item.NUMERO_IDENTIFICACION : ";")}" + System.Environment.NewLine);
                                    }

                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 5)
                        {
                            //Activación Ahorro Voluntario Afiliado Veterano
                            nameFile = "NUEVO_" + now.ToString("yyyyMM") + "__PENSIONADOSPOLICIANACIONAL_225.txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    consecutivo += 1;
                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                    {
                                        numeroFormateado = Math.Round(item.PORCENTAJE_DESCUENTO, 1).ToString();
                                    }
                                    else
                                    {
                                        numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                    }

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));
                                    var cliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(item.TIPO_IDENTIFICACION_N, item.NUMERO_IDENTIFICACION);
                                    if (cliente.Data != null)
                                    {
                                        item.TIPO_AFILIADO = cliente.Data.IdTipoActor == 3 ? "PP" : item.TIPO_AFILIADO;
                                    }
                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                    if (item.ES_ALTA)
                                    {
                                        line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};{12};{item.TIPO_AFILIADO};{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};225;{item.APORTE};{item.I};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;{numeroFormateado};{item.ACTIVACION_NOVEDAD};;{string.Format(item.TIPO_AFILIADO == "PP" ? item.TIPO_IDENTIFICACION + ";" + item.NUMERO_IDENTIFICACION : ";")}" + System.Environment.NewLine);
                                    }
                                    else
                                    {
                                        line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};{12};{item.TIPO_AFILIADO};{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};225;{item.APORTE};{item.I};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;{numeroFormateado};;;;{string.Format(item.TIPO_AFILIADO == "PP" ? item.TIPO_IDENTIFICACION + ";" + item.NUMERO_IDENTIFICACION : ";")}" + System.Environment.NewLine);
                                    }
                                }
                            }   
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 6)
                        {
                            //Activación Ahorro Voluntario Afiliado Veterano
                            nameFile = "NUEVO_" + now.ToString("yyyyMM") + "__PENSIONADOSPOLICIANACIONAL_225.txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    consecutivo += 1;
                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                    {
                                        numeroFormateado = Math.Round(item.PORCENTAJE_DESCUENTO, 1).ToString();
                                    }
                                    else
                                    {
                                        numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                    }

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));
                                    var cliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(item.TIPO_IDENTIFICACION_N, item.NUMERO_IDENTIFICACION);
                                    if (cliente.Data != null) 
                                    {
                                        item.TIPO_AFILIADO = cliente.Data.IdTipoActor == 3 ? "PP" : item.TIPO_AFILIADO;
                                    }
                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                    if (item.ES_ALTA)
                                    {
                                        line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};{12};{item.TIPO_AFILIADO};{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};225;{item.APORTE};{item.I};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;{numeroFormateado};{item.ACTIVACION_NOVEDAD};;{string.Format(item.TIPO_AFILIADO == "PP" ? item.TIPO_IDENTIFICACION + ";" + item.NUMERO_IDENTIFICACION : ";")}" + System.Environment.NewLine);
                                    }
                                    else
                                    {
                                        line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};{12};{item.TIPO_AFILIADO};{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};225;{item.APORTE};{item.I};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;{numeroFormateado};;;;{string.Format(item.TIPO_AFILIADO == "PP" ? item.TIPO_IDENTIFICACION + ";" + item.NUMERO_IDENTIFICACION : ";")}" + System.Environment.NewLine);
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    //Direccion de Sanidad Policia Nacional
                    #region Direccion de Sanidad Policia Nacional
                    case 6:
                        //Ahorro Onligatorio 03
                        if (generarArchivoRequest.IdConceptoNovedad == 1)
                        {
                            nameFile = string.Format(generarArchivoRequest.EsAlta == true ? "NUEVO_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_SANIDADPOLICIANACIONAL.txt" : "CANCEL_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_SANIDADPOLICIANACIONAL.txt");
                            foreach(var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    consecutivo += 1;
                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                    {
                                        numeroFormateado = item.PORCENTAJE_DESCUENTO.ToString();
                                    }
                                    else
                                    {
                                        numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                    }

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                    line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};;;{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{"3"};{item.APORTE};{item.I};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;{string.Format(item.ES_ALTA == false ? string.Empty : numeroFormateado + ";SI;;;")}" + System.Environment.NewLine);
                                }
                            }
                            
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 2)
                        {
                            //Ahorro Voluntario 03              
                            nameFile = string.Format(generarArchivoRequest.EsAlta == true ? nameFile = "NUEVO_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_SANIDADPOLICIANACIONAL.txt" : nameFile = "CANCEL_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_SANIDADPOLICIANACIONAL.txt");
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    consecutivo += 1;
                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                    {
                                        numeroFormateado = item.PORCENTAJE_DESCUENTO.ToString();
                                    }
                                    else
                                    {
                                        numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                    }

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                    line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};;;{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{Regex.Replace(item.NVD_NOVEDADES_ALTA_BAJA, "[^0-9]", "")};{item.APORTE};{item.I};{item.FECHA_REPORTE.ToString("dd/MM/yyyy")};;;{numeroFormateado};{"SI"};;;" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 3)
                        {
                            //Fondo de Solidaridad  106                   
                            nameFile = string.Format(generarArchivoRequest.EsAlta == true ? nameFile = "NUEVO_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_SANIDADPOLICIANACIONAL.txt" : nameFile = "CANCEL_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_SANIDADPOLICIANACIONAL.txt");
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    consecutivo += 1;
                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                    {
                                        numeroFormateado = item.PORCENTAJE_DESCUENTO.ToString();
                                    }
                                    else
                                    {
                                        numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                    }

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                    line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};;;{5};{11};{item.FECHA_REPORTE.ToString("dd/MM/yyyy")};{Regex.Replace(item.NVD_NOVEDADES_ALTA_BAJA, "[^0-9]", "")};{item.APORTE};{"D"};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{numeroFormateado};{"SI"};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 4)
                        {
                            //Ahorro Voluntario Cesantias 1637             
                            nameFile = string.Format(generarArchivoRequest.EsAlta == true ? "NUEVO_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_SANIDADPOLICIANACIONAL.txt" : "CANCEL_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_SANIDADPOLICIANACIONALR.txt");
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    consecutivo += 1;
                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                    {
                                        numeroFormateado = item.PORCENTAJE_DESCUENTO.ToString();
                                    }
                                    else
                                    {
                                        numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                    }

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                    line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};;;{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{Regex.Replace(item.NVD_NOVEDADES_ALTA_BAJA, "[^0-9]", "")};{item.APORTE};{"I"};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;{string.Format(item.ES_ALTA == true ? numeroFormateado + ";SI;" + /*item.FECHA_PERIODO.ToString("dd/MM/yyyy") +*/ ";;;" + item.TOKEN : string.Empty)}" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 6)
                        {
                            //Fondo de Solidaridad  03                      
                            nameFile = string.Format(generarArchivoRequest.EsAlta == true ? "NUEVO_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_SANIDADPOLICIANACIONAL.txt" : "CANCEL_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_SANIDADPOLICIANACIONAL.txt");
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    consecutivo += 1;
                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                    {
                                        numeroFormateado = item.PORCENTAJE_DESCUENTO.ToString();
                                    }
                                    else
                                    {
                                        numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                    }

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                    line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};;;{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{"3"};{item.APORTE};{"I"};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{(item.ES_ALTA == true ? numeroFormateado : "")};{(item.ES_ALTA == true ? "SI" : "")};{(item.ES_ALTA == true ? ";;" : item.FECHA_REPORTE.ToString("dd/MM/yyyy") + ";;;")} " + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        break;
                    #endregion
                    //Gabinete
                    #region Gabinete
                    case 7:
                        //Ahorro Obligatorio 9103                   
                        if (generarArchivoRequest.IdConceptoNovedad == 1)
                        {
                            //Encabezado
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {

                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                }
                            }
                           
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 2)
                        {
                            //Ahorro Voluntario 9098
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;

                            nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : item.FECHA_INICIO.ToString("yyyyMM"))}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 3)
                        {
                            //Fondo Solidaridad 9405
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_FIN;
                                    fechaFin = fechaFin.AddYears(14);

                                    line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_INICIO.ToString("yyyyMM")}{item.FECHA_PERIODO.ToString("yyyyMM")}" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 4)
                        {
                            //Ahorro AVAC 984Q
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 6)
                        {
                            //Ahorro AVC 982B
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                }
                            }
                               
                        }
                        break;
                    #endregion
                    //Pensionados FFMM
                    #region Pensionados FFMM
                    case 8:
                        if (generarArchivoRequest.OtroFormato)
                        {
                            DataTable dataTablePFFMM = new DataTable();
                            dataTablePFFMM.Columns.Add("ITEM", typeof(int));
                            dataTablePFFMM.Columns.Add("GRADO", typeof(string));
                            dataTablePFFMM.Columns.Add("APELLIDOS_Y_NOMBRES", typeof(string));
                            dataTablePFFMM.Columns.Add("CODIGO_MILITAR", typeof(string));
                            dataTablePFFMM.Columns.Add("No_CEDULA", typeof(string));
                            dataTablePFFMM.Columns.Add("FUERZA", typeof(string));
                            dataTablePFFMM.Columns.Add("CODIGO_DESTINO", typeof(string));
                            dataTablePFFMM.Columns.Add("NUMERO_LIBRANZA", typeof(string));
                            dataTablePFFMM.Columns.Add("ANO_MES_DIA_LIBRANZA", typeof(string));
                            dataTablePFFMM.Columns.Add("CUOTA_MENSUAL", typeof(string));
                            dataTablePFFMM.Columns.Add("TOTAL_A_PAGAR", typeof(string));
                            dataTablePFFMM.Columns.Add("FECHA_INICIO", typeof(string));
                            dataTablePFFMM.Columns.Add("FECHA_TERMINO", typeof(string));
                            dataTablePFFMM.Columns.Add("TOTAL_CUOTA", typeof(string));
                            dataTablePFFMM.Columns.Add("CONCEPTO", typeof(string));
                            int item = 0;
                            foreach (var generacionArchivos in dataGroup)
                            {
                                foreach(var generacionArchivo in generacionArchivos.ToList())
                                {
                                    item++; 
                                    var clienteDto = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(generacionArchivo.TIPO_IDENTIFICACION_N,generacionArchivo.NUMERO_IDENTIFICACION);
                                    DataRow newRow = dataTablePFFMM.NewRow();
                                    newRow["ITEM"] = item;
                                    newRow["GRADO"] =clienteDto.Data!=null? clienteDto.Data.IdGrado:"";
                                    newRow["APELLIDOS_Y_NOMBRES"] = clienteDto.Data != null ? clienteDto.Data.PrimerApellido + " " + clienteDto.Data.SegundoApellido+" "+ clienteDto.Data.PrimerNombre + " " + clienteDto.Data.SegundoNombre:"";
                                    newRow["CODIGO_MILITAR"] = generacionArchivo.COD_MILITAR;
                                    newRow["No_CEDULA"] = generacionArchivo.NUMERO_IDENTIFICACION;
                                    newRow["FUERZA"] = clienteDto.Data != null ? clienteDto.Data.IdFuerza : "";
                                    newRow["CODIGO_DESTINO"] = generacionArchivo.CODIGO_EMPRESA;
                                    newRow["NUMERO_LIBRANZA"] = generacionArchivo.NVD_NOVEDADES_ALTA_BAJA;
                                    newRow["ANO_MES_DIA_LIBRANZA"] = string.Format(25 + generacionArchivo.FECHA_PERIODO.ToString("dd/MM/yyyy"));//FECHA_PAGO //Siempre 25 de cada mes
                                    newRow["CUOTA_MENSUAL"] = generacionArchivo.PORCENTAJE_DESCUENTO;
                                    newRow["TOTAL_A_PAGAR"] = generacionArchivo.PORCENTAJE_DESCUENTO;
                                    newRow["FECHA_INICIO"] = generacionArchivo.FECHA_INICIO.ToString("dd/MM/yyyy");
                                    newRow["FECHA_TERMINO"] = generacionArchivo.FECHA_FIN.ToString("dd/MM/yyyy");
                                    newRow["TOTAL_CUOTA"] = generacionArchivo.ES_ALTA ? generacionArchivo.NUMERO_CUOTAS:"0";
                                    newRow["CONCEPTO"] = generacionArchivo.ES_ALTA?"AFILICIÓN":"CANCELACIÓN";
                                    dataTablePFFMM.Rows.Add(newRow);
                                }
                            }
                            return new Response<GeneracionArchivosResponse> { Data = new() { ArchivoB64 = await _reportesBusinessLogic.GenerarExcel(dataTablePFFMM, "Caja Promotora"), NameFile = "Pensionados FFMM.xlsx" } };
                        }
                        else
                        {
                            //Ahorro Obligatorio 9103                   
                            if (generarArchivoRequest.IdConceptoNovedad == 1)
                            {
                                //Encabezado
                                line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "3" + System.Environment.NewLine;
                                nameFile = generarArchivoRequest.Codigo + ".txt";
                                foreach (var items in dataGroup)
                                {
                                    foreach (var item in items.ToList())
                                    {

                                        var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                        if (porDes >= 10)
                                        {

                                            numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                        }
                                        else
                                        {
                                            numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                        }

                                        string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                        // Obtener el año y mes desde la variable x
                                        int anio_x = int.Parse(x.Substring(0, 4));
                                        int mes_x = int.Parse(x.Substring(4, 2));

                                        // Crear la fecha del primer día del mes correspondiente a la variable x
                                        DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                        DateTime fechaFin = item.FECHA_PERIODO;
                                        fechaFin = fechaFin.AddYears(14);

                                        if (item.COD_MILITAR == null || item.COD_MILITAR == "")
                                        {
                                            identificacion = item.NUMERO_IDENTIFICACION.PadLeft(11, '0');
                                        }
                                        else
                                        {
                                            if (identificacion.Length == 11)
                                            {
                                                identificacion = item.COD_MILITAR;
                                            }
                                            else
                                            {
                                                identificacion = item.COD_MILITAR.PadLeft(11, '0');
                                            }

                                        }

                                        line += ($"{identificacion}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{string.Format(item.VALOR_DESCUENTO.ToString("D12") == "04000" ? "04500" : item.VALOR_DESCUENTO.ToString("D12"))}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                    }
                                }

                            }
                            else if (generarArchivoRequest.IdConceptoNovedad == 2)
                            {
                                //Ahorro Voluntario 9098
                                //Encabezado                                                            
                                line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "3" + System.Environment.NewLine;

                                nameFile = generarArchivoRequest.Codigo + ".txt";
                                foreach (var items in dataGroup)
                                {
                                    foreach (var item in items.ToList())
                                    {
                                        var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                        if (porDes >= 10)
                                        {

                                            numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                        }
                                        else
                                        {
                                            numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                        }

                                        string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                        // Obtener el año y mes desde la variable x
                                        int anio_x = int.Parse(x.Substring(0, 4));
                                        int mes_x = int.Parse(x.Substring(4, 2));

                                        // Crear la fecha del primer día del mes correspondiente a la variable x
                                        DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                        DateTime fechaFin = item.FECHA_PERIODO;
                                        fechaFin = fechaFin.AddYears(14);

                                        if (item.COD_MILITAR == null || item.COD_MILITAR == "")
                                        {
                                            identificacion = item.NUMERO_IDENTIFICACION.PadLeft(11, '0');
                                        }
                                        else
                                        {
                                            if (identificacion.Length == 11)
                                            {
                                                identificacion = item.COD_MILITAR;
                                            }
                                            else
                                            {
                                                identificacion = item.COD_MILITAR.PadLeft(11, '0');
                                            }

                                        }

                                        line += ($"{identificacion}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{string.Format(item.FECHA_PERIODO.ToString("yyyyMM"))}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                    }
                                }

                            }
                            else if (generarArchivoRequest.IdConceptoNovedad == 3)
                            {
                                //Fondo Solidaridad 9405
                                //Encabezado                                                            
                                line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "3" + System.Environment.NewLine;
                                nameFile = generarArchivoRequest.Codigo + ".txt";
                                foreach (var items in dataGroup)
                                {
                                    foreach (var item in items.ToList())
                                    {
                                        var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                        if (porDes >= 10)
                                        {

                                            numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                        }
                                        else
                                        {
                                            numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                        }

                                        string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                        // Obtener el año y mes desde la variable x
                                        int anio_x = int.Parse(x.Substring(0, 4));
                                        int mes_x = int.Parse(x.Substring(4, 2));

                                        // Crear la fecha del primer día del mes correspondiente a la variable x
                                        DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                        DateTime fechaFin = item.FECHA_FIN;
                                        fechaFin = fechaFin.AddYears(14);

                                        if (item.COD_MILITAR == null || item.COD_MILITAR == "")
                                        {
                                            identificacion = item.NUMERO_IDENTIFICACION.PadLeft(11, '0');
                                        }
                                        else
                                        {
                                            if (identificacion.Length == 11)
                                            {
                                                identificacion = item.COD_MILITAR;
                                            }
                                            else
                                            {
                                                identificacion = item.COD_MILITAR.PadLeft(11, '0');
                                            }

                                        }

                                        line += ($"{identificacion}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{item.FECHA_PERIODO.ToString("yyyyMM")}" + System.Environment.NewLine);
                                    }
                                }

                            }
                            else if (generarArchivoRequest.IdConceptoNovedad == 5)
                            {
                                //Ahorro AVAC 984Q
                                //Encabezado                                                            
                                line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "3" + System.Environment.NewLine;
                                nameFile = generarArchivoRequest.Codigo + ".txt";
                                foreach (var items in dataGroup)
                                {
                                    foreach (var item in items.ToList())
                                    {
                                        var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                        if (porDes >= 10)
                                        {

                                            numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                        }
                                        else
                                        {
                                            numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                        }

                                        string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                        // Obtener el año y mes desde la variable x
                                        int anio_x = int.Parse(x.Substring(0, 4));
                                        int mes_x = int.Parse(x.Substring(4, 2));

                                        // Crear la fecha del primer día del mes correspondiente a la variable x
                                        DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                        DateTime fechaFin = item.FECHA_PERIODO;
                                        fechaFin = fechaFin.AddYears(14);

                                        if (item.COD_MILITAR == null || item.COD_MILITAR == "")
                                        {
                                            identificacion = item.NUMERO_IDENTIFICACION.PadLeft(11, '0');
                                        }
                                        else
                                        {
                                            if (identificacion.Length == 11)
                                            {
                                                identificacion = item.COD_MILITAR;
                                            }
                                            else
                                            {
                                                identificacion = item.COD_MILITAR.PadLeft(11, '0');
                                            }

                                        }

                                        line += ($"{identificacion}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                    }
                                }

                            }
                            else if (generarArchivoRequest.IdConceptoNovedad == 6)
                            {
                                //Ahorro AVC 982B
                                //Encabezado                                                            
                                line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "3" + System.Environment.NewLine;
                                nameFile = generarArchivoRequest.Codigo + ".txt";
                                foreach (var items in dataGroup)
                                {
                                    foreach (var item in items.ToList())
                                    {
                                        var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                        if (porDes >= 10)
                                        {

                                            numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                        }
                                        else
                                        {
                                            numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                        }

                                        string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                        // Obtener el año y mes desde la variable x
                                        int anio_x = int.Parse(x.Substring(0, 4));
                                        int mes_x = int.Parse(x.Substring(4, 2));

                                        // Crear la fecha del primer día del mes correspondiente a la variable x
                                        DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                        DateTime fechaFin = item.FECHA_PERIODO;
                                        fechaFin = fechaFin.AddYears(14);

                                        if (item.COD_MILITAR == null || item.COD_MILITAR == "")
                                        {
                                            identificacion = item.NUMERO_IDENTIFICACION.PadLeft(11, '0');
                                        }
                                        else
                                        {
                                            if (identificacion.Length == 11)
                                            {
                                                identificacion = item.COD_MILITAR;
                                            }
                                            else
                                            {
                                                identificacion = item.COD_MILITAR.PadLeft(11, '0');
                                            }
                                        }

                                        line += ($"{identificacion}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                    }
                                }

                            }
                        }
                        break;
                    #endregion
                    //Comando General FFMM
                    #region Comando General FFMM
                    case 9:                                                
                        nameFilePdf = "COMANDO GENERAL FFMM";  
                        fileProcess = await CreateReportHtml
                                (
                                    data,
                                    htmlTemplateGlobal,
                                    await _catalogosBusinessLogic.ObtenerUnidadesEjecutoras(),                                      
                                    nameFilePdf,
                                    generarArchivoRequest
                                );
                        
                        pdf = GeneratePdfReportFromHtml(fileProcess);
                        return new Response<GeneracionArchivosResponse> { Data = new() { ArchivoB64 = Convert.ToBase64String(pdf), NameFile = nameFilePdf + ".pdf" } };

                    #endregion
                    //Armada Nacional
                    #region ArmadaNacional
                    case 10:
                        //Ahorro Obligatorio 9103                   
                        if (generarArchivoRequest.IdConceptoNovedad == 1)
                        {
                            //Encabezado
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.Codigo + ".txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 2)
                        {
                            //Ahorro Voluntario 9098
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;

                            nameFile = generarArchivoRequest.Codigo + ".txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : item.FECHA_INICIO.ToString("yyyyMM"))}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 3)
                        {
                            //Fondo Solidaridad 9405
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.Codigo + ".txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_FIN;
                                    fechaFin = fechaFin.AddYears(14);

                                    line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_INICIO.ToString("yyyyMM")}{item.FECHA_PERIODO.ToString("yyyyMM")}" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 4)
                        {
                            //Ahorro AVAC 984Q
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.Codigo + ".txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 6)
                        {
                            //Ahorro AVC 982B
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.Codigo + ".txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                }
                            }
                                
                        }

                        break;
                    #endregion 
                    //Fuerza Aerea
                    #region FuerzaAerea
                    case 11:
                        if (generarArchivoRequest.OtroFormato)
                        {
                            nameFilePdf = "FUERZA AEROESPACIAL DE COLOMBIA";
                            fileProcess = await CreateReportHtml
                                    (
                                        data,
                                        htmlTemplateGlobal,
                                        await _catalogosBusinessLogic.ObtenerUnidadesEjecutoras(),
                                        nameFilePdf,
                                        generarArchivoRequest
                                    );

                            pdf = GeneratePdfReportFromHtml(fileProcess);
                            return new Response<GeneracionArchivosResponse> { Data = new() { ArchivoB64 = Convert.ToBase64String(pdf), NameFile = nameFilePdf + ".pdf" } };
                        }
                        else
                        {
                            //Ahorro Obligatorio 9103                   
                            if (generarArchivoRequest.IdConceptoNovedad == 1)
                            {
                                //Encabezado
                                line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;
                                nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                                foreach (var items in dataGroup)
                                {
                                    foreach (var item in items.ToList())
                                    {

                                        var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                        if (porDes >= 10)
                                        {

                                            numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                        }
                                        else
                                        {
                                            numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                        }

                                        string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                        // Obtener el año y mes desde la variable x
                                        int anio_x = int.Parse(x.Substring(0, 4));
                                        int mes_x = int.Parse(x.Substring(4, 2));

                                        // Crear la fecha del primer día del mes correspondiente a la variable x
                                        DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                        DateTime fechaFin = item.FECHA_PERIODO;
                                        fechaFin = fechaFin.AddYears(14);

                                        line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                    }
                                }

                            }
                            else if (generarArchivoRequest.IdConceptoNovedad == 2)
                            {
                                //Ahorro Voluntario 9098
                                //Encabezado                                                            
                                line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;

                                nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                                foreach (var items in dataGroup)
                                {
                                    foreach (var item in items.ToList())
                                    {
                                        var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                        if (porDes >= 10)
                                        {

                                            numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                        }
                                        else
                                        {
                                            numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                        }

                                        string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                        // Obtener el año y mes desde la variable x
                                        int anio_x = int.Parse(x.Substring(0, 4));
                                        int mes_x = int.Parse(x.Substring(4, 2));

                                        // Crear la fecha del primer día del mes correspondiente a la variable x
                                        DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                        DateTime fechaFin = item.FECHA_PERIODO;
                                        fechaFin = fechaFin.AddYears(14);

                                        line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : item.FECHA_INICIO.ToString("yyyyMM"))}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                    }
                                }

                            }
                            else if (generarArchivoRequest.IdConceptoNovedad == 3)
                            {
                                //Fondo Solidaridad 9405
                                //Encabezado                                                            
                                line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;
                                nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                                foreach (var items in dataGroup)
                                {
                                    foreach (var item in items.ToList())
                                    {
                                        var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                        if (porDes >= 10)
                                        {

                                            numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                        }
                                        else
                                        {
                                            numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                        }

                                        string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                        // Obtener el año y mes desde la variable x
                                        int anio_x = int.Parse(x.Substring(0, 4));
                                        int mes_x = int.Parse(x.Substring(4, 2));

                                        // Crear la fecha del primer día del mes correspondiente a la variable x
                                        DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                        DateTime fechaFin = item.FECHA_FIN;
                                        fechaFin = fechaFin.AddYears(14);

                                        line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_INICIO.ToString("yyyyMM")}{item.FECHA_PERIODO.ToString("yyyyMM")}" + System.Environment.NewLine);
                                    }
                                }

                            }
                            else if (generarArchivoRequest.IdConceptoNovedad == 4)
                            {
                                //Ahorro AVAC 984Q
                                //Encabezado                                                            
                                line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;
                                nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                                foreach (var items in dataGroup)
                                {
                                    foreach (var item in items.ToList())
                                    {
                                        var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                        if (porDes >= 10)
                                        {

                                            numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                        }
                                        else
                                        {
                                            numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                        }

                                        string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                        // Obtener el año y mes desde la variable x
                                        int anio_x = int.Parse(x.Substring(0, 4));
                                        int mes_x = int.Parse(x.Substring(4, 2));

                                        // Crear la fecha del primer día del mes correspondiente a la variable x
                                        DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                        DateTime fechaFin = item.FECHA_PERIODO;
                                        fechaFin = fechaFin.AddYears(14);

                                        line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                    }
                                }

                            }
                            else if (generarArchivoRequest.IdConceptoNovedad == 6)
                            {
                                //Ahorro AVC 982B
                                //Encabezado                                                            
                                line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "1" + System.Environment.NewLine;
                                nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                                foreach (var items in dataGroup)
                                {
                                    foreach (var item in items.ToList())
                                    {
                                        var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                        if (porDes >= 10)
                                        {

                                            numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                        }
                                        else
                                        {
                                            numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                        }

                                        string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                        // Obtener el año y mes desde la variable x
                                        int anio_x = int.Parse(x.Substring(0, 4));
                                        int mes_x = int.Parse(x.Substring(4, 2));

                                        // Crear la fecha del primer día del mes correspondiente a la variable x
                                        DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                        DateTime fechaFin = item.FECHA_PERIODO;
                                        fechaFin = fechaFin.AddYears(14);

                                        line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                    }
                                }

                            }
                        }
                        break;
                    #endregion
                    //Caja Sueldos de Retiro Policía (CASUR)
                    #region Caja Sueldos de Retiro Policía (CASUR)
                    case 12:
                        if (generarArchivoRequest.OtroFormato)
                        {
                            foreach (var items in dataGroup.ToList())
                            {
                                foreach(var item in items)
                                {
                                    var cliente = await _iClientRequestProvider.ObtenerInfoCliente(item.TIPO_IDENTIFICACION_N, item.NUMERO_IDENTIFICACION);
                                    decimal cuota = 0;
                                    if (generarArchivoRequest.IdConceptoNovedad == 1)
                                    {
                                        decimal porcentaje = Convert.ToDecimal(4.5);
                                        if (cliente.Data != null)
                                        {
                                            if (cliente.Data.Cuentas.Count > 0)
                                            {
                                                foreach (var cuenta in cliente.Data.Cuentas.OrderBy(x => x.CuentaId))
                                                {
                                                    cuota = cuenta.AsignacionSalarial * (porcentaje / 100);
                                                    cuota = cuota > 0 ? Math.Round(cuota, 2) : cuota;
                                                }
                                            }
                                        }
                                        nameFile = generarArchivoRequest.Codigo + ".txt";
                                        line += ($"{item.FUERZA_CAPROVIMPO},{item.NVD_NOVEDADES_ALTA_BAJA},{item.NUMERO_IDENTIFICACION},{(item.ES_ALTA == true ? cuota : 0)},{(item.ES_ALTA == true ? item.NUMERO_CUOTAS : 0 )},{(item.ES_ALTA == true ? item.TIPO_NOVEDAD : "R")}") + System.Environment.NewLine;
                                      
                                    }
                                    else if (generarArchivoRequest.IdConceptoNovedad == 3)
                                    {
                                        decimal porcentaje = Convert.ToDecimal(7);
                                        if (cliente.Data != null)
                                        {
                                            if (cliente.Data.Cuentas.Count > 0)
                                            {
                                                foreach (var cuenta in cliente.Data.Cuentas.OrderBy(x => x.CuentaId))
                                                {
                                                    cuota = cuenta.AsignacionSalarial * (porcentaje / 100);
                                                    cuota = cuota > 0 ? Math.Round(cuota, 2) : cuota;
                                                }
                                            }
                                        }
                                        nameFile = generarArchivoRequest.Codigo + ".txt";
                                        line += ($"{item.FUERZA_CAPROVIMPO},{item.NVD_NOVEDADES_ALTA_BAJA},{item.NUMERO_IDENTIFICACION},{(item.ES_ALTA == true ? cuota : 0)},{(item.ES_ALTA == true ? 1 : 0)},{(item.ES_ALTA == true ? item.TIPO_NOVEDAD : "R")}") + System.Environment.NewLine;

                                        
                                    }
                                    else if (generarArchivoRequest.IdConceptoNovedad == 5)
                                    {
                                        decimal porcentaje5 = Convert.ToDecimal(8);
                                        if (cliente.Data != null)
                                        {
                                            if (cliente.Data.Cuentas.Count > 0)
                                            {
                                                foreach (var cuenta in cliente.Data.Cuentas.OrderBy(x => x.CuentaId))
                                                {
                                                    cuota = cuenta.AsignacionSalarial * (porcentaje5 / 100);
                                                    cuota = cuota > 0 ? Math.Round(cuota, 2) : cuota;
                                                }
                                            }
                                        }
                                        nameFile = generarArchivoRequest.Codigo + ".txt";
                                        line += ($"{item.FUERZA_CAPROVIMPO},{item.NVD_NOVEDADES_ALTA_BAJA},{item.NUMERO_IDENTIFICACION},{(item.ES_ALTA == true ? cuota : 0)},{(item.ES_ALTA == true ? item.NUMERO_CUOTAS : 0)},{(item.ES_ALTA == true ? item.TIPO_NOVEDAD : "R")}") + System.Environment.NewLine;
 
                                    }                        
                                    if (generarArchivoRequest.IdConceptoNovedad == 6)
                                    {
                                        decimal porcentaje6 = Convert.ToDecimal(10);
                                        if (cliente.Data != null)
                                        {
                                            if (cliente.Data.Cuentas.Count > 0)
                                            {
                                                foreach (var cuenta in cliente.Data.Cuentas.OrderBy(x => x.CuentaId))
                                                {
                                                    cuota = cuenta.AsignacionSalarial * (porcentaje6 / 100);
                                                    cuota = cuota > 0 ? Math.Round(cuota, 2) : cuota;
                                                }
                                            }
                                        }
                                        nameFile = generarArchivoRequest.Codigo + ".txt";
                                        line += ($"{item.FUERZA_CAPROVIMPO},{Regex.Replace(item.NVD_NOVEDADES_ALTA_BAJA, "[^0-9]", "")},{item.NUMERO_IDENTIFICACION},{(item.ES_ALTA == true ? cuota : 0)},{(item.ES_ALTA == true ? item.NUMERO_CUOTAS : 0)},{(item.ES_ALTA == true ? item.TIPO_NOVEDAD : "R")}") + System.Environment.NewLine;
                                        
                                    }
                                }
                            }
                            break;
                        }
                        else
                        {
                            DataTable dataTableCasur = new DataTable();
                            dataTableCasur.Columns.Add("CEDULA", typeof(string));
                            dataTableCasur.Columns.Add("VALOR_CUOTA", typeof(string));
                            dataTableCasur.Columns.Add("CUOTAS_PENDIENTES", typeof(string));
                            string codigo = string.Empty;

                            if (generarArchivoRequest.IdConceptoNovedad == 1 || generarArchivoRequest.IdConceptoNovedad == 5 || generarArchivoRequest.IdConceptoNovedad == 6)
                            {
                                foreach (var items in dataGroup.ToList())
                                {
                                    foreach (GeneracionArchivos generacionArchivo in items.ToList())
                                    {
                                        var cliente = await _iClientRequestProvider.ObtenerInfoCliente(generacionArchivo.TIPO_IDENTIFICACION_N, generacionArchivo.NUMERO_IDENTIFICACION);
                                        decimal cuota = 0;
                                        switch (generarArchivoRequest.IdConceptoNovedad)
                                        {
                                            case 1:
                                                decimal porcentaje = Convert.ToDecimal(4.5);
                                                if (cliente.Data != null)
                                                {
                                                    if (cliente.Data.Cuentas.Count > 0)
                                                    {
                                                        foreach (var cuenta in cliente.Data.Cuentas.OrderBy(x => x.CuentaId))
                                                        {
                                                            cuota = cuenta.AsignacionSalarial * (porcentaje / 100);
                                                            cuota = cuota > 0 ? Math.Round(cuota, 2) : cuota;
                                                        }
                                                    }
                                                }
                                                break;
                                            case 5:
                                                decimal porcentaje5 = Convert.ToDecimal(8);
                                                if (cliente.Data != null)
                                                {
                                                    if (cliente.Data.Cuentas.Count > 0)
                                                    {
                                                        foreach (var cuenta in cliente.Data.Cuentas.OrderBy(x => x.CuentaId))
                                                        {
                                                            cuota = cuenta.AsignacionSalarial * (porcentaje5 / 100);
                                                            cuota = cuota > 0 ? Math.Round(cuota, 2) : cuota;
                                                        }
                                                    }
                                                }
                                                break;
                                            case 6:
                                                decimal porcentaje6 = Convert.ToDecimal(10);
                                                if (cliente.Data != null)
                                                {
                                                    if (cliente.Data.Cuentas.Count > 0)
                                                    {
                                                        foreach (var cuenta in cliente.Data.Cuentas.OrderBy(x => x.CuentaId))
                                                        {
                                                            cuota = cuenta.AsignacionSalarial * (porcentaje6 / 100);
                                                            cuota = cuota > 0 ? Math.Round(cuota, 2) : cuota;
                                                        }
                                                    }
                                                }
                                                break;
                                        }
                                        codigo = generacionArchivo.NVD_NOVEDADES_ALTA_BAJA;
                                        DataRow newRow = dataTableCasur.NewRow();
                                        newRow["CEDULA"] = generacionArchivo.NUMERO_IDENTIFICACION;
                                        newRow["VALOR_CUOTA"] = generacionArchivo.ES_ALTA ? cuota : "0";
                                        newRow["CUOTAS_PENDIENTES"] = generacionArchivo.ES_ALTA ? generacionArchivo.NUMERO_CUOTAS : "0";
                                        dataTableCasur.Rows.Add(newRow);
                                    }
                                }
                            }
                            if (generarArchivoRequest.IdConceptoNovedad == 3)
                            {
                                foreach (var items in dataGroup.ToList())
                                {
                                    foreach (GeneracionArchivos generacionArchivo in items.ToList())
                                    {
                                        var cliente = await _iClientRequestProvider.ObtenerInfoCliente(generacionArchivo.TIPO_IDENTIFICACION_N, generacionArchivo.NUMERO_IDENTIFICACION);
                                        decimal cuota = 0;
                                        decimal porcentaje = Convert.ToDecimal(7);
                                        if (cliente.Data != null)
                                        {
                                            if (cliente.Data.Cuentas.Count > 0)
                                            {
                                                foreach (var cuenta in cliente.Data.Cuentas.OrderBy(x => x.CuentaId))
                                                {
                                                    cuota = cuenta.AsignacionSalarial * (porcentaje / 100);
                                                    cuota = cuota > 0 ? Math.Round(cuota, 2) : cuota;
                                                }
                                            }
                                        }
                                        codigo = generacionArchivo.NVD_NOVEDADES_ALTA_BAJA;
                                        DataRow newRow = dataTableCasur.NewRow();
                                        newRow["CEDULA"] = generacionArchivo.NUMERO_IDENTIFICACION;
                                        newRow["VALOR_CUOTA"] = cuota;
                                        newRow["CUOTAS_PENDIENTES"] = "1";
                                        dataTableCasur.Rows.Add(newRow);
                                    }
                                }
                            }

                            return new Response<GeneracionArchivosResponse> { Data = new() { ArchivoB64 = await _reportesBusinessLogic.GenerarExcel(dataTableCasur, "Caja Promotora"), NameFile = "planilla_" + codigo + ".xlsx" } };

                        }
                   

                    #endregion
                    //DIRECCIÓN GENERAL SANIDAD MILITAR
                    #region DIRECCIÓN GENERAL SANIDAD MILITAR
                    case 13:
                        
                        nameFilePdf = "DIRECCIÓN GENERAL SANIDAD MILITAR";                        

                        fileProcess = await CreateReportHtml
                                (
                                    data,
                                    htmlTemplateGlobal,
                                    await _catalogosBusinessLogic.ObtenerUnidadesEjecutoras(),
                                    nameFilePdf,
                                    generarArchivoRequest
                                );
                        
                        pdf = GeneratePdfReportFromHtml(fileProcess);
                        return new Response<GeneracionArchivosResponse> { Data = new() { ArchivoB64 = Convert.ToBase64String(pdf), NameFile = nameFilePdf + ".pdf" } };

                        
                    #endregion
                    //DIRECCIÓN GENERAL SANIDAD EJÉRCITO
                    #region DIRECCIÓN GENERAL SANIDAD EJÉRCITO
                    case 14:
                        nameFilePdf = "DIRECCIÓN GENERAL SANIDAD EJÉRCITO";                        

                        fileProcess = await CreateReportHtml
                                (
                                    data,
                                    htmlTemplateGlobal,
                                    await _catalogosBusinessLogic.ObtenerUnidadesEjecutoras(),
                                    nameFilePdf,
                                    generarArchivoRequest
                                );
                        
                        pdf = GeneratePdfReportFromHtml(fileProcess);
                        return new Response<GeneracionArchivosResponse> { Data = new() { ArchivoB64 = Convert.ToBase64String(pdf), NameFile = nameFilePdf + ".pdf" } };

                    #endregion
                    //DIRECCIÓN GENERAL SANIDAD ARMADA
                    #region DIRECCIÓN GENERAL SANIDAD ARMADA
                    case 15:
                        nameFilePdf = "DIRECCIÓN GENERAL SANIDAD ARMADA";
                        
                            fileProcess = await CreateReportHtml
                                (
                                    data,
                                    htmlTemplateGlobal,
                                    await _catalogosBusinessLogic.ObtenerUnidadesEjecutoras(),
                                    nameFilePdf,
                                    generarArchivoRequest
                                );
                        
                        pdf = GeneratePdfReportFromHtml(fileProcess);
                        return new Response<GeneracionArchivosResponse> { Data = new() { ArchivoB64 = Convert.ToBase64String(pdf), NameFile = nameFilePdf + ".pdf" } };

                    #endregion
                    //DIRECCIÓN GENERAL SANIDAD FUERZA AÉREA
                    #region DIRECCIÓN GENERAL SANIDAD FUERZA AÉREA
                    case 16:
                        nameFilePdf = "DIRECCIÓN GENERAL SANIDAD FUERZA AÉREA";
                       
                        fileProcess = await CreateReportHtml
                                (
                                    data,
                                    htmlTemplateGlobal,
                                    await _catalogosBusinessLogic.ObtenerUnidadesEjecutoras(),
                                    nameFilePdf,
                                    generarArchivoRequest
                                );
                        
                        pdf = GeneratePdfReportFromHtml(fileProcess);
                        return new Response<GeneracionArchivosResponse> { Data = new() { ArchivoB64 = Convert.ToBase64String(pdf), NameFile = nameFilePdf + ".pdf" } };
 
                    #endregion
                    //CAJA DE RETIRO DE LAS FFMM
                    #region CAJA DE RETIRO DE LAS FFMM
                    case 17:
                        nameFile = "np.txt";
                       
                        foreach (var items in dataGroup)
                        {
                            foreach (var item in items.ToList())
                            {
                                decimal descuento = 0;
                                var cliente = await _iClientRequestProvider.ObtenerInfoCliente(item.TIPO_IDENTIFICACION_N, item.NUMERO_IDENTIFICACION);
                                if (generarArchivoRequest.IdConceptoNovedad == 1)
                                {
                                    decimal porcentaje =Convert.ToDecimal(4.5);
                                    if (cliente.Data != null)
                                    {
                                        if (cliente.Data.Cuentas.Count > 0)
                                        {
                                            foreach (var cuenta in cliente.Data.Cuentas.OrderBy(x=>x.CuentaId))
                                            {
                                                descuento = cuenta.AsignacionSalarial * (porcentaje/ 100);
                                                descuento = descuento > 0 ? Math.Round(descuento, 2) : descuento;
                                            }
                                        }
                                    }
                                    line += ($"{item.FUERZA_CAJAHONOR},{item.NVD_NOVEDADES_ALTA_BAJA},{item.NUMERO_IDENTIFICACION},{descuento},{(generarArchivoRequest.IdConceptoNovedad == 3 ? "1" : item.ES_ALTA ? item.NUMERO_CUOTAS : "0")},{(item.ES_ALTA ? item.I : "R")},{item.TIPO_IDENTIFICACION},{item.NUMERO_IDENTIFICACION}" + System.Environment.NewLine);

                                }
                               else if (generarArchivoRequest.IdConceptoNovedad == 3)
                                {
                                    decimal porcentaje = Convert.ToDecimal(7);
                                    if (cliente.Data != null)
                                    {
                                        if (cliente.Data.Cuentas.Count > 0)
                                        {
                                            foreach (var cuenta in cliente.Data.Cuentas.OrderBy(x => x.CuentaId))
                                            {
                                                descuento = cuenta.AsignacionSalarial * (porcentaje / 100);
                                                descuento = descuento > 0 ? Math.Round(descuento, 2) : descuento;
                                            }
                                        }
                                    }
                                    line += ($"{item.FUERZA_CAJAHONOR},{item.NVD_NOVEDADES_ALTA_BAJA},{item.NUMERO_IDENTIFICACION},{descuento},{(generarArchivoRequest.IdConceptoNovedad == 3 ? "1" : item.ES_ALTA ? item.NUMERO_CUOTAS : "0")},{(item.ES_ALTA ? item.I : "R")},{item.TIPO_IDENTIFICACION},{item.NUMERO_IDENTIFICACION}" + System.Environment.NewLine);

                                }
                                else if (generarArchivoRequest.IdConceptoNovedad == 5)
                                {
                                    decimal porcentaje = Convert.ToDecimal(8);
                                    if (cliente.Data != null)
                                    {
                                        if (cliente.Data.Cuentas.Count > 0)
                                        {
                                            foreach (var cuenta in cliente.Data.Cuentas.OrderBy(x => x.CuentaId))
                                            {
                                                descuento = cuenta.AsignacionSalarial * (porcentaje / 100);
                                                descuento = descuento > 0 ? Math.Round(descuento, 2) : descuento;
                                            }
                                        }
                                    }
                                    line += ($"{item.FUERZA_CAJAHONOR},{item.NVD_NOVEDADES_ALTA_BAJA},{item.NUMERO_IDENTIFICACION},{descuento},{(generarArchivoRequest.IdConceptoNovedad == 3 ? "1" : item.ES_ALTA ? item.NUMERO_CUOTAS : "0")},{(item.ES_ALTA ? item.I : "R")},{item.TIPO_IDENTIFICACION},{item.NUMERO_IDENTIFICACION}" + System.Environment.NewLine);

                                }
                                else if (generarArchivoRequest.IdConceptoNovedad == 6)
                                {
                                    decimal porcentaje = Convert.ToDecimal(10);
                                    if (cliente.Data != null)
                                    {
                                        if (cliente.Data.Cuentas.Count > 0)
                                        {
                                            foreach (var cuenta in cliente.Data.Cuentas.OrderBy(x => x.CuentaId))
                                            {
                                                descuento = cuenta.AsignacionSalarial * (porcentaje / 100);
                                                descuento = descuento > 0 ? Math.Round(descuento, 2) : descuento;
                                            }
                                        }
                                    }
                                    line += ($"{item.FUERZA_CAJAHONOR},{item.NVD_NOVEDADES_ALTA_BAJA},{item.NUMERO_IDENTIFICACION},{descuento},{(generarArchivoRequest.IdConceptoNovedad == 3 ? "1" : item.ES_ALTA ? item.NUMERO_CUOTAS : "0")},{(item.ES_ALTA ? item.I : "R")},{item.TIPO_IDENTIFICACION},{item.NUMERO_IDENTIFICACION}" + System.Environment.NewLine);

                                }
                            }
                        }
                        break;
                        //nameFilePdf = "CAJA DE RETIRO DE LAS FFMM";                        

                        //foreach (var item in data.ToList())
                        //{
                        //    fileProcess = await CreateReportHtml
                        //        (
                        //            data,
                        //            htmlTemplateGlobal,
                        //            await _catalogosBusinessLogic.ObtenerFuerzas(),
                        //            nameFilePdf,
                        //            generarArchivoRequest
                        //        );
                        //}
                        //pdf = GeneratePdfReportFromHtml(fileProcess);
                        //return new Response<GeneracionArchivosResponse> { Data = new() { ArchivoB64 = Convert.ToBase64String(pdf), NameFile = nameFilePdf + ".pdf" } };

                        
                    #endregion
                    //Soldados Ejercito Nacional
                    #region SoldadosEjercitoNacional
                    case 18:
                        //Ahorro Obligatorio 9103
                        if (generarArchivoRequest.IdConceptoNovedad == 1)
                        {
                            //Encabezado
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "3" + System.Environment.NewLine;
                            nameFile =generarArchivoRequest.NombreArchivo==null? generarArchivoRequest.Codigo + ".txt":generarArchivoRequest.NombreArchivo;
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    if (item.COD_MILITAR == null || item.COD_MILITAR == "")
                                    {
                                        identificacion = item.NUMERO_IDENTIFICACION;
                                    }
                                    else
                                    {
                                        identificacion = item.COD_MILITAR;
                                    }

                                    line += ($"{identificacion.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 2)
                        {
                            //Ahorro Voluntario 9098
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "3" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    if (item.COD_MILITAR == null || item.COD_MILITAR == "")
                                    {
                                        identificacion = item.NUMERO_IDENTIFICACION;
                                    }
                                    else
                                    {
                                        identificacion = item.COD_MILITAR;
                                    }

                                    line += ($"{identificacion.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                }
                            }
                               
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 3)
                        {
                            //Fondo Solidaridad 9405
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "3" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_FIN;
                                    fechaFin = fechaFin.AddYears(14);

                                    if (item.COD_MILITAR == null || item.COD_MILITAR == "")
                                    {
                                        identificacion = item.NUMERO_IDENTIFICACION;
                                    }
                                    else
                                    {
                                        identificacion = item.COD_MILITAR;
                                    }

                                    line += ($"{identificacion.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{item.FECHA_PERIODO.ToString("yyyyMM")} " + System.Environment.NewLine);
                                }
                            }
                               
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 4)
                        {
                            //Ahorro AVAC 984Q
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "3" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    if (item.COD_MILITAR == null || item.COD_MILITAR == "")
                                    {
                                        identificacion = item.NUMERO_IDENTIFICACION;
                                    }
                                    else
                                    {
                                        identificacion = item.COD_MILITAR;
                                    }

                                    line += ($"{identificacion.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))} " + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 6)
                        {
                            //Ahorro AVC 982B
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "3" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.NombreArchivo == null ? generarArchivoRequest.Codigo + ".txt" : generarArchivoRequest.NombreArchivo;
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    if (item.COD_MILITAR == null || item.COD_MILITAR == "")
                                    {
                                        identificacion = item.NUMERO_IDENTIFICACION;
                                    }
                                    else
                                    {
                                        identificacion = item.COD_MILITAR;
                                    }

                                    line += ($"{identificacion.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{string.Format(item.ES_ALTA ? item.FECHA_PERIODO.ToString("yyyyMM") : item.FECHA_INICIO.ToString("yyyyMM"))}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))} " + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        break;
                    #endregion
                    //Soldados Armada Nacional
                    #region SoldadosArmadaNacional
                    case 19:
                        //Ahorro Obligatorio 9103
                        if (generarArchivoRequest.IdConceptoNovedad == 1)
                        {
                            //Encabezado
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "3" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.Codigo + ".txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 2)
                        {
                            //Ahorro Voluntario 9098
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "3" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.Codigo + ".txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))}" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 3)
                        {
                            //Fondo Solidaridad 9405
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "3" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.Codigo + ".txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_FIN;
                                    fechaFin = fechaFin.AddYears(14);

                                    line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{item.FECHA_PERIODO.ToString("yyyyMM")} " + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 4)
                        {
                            //Ahorro AVAC 984Q
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "3" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.Codigo + ".txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{item.FECHA_PERIODO.ToString("yyyyMM")}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))} " + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 6)
                        {
                            //Ahorro AVC 982B
                            //Encabezado                                                            
                            line = "000000000000000" + data.ToList().Count.ToString().PadLeft(6, '0') + "REGISTRO DE CONTROL" + "      " + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "     " + "3" + System.Environment.NewLine;
                            nameFile = generarArchivoRequest.Codigo + ".txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    var porDes = (int)item.PORCENTAJE_DESCUENTO;
                                    if (porDes >= 10)
                                    {

                                        numeroFormateado = porDes.ToString().PadLeft(2, '0') + "000";
                                    }
                                    else
                                    {
                                        numeroFormateado = porDes.ToString("D1").PadLeft(2, '0') + "000";
                                    }

                                    string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                    // Obtener el año y mes desde la variable x
                                    int anio_x = int.Parse(x.Substring(0, 4));
                                    int mes_x = int.Parse(x.Substring(4, 2));

                                    // Crear la fecha del primer día del mes correspondiente a la variable x
                                    DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);

                                    DateTime fechaFin = item.FECHA_PERIODO;
                                    fechaFin = fechaFin.AddYears(14);

                                    line += ($"{item.NUMERO_IDENTIFICACION.PadLeft(11, '0')}{item.NVD_NOVEDADES_ALTA_BAJA}{item.CODIGO_CAJA.ToString("D6")}{fechaPrimerDiaX.ToString("yyyyMMdd")}{item.VALOR_DESCUENTO.ToString("D12")}{string.Format(item.ES_ALTA == false ? "00000" : numeroFormateado)}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : item.FECHA_INICIO.ToString("yyyyMM"))}{string.Format(item.ES_ALTA == false ? item.FECHA_PERIODO.ToString("yyyyMM") : fechaFin.ToString("yyyyMM"))} " + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        break;
                    #endregion
                    //Dirección General Marítima 
                    #region Dirección General Marítima 
                    case 20:
                        if (generarArchivoRequest.IdConceptoNovedad == 1)
                        {
                            nameFile = generarArchivoRequest.Codigo + ".txt";
                            foreach(var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    line += ($"{item.NUMERO_IDENTIFICACION};{Regex.Replace(item.NVD_NOVEDADES_ALTA_BAJA, "[^0-9]", "")};{item.FECHA_PERIODO.ToString("yyyyMM")};{item.FECHA_PERIODO.ToString("yyyyMM")};{string.Format(item.ES_ALTA == true ? "2;" : "1;")}" + System.Environment.NewLine);
                                }
                            }
                            
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 2)
                        {
                            nameFile = generarArchivoRequest.Codigo + ".txt";
                            foreach(var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    line += ($"{item.NUMERO_IDENTIFICACION};{Regex.Replace(item.NVD_NOVEDADES_ALTA_BAJA, "[^0-9]", "")};{item.FECHA_PERIODO.ToString("yyyyMM")};{item.FECHA_PERIODO.ToString("yyyyMM")};{string.Format(item.ES_ALTA == true ? "2;" : "1;")}" + System.Environment.NewLine);
                                }
                            }

                                
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 3)
                        {
                            nameFile = generarArchivoRequest.Codigo + ".txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    line += ($"{item.NUMERO_IDENTIFICACION};{Regex.Replace(item.NVD_NOVEDADES_ALTA_BAJA, "[^0-9]", "")};{item.FECHA_PERIODO.ToString("yyyyMM")};{item.FECHA_PERIODO.ToString("yyyyMM")};{string.Format(item.ES_ALTA == true ? "2;" : "1;")}" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        if (generarArchivoRequest.IdConceptoNovedad == 4)
                        {
                            nameFile = generarArchivoRequest.Codigo + ".txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    line += ($"{item.NUMERO_IDENTIFICACION};{item.NVD_NOVEDADES_ALTA_BAJA};{item.FECHA_PERIODO.ToString("yyyyMM")};{item.FECHA_PERIODO.ToString("yyyyMM")};{string.Format(item.ES_ALTA == true ? "2;" : "1;")}" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        if (generarArchivoRequest.IdConceptoNovedad == 6)
                        {
                            nameFile = generarArchivoRequest.Codigo + ".txt";
                            foreach (var items in dataGroup)
                            {
                                foreach (var item in items.ToList())
                                {
                                    line += ($"{item.NUMERO_IDENTIFICACION};{Regex.Replace(item.NVD_NOVEDADES_ALTA_BAJA, "[^0-9]", "")};{item.FECHA_PERIODO.ToString("yyyyMM")};{item.FECHA_PERIODO.ToString("yyyyMM")};{string.Format(item.ES_ALTA == true ? "2;" : "1;")}" + System.Environment.NewLine);
                                }
                            }
                                
                        }
                        break;
                    #endregion
                    //UNIDAD ADMINISTRATIVA ESPECIAL DE LA JUSTICIA PENAL MILITAR Y POLICIAL
                    #region UNIDAD ADMINISTRATIVA ESPECIAL DE LA JUSTICIA PENAL MILITAR Y POLICIAL
                    case 21:
                        nameFilePdf = "UNIDAD ADMINISTRATIVA ESPECIAL DE LA JUSTICIA PENAL MILITAR Y POLICIAL";                        

                        fileProcess = await CreateReportHtml
                                (
                                   data,
                                    htmlTemplateGlobal,
                                    await _catalogosBusinessLogic.ObtenerUnidadesEjecutoras(),
                                    nameFilePdf,
                                    generarArchivoRequest
                                );
                        
                        pdf = GeneratePdfReportFromHtml(fileProcess);
                        return new Response<GeneracionArchivosResponse> { Data = new() { ArchivoB64 = Convert.ToBase64String(pdf), NameFile = nameFilePdf + ".pdf" } };
                        
                    #endregion
                    //Policia Bienestar
                    #region Policia Bienestar
                    case 22:
                        //Ahorro Onligatorio 03
                        if (generarArchivoRequest.IdConceptoNovedad == 1)
                        {
                            nameFile = string.Format(generarArchivoRequest.EsAlta == true ? "NUEVO_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_POLICIABIENESTAR.txt" : "CANCEL_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_POLICIABIENESTAR.txt");
                            foreach (var item in data.ToList())
                            {
                                consecutivo += 1;
                                string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                {
                                    numeroFormateado = item.PORCENTAJE_DESCUENTO.ToString();
                                }
                                else
                                {
                                    numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                }

                                // Obtener el año y mes desde la variable x
                                int anio_x = int.Parse(x.Substring(0, 4));
                                int mes_x = int.Parse(x.Substring(4, 2));

                                // Crear la fecha del primer día del mes correspondiente a la variable x
                                DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};;;{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{"3"};{item.APORTE};{item.I};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{string.Format(generarArchivoRequest.EsAlta == false ? string.Empty : numeroFormateado + ";SI;")};;" + System.Environment.NewLine);
                            }
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 2)
                        {
                            //Ahorro Voluntario 03              
                            nameFile = string.Format(generarArchivoRequest.EsAlta == true ? nameFile = "NUEVO_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_POLICIABIENESTAR.txt" : nameFile = "CANCEL_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_POLICIABIENESTAR.txt");
                            foreach (var item in data.ToList())
                            {
                                consecutivo += 1;
                                string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                {
                                    numeroFormateado = item.PORCENTAJE_DESCUENTO.ToString();
                                }
                                else
                                {
                                    numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                }

                                // Obtener el año y mes desde la variable x
                                int anio_x = int.Parse(x.Substring(0, 4));
                                int mes_x = int.Parse(x.Substring(4, 2));

                                // Crear la fecha del primer día del mes correspondiente a la variable x
                                DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};;;{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{item.NVD_NOVEDADES_ALTA_BAJA};{item.APORTE};{item.I};{item.FECHA_REPORTE.ToString("dd/MM/yyyy")};{numeroFormateado};{"SI"};;;" + System.Environment.NewLine);
                            }
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 3)
                        {
                            //Fondo de Solidaridad  106                   
                            nameFile = string.Format(generarArchivoRequest.EsAlta == true ? nameFile = "NUEVO_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_POLICIABIENESTAR.txt" : nameFile = "CANCEL_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_POLICIABIENESTAR.txt");
                            foreach (var item in data.ToList())
                            {
                                consecutivo += 1;
                                string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                {
                                    numeroFormateado = item.PORCENTAJE_DESCUENTO.ToString();
                                }
                                else
                                {
                                    numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                }

                                // Obtener el año y mes desde la variable x
                                int anio_x = int.Parse(x.Substring(0, 4));
                                int mes_x = int.Parse(x.Substring(4, 2));

                                // Crear la fecha del primer día del mes correspondiente a la variable x
                                DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};;;{5};{11};{item.FECHA_REPORTE.ToString("dd/MM/yyyy")};{item.NVD_NOVEDADES_ALTA_BAJA};{item.APORTE};{"D"};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{numeroFormateado};{"SI"};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};;;" + System.Environment.NewLine);
                            }
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 4)
                        {
                            //Ahorro Voluntario Cesantias 1637             
                            nameFile = string.Format(generarArchivoRequest.EsAlta == true ? "NUEVO_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_POLICIABIENESTAR.txt" : "CANCEL_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_POLICIABIENESTAR.txt");
                            foreach (var item in data.ToList())
                            {
                                consecutivo += 1;
                                string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                {
                                    numeroFormateado = item.PORCENTAJE_DESCUENTO.ToString();
                                }
                                else
                                {
                                    numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                }

                                // Obtener el año y mes desde la variable x
                                int anio_x = int.Parse(x.Substring(0, 4));
                                int mes_x = int.Parse(x.Substring(4, 2));

                                // Crear la fecha del primer día del mes correspondiente a la variable x
                                DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};;;{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{item.NVD_NOVEDADES_ALTA_BAJA};{item.APORTE};{"I"};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{string.Format(item.ES_ALTA == true ? numeroFormateado + ";SI;" + /*item.FECHA_PERIODO.ToString("dd/MM/yyyy") +*/ ";;;" + item.TOKEN : string.Empty)}" + System.Environment.NewLine);
                            }
                        }
                        else if (generarArchivoRequest.IdConceptoNovedad == 6)
                        {
                            //Fondo de Solidaridad  03                      
                            nameFile = string.Format(generarArchivoRequest.EsAlta == true ? nameFile = "NUEVO_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_POLICIABIENESTAR.txt" : nameFile = "CANCEL_" + data.ToList()[0].FECHA_PERIODO.ToString("yyyyMM") + "_" + generarArchivoRequest.Codigo + "_POLICIABIENESTAR.txt");
                            foreach (var item in data.ToList())
                            {
                                consecutivo += 1;
                                string x = item.FECHA_PERIODO.ToString("yyyyMMdd");

                                if (item.PORCENTAJE_DESCUENTO >= 4 && item.PORCENTAJE_DESCUENTO <= 5)
                                {
                                    numeroFormateado = item.PORCENTAJE_DESCUENTO.ToString();
                                }
                                else
                                {
                                    numeroFormateado = ((int)(item.PORCENTAJE_DESCUENTO)).ToString() + ".0";
                                }

                                // Obtener el año y mes desde la variable x
                                int anio_x = int.Parse(x.Substring(0, 4));
                                int mes_x = int.Parse(x.Substring(4, 2));

                                // Crear la fecha del primer día del mes correspondiente a la variable x
                                DateTime fechaPrimerDiaX = new DateTime(anio_x, mes_x, 1);
                                line += ($"{consecutivo};{item.TIPO_IDENTIFICACION};{item.NUMERO_IDENTIFICACION};;;{5};{11};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{"3"};{item.APORTE};{"I"};{fechaPrimerDiaX.ToString("dd/MM/yyyy")};{(item.ES_ALTA == true ? numeroFormateado : "")};{(item.ES_ALTA == true ? "SI" : "")};" + System.Environment.NewLine);
                            }
                        }

                        break;
                    #endregion
                    case 23:

                        break;
                    //DIRECCIÓN DE VETERANOS Y REHABILITACIÓN INCLUSIVA
                    #region DIRECCIÓN DE VETERANOS Y REHABILITACIÓN INCLUSIVA
                    case 24:
                        nameFilePdf = "DIRECCIÓN DE VETERANOS Y REHABILITACIÓN INCLUSIVA";                        
                        fileProcess = await CreateReportHtml
                                (
                                    data,
                                    htmlTemplateGlobal,
                                    await _catalogosBusinessLogic.ObtenerUnidadesEjecutoras(),
                                    nameFilePdf,
                                    generarArchivoRequest
                                );
                        
                        pdf = GeneratePdfReportFromHtml(fileProcess);
                        return new Response<GeneracionArchivosResponse> { Data = new() { ArchivoB64 = Convert.ToBase64String(pdf), NameFile = nameFilePdf + ".pdf" } };
                        
                    #endregion
                    default:
                        throw new Exception("Error Reporte No Encontrado");
                }
            }
           return new Response<GeneracionArchivosResponse> { Data = new() { ArchivoB64 = await _reportesBusinessLogic.GenerarTxt(line), NameFile = nameFile } };
        }

        public async Task<string> CreateReportHtml(IEnumerable<GeneracionArchivos> generacionArchivos, string html, Response<IEnumerable<UnidadEjecutoraDto>> fuerzasDto, string nameUE, GenerarArchivoRequest generarArchivoRequest)
        {
            int consecutivo = 0;            
            
            if (generacionArchivos.Count() > 0)
            {
                consecutivo += 1;
                string details = string.Empty;
                DateTime dateTime = DateTime.Now;
                DateTime fechaPersonalizada = new DateTime(dateTime.Year, dateTime.Month, 1);
                string reg = string.Empty;
                string tipoNovedad = string.Empty;
                var user = await _usersRepository.ObtenerUsuarioPorId(generarArchivoRequest.IdCodigoUsuario);
                string htmlGenerate = string.Empty;
                string htmlGlobal = html;
                var dataGroup=generacionArchivos.GroupBy(x=>x.ES_ALTA).OrderByDescending(x=>x.Key);
                foreach (var items in dataGroup)
                {
                    html = htmlGlobal;
                    
                    tipoNovedad = items.Key == true ? "ALTAS" : "BAJA";
                    foreach (var item in items)
                    {
                        var infoClienteDto = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(item.TIPO_IDENTIFICACION_N, item.NUMERO_IDENTIFICACION);
                        if (infoClienteDto.Data == null)
                        {
                            continue;
                        }
                        var fuerza = fuerzasDto.Data.Where(x => x.Id == infoClienteDto.Data.IdUnidadEjecutora).FirstOrDefault();
                        if (generarArchivoRequest.Codigo == "9098" || generarArchivoRequest.Codigo == "984Q" && generarArchivoRequest.EsAlta == true)
                        {
                            reg = $@"<tr style='border: 1px solid black;'>
                                  <td style='border: 1px solid black; padding: 8px; text-align: left;'>{consecutivo}</td>
                                  <td style='border: 1px solid black; padding: 8px; text-align: left;'>{(infoClienteDto.Data!=null? infoClienteDto.Data.PrimerNombre + " " + infoClienteDto.Data.SegundoNombre + " " + infoClienteDto.Data.PrimerApellido + " " + infoClienteDto.Data.SegundoApellido:"")}</td>
                                  <td style='border: 1px solid black; padding: 8px; text-align: left;'>{string.Format(item.COD_MILITAR == null ? "0000" : item.COD_MILITAR)}</td>
                                  <td style='border: 1px solid black; padding: 8px; text-align: left;'>{(infoClienteDto.Data != null ? fuerza!=null?fuerza.Nombre:"":"")}</td>
                                  <td style='border: 1px solid black; padding: 8px; text-align: left;'>{item.NVD_NOVEDADES_ALTA_BAJA}</td>                                  
                                  <td style='border: 1px solid black; padding: 8px; text-align: left;'>{Convert.ToInt32(item.PORCENTAJE_DESCUENTO) + "%"}</td>
                                  <td style='border: 1px solid black; padding: 8px; text-align: left;'>{fechaPersonalizada.ToString("yyyy MM dd")}</td>
                                  <td style='border: 1px solid black; padding: 8px; text-align: left;'>{item.FECHA_PERIODO.ToString("yyyy MM")}</td>
                                  <td style='border: 1px solid black; padding: 8px; text-align: left;'>{item.NUMERO_IDENTIFICACION}</td>
                                </tr>";
                        }
                        else
                        {
                            reg = $@"<tr style='border: 1px solid black;'>
                                  <td style='border: 1px solid black; padding: 8px; text-align: left;'>{consecutivo}</td>
                                  <td style='border: 1px solid black; padding: 8px; text-align: left;'>{(infoClienteDto.Data != null ? infoClienteDto.Data.PrimerNombre + " " + infoClienteDto.Data.SegundoNombre + " " + infoClienteDto.Data.PrimerApellido + " " + infoClienteDto.Data.SegundoApellido:"")}</td>
                                  <td style='border: 1px solid black; padding: 8px; text-align: left;'>{string.Format(item.COD_MILITAR == null ? "0000" : item.COD_MILITAR)}</td>
                                  <td style='border: 1px solid black; padding: 8px; text-align: left;'>{(infoClienteDto.Data != null ? fuerza != null ? fuerza.Nombre : "" : "")}</td>
                                  <td style='border: 1px solid black; padding: 8px; text-align: left;'>{item.NVD_NOVEDADES_ALTA_BAJA}</td>
                                  <td style='border: 1px solid black; padding: 8px; text-align: left;'>{fechaPersonalizada.ToString("yyyy MM dd")}</td>
                                  <td style='border: 1px solid black; padding: 8px; text-align: left;'>{item.FECHA_PERIODO.ToString("yyyy MM")}</td>
                                  <td style='border: 1px solid black; padding: 8px; text-align: left;'>{item.NUMERO_IDENTIFICACION}</td>
                                </tr>";
                        }
                        consecutivo+=1;
                        details = string.Concat(details, reg);
                    }
                   

                    var infoUseRol = await _iClientRequestProvider.ObtenerInfoCliente(user.TID_ID, user.USR_IDENTIFICACION);

                    var gradoProfesional = await _catalogosBusinessLogic.ObtenerGrados();

                    htmlGenerate =string.Concat(htmlGenerate,string.Format(html,
                            details, //0
                            nameUE, //1
                            dateTime.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-CO")),//2
                            dateTime.ToString("yyyy"),//3
                            dateTime.ToString("dd-MMM-yyyy"),//4
                            "LISNOV" + generacionArchivos.ToList()[0].FECHA_PERIODO.ToString("MM"),//5
                            tipoNovedad, //6
                            user.USR_NOMBRE + " " + user.USR_SEGUNDONOMBRE + " " + user.USR_APELLIDO + " " + user.USR_SEGUNDOAPELLIDO,//7
                            infoUseRol.Data!=null?gradoProfesional.Data.ToList().Where(x => x.Id == infoUseRol.Data.GradoId).ToList()[0].Descripcion:""
                            ));
                    details = string.Empty;
                    consecutivo = 1;
                    if (items.Key.Equals(true)&& items.Count()>1)
                        htmlGenerate = string.Concat(htmlGenerate, "<div style=\"page-break-after:always\"></div>");
                }
                       

                return htmlGenerate;
            }
            return  string.Format("Error en el Reporte");

        }
        #region GeneratePDF
        public byte[] GeneratePdfReportFromHtml(string html)
        {
            GlobalSettings globalSettings = new GlobalSettings();
            globalSettings.ColorMode = WkHtmlToPdfDotNet.ColorMode.Color;
            globalSettings.Orientation = Orientation.Portrait;
            globalSettings.PaperSize = WkHtmlToPdfDotNet.PaperKind.Letter;
            globalSettings.Margins = new MarginSettings { Top = 25, Bottom = 34 };
            ObjectSettings objectSettings = new ObjectSettings();
            objectSettings.PagesCount = true;
            objectSettings.HtmlContent = html;
            WkHtmlToPdfDotNet.WebSettings webSettings = new WkHtmlToPdfDotNet.WebSettings();
            webSettings.DefaultEncoding = "utf-8";
            HeaderSettings headerSettings = new HeaderSettings();
            //headerSettings.FontSize = 15;
            //headerSettings.FontName = "Ariel";
            //headerSettings.Right = "Page [page] of [toPage]";
            //headerSettings.Line = true;
            headerSettings.HtmlUrl = AppDomain.CurrentDomain.BaseDirectory + "Reportes\\HTML\\Templates\\HTML\\Novedades\\HeaderPdf.html";
            FooterSettings footerSettings = new FooterSettings();
            footerSettings.FontSize = 8;
            footerSettings.FontName = "Ariel";
            //footerSettings.Center = "<div style='heigth:300px;'>hshshshshsh</div>";
            //footerSettings.Line = true;
            footerSettings.HtmlUrl = AppDomain.CurrentDomain.BaseDirectory+ "Reportes\\HTML\\Templates\\HTML\\Novedades\\FooterPdf.html";
            //objectSettings.HeaderSettings = headerSettings;
            objectSettings.FooterSettings = footerSettings;
            objectSettings.HeaderSettings=headerSettings;
            objectSettings.WebSettings = webSettings;
            HtmlToPdfDocument htmlToPdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };
            return _converter.Convert(htmlToPdfDocument);
        }
        #endregion

        public async Task<Response<NovedadesAfiliadoDto>> InsertarNovedadAfiliado(NovedadesAfiliadoDto request)
        {
            if (request.IdTipoIdentificacion == 9)
            {
                var cliente = _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(request.IdTipoIdentificacion, request.Identificacion);
                if (cliente.Result.Data != null)
                {
                    request.IdTipoIdentificacion = cliente.Result.Data.IdTipoIdentificacion;
                    request.Identificacion = cliente.Result.Data.Identificacion;
                }
            }
            return new Response<NovedadesAfiliadoDto> { Data = this._mapper.Map<NovedadesAfiliadoDto>(await _repositoryRepository.InsertarNovedadAfiliado(this._mapper.Map<NovedadesAfiliado>(request))) };
        }

        public async Task<Response<NovedadesAfiliadoDto>> ObtenerNovedadAfiliadoPorId(int idNovedad)
        {
            var data = await _repositoryRepository.ObtenerNovedadAfiliadoPorId(idNovedad);
            return new Response<NovedadesAfiliadoDto> { Data = this._mapper.Map<NovedadesAfiliadoDto>(data) };
        }

        public async Task<Response<IEnumerable<NovedadesAfiliadoDto>>> ObtenerNovedadesAfiliado(int idTipoIdentificacion, string identificacion)
        {
            if (idTipoIdentificacion == 9)
            {
                var cliente = await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(idTipoIdentificacion, identificacion);
                if (cliente != null)
                {
                    idTipoIdentificacion = cliente.Data.IdTipoIdentificacion;
                    identificacion = cliente.Data.Identificacion;
                }
            }
            return new Response<IEnumerable<NovedadesAfiliadoDto>> { Data = this._mapper.Map<IEnumerable<NovedadesAfiliadoDto>>(await _repositoryRepository.ObtenerNovedadesAfiliado(idTipoIdentificacion, identificacion)) };
        }

        public async Task<Response<IEnumerable<NovedadPorUnidadEjecutoraDto>>> ObtenerNovedadesPorUnidadEjecutora(NovedadPorUnidadEjecutoraDto request)
        {
            return new Response<IEnumerable<NovedadPorUnidadEjecutoraDto>> { Data = this._mapper.Map<IEnumerable<NovedadPorUnidadEjecutoraDto>>(await _repositoryRepository.ObtenerNovedadesPorUnidadEjecutora(_mapper.Map<NovedadPorUnidadEjecutora>(request))) };
        }

        public async Task<Response<IEnumerable<NovedadesCronogramaDto>>> ObtenerNovedadesUnidadEjecutora(int idUnidadEjecutora)
        {
            return new Response<IEnumerable<NovedadesCronogramaDto>> { Data = this._mapper.Map<IEnumerable<NovedadesCronogramaDto>>(await _repositoryRepository.ObtenerNovedadesCronograma(idUnidadEjecutora)) };
        }

        /// <summary>
        /// Metodo obtener info cliente
        /// </summary>
        /// <param name="Cliente">Modelo dto cliente</param>
        /// <uthor>ONicolas Florez Sarraola</uthor>
        /// <date>12/03/2021</date>
        /// <returns>Modelo dto de un cliente</returns>
        public async Task<Response<IEnumerable<CronogramaDto>>> ObtenerUnidadesEjecutorasCronograma()
        {
            var cronogramaUnidadesEjecutoras = this._mapper.Map<IEnumerable<Cronograma>, IEnumerable<CronogramaDto>>(await _repositoryRepository.ObtenerUnidadesEjecutorasCronograma());
            foreach (var item in cronogramaUnidadesEjecutoras)
            {
                item.Novedades = this._mapper.Map<IEnumerable<NovedadesCronogramaDto>>(await _repositoryRepository.ObtenerNovedadesCronograma(item.IdUnidadEjecutora)).ToList();
            }
            return new Response<IEnumerable<CronogramaDto>> { Data = cronogramaUnidadesEjecutoras };
        }

        public async Task<Response<IEnumerable<DescargaDefinitivaDto>>> InsertarDescargaDefinitiva(DescargaDefinitivaDto descargaDefinitivaDto)
        {
            try
            {
                var descargaDefinitiva = this._mapper.Map<DescargaDefinitivaDto, DescargaDefinitiva>(descargaDefinitivaDto);
                var result = new Response<IEnumerable<DescargaDefinitivaDto>> { Data = this._mapper.Map<IEnumerable<DescargaDefinitivaDto>>(await _repositoryRepository.InsertarDescargaDefinitiva(descargaDefinitiva)) };
                return result;
            }
            catch(Exception ex)
            {
                return new Response<IEnumerable<DescargaDefinitivaDto>>();
            }
        }

        public async Task<Response<IEnumerable<DescargaDefinitivaDto>>> ActualizarEstadoDescargaDefinitiva(DescargaDefinitivaDto descargaDefinitivaDto)
        {
            var descargaDefinitiva = this._mapper.Map<DescargaDefinitivaDto, DescargaDefinitiva>(descargaDefinitivaDto);
            var descargasDefinitivas = this._mapper.Map<IEnumerable<DescargaDefinitiva>, IEnumerable<DescargaDefinitivaDto>>(await _repositoryRepository.ActualizarEstadoDescargaDefinitiva(descargaDefinitiva)).ToList();
            return new Response<IEnumerable<DescargaDefinitivaDto>> { Data = descargasDefinitivas };
        }

        public async Task<Response<IEnumerable<DescargaDefinitivaDto>>> ObtenerArchivosDescargaDefinitiva(DescargaDefinitivaDto descargaDefinitivaDto)
        {
            var descargaDefinitiva = this._mapper.Map<DescargaDefinitivaDto, DescargaDefinitiva>(descargaDefinitivaDto);
            var descargasDefinitivas = this._mapper.Map<IEnumerable<DescargaDefinitiva>, IEnumerable<DescargaDefinitivaDto>>(await _repositoryRepository.ObtenerArchivosDescargaDefinitiva(descargaDefinitiva)).ToList();
            return new Response<IEnumerable<DescargaDefinitivaDto>> { Data = descargasDefinitivas };
        }
    }
}
