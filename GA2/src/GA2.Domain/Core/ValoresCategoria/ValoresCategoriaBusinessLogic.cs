using AutoMapper;
using GA2.Application.Dto.Escrituras;
using GA2.Application.Dto.ValoresCategoria;
using GA2.Domain.Entities.ValoresCategoria;
using GA2.Infraestructure.Repositories.ValoresCategoria;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ValoresCategoria
{
    public class ValoresCategoriaBusinessLogic : IValoresCategoriaBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IValoresCategoriaRepository _iValoresCategoriaRepository;

        public ValoresCategoriaBusinessLogic(IMapper mapper, IValoresCategoriaRepository iValoresCategoriaRepository)
        {
            _mapper = mapper;
            _iValoresCategoriaRepository = iValoresCategoriaRepository;
        }

        public async Task<Response<ValoresCategoriaDto>> ActualizarValoresCategoria(ValoresCategoriaDto ValoresCategoria)
        {
            ValoresCategorias valoresCategoriasRequest = this._mapper.Map<ValoresCategorias>(ValoresCategoria);
            return new Response<ValoresCategoriaDto> { Data = this._mapper.Map<ValoresCategoriaDto>(await this._iValoresCategoriaRepository.ActualizarValoresCategoria(valoresCategoriasRequest)) };
        }

        public async Task<Response<IEnumerable<ValoresCategoriaDto>>> CrearValoresCategoria(CrearValoresDto ValoresCategoria)
        {
            ValoresCategoria.Porc_Aumento /= 100;
            return new Response<IEnumerable<ValoresCategoriaDto>> { Data = this._mapper.Map<IEnumerable<ValoresCategoriaDto>>(await this._iValoresCategoriaRepository.CrearValoresCategoria(ValoresCategoria)) };
        }

        public async Task<Response<IEnumerable<ValoresCategoriaDto>>> ObtenerValoresCategoria(int Anio)
        {
            return new Response<IEnumerable<ValoresCategoriaDto>> { Data = this._mapper.Map<IEnumerable<ValoresCategoriaDto>>(await this._iValoresCategoriaRepository.ObtenerValoresCategoria(Anio)) };
        }
    }
}
