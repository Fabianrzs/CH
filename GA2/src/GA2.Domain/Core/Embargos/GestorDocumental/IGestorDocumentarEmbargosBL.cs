using GA2.Application.Dto;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Embargos.GestorDocumental
{
    public interface IGestorDocumentarEmbargosBL
    {
        Task<Response<Form_InsertResponse>> RadicarGDPEmbargos(RadicadoEmbargoDto embargos);
        Task<Response<Form_InsertResponse>> RadicarGDPEmbargosCorrespondencia(EmbargosDto embargos);
        Task<Response<Form_GetDataResponse>> ObtenerCorrespondenciaEmbargos(string IdentificacionCliente);
    }
}
