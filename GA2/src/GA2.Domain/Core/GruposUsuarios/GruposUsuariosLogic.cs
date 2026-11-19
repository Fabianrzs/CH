using AutoMapper;

using GA2.Application.Dto.GrupoUsuarios;
using GA2.Domain.Entities.GruposUsuarios;
using GA2.Infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA2.Transversals.Commons;
using GA2.Application.Dto.GrupoOpciones;
using GA2.Application.Dto.BUA;
using GA2.Application.Main.ProviderRequest;
using GA2.Application.Main;

namespace GA2.Domain.Core
{
    public class GruposUsuariosLogic : IGruposUsuariosLogic
    {
        private readonly IGruposUsuariosRepository _gruposUsuariosRepository;
        private readonly IMapper _mapper;
        private readonly IGrupoOpcionesRepository _grupoOpcionesRepository;
        private readonly IClientRequestProvider _iClientRequestProvider;

        public GruposUsuariosLogic(
            IGruposUsuariosRepository gruposUsuariosRepository,
            IGrupoOpcionesRepository grupoOpcionesRepository,
            IMapper mapper,
            IClientRequestProvider clientProvider)
        {
            _gruposUsuariosRepository = gruposUsuariosRepository;
            _grupoOpcionesRepository = grupoOpcionesRepository;
            _mapper = mapper;
            _iClientRequestProvider = clientProvider;
        }

        public async Task<Response<GruposUsuariosDto>> CreateGroupUser(GruposUsuariosDto grupo, string userId)
        {
            GrupoUsuario grupoUser=new GrupoUsuario();

            grupoUser.CODIGO = grupo.Codigo;
            grupoUser.NOMBRE = grupo.Nombre;
            grupoUser.DESCRIPCION = grupo.Descripcion;
            grupoUser.ESTADO = grupo.Estado;
            grupoUser.USR_CREADOR = Guid.Parse(userId);

            var gp= await this._gruposUsuariosRepository.ObtenerGrupoUsuarioById(grupo.Codigo);

            if (gp != null)
            {
                return new Response<GruposUsuariosDto> { IsSuccess = false, ReturnMessage = "Ya existe un grupo de usuario con este código:" + grupo.Codigo };
            }
            else {

                var repust = await this._gruposUsuariosRepository.CrearGrupoUsuario(grupoUser);
                foreach (GrupoOpcionesDto item in grupo.opciones)
                {
                    await this._gruposUsuariosRepository.InsertGrupoOpciones(item.Id,repust.ID);       
                }

               return new Response<GruposUsuariosDto> { Data = grupo };

            }
        }

        public async Task<Response<GruposUsuariosDto>> DeleteGrupoUsuario(GruposUsuariosDto grupo,string userId)
        {  
            var gp = await this._gruposUsuariosRepository.DeleteGrupoUsuarioById(grupo.Id);
            return new Response<GruposUsuariosDto> { Data = this._mapper.Map<GruposUsuariosDto>(gp) };
        }

        public async Task<Response<IEnumerable<GruposUsuariosDto>>> ObtenerGruposUsuarios()
        {


            var grupousuarios = this._mapper.Map<IEnumerable<GrupoUsuario>, IEnumerable<GruposUsuariosDto>>(await this._gruposUsuariosRepository.ObtenerGruposUsuarios());

            List<GruposUsuariosDto> OpcionList = new List<GruposUsuariosDto>();

            foreach (GruposUsuariosDto item in grupousuarios)
            {

                var dataOpcion = await this._grupoOpcionesRepository.GetGrupoOpcionesGrupoById(item.Id);
                var maperOption = this._mapper.Map<IEnumerable<GrupoOpcionesDto>>(dataOpcion);

                item.opciones= (List<GrupoOpcionesDto>)maperOption;
                OpcionList.Add(item);
            }

            return new Response<IEnumerable<GruposUsuariosDto>> { Data = OpcionList };

        }

        public async Task<Response<GruposUsuariosDto>> UpdateGrupoUsuario(GruposUsuariosDto grupo, string userId)
        {
            GrupoUsuario gpo = new GrupoUsuario();
            gpo.ID = grupo.Id;
            gpo.NOMBRE = grupo.Nombre;
            gpo.DESCRIPCION = grupo.Descripcion;
            gpo.CODIGO = grupo.Codigo;
            gpo.ESTADO = grupo.Estado;
            gpo.USR_MODIFICADOR = Guid.Parse(userId);

            var gr = await this._gruposUsuariosRepository.ActualizarGrupoUsuario(gpo);


            await this._grupoOpcionesRepository.EliminarGruposOpcionesGrupoById(grupo.Id);

            foreach (GrupoOpcionesDto item in grupo.opciones)
            {
                await this._grupoOpcionesRepository.InsertGruposOpcionesGrupoById(grupo.Id, item.Id);
            }

            InsertarPermisoActualizacionDto insertarPermisoActualizacionDto = new InsertarPermisoActualizacionDto();
            insertarPermisoActualizacionDto.Perfil = grupo.Codigo;
            var permiso = grupo.opciones.Any(x => x.Codigo.Equals("ACTUALIZAHOJAVIDA"));

            if (permiso)
            {
                insertarPermisoActualizacionDto.Elimina = 0;  
            }
            else
            {
                insertarPermisoActualizacionDto.Elimina = 1;  
            }


            await _iClientRequestProvider.InsertarPermisoActualizacion(insertarPermisoActualizacionDto);

            return new Response<GruposUsuariosDto> { Data = grupo };

        }
        public async Task<Response<IEnumerable<ResponseGruposUsuarioPorId>>> ObtenerGruposUsuarioPorId(Guid request)
        {
            var grupos = await _grupoOpcionesRepository.ObtenerGruposUsuarioPorId(request);
            return new Response<IEnumerable<ResponseGruposUsuarioPorId>> { Data = grupos };
        }
    }
}
