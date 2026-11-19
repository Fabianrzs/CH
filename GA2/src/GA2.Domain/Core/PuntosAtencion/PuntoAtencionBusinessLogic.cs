using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.Escrituras;
using GA2.Application.Dto.PuntosAtencion;
using GA2.Domain.Entities.Escritura;
using GA2.Domain.Entities.PuntosAtencion;
using GA2.Infraestructure.Repositories.PuntosAtencion;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.PuntosAtencion
{
    public class PuntoAtencionBusinessLogic : IPuntoAtencionBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IPuntoAtencionRepository _IpuntoAtencionRepository;
        public PuntoAtencionBusinessLogic(IMapper mapper, IPuntoAtencionRepository puntoAtencionRepository)
        {
            _mapper = mapper;
            _IpuntoAtencionRepository = puntoAtencionRepository;
        }
        public async Task<Response<PuntosAtencionDto>> ActualizarPuntoAtencion(PuntosAtencionDto PuntoAtencion)
        {
            PuntoAtencion PARequest = this._mapper.Map<PuntoAtencion>(PuntoAtencion);
            return new Response<PuntosAtencionDto> { Data = this._mapper.Map<PuntosAtencionDto>(await this._IpuntoAtencionRepository.ActualizarPuntosAtencion(PARequest)) };
        }

        public async Task<Response<PuntosAtencionDto>> CrearPuntoAtencion(PuntosAtencionDto PuntoAtencion)
        {
            PuntoAtencion PARequest = this._mapper.Map<PuntoAtencion>(PuntoAtencion);
            return new Response<PuntosAtencionDto> { Data = this._mapper.Map<PuntosAtencionDto>(await this._IpuntoAtencionRepository.CrearPuntosAtencion(PARequest)) };
        }

        public async Task<Response<IEnumerable<PuntosAtencionDto>>> ObtenerPuntoAtencion()
        {
            IEnumerable<PuntosAtencionDto> ResponseData = this._mapper.Map<IEnumerable<PuntosAtencionDto>>(
                await this._IpuntoAtencionRepository.ObtenerPuntosAtencion());
            return new Response<IEnumerable<PuntosAtencionDto>>
            {
                Data = ResponseData,
            };
        }
        public async Task<Response<PuntosAtencionDto>> ObtenerPuntoAtencionPorId(int IdPuntoAtencion)
        {
            return new Response<PuntosAtencionDto> { Data = this._mapper.Map<PuntosAtencionDto>(await this._IpuntoAtencionRepository.ObtenerPuntosAtencionPorId(IdPuntoAtencion)) };
        }
    }
}
