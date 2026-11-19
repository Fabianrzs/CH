using GA2.Application.Dto.ValoresCategoria;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ValoresCategoria
{
    public interface IValoresCategoriaBusinessLogic
    {
        Task<Response<ValoresCategoriaDto>> ActualizarValoresCategoria(ValoresCategoriaDto ValoresCategoria);

        Task<Response<IEnumerable<ValoresCategoriaDto>>> ObtenerValoresCategoria(int Anio);

        Task<Response<IEnumerable<ValoresCategoriaDto>>> CrearValoresCategoria(CrearValoresDto ValoresCategoria);
    }
}
