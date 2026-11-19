using GA2.Application.Dto.Constructora;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IConstructoraBussines
    {
        Task<Response<ConstructoraDto>> AgregaConstructora(ConstructoraDto constructora);

        Task<Response<IEnumerable<ConstructoraDto>>> ConsultarConstructora();

        Task<Response<ConstructoraDto>> ConsultarConstructoraById(Guid constructora);
        Task<Response<string>> EliminarConstructoraById(Guid constructora);
        Task<Response<string>> ActualizarConstructora(ConstructoraDto constructora);
    }
}
