using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito.Simulador.DocumentosAct;
using GA2.Application.Dto.Credito.Simulador.SolicitudCredito.Documento;
using GA2.Application.Dto.ReporteSimulacion;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReporteSimulacionBL
{
    public class ReporteSimulacionBusinessLogic : IReporteSimulacionBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        public ReporteSimulacionBusinessLogic(IMapper mapper, ICreditoBusinessLogic creditoBusinessLogic, IReportesBusinessLogic reportesBusinessLogic)
        {
            _mapper = mapper;
            _creditoBusinessLogic = creditoBusinessLogic;
            _reportesBusinessLogic = reportesBusinessLogic;
        }
        public async Task<Response<bool>> GuardarReporteSimulaciones(RequestReporteSimulacionDto Reporte)
        {
            try
            {
                #region Obtener Detalle Simulacion, Generacion Archivo y Guardado Reporte
                var SimulacionDetalle = await this._creditoBusinessLogic.ObtenerDetalleSimulacionCliente(Reporte.Smc_id);
                var ReporteSimulacion = await _reportesBusinessLogic.ReporteSimulacionCredito((List<Application.Dto.SimulacionDto>)SimulacionDetalle.Data, 1);
                using (var memoryStream = new MemoryStream(ReporteSimulacion.Data.ArchivoBytes))
                {
                    IFormFile formFile = new FormFile(memoryStream, 0, ReporteSimulacion.Data.ArchivoBytes.Length, "archivo", ReporteSimulacion.Data.FileName + ".pdf");
                    var SaveReport = await SaveReportSimulacion(formFile, Reporte);
                    if (SaveReport.Data)
                    {
                        return new Response<bool>() { Data = true };
                    }
                }
                #endregion
                return new Response<bool>() { Data = false };

            }
            catch (Exception ex)
            {
                return new Response<bool>() { Data = false };
            }
        }

        private async Task<Response<bool>> SaveReportSimulacion(IFormFile request, RequestReporteSimulacionDto Reporte)
        {
            try
            {
                //Para el AFL 1 se consume un servicio diferente dado que no existe un soc_id
                var RutaImagen = await _creditoBusinessLogic.CargaDocumentoSolicCredito(request);
                var Tdc_id = 0;
                if (Reporte.Afl_id == 2)
                {
                    #region Busqueda Documnetos PreAprobado y Guardado
                    DocumentosActSimulacionDto documentosActSimulacionDto = new DocumentosActSimulacionDto()
                    {
                        SLS_ID = Reporte.SimulacionId,
                        AFL_ID = Reporte.Afl_id
                    };
                    var documentosActRespuestaDto = await this._creditoBusinessLogic.ObtenerDocumentosActSimulacion(documentosActSimulacionDto);
                    Tdc_id = documentosActRespuestaDto.Data.Where(x => x.TIPO_DOCUMENTO.Equals("Comunicación oficial de preaprobado", StringComparison.OrdinalIgnoreCase)).Select(x => x.TDC_ID).FirstOrDefault();

                    RequestDocumentoSimDto requestDocumentoSimDto = new RequestDocumentoSimDto()
                    {
                        idTipoDocumento = Tdc_id, //Se quema el que esta en BD, se esta buscando la manera de no quemarlo
                        codigoBarras = "",
                        creadoPor = new Guid(),
                        fechaCreacion = DateTime.Now,
                        rutaImagen = RutaImagen.Data,
                        idSimulacion = Reporte.Smc_id
                    };
                    var DocumentoGuardado = await this._creditoBusinessLogic.CrearDocumentoSimulacion(requestDocumentoSimDto);
                    return new Response<bool> { Data = DocumentoGuardado.IsSuccess };
                    #endregion
                }
                else
                {
                    #region Busqueda Documento y Guardado
                    DocumentosActSolicitudDto documentosActSimulacionDto = new DocumentosActSolicitudDto()
                    {
                        SOC_ID = Reporte.SimulacionId,
                        AFL_ID = Reporte.Afl_id
                    };
                    var documentosActRespuestaDto = await this._creditoBusinessLogic.ObtenerDocumentosActSolicitud(documentosActSimulacionDto);
                    Tdc_id = documentosActRespuestaDto.Data.Where(x => x.TIPO_DOCUMENTO.Equals("Reporte simulacion comite de credito", StringComparison.OrdinalIgnoreCase)).Select(x => x.TDC_ID).FirstOrDefault();

                    RequestDocumentoSolicCreditoDto requestDocumentoSimDto = new RequestDocumentoSolicCreditoDto()
                    {
                        idTipoDocumento = Tdc_id, //Se quema el que esta en BD, se esta buscando la manera de no quemarlo
                        codigoBarras = "",
                        creadoPor = new Guid(),
                        fechaCreacion = DateTime.Now,
                        rutaImagen = RutaImagen.Data,
                        idSolicitudCredito = Reporte.SimulacionId
                    };
                    var DocumentoGuardado = await this._creditoBusinessLogic.CrearDocumentoSolicCredito(requestDocumentoSimDto);
                    return new Response<bool> { Data = DocumentoGuardado.IsSuccess };
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return new Response<bool> { Data = false, IsSuccess = false, ReturnMessage = "Error guardando reporte de simulación, Error:" + ex.Message };
            }

        }
    }
}
