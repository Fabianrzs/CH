using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IGestorDocumentalBusinessLogic
    {
        Task<Response<Form_InsertResponse>> CrearRegistro(PeticionCreditoBasicaDto peticionCredito);
        Task<object> ActualizarRegistro(RequestUpdateDto request);
        Task<object> AttachFiles(Application.Dto.AttachRequestDto request);
        Task<object> CrearFlujoFirmasComite(RequestFlujoFirmasDto request);
        Task<object> Form_AttachFilesAsync(Application.Dto.AttachRequestDto request);
        Task<Response<Form_InsertResponse>> RadicarGDPreApronado(SolicitudSimulacionDto preAprobado);
        Task<Response<Form_GetDataResponse>> validarExpedientePadre(SolicitudSimulacionDto preAprobado);
        Task<Response<Form_InsertResponse>> RadicarExpedientePadre(SolicitudSimulacionDto preAprobado);

        Task<Response<Form_InsertResponse>> RadicadoFlujo83(DatosIdentificacionDto datosIdentificacion); 
        //Task<Response<Form_InsertResponse>> RadicarGDSolicitudCredito(SolicitudCreditoDto solicitud);
    }
}