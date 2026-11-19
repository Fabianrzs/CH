using AutoMapper;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class OpcionesTemporalesBusinessLogic : IOpcionesTemporalesBusinessLogic
    {
        /// <summary>
        /// Inyeccion de formuarlio repositorio
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>21/04/2021</date>
        private readonly IOpcionesTemporalesRepository _opcionesTemporalesRepository;

        /// <summary>
        /// Mappeador
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formularioRepository"></param>
        /// <param name="mapper"></param>
        public OpcionesTemporalesBusinessLogic(IOpcionesTemporalesRepository opcionesTemporalesRepository, IMapper mapper)
        {
            _opcionesTemporalesRepository = opcionesTemporalesRepository;
            this.mapper = mapper;
        }
        public async Task<Response<OpcionTemporalUsuarioDto>> EditarOpcionesTemporalesUsuario(OpcionTemporalUsuarioDto opcionTemporalUsuarioDto)
        {
            return new Response<OpcionTemporalUsuarioDto>
            {
                Data = this.mapper.Map<OpcionTemporalUsuarioDto>(
                        await this._opcionesTemporalesRepository.EditarOpcionesTemporalesUsuario(
                            this.mapper.Map<OpcionTemporalUsuario>(opcionTemporalUsuarioDto)))
            };
        }

        public async Task<Response<OpcionTemporalUsuarioDto>> EliminarOpcionesTemporalesUsuario(int idOpcionTemporalUsuario)
        {
            return new Response<OpcionTemporalUsuarioDto>
            {
                Data = this.mapper.Map<OpcionTemporalUsuarioDto>(
                        await this._opcionesTemporalesRepository.EliminarOpcionesTemporalesUsuario(idOpcionTemporalUsuario))
            };
        }

        public async Task<Response<OpcionTemporalUsuarioDto>> InsertarOpcionesTemporalesUsuario(OpcionTemporalUsuarioDto opcionTemporalUsuarioDto)
        {
            return new Response<OpcionTemporalUsuarioDto>
            {
                Data = this.mapper.Map<OpcionTemporalUsuarioDto>(
                        await this._opcionesTemporalesRepository.InsertarOpcionesTemporalesUsuario(
                            this.mapper.Map<OpcionTemporalUsuario>(opcionTemporalUsuarioDto)))
            };
        }

        public async Task<Response<IEnumerable<OpcionTemporalUsuarioDto>>> ObtenerOpcionesTemporalesUsuario(Guid idUsuario)
        {
            return new Response<IEnumerable<OpcionTemporalUsuarioDto>>
            {
                Data = this.mapper.Map<IEnumerable<OpcionTemporalUsuarioDto>>(
                        await this._opcionesTemporalesRepository.ObtenerOpcionesTemporalesUsuario(idUsuario))
            };
        }
    }
}
