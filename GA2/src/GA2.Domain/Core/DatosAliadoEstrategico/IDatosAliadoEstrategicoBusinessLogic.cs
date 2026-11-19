using GA2.Application.Dto;
using GA2.Application.Dto.DatosAliadoEstrategico;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.DatosAliadoEstrategico
{
    public interface IDatosAliadoEstrategicoBusinessLogic
    {
        Task<Response<DatosAliadoEstrategicoDto>> CrearDatosAliadoEstrategico(DatosAliadoEstrategicoDto aliadoEstrategico);
        Task<Response<DatosAliadoEstrategicoDto>> ActualizarDatosAliadoEstrategico(DatosAliadoEstrategicoDto aliadoEstrategico);
        Task<Response<DatosAliadoEstrategicoDto>> ObtenerDatosAliadoEstrategico(Guid aliadoEstrategico);

    }
}