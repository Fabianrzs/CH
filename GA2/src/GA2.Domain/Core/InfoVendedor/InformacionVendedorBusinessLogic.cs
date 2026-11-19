using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.InfoVendedor;
using GA2.Domain.Entities;
using GA2.Domain.Entities.InfoVendedor;
using GA2.Transversals.Commons;
using GA2.Infraestructure.Repositories.InfoVendedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GA2.Domain.Core.InfoVendedor
{
    public class InformacionVendedorBusinessLogic : IInformacionVendedorBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IInformacionVendedorRepository _informacionVendedorRepository;

        public InformacionVendedorBusinessLogic(IMapper mapper, IInformacionVendedorRepository informacionVendedorRepository)
        {
            _mapper = mapper;
            _informacionVendedorRepository = informacionVendedorRepository;
        }

        public async Task<Response<InformacionVendedorDto>> CrearInformacionVendedor(InformacionVendedorDto informacionVendedorDto)
        {
            InformacionVendedor InfVendedorRequest = this._mapper.Map<InformacionVendedor>(informacionVendedorDto);

            var resultado = await this._informacionVendedorRepository.CrearInformacionVendedor(InfVendedorRequest);

            if (resultado.ErrorMessage == "Error: Cuenta existente para la solicitud")
            {
                return new Response<InformacionVendedorDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ReturnMessage = resultado.ErrorMessage
                };
            }
            else
            {
                var nuevoInformacionVendedorData = this._mapper.Map<InformacionVendedorDto>(resultado);
                return new Response<InformacionVendedorDto>
                {
                    Data = nuevoInformacionVendedorData,
                    IsSuccess = true,
                    ReturnMessage = null
                };
            }
        }

        public async Task<Response<IEnumerable<InformacionVendedorDto>>> ObtenerInformacionVendedor(consultarVendedorDto request)
        {
            return new Response<IEnumerable<InformacionVendedorDto>> { Data = this._mapper.Map<IEnumerable<InformacionVendedorDto>>(await this._informacionVendedorRepository.ObtenerInformacionVendedor(request)) };
        }

        public async Task<Response<IEnumerable<InformacionVendedorDto>>> EliminarInformacionVendedor(InformacionVendedorDto request)
        {
            var solicitud = _mapper.Map<InformacionVendedor>(request);
            return new Response<IEnumerable<InformacionVendedorDto>>() { Data = _mapper.Map<IEnumerable<InformacionVendedorDto>>(await this._informacionVendedorRepository.EliminarInformacionVendedor(solicitud)) };
        }
    }
}
