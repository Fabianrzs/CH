using GA2.Application.Dto.DatosAliadoEstrategico;
using GA2.Application.Dto.ValidacionTelAfiliado;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ValidacionTelAfiliado
{
    public interface IValidacionTelAfiliadoBusinessLogic
    {
        Task<Response<ValidacionTelAfiliadoDto>> CrearValTelAfiliado(ValidacionTelAfiliadoDto TelAfiliado);
        Task<Response<ValidacionTelAfiliadoDto>> ObtenerValTelAfiliado(Guid Soc_Id);
    }
}