using AutoMapper;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class EstadoConvocatoriaBussineLogic : IEstadoConvocatoriaBussinesLogic
    {
        private readonly IEstadoConvocatoriaRepository _estadoConvocatoriaRepository;
        private readonly IMensajeRepository _messageRepository;
        private readonly IMapper _mapper;
        public EstadoConvocatoriaBussineLogic(IEstadoConvocatoriaRepository stateRepository, IMapper mapper, IMensajeRepository messageRepository)
        {
            _estadoConvocatoriaRepository = stateRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<EstadoConvocatoriaDto>>> ObtenerEstadosConvocatoria()
        {
           var responseData = await _estadoConvocatoriaRepository.ObtenerEstadosConvocatoria();
            var resultMap = _mapper.Map<IEnumerable<EstadoConvocatoriaDto>>(responseData);
            if (resultMap.Any())
            {
                return new Response<IEnumerable<EstadoConvocatoriaDto>> { Data = resultMap, ReturnMessage ="Exitoso" };
            }
            return new Response<IEnumerable<EstadoConvocatoriaDto>> { Data = null, ReturnMessage = "No hay datos" };

        }
    }
}
