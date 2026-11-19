using GA2.Application.Dto;
using GA2.Application.Dto.Identity;
using GA2.Transversals.Commons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface ILoginBusinessLogic
    {
        Task<Response<ResponseLoginDto>> LoginGA2(LoginDto loginDto);
        Task<Response<LoginPAAResponse>> LoginGA2Paa(LoginPaaDto loginPaaDto);
        Task<Response<ResponseLoginDto>> LoginUEJ(LoginDto loginDto);
        Task<Response<IEnumerable<MenuDto>>> ObtenerMenu();
        Task<Response<string>> RecuperaContrasena(RecuperarContrasenaDto recuperarContrasenaDto);
        Task<Response<UsuarioDto>> ValidaToken(string token);
        Task<Response<bool>> UpdateSession(string idUser);
        Task<Response<bool>> Logout(string idUser);
    }
}