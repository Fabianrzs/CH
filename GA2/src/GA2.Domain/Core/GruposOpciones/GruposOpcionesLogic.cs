using AutoMapper;
using Dapper;
using GA2.Application.Dto.GrupoOpciones;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSAportes;
using GA2.Domain.Entities;
using GA2.Infraestructure.Data.GrupoUsuarios;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class GruposOpcionesLogic : IGruposOpcionesLogic
    {

        private readonly IGrupoOpcionesRepository _grupoOpcionesRepository;    
        private readonly IMapper _mapper;

        public GruposOpcionesLogic(
         IGrupoOpcionesRepository grupoOpcionesRepository,
         IMapper mapper)
        {
            _grupoOpcionesRepository = grupoOpcionesRepository;
            _mapper = mapper;
        }

        public async Task<Response<GrupoOpcionesDto>> CrearGruposOpciones(GrupoOpcionesDto grupo, string userId)
        {

            GrupoOpciones grupoOpciones = new()
            {
                CODIGO = grupo.Codigo,
                NOMBRE = grupo.Nombre,
                DESCRIPCION = grupo.Descripcion,
                ESTADO = grupo.Estado,
                USR_CREADOR = Guid.Parse(userId)
            };

            var gp = await this._grupoOpcionesRepository.ObtenerGrupoOpcionesById(grupo.Codigo);

            if (gp != null)
            {
                return new Response<GrupoOpcionesDto> { IsSuccess = false, ReturnMessage = "Ya existe un grupo de opciones con este código:" + grupo.Codigo };
            }
            else
            {

                var repust = await this._grupoOpcionesRepository.CrearGrupoOpciones(grupoOpciones);
                foreach (OpcionesDto item in grupo.opciones)
                {
                    await this._grupoOpcionesRepository.InsertOpcionesGrupoById(repust.ID, item.Id);
                    foreach (SubOpcionesDto subOption in item.SubOpciones)
                    {
                        await this._grupoOpcionesRepository.InsertSubOpciones(subOption.Id, repust.ID);
                    }
                }
                return new Response<GrupoOpcionesDto> { Data = grupo };

            }

        }

        public async Task<Response<GrupoOpcionesDto>> ActualizaGruposOpciones(GrupoOpcionesDto grupo, string userId)
        {

            GrupoOpciones grupoOpciones = new()
            {
                ID = grupo.Id,
                CODIGO = grupo.Codigo,
                NOMBRE = grupo.Nombre,
                DESCRIPCION = grupo.Descripcion,
                ESTADO = grupo.Estado,
                USR_MODIFICADOR = Guid.Parse(userId)
            };

            await this._grupoOpcionesRepository.ActualizaGrupoOpciones(grupoOpciones);

            await this._grupoOpcionesRepository.EliminarOpcionesGrupoById(grupoOpciones.ID);

            foreach (OpcionesDto item in grupo.opciones)
            {
                await this._grupoOpcionesRepository.InsertOpcionesGrupoById(grupo.Id, item.Id);

                foreach (SubOpcionesDto subOption in item.SubOpciones)
                {
                    await this._grupoOpcionesRepository.InsertSubOpciones(subOption.Id, grupo.Id);
                }
            }


            return new Response<GrupoOpcionesDto> { Data = grupo };

        }

        public async Task<Response<GrupoOpcionesDto>> EliminarGruposOpciones(GrupoOpcionesDto grupo, string userId)
        {

            var gp = await this._grupoOpcionesRepository.EliminarGruposOpciones(grupo.Id);
            return new Response<GrupoOpcionesDto> { Data = this._mapper.Map<GrupoOpcionesDto>(gp) };
        
        }

        public async Task<Response<IEnumerable<GrupoOpcionesDto>>> ObtenerGruposOpciones()
        {     
            var grupoOpciones = this._mapper.Map<IEnumerable<GrupoOpciones>, IEnumerable<GrupoOpcionesDto>>(await this._grupoOpcionesRepository.ObtenerGrupoOpciones());

            List<GrupoOpcionesDto> OpcionList = new List<GrupoOpcionesDto>();

           

            foreach (GrupoOpcionesDto item in grupoOpciones)
            {

                var dataOpcion = await this._grupoOpcionesRepository.GetOpcionesGrupoById(item.Id);
                var maperOption = this._mapper.Map<IEnumerable<OpcionesDto>>(dataOpcion);


                foreach (var option in maperOption)
                {
                    option.SubOpciones = this._mapper.Map<IEnumerable<SubOpcionesDto>>(await this._grupoOpcionesRepository.ObtenerSubOpcionesByGrupoId(item.Id, option.Id)).ToList();
                    option.SubOpciones.ForEach(x => x.Checked = true);
                }

                item.opciones= maperOption.ToList();
                OpcionList.Add(item);
            }

            return new Response<IEnumerable<GrupoOpcionesDto>> { Data = OpcionList };

          
        }

        public async Task<Response<IEnumerable<OpcionesDto>>> ObtenerOpciones()
        {

            var dataOpcion = await this._grupoOpcionesRepository.GetAllOpciones();
            var maperOption = this._mapper.Map<IEnumerable<OpcionesDto>>(dataOpcion);

            foreach (var item in maperOption)
            {
                item.SubOpciones = this._mapper.Map<IEnumerable<SubOpcionesDto>>(await _grupoOpcionesRepository.ObtenerSubOpcinesPorIdOpcion(item.Id)).ToList();
                //item.SubOpciones.ForEach(x => x.Checked = true);
            }

            return new Response<IEnumerable<OpcionesDto>> { Data = maperOption };

        }

        public async Task<Response<OpcionesDto>> CrearOpciones(OpcionesDto Opcion, string userId)
        {
           

            GrupoOpciones Opciones = new GrupoOpciones();
            Opciones.CODIGO = Opcion.Codigo;
            Opciones.NOMBRE = Opcion.Nombre;
            Opciones.DESCRIPCION = Opcion.Descripcion;
            Opciones.ESTADO = Opcion.Estado;
            Opciones.USR_CREADOR = Guid.Parse(userId);

            var dataOpcion = await this._grupoOpcionesRepository.CrearOpciones(Opciones);
            return new Response<OpcionesDto> { Data = this._mapper.Map<OpcionesDto>(dataOpcion)  };
        }
    }
}
