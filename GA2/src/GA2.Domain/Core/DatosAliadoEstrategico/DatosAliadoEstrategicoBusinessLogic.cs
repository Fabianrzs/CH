using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.DatosAliadoEstrategico;
using GA2.Domain.Entities;
using GA2.Domain.Entities.DatosAliadoEstrategico;
using GA2.Infraestructure.Repositories.DatosAliadoEstrategico;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.DatosAliadoEstrategico
{
    public class DatosAliadoEstrategicoBusinessLogic : IDatosAliadoEstrategicoBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IDatosAliadoEstrategicoRepository _datosAliadoEstrategicoRepository;

        public DatosAliadoEstrategicoBusinessLogic(IMapper mapper, IDatosAliadoEstrategicoRepository datosAliadoEstrategicoRepository)
        {
            _mapper = mapper;
            _datosAliadoEstrategicoRepository = datosAliadoEstrategicoRepository;
        }

        public async Task<Response<DatosAliadoEstrategicoDto>> ActualizarDatosAliadoEstrategico(DatosAliadoEstrategicoDto aliadoEstrategico)
        {
            DatosAliadoEstrategicos aliado = this._mapper.Map<DatosAliadoEstrategicos>(aliadoEstrategico);
            return new Response<DatosAliadoEstrategicoDto> { Data = this._mapper.Map<DatosAliadoEstrategicoDto>(await this._datosAliadoEstrategicoRepository.ActualizarDatosAliadoEstrategico(aliado)) };
        }

        public async Task<Response<DatosAliadoEstrategicoDto>> CrearDatosAliadoEstrategico(DatosAliadoEstrategicoDto aliadoEstrategico)
        {
            try
            {
                DatosAliadoEstrategicos aliado = _mapper.Map<DatosAliadoEstrategicos>(aliadoEstrategico);

                var resultado = await _datosAliadoEstrategicoRepository.CrearDatosAliadoEstrategico(aliado);

                return new Response<DatosAliadoEstrategicoDto>
                {
                    IsSuccess = true,
                    Data = _mapper.Map<DatosAliadoEstrategicoDto>(resultado),
                    ReturnMessage = "Proceso realizado correctamente."
                };
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;

                if (ex.InnerException != null)
                {
                    mensaje += " | InnerException: " + ex.InnerException.Message;
                }

                return new Response<DatosAliadoEstrategicoDto>
                {
                    IsSuccess = false,
                    Data = null,
                    ReturnMessage = mensaje
                };
            }
        }

        public async Task<Response<DatosAliadoEstrategicoDto>> ObtenerDatosAliadoEstrategico(Guid aliadoEstrategico)
        {
            return new Response<DatosAliadoEstrategicoDto> { Data = this._mapper.Map<DatosAliadoEstrategicoDto>(await this._datosAliadoEstrategicoRepository.ObtenerDatosAliadoEstrategico(aliadoEstrategico)) };
        }
    }
}