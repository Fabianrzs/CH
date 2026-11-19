using AutoMapper;
using GA2.Application.Dto.Escrituras;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA2.Domain.Entities.Escritura;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.Escrituras;

namespace GA2.Domain.Core.Escrituras
{
    public class EscrituraBusinessLogic : IEscrituraBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IEscriturasRepository _escrituraRepository;
        public EscrituraBusinessLogic(IMapper mapper, IEscriturasRepository escriturasRepository)
        {
            _mapper = mapper;
            _escrituraRepository = escriturasRepository;
        }

        public async Task<Response<CreacionEscritura>> CrearEscritura(CreacionEscritura escritura)
        {
            Escritura EscrituraRequest = this._mapper.Map<Escritura>(escritura);
            return new Response<CreacionEscritura> { Data = this._mapper.Map<CreacionEscritura>(await this._escrituraRepository.CrearEscritura(EscrituraRequest)) };
        }

        public async Task<Response<ObtenerEscrituras>> ObtenerEscritura(RequestObtenerEscrituras escritura)
        {
            Escritura EscrituraRequest = this._mapper.Map<Escritura>(escritura);
            return new Response<ObtenerEscrituras> { Data = this._mapper.Map<ObtenerEscrituras>(await this._escrituraRepository.ObtenerEscritura(EscrituraRequest)) };
        }
    }
}
