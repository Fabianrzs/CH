using AutoMapper;
using GA2.Application.Dto.DatosAliadoEstrategico;
using GA2.Application.Dto.ValidacionTelAfiliado;
using GA2.Domain.Entities;
using GA2.Domain.Entities.DatosAliadoEstrategico;
using GA2.Domain.Entities.ValidacionTelAfiliado;
using GA2.Infraestructure.Repositories.DatosAliadoEstrategico;
using GA2.Infraestructure.Repositories.ValidacionTelAfiliado;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ValidacionTelAfiliado
{
    public class ValidacionTelAfiliadoBusinessLogic : IValidacionTelAfiliadoBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IValidacionTelAfiliadoRepository _validacionTelAfiliadoRepository;

        public ValidacionTelAfiliadoBusinessLogic(IMapper mapper, IValidacionTelAfiliadoRepository validacionTelAfiliadoRepository)
        {
            _mapper = mapper;
            _validacionTelAfiliadoRepository = validacionTelAfiliadoRepository;
        }

        public async Task<Response<ValidacionTelAfiliadoDto>> CrearValTelAfiliado(ValidacionTelAfiliadoDto TelAfiliado)
        {
            ValidacionTelAfiliados afiliado = this._mapper.Map<ValidacionTelAfiliados>(TelAfiliado);
            return new Response<ValidacionTelAfiliadoDto> { Data = this._mapper.Map<ValidacionTelAfiliadoDto>(await this._validacionTelAfiliadoRepository.CrearValTelAfiliado(afiliado)) };
        }

        public async Task<Response<ValidacionTelAfiliadoDto>> ObtenerValTelAfiliado(Guid Soc_Id)
        {
            return new Response<ValidacionTelAfiliadoDto> { Data = this._mapper.Map<ValidacionTelAfiliadoDto>(await this._validacionTelAfiliadoRepository.ObtenerValTelAfiliado(Soc_Id)) };
        }
    }
}