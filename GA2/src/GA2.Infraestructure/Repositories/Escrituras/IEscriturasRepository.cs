using GA2.Domain.Entities;
using GA2.Domain.Entities.Escritura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.Escrituras
{
    public interface IEscriturasRepository
    {
        Task<Escritura> CrearEscritura(Escritura EscrituraRequest);
        Task<Escritura> ObtenerEscritura(Escritura EscrituraRequest);
    }
}
