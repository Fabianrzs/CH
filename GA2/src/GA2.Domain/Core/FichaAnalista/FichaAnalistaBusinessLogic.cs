using AutoMapper;
using GA2.Application.Dto.Escrituras;
using GA2.Application.Dto.FichaAnalista;
using GA2.Domain.Entities.Escritura;
using GA2.Domain.Entities.FichaAnalista;
using GA2.Infraestructure.Repositories.FichaAnalista;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.FichaAnalista
{
    public class FichaAnalistaBusinessLogic : IFichaAnalistaBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IFichaAnalistaRepository _IfichaAnalistaRepository;
        public FichaAnalistaBusinessLogic(IMapper mapper, IFichaAnalistaRepository IfichaAnalistaRepository)
        {
            _mapper=mapper;
            _IfichaAnalistaRepository=IfichaAnalistaRepository;
        }

        public async Task<Response<FichaAnalistaDto>> CreaFichaAnalista(FichaAnalistaDto fichaAnalista)
        {
            FichaAnalistas FichaAnalistaRequest = this._mapper.Map<FichaAnalistas>(fichaAnalista);
            return new Response<FichaAnalistaDto> { Data = this._mapper.Map<FichaAnalistaDto>(await this._IfichaAnalistaRepository.CrearFichaAnalista(FichaAnalistaRequest)) };
        }

        public async Task<Response<IEnumerable<FichaAnalistaDto>>> ObtenerFichaAnalista(ObtenerFichaAnalistaDto fichaAnalista)
        {
            return new Response<IEnumerable<FichaAnalistaDto>> { Data = this._mapper.Map<IEnumerable<FichaAnalistaDto>>(await this._IfichaAnalistaRepository.ObtenerFichaAnalista(fichaAnalista)) };
        }
    }
}
