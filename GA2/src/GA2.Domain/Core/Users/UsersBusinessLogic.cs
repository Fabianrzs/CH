using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.GrupoUsuarios;
using GA2.Application.Main;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Email;
using GA2.Domain.Entities.GruposUsuarios;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    /// <summary>
    /// User Core Negocio
    /// <author>OScar Julian Rojas Garces</author>
    /// <date>24/02/2021</date>
    /// </summary>
    public class UsersBusinessLogic : IUsersBusinessLogic
    {
        /// <summary>
        /// Propiedades privadas Core Negocio
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// IUser Repository
        /// </summary>
        private readonly IUsersRepository _usersRepository;

        /// <summary>
        /// Crypto GA para cifrar datos
        /// </summary>
        private readonly ICryptoGA2 _cryptoGA2;

        /// <summary>
        /// Configuracion de aplicacion
        /// </summary>
        private readonly IConfiguration _configuration;

        private readonly IOpcionesTemporalesRepository _opcionesTemporalesRepository;

        /// <summary>
        /// Constructor Negocio Users
        /// </summary>
        /// <param name="mapper">Instancia mapper</param>
        /// <param name="usersRepository">Instancia repositorio usuarios</param>
        /// <param name="cryptoGA2">Cryptografia para datos</param>
        /// <param name="configuration">Configuracion de aplicacion</param>
        public UsersBusinessLogic(
            IMapper mapper,
            IUsersRepository usersRepository,
            ICryptoGA2 cryptoGA2,
            IConfiguration configuration,
            IOpcionesTemporalesRepository opcionesTemporalesRepository

            )
        {
            _mapper = mapper;
            _usersRepository = usersRepository;
            _cryptoGA2 = cryptoGA2;
            _configuration = configuration;
            _opcionesTemporalesRepository = opcionesTemporalesRepository;
        }

        /// <summary>
        /// Metodo crear usuario
        /// </summary>
        /// <param name="usuario">Modelo dto usuario</param>
        /// <uthor>Oscar Julian Rojas Garces</uthor>
        /// <date>24/02/2021</date>
        /// <returns>Modelo dto usuario</returns>
        public async Task<Response<UsuarioDto>> CreateUser(UsuarioDto usuario,Guid idUser)
        { 
            var phraseKey = _configuration["AppMain:PhraseKey"];
            var hash = _configuration["AppMain:Hash"];
            usuario.Contrasena = this._cryptoGA2.EncryptMD5(usuario.Contrasena + hash, phraseKey);

            User user=new User();
            user.USR_NOMBRE = usuario.Nombre;
            user.USR_PASSWORD = usuario.Contrasena;
            user.USR_EMAIL = usuario.Correo;
            user.USR_IDENTIFICACION = usuario.Identificacion;
            user.USR_USERNAME = usuario.NombreUsuario;
            user.USR_SEGUNDOAPELLIDO = usuario.SegundoApellido;
            user.USR_SEGUNDONOMBRE = usuario.SegundoNombre;
            user.TID_ID = usuario.TipoIdentificationId;
            user.USR_CREADOPOR = idUser;
            user.USR_ESTADO= true;
            user.USR_APELLIDO = usuario.Apellido;
            user.ACTIVE_DIRECTORY = usuario.Directory;

            var userIdentificacion = await this._usersRepository.ObtenerUsuarioPorIdentificacion(usuario.Identificacion, usuario.TipoIdentificationId);

            if (userIdentificacion != null) {
                return new Response<UsuarioDto> { IsSuccess = false, ReturnMessage = "Ya existe un usuario registrado con ese número de documento." };
            } else {

                var response = await this._usersRepository.CreateUser(user);

                foreach (GruposUsuariosDto item in usuario.Roles)
                {
                    await this._usersRepository.InsertGruposPorUsuarios(Guid.Parse(response.USR_ID.ToString()),item.Id);
                }

                foreach (OpcionTemporalUsuarioDto item in usuario.opcionTemporales)
                {
                    item.IdUsuario = Guid.Parse(response.USR_ID.ToString());
                    await this._opcionesTemporalesRepository.InsertarOpcionesTemporalesUsuario(this._mapper.Map<OpcionTemporalUsuario>(item));
                }

                return new Response<UsuarioDto> { Data = usuario };
            } 
                       
        }

        /// <summary>
        /// Metodo actualizar usuario
        /// </summary>
        /// <param name="usuario">Modelo dto usuario</param>
        /// <uthor>Oscar Julian Rojas Garces</uthor>
        /// <date>24/02/2021</date>
        /// <returns>Modelo dto usuario</returns>
        public async Task<Response<UsuarioDto>> UpdateUser(UsuarioDto usuario)
        {
            User user = new User();
            user.USR_ID = usuario.Id;
            user.USR_NOMBRE = usuario.Nombre;
            user.USR_EMAIL = usuario.Correo;
            user.USR_IDENTIFICACION = usuario.Identificacion;
            user.USR_USERNAME = usuario.NombreUsuario;
            user.USR_SEGUNDOAPELLIDO = usuario.SegundoApellido;
            user.USR_SEGUNDONOMBRE = usuario.SegundoNombre;
            user.TID_ID = usuario.TipoIdentificationId;
            user.USR_ESTADO = usuario.Estado;
            user.USR_APELLIDO = usuario.Apellido;
            user.ACTIVE_DIRECTORY = usuario.Directory;


            var phraseKey = _configuration["AppMain:PhraseKey"];
            var hash = _configuration["AppMain:Hash"];


            var credential = this._cryptoGA2.EncryptMD5(usuario.Contrasena + hash, phraseKey);

            await this._usersRepository.UpdateUser(user);
            await this._usersRepository.DeleteGrupoUsuarioUserById(Guid.Parse(usuario.Id.ToString()));

            if (!string.IsNullOrEmpty(usuario.Contrasena))
                await this._usersRepository.ActualizarContrasenaUsuarioById((Guid)user.USR_ID, credential);


            foreach (GruposUsuariosDto item in usuario.Roles)
            {
                await this._usersRepository.InsertGruposPorUsuarios(Guid.Parse(user.USR_ID.ToString()),item.Id);

            }
 
            return new Response<UsuarioDto> { Data = usuario };
        }

        /// <summary>
        /// Metodo elimnar usuario
        /// </summary>
        /// <param name="user">Modelo dto usuario</param>
        /// <uthor>Oscar Julian Rojas Garces</uthor>
        /// <date>24/02/2021</date>
        /// <returns>Modelo dto usuario</returns>
        public async Task<Response<UsuarioDto>> DeleteUser(Guid user)
        {           
            await this._usersRepository.DeleteGrupoUsuarioUserById(user);
            var user_ = await this._usersRepository.DeleteUser(user);

            return new Response<UsuarioDto> { Data = this._mapper.Map<UsuarioDto>(user_) };
        }

        /// <summary>
        /// Metodo obtener usuarios
        /// </summary>
        /// <param name="usuario">Modelo dto usuario</param>
        /// <uthor>Oscar Julian Rojas Garces</uthor>
        /// <date>24/02/2021</date>
        /// <returns>Modelo dto lista usuario</returns>
        public async Task<Response<IEnumerable<UsuarioDto>>> GetUsers()
        {
            var response = await this._usersRepository.ObtenerUsuariosDos();
            var usuarios = this._mapper.Map<IEnumerable<User>, IEnumerable<UsuarioDto>>(response);

            List<UsuarioDto> UsersList=new List<UsuarioDto>();

            int i = 0;

            foreach (UsuarioDto item in usuarios)
            {    
                item.Roles = (List<GruposUsuariosDto>) this._mapper.Map<IEnumerable<GruposUsuariosDto>>(await this._usersRepository.GetGrupoUserById(Guid.Parse(item.Id.ToString())));
                item.NombreCompleto = item.Nombre + ' ' + item.SegundoNombre + ' ' + item.Apellido + ' ' + item.SegundoApellido;
                UsersList.Add(item);

                i++;
            }

           
                
            return new Response<IEnumerable<UsuarioDto>> { Data = UsersList };
        }

        public async Task<Response<UsuarioDto>> UpdateUserPassword(UsuarioDto usuario)
        {

            var phraseKey = _configuration["AppMain:PhraseKey"];
            var hash = _configuration["AppMain:Hash"];
            usuario.Contrasena = this._cryptoGA2.EncryptMD5(usuario.Contrasena + hash, phraseKey);

            await this._usersRepository.ActualizarContrasenaUsuario(usuario.Token,usuario.Contrasena);
            return new Response<UsuarioDto> { Data = usuario };

        }
    }
}
