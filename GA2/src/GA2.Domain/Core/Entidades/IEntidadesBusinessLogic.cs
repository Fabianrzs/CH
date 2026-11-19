using GA2.Application.Dto;
using GA2.Application.Dto.Entidades;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Entities.Entidades;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IEntidadesBusinessLogic
    {
        #region EntidadEducativa
        Task<Response<IEnumerable<EntidadEducativaDto>>> ObtenerEntidadEducativa();
        Task<Response<IEnumerable<EntidadEducativaDto>>> ObtenerEntidadEducativaPorNombreNit(EntidadEducativaDto entidadEducativa);
        Task<Response<EntidadEducativaDto>> CrearEntidadEducativa(EntidadEducativaDto entidadEducativa);
        Task<Response<EntidadEducativaDto>> ActualizarEntidadEducativa(EntidadEducativaDto entidadEducativa);
        Task<Response<EntidadEducativaDto>> EliminarEntidadEducativaPorId(string idEntidadEducativa);
        Task<Response<IEnumerable<SedeEntidadEducativaDto>>> ObtenerSedesPorEntidadEducativa(Guid idEntidadEducativa);
        Task<Response<SedeEntidadEducativaDto>> CrearSedeEntidadEducativa(SedeEntidadEducativaDto sedeEntidadEducativa);
        Task<Response<CrearAnotacionDTO>> CrearAnotacion(CrearAnotacionDTO crearAnotacion);
        Task<Response<IEnumerable<RespuestaAnotacionDTO>>> ConsultarAnotaciones(Guid entidadId);
        Task<Response<SedeEntidadEducativaDto>> ActualizarSedeEntidadEducativa(SedeEntidadEducativaDto sedeEntidadEducativa);
        Task<Response<SedeEntidadEducativaDto>> EliminarSedesEntidadEducativaPorId(IdentificadoresEntidadEducativa sedeEntidadEducativa); 
        Task<Response<BloqueoEntidadEducativaDto>> BloqueoEntidadEducativa(BloqueoEntidadEducativaDto bloqueoEntidadEducativa);
        Task<Response<BloqueoEntidadEducativaDto>> DesbloqueoEntidadEducativa(BloqueoEntidadEducativaDto bloqueoEntidadEducativa);
        Task<Response<IEnumerable<BloqueoEntidadEducativaDto>>> ObtenerBloqueoEntidadEducativa(string idEntidadEducativa);
        Task<Response<IEnumerable<FidecomisoDto>>> ObtenerCuentaBancariaPorEntidad(string idEntidad);
        Task<Response<CuentaBancariaEntidadEducativaDto>> CrearCuentaBancariaEntidadEducativa(CuentaBancariaEntidadEducativaDto cuentaBancariaEntidadEducativa);
        Task<Response<CuentaBancariaEntidadEducativaDto>> ActualizarCuentaBancariaEntidadEducativa(CuentaBancariaEntidadEducativaDto cuentaBancariaEntidadEducativa);
        Task<Response<CuentaBancariaEntidadEducativaDto>> EliminarCuentaBancariaEntidadEducativa(Guid idCuenta);
        Task<Response<IEnumerable<ProgramaEducativoDto>>> ObtenerProgramaEducativo(Guid idEntidad);
        Task<Response<ListarProgramaEducativoDto>> CrearProgramaEducativo(ProgramaEducativoDto programaEducativo);
        Task<Response<IEnumerable<ListarProgramaEducativoDto>>> ObtenerProgramaEducativoPorEntidad(Guid idEntidad);
        Task<Response<ProgramaEducativoDto>> EliminarProgramaEducativoPorId(IdentificadoresEntidadEducativa entidadEducativa);
        Task<Response<ProgramaEducativoDto>> ActualizarProgramaEducativo(ProgramaEducativoDto programaEducativo);
        Task<Response<bool>> CargarDocumentosEntidad(List<CargarDocumentosEntidadDto> cargarDocumentosEntidad, Guid idEntidad);
        Task<Response<IEnumerable<ConsultarArchivosPorEntidadDto>>> ConsultarArchivosPorEntidad(Guid idEntidad);
        Task<FileResult> DescargarArchivoPorId(string rutaStorage);
        Task<Response<EntidadEducativaDto>> ObtenerEntidadEducativaDetalle(Guid idEntidad);
        Task<Response<bool>> EliminarArchivoEntidad(EliminarArchivoEntidadDto eliminarArchivoEntidadDto);
        Task<Response<string>> CargaDocumentoEntidades(IFormFile request);
        Task<Response<ResponseDocumentoSolicitudDto>> CrearDocumentoEntidades(RequestDocumentoEntidadesDto requestDocumentoEntidades);
        Task<Response<IEnumerable<EntidadEducativaDto>>> ObtenerEntidadEducativaFideicomiso();
        Task<Response<IEnumerable<FidecomisoDto>>> CrearFideicomisoEntidadEducativa(FidecomisoDto fidecomiso);
        Task<Response<IEnumerable<FidecomisoDto>>> EliminarFideicomiso(IdentificadoresEntidadEducativa fidecomiso);
        Task<Response<IEnumerable<FidecomisoDto>>> ActualizarFideicomisoEntidadEducativa(FidecomisoDto fidecomiso);
        Task<Response<IEnumerable<EntidadEducativaDto>>> ConsultarFideicomisosEntidad(EntidadEducativaDto data);
        Task<Response<IEnumerable<TramiteEntidadesDto>>> ConsultarDataTramiteEntidades(Guid idEntidad);
        Task<Response<string>> EliminarSolicitudes(string Identificacion);
        Task<Response<IEnumerable<HistorialCambiosDto>>> ObtenerHistorialCambios(Guid idEntidad);
        Task<Response<IEnumerable<HistorialCambiosIntervinienteDto>>> ObtenerHistorialCambiosIntervinientes(Guid idIntervinientes);
        Task<Response<string>> CrearNovedadesEntidadEducativa(NovedadesEntidadEducativaRequestDto request);
        Task<Response<string>> CrearHistorialEntidad(HistorialCambiosDto request);
        #endregion EntidadEducativa

    }
}