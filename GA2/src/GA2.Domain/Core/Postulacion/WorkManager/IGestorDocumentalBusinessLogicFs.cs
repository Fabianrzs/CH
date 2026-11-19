using GA2.Application.Dto;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace GA2.Domain.Core.Postulacion.WorkManager
{
    public interface IGestorDocumentalBusinessLogicFs
    {
        Task<Response<Form_InsertResponse>> RadicarGDFondoSolidaridad(Guid idSolicitud);
        Task<InsertRequestDto> GetInsertRequestFondoSolidaridad(SolicitudPostulante solicitud);
    }
}
