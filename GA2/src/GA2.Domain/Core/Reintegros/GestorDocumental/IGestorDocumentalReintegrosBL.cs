using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA2.Transversals.Commons;
using GA2.Domain.Entities.Postulacion;

namespace GA2.Domain.Core.Reintegros.GestorDocumental
{
    public interface IGestorDocumentalReintegrosBL
    {
        Task<Response<Form_GetDataResponse>> ObtenerCorrespondenciaReintegros(string radicado);
        Task<Response<Form_UpdateResponse>> ActualizarRadicadoPostulacion(ActualizarPostulacionDto actualizarPostulacionDto);
        Task<Response<Form_InsertResponse>> RadicarGDPComunicaciones(InsertarComunicacionDto insertarComunicacion);

    }
}
