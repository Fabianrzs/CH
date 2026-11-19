using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.Constructora;
using GA2.Domain.Entities.Constructora;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class ConstructoraBussines : IConstructoraBussines
    {
        private readonly IConstructoraRepository _constructoraRepository;
        private readonly IMapper _mapper;
        public ConstructoraBussines(
            IConstructoraRepository constructoraRepository,
            IMapper mapper) 
        {
            _constructoraRepository = constructoraRepository;
            _mapper = mapper;   
        }
        public async Task<Response<ConstructoraDto>> AgregaConstructora(ConstructoraDto constructora)
        {
            var requestMapper = _mapper.Map<Constructora>(constructora);
            var result = await _constructoraRepository.AgregaConstructora(requestMapper);
            var resultMapper = _mapper.Map<ConstructoraDto>(result);
            return new Response<ConstructoraDto> { Data = resultMapper, IsSuccess = false, ReturnMessage = "OK" };
        }
        public async Task<Response<string>> ActualizarConstructora(ConstructoraDto constructora)
        {
            var requestMapper = _mapper.Map<Constructora>(constructora);
            var result = await _constructoraRepository.ActualizarConstructora(requestMapper);
            if (result > 0)
            {
                return new Response<string> { Data = "", IsSuccess = false, ReturnMessage = "OK" };
            }
            return new Response<string> { Data = "Valida la data", IsSuccess = false, ReturnMessage = "OK" };
        }

        public  async Task<Response<IEnumerable<ConstructoraDto>>> ConsultarConstructora()
        {
           var result = await _constructoraRepository.ConsultarConstructora();
            var resultMapper = _mapper.Map<IEnumerable<ConstructoraDto>>(result);
            return new Response<IEnumerable<ConstructoraDto>> { Data = resultMapper, IsSuccess = false, ReturnMessage = "OK" };

        }

        public async Task<Response<ConstructoraDto>> ConsultarConstructoraById(Guid constructora)
        {
            var result = await _constructoraRepository.ConsultarConstructoraById(constructora);
            var resultMapper = _mapper.Map<ConstructoraDto>(result);
            return new Response<ConstructoraDto> { Data = resultMapper, IsSuccess = false, ReturnMessage = "OK" };
        }
        public async Task<Response<string>> EliminarConstructoraById(Guid constructora)
        {
            var result = await _constructoraRepository.EliminarConstructora(constructora);
            if (result > 0)
            {
                return new Response<string> { Data = "Exitoso", IsSuccess = false, ReturnMessage = "Error" };
            }

            return new Response<string> { Data = "No se logro eliminar, valide", IsSuccess = false, ReturnMessage = "Error" };
        }
    }
}
