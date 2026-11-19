using GA2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface ILoginGA2Repository
    {
        Task<User> AccesoUsuario(User user);
        Task<IEnumerable<Menu>> ObtenerMenu();
        Task<IEnumerable<SubMenu>> ObtenerSubmenu();
        Task<User> RecuperaContrasena(string tipo, string numero);
        Task<User> ActualizarTokenUsuario(Guid idUser, string token);
        Task<User> ValidaTokenRestablecimiento(string token);
        Task<IEnumerable<GrupoOpciones>> ObtenerOpcionesUsuariosPorId(Guid id);
        Task<User> LoginUserUEJ(User user);
        Task<LoginSession> CheckSessionLogin(Guid idUsuario);
        Task CreateSessionLogin(Guid idUsuario);
        Task<bool> UpdateSession(Guid idUsuario);
        Task Logout(Guid idUsuario);
    }
}