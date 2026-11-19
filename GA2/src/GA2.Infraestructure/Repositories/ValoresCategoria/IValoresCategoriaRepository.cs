using GA2.Application.Dto.ValoresCategoria;
using GA2.Domain.Entities.ValoresCategoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.ValoresCategoria
{
    public interface IValoresCategoriaRepository
    {
        Task<ValoresCategorias> ActualizarValoresCategoria(ValoresCategorias ValoresRequest);
        Task<IEnumerable<ValoresCategorias>> ObtenerValoresCategoria(int Anio);
        Task<IEnumerable<ValoresCategorias>> CrearValoresCategoria(CrearValoresDto ValoresRequest);
    }
}
