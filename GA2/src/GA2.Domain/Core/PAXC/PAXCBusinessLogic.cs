using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.CapXc;
using GA2.Infraestructure.Repositories.PAXC;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.PAXC
{
    public class PAXCBusinessLogic : IPAXCBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IPAXCRepository _iPAXCRepository;
        public PAXCBusinessLogic(IMapper mapper, IPAXCRepository iPAXCRepository)
        {
            _mapper = mapper;
            _iPAXCRepository = iPAXCRepository;
        }
        public async Task<Response<ObtenerPAXCDto>> ObtenerPAXC(int IdCiudad)
        {
            List<ObtenerUserXCiudadDTO> LstAbogados = (List<ObtenerUserXCiudadDTO>)this._mapper.Map<IEnumerable<ObtenerUserXCiudadDTO>>(await this._iPAXCRepository.ObtenerPeritosXCiudad(IdCiudad));
            List<ObtenerUserXCiudadDTO> LstPeritos = (List<ObtenerUserXCiudadDTO>)this._mapper.Map<IEnumerable<ObtenerUserXCiudadDTO>>(await this._iPAXCRepository.ObtenerAbogadosXCiudad(IdCiudad));
            ObtenerPAXCDto obtenerPAXCDto = new ObtenerPAXCDto()
            {
                LstAbogados = LstAbogados,
                LstPeritos = LstPeritos
            };

            return new Response<ObtenerPAXCDto> { Data = obtenerPAXCDto };
        }
    }
}
