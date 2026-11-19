using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using GA2.Transversals.Commons.Constants;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Main
{
    /// <summary>
    /// Implementacion de generacion de token
    /// <author>Oscar Julian Rojas Garces</author>
    /// <date>24/02/2021</date>
    /// </summary>
    public class TokenClaims : ITokenClaims
    {
        /// <summary>
        /// Instancia de configuracion aplicacion
        /// </summary>
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenClaims> _logger;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="configuration">Instancia de configuracion</param>
        /// <author>Oscar Julian Rojas Garces</author>
        /// <date>24/02/2021</date>
        public TokenClaims(IConfiguration configuration, ILogger<TokenClaims> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Generador de tokens 
        /// </summary>
        /// <param name="user">Usuario validado</param>
        /// <returns>Token string</returns>
        public async Task<string> GetTokenAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this._configuration["JwtOptions:Key"]);

            var claims = new List<Claim>();

            if (user == null)
            {
                await Task.CompletedTask;
                return null;
            }
            foreach (PropertyInfo prop in user.GetType().GetProperties())
            {
                _ = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (prop.Name != "USR_PASSWORD")
                    if (prop.GetValue(user, null) != null)
                        claims.Add(new Claim(prop.Name, prop.GetValue(user, null).ToString()));
            }

            claims.Add(new Claim("Name", user.USR_ID.ToString()));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            await Task.CompletedTask;
            return tokenHandler.WriteToken(token);
        }


        /// <summary>
        /// Generador de tokens 
        /// </summary>
        /// <param name="cliente">Cliente Bua jwt</param>
        /// <returns>Token string</returns>
        public async Task<string> GetTokenAsync(ClienteDto cliente)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this._configuration["JwtOptions:Key"]);

            var claims = new List<Claim>();

            if (cliente == null)
            {
                await Task.CompletedTask;
                return null;
            }
            foreach (PropertyInfo prop in cliente.GetType().GetProperties())
            {
                _ = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (prop.GetValue(cliente, null) != null)
                    claims.Add(new Claim(prop.Name, prop.GetValue(cliente, null).ToString()));
            }

            claims.Add(new Claim("Name", cliente.IdCliente.ToString()));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            await Task.CompletedTask;
            return tokenHandler.WriteToken(token);
        }



        /// <summary>
        /// Generate Token para Proveedor de servicios
        /// </summary>
        /// <returns>Jwt String</returns>
        public async Task<string> GetTokenToProviderAsync(ClienteDto cliente)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtOptions:Key"]);

            var claims = new List<Claim>
            {
                new Claim("IdCliente", cliente.IdCliente.ToString()),
                new Claim("Identificacion", cliente.Identificacion),
                new Claim("IdTipoIdentificacion", cliente.IdTipoIdentificacion.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            await Task.CompletedTask;
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Generate Token para Proveedor de servicios
        /// </summary>
        /// <returns>Jwt String</returns>
        public async Task<string> GetTokenToProviderAsync()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtOptions:Key"]);

            var claims = new List<Claim>
            {
                new Claim("AppIdentity", Guid.NewGuid().ToString()),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            await Task.CompletedTask;
            return tokenHandler.WriteToken(token);
        }


        /// <summary>
        /// Es token valido 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool EsTokenValido(string token, out IEnumerable<Claim> claims)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException("Debe logueaser para poder acceder al metodo.");
            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var token_ = token.Split(' ')[1];
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token_, tokenValidationParameters, out SecurityToken validatedToken);
                claims = tokenValid.Claims;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error EsTokenValido ");
                claims = null;
                return false;
            }
        }

        /// <summary>
        /// Obtener parametros de validacion token
        /// </summary>
        /// <returns>Token validation model</returns>
        private TokenValidationParameters GetTokenValidationParameters()
        {
            var configurationJwt = _configuration;
            var key = Encoding.ASCII.GetBytes(""
                //configurationJwt
                );
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidIssuer = GA2Constants.ApiName,
                ValidAudience = GA2Constants.ApiName,
                ValidateAudience = false,
            };
        }

        /// <summary>
        /// Valida el token para devolver una httpRosponsedata dependiendo de la autenticacion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public HttpResponseData TokenValidation(HttpRequestData request)
        {
            var response = request.CreateResponse();
            try
            {
                var token = request.Headers.TryGetValues("Authorization", out IEnumerable<string> valores) ? request.Headers.GetValues("Authorization").FirstOrDefault() : null;

                //if (token == null)
                //{
                //    response.WriteAsJsonAsync(new Response<object>() { ReturnMessage = "Unauthorized - No token sent" });
                //    response.StatusCode = HttpStatusCode.Unauthorized;
                //    response.Headers.Remove("Content-Type");
                //}
                //else if (!EsTokenValido(token.ToString(), out IEnumerable<Claim> claims))
                //{
                //    response.WriteAsJsonAsync(new Response<object>() { ReturnMessage = "Unauthorized - Not valid Token" });
                //    response.StatusCode = HttpStatusCode.Unauthorized;
                //    response.Headers.Remove("Content-Type");
                //}
                //else
                //{
                //    response.StatusCode = HttpStatusCode.OK;
                //    response.Headers.Remove("Content-Type");
                //}
                response.StatusCode = HttpStatusCode.OK;
                response.Headers.Remove("Content-Type");
            }
            catch (SecurityTokenException ex)
            {

                response.WriteAsJsonAsync(new Response<object>() { ReturnMessage = ex.Message });
            }

            return response;
        }
    }
}
