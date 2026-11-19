using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.GrupoOpciones;
using GA2.Application.Dto.Identity;
using GA2.Application.Main;
using GA2.Application.Main.TU;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using GA2.Transversals.Commons.Constants;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    /// <summary>
    /// Login business login GA2
    /// </summary>
    /// <author>Oscar Julian Rojas</author>
    /// <date>25/03/2021</date>
    public class LoginBusinessLogic : ILoginBusinessLogic
    {
        /// <summary>
        /// Propiedades aplicacion 
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>03/05/2021</date>
        private readonly IMapper _mapper;
        private readonly ITokenClaims _tokenClaims;
        private readonly ILoginGA2Repository _loginGA2Repository;
        private readonly ILoginRequestProvider _loginRequestProvider;
        private readonly IOptions<AppMainOptions> _appOptions;
        private readonly IOptions<EmailOptions> _emailOptions;
        private readonly ICryptoGA2 _cryptoGA2;
        private readonly IOpcionesTemporalesRepository _opcionesTemporalesRepository;
        private readonly IGrupoOpcionesRepository _grupoOpcionesRepository;

        /// <summary>
        /// Constructor de negocio login GA2
        /// </summary>
        /// <param name="mapper">Dependencia de mapper</param>
        /// <param name="tokenClaims">Dependencias de claims</param>
        /// <param name="loginGA2Repository">Dependencia de loginrepository</param>
        /// <param name="loginRequestProvider">Dependencia de loginrepository</param>
        /// <param name="configuration">Configuracion aplicacion</param>
        /// <author>Oscar Julian Rojas</author>
        /// <date>03/05/2021</date>
        public LoginBusinessLogic(
            IMapper mapper,
            ITokenClaims tokenClaims,
            ILoginGA2Repository loginGA2Repository,
            ILoginRequestProvider loginRequestProvider,
            ICryptoGA2 cryptoGA2,
            IOptions<AppMainOptions> appOptions,
            IOptions<EmailOptions> emailOptions,
            IOpcionesTemporalesRepository opcionesTemporalesRepository,
            IGrupoOpcionesRepository grupoOpcionesRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(Mapper));
            _tokenClaims = tokenClaims;
            _loginGA2Repository = loginGA2Repository;
            _loginRequestProvider = loginRequestProvider;
            _cryptoGA2 = cryptoGA2;
            _appOptions = appOptions;
            _emailOptions = emailOptions;
            _opcionesTemporalesRepository = opcionesTemporalesRepository;
            _grupoOpcionesRepository = grupoOpcionesRepository;
        }

        /// <summary>
        /// Metodo login aplicacion GA2
        /// </summary>
        /// <param name="loginDto">Datos de logueo</param>
        /// <author>Oscar Julian Rojas</author>
        /// <date>03/05/2021</date>
        /// <returns>Claims Token Jwt</returns>
        public async Task<Response<ResponseLoginDto>> LoginGA2(LoginDto loginDto)
        {
            var phraseKey = _appOptions.Value.PhraseKey;
            var hash = _appOptions.Value.Hash;
            loginDto.Credential = this._cryptoGA2.EncryptMD5(loginDto.Credential + hash, phraseKey);

            var login = this._mapper.Map<User>(loginDto);

            var user = await this._loginGA2Repository.AccesoUsuario(login);


            if (user != null && user.USR_ESTADO)
            {
                // Specify the time zone for UTC-5 (Eastern Standard Time)
                string timeZoneId = "Eastern Standard Time";

                // Get the time zone information
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

                // Convert the input datetime to UTC-5
                DateTime utcMinus5DateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZone);

                //Se valida si la ultima sesion sigue activa
                var checkSession = await _loginGA2Repository.CheckSessionLogin(user.USR_ID.Value);
                if (checkSession != null && (checkSession.LSS_FECHA_ULTIMA_ACTUALIZACION > utcMinus5DateTime.AddMinutes(-3)))
                {
                    if (checkSession.LSS_ACTIVA)
                        return new Response<ResponseLoginDto> { Data = null, ReturnMessage = IdentityConstants.UsuarioYaTieneSesionAbierta };
                }

                var directory = user.ACTIVE_DIRECTORY;

                var op = await this._loginGA2Repository.ObtenerOpcionesUsuariosPorId(Guid.Parse(user.USR_ID.ToString()));
                var opTemp = await this._opcionesTemporalesRepository.ObtenerOpcionesTemporalesUsuario(Guid.Parse(user.USR_ID.ToString()));

                List<GrupoOpciones> grupoOpcionesList = op.ToList();

                foreach (var item in opTemp)
                {
                    if (!grupoOpcionesList.Any(x => x.ID == item.OPC_ID) && item.UOT_FECHA_HASTA >= DateTime.Now)
                    {
                        GrupoOpciones grupoOpciones = new()
                        {
                            ID = item.OPC_ID,
                            CODIGO = item.OPC_CODIGO,
                            DESCRIPCION = item.OPC_DESCRIPCION,
                            NOMBRE = item.OPC_NOMBRE
                        };

                        grupoOpcionesList.Add(grupoOpciones);
                    }
                }

                op = grupoOpcionesList.AsEnumerable();

                var token = await this._tokenClaims.GetTokenAsync(user);
                ResponseLoginDto response = new()
                {
                    token = token
                };

                //if (!directory)
                //{
                    if (user.USR_PASSWORD.Equals(loginDto.Credential) || loginDto.IsActiveDirectory)
                    {
                        if (op != null)
                        {
                            var opcions = this._mapper.Map<IEnumerable<GrupoOpciones>, IEnumerable<OpcionesDto>>(op);             // this._mapper.Map<GrupoOpciones>(await this._loginGA2Repository.ObtenerOpcionesUsuariosById(user.USR_ID));
                            response.opciones = (List<OpcionesDto>)opcions;

                            foreach (var item in opcions)
                            {
                                item.SubOpciones = this._mapper.Map<IEnumerable<SubOpcionesDto>>(await this._grupoOpcionesRepository.ObtenerSubOpcionesByGrupoId(item.IdGrupo, item.Id)).ToList();
                            }

                        }
                        else
                        {
                            List<OpcionesDto> list = new();
                            response.opciones = list;
                        }

                        response.USR_ID = user.USR_ID;
                        response.USR_PRIMER_NOMBRE = user.USR_NOMBRE;
                        response.USR_SEGUNDO_NOMBRE = user.USR_SEGUNDONOMBRE;
                        response.USR_PRIMER_APELLIDO = user.USR_APELLIDO;
                        response.USR_SEGUNDO_APELLIDO = user.USR_SEGUNDOAPELLIDO; 
                        await _loginGA2Repository.CreateSessionLogin(user.USR_ID.Value);
                        return new Response<ResponseLoginDto> { Data = response };
                    }
                    else
                        return new Response<ResponseLoginDto> { Data = null, ReturnMessage = IdentityConstants.UsuarioInvalido };
                //}
                //else
                //{
                //    var autenticate = AutenticarDirectoryActive(loginDto.UserName, loginDto.Credential);
                //    if (autenticate)
                //    {
                //        if (op != null)
                //        {
                //            var opcions = this._mapper.Map<IEnumerable<GrupoOpciones>, IEnumerable<OpcionesDto>>(op);
                //            response.opciones = (List<OpcionesDto>)opcions;
                //        }
                //        else
                //        {
                //            List<OpcionesDto> list = new();
                //            response.opciones = list;

                //        }

                //        return new Response<ResponseLoginDto> { Data = response };

                //    }
                //    else
                //        return new Response<ResponseLoginDto> { Data = null, ReturnMessage = IdentityConstants.UsuarioInvalido };
                //}
            }
            else
                return new Response<ResponseLoginDto> { Data = null, ReturnMessage = IdentityConstants.UsuarioInvalido };
        }

        /// <summary>
        /// Metodo login aplicacion GA2
        /// </summary>
        /// <param name="loginDto">Datos de logueo</param>
        /// <author>Oscar Julian Rojas</author>
        /// <date>03/05/2021</date>
        /// <returns>Claims Token Jwt</returns>
        public async Task<Response<ResponseLoginDto>> LoginUEJ(LoginDto loginDto)
        {
            var phraseKey = _appOptions.Value.PhraseKey;
            var hash = _appOptions.Value.Hash;
            loginDto.Credential = this._cryptoGA2.EncryptMD5(loginDto.Credential + hash, phraseKey);

            var login = this._mapper.Map<User>(loginDto);

            var user = await this._loginGA2Repository.LoginUserUEJ(login); //tenía el LoginUser de GA2, se corrigió por LoginUserUEJ

            if (user != null)
            {
                var directory = user.ACTIVE_DIRECTORY;

                var op = await this._loginGA2Repository.ObtenerOpcionesUsuariosPorId(Guid.Parse(user.USR_ID.ToString()));
                var token = await this._tokenClaims.GetTokenAsync(user);
                ResponseLoginDto response = new()
                {
                    token = token
                };

                if (!directory)
                {
                    if (user.USR_PASSWORD.Equals(loginDto.Credential))
                    {
                        if (op != null)
                        {
                            var opcions = this._mapper.Map<IEnumerable<GrupoOpciones>, IEnumerable<OpcionesDto>>(op);             // this._mapper.Map<GrupoOpciones>(await this._loginGA2Repository.ObtenerOpcionesUsuariosById(user.USR_ID));
                            response.opciones = (List<OpcionesDto>)opcions;
                        }
                        else
                        {
                            List<OpcionesDto> list = new();
                            response.opciones = list;
                        }
                        return new Response<ResponseLoginDto> { Data = response };
                    }
                    else
                        return new Response<ResponseLoginDto> { Data = null, ReturnMessage = IdentityConstants.UsuarioInvalido };
                }
                else
                {
                    var autenticate = AutenticarDirectoryActive(loginDto.UserName, loginDto.Credential);
                    if (autenticate)
                    {
                        if (op != null)
                        {
                            var opcions = this._mapper.Map<IEnumerable<GrupoOpciones>, IEnumerable<OpcionesDto>>(op);
                            response.opciones = (List<OpcionesDto>)opcions;
                        }
                        else
                        {
                            List<OpcionesDto> list = new();
                            response.opciones = list;

                        }

                        return new Response<ResponseLoginDto> { Data = response };

                    }
                    else
                        return new Response<ResponseLoginDto> { Data = null, ReturnMessage = IdentityConstants.UsuarioInvalido };
                }
            }
            else
                return new Response<ResponseLoginDto> { Data = null, ReturnMessage = IdentityConstants.UsuarioInvalido };
        }

        /// <summary>
        /// Metodo login aplicacion GA2
        /// </summary>
        /// <param name="loginDto">Datos de logueo</param>
        /// <author>Oscar Julian Rojas</author>
        /// <date>03/05/2021</date>
        /// <returns>Claims Token Jwt</returns>
        public async Task<Response<LoginPAAResponse>> LoginGA2Paa(LoginPaaDto loginPaaDto)
        {
            var cliente = await this._loginRequestProvider.ObtenerClienteLoginGA2Paa(loginPaaDto);
            if (cliente != null)
            {
                if (cliente.Data != null)
                {
                    LoginPAAResponse response = new()
                    {
                        Identificacion = cliente.Data.Identificacion,
                        IdTipoIdentificacion = cliente.Data.IdTipoIdentificacion,
                        PrimerApellido = cliente.Data.PrimerApellido,
                        PrimerNombre = cliente.Data.PrimerNombre,
                        SegundoApellido = cliente.Data.SegundoApellido,
                        SegundoNombre = cliente.Data.SegundoNombre,
                        NumeroTelefono = cliente.Data.Telefono,
                        Token = await this._tokenClaims.GetTokenToProviderAsync()
                    };

                    return new Response<LoginPAAResponse> { Data = response };
                }
            }
            return new Response<LoginPAAResponse> { Data = null, ReturnMessage = IdentityConstants.LoginInvalidoPaa };
        }

        /// <summary>
        /// Obtener lista de menu
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>03/05/2021</date>
        /// <returns>Lista de menu</returns>
        public async Task<Response<IEnumerable<MenuDto>>> ObtenerMenu()
        {
            List<MenuDto> menu = this._mapper.Map<IEnumerable<MenuDto>>(await this._loginGA2Repository.ObtenerMenu()).ToList();
            List<SubMenuDto> submenus = this._mapper.Map<IEnumerable<SubMenuDto>>(await this._loginGA2Repository.ObtenerSubmenu()).ToList();
            foreach (MenuDto item in menu)
                item.SubMenus = submenus.Where(x => x.MenuId == item.Id).ToList();

            return new Response<IEnumerable<MenuDto>> { Data = menu };
        }

        public async Task<Response<string>> RecuperaContrasena(RecuperarContrasenaDto recuperarContrasenaDto)
        {
            var repust = await this._loginGA2Repository.RecuperaContrasena(recuperarContrasenaDto.Tipo.ToString(), recuperarContrasenaDto.Numero);

            if (repust != null)
            {
                string token = GetSHA256(Guid.NewGuid().ToString());
                var user = await this._loginGA2Repository.ActualizarTokenUsuario(Guid.Parse(repust.USR_ID.ToString()), token);

                if (user != null)
                {

                    string url = recuperarContrasenaDto.Url + user.TOKEN;

                    MailMessage correo = new MailMessage();
                    correo.From = new MailAddress(this._emailOptions.Value.UserName, "CajaHonor", System.Text.Encoding.UTF8);
                    correo.To.Add(user.USR_EMAIL);
                    correo.Subject = "Restablecer contraseña";
                    correo.Body = "<h3>Restablecer contraseña </h3> <p>Has realizado una solicitud de cambio de contraseña.</p> <p>Si el enlace no funciona, copia y pega el siguiente texto en labarra de direcciones de tu navegador:</p> <div><a href='" + url + "'>" + url + "</a> </div>";
                    correo.IsBodyHtml = true;
                    correo.Priority = MailPriority.Normal;

                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;
                    smtp.Host = "smtp-mail.outlook.com";
                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential(this._emailOptions.Value.UserName, this._emailOptions.Value.Password);

                    smtp.EnableSsl = true;
                    smtp.Send(correo);

                    return new Response<string> { IsSuccess = true, ReturnMessage = IdentityConstants.LoginRecuperaContrasena };
                }
                else
                    return new Response<string> { IsSuccess = true, ReturnMessage = IdentityConstants.LoginRecuperaContrasena };

            }
            else
                return new Response<string> { IsSuccess = true, ReturnMessage = IdentityConstants.LoginRecuperaContrasena };
        }

        /// <summary>
        /// Autenticacion de directorio activo
        /// </summary>
        /// <param name="path">Dominio del directorio</param>
        /// <returns></returns>
        private bool AutenticarDirectoryActive(string userName, string password)
        {
            try
            {
                DirectoryEntry de = new DirectoryEntry("LDAP://cpvmp-01x.caprovimpo.local:389/DC=caprovimpo,DC=Local", userName, password);
                DirectorySearcher dsearch = new DirectorySearcher(de);
                dsearch.Filter = "sAMAccountName=" + userName + "";
                SearchResult results = null;
                results = dsearch.FindOne();
                return results.Properties.Count != 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        public async Task<Response<UsuarioDto>> ValidaToken(string token)
        {
            var userIdentificacion = await this._loginGA2Repository.ValidaTokenRestablecimiento(token);
            if (userIdentificacion != null)
                return new Response<UsuarioDto> { IsSuccess = true, ReturnMessage = "El token es valido" };
            else
                return new Response<UsuarioDto> { IsSuccess = false, ReturnMessage = "El enlace de restablecimiento de contraseña ha expirado" };
        }

        public async Task<Response<bool>> UpdateSession(string idUser)
        {
            var active = await this._loginGA2Repository.UpdateSession(Guid.Parse(idUser));

            return new Response<bool> { IsSuccess = true, ReturnMessage = "El enlace de restablecimiento de contraseña ha expirado", Data = active };
        }

        public async Task<Response<bool>> Logout(string idUser)
        {
            await this._loginGA2Repository.Logout(Guid.Parse(idUser));

            return new Response<bool> { IsSuccess = true, ReturnMessage = "El enlace de restablecimiento de contraseña ha expirado" };
        }
    }
}
