using GA2.Domain.Entities;
using GA2.Domain.Entities.GruposUsuarios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface IUsersRepository
    {
        Task<User> CreateUser(User user);
        Task<User> DeleteUser(Guid Id);
        Task<IEnumerable<User>> ObtenerUsuariosDos();
        Task<User> ObtenerUsuarioPorId(Guid userId);
        Task<User> UpdateUser(User user);
        Task<User> ObtenerUsuarioPorIdentificacion(string identificacion, int tipoIdentificationId);
        Task<UsuariosPorGrupos> InsertGruposPorUsuarios(Guid userId, int grupoId);
        Task<IEnumerable<GrupoUsuario>> GetGrupoUserById(Guid userId);
        Task<GrupoUsuario> DeleteGrupoUsuarioUserById(Guid userId);
        Task<User> ActualizarContrasenaUsuario(string token, string password);
        Task<User> ActualizarContrasenaUsuarioById(Guid idUsuario, string password);
    }
}