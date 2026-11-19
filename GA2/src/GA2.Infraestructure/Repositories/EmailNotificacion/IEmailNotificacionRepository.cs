using GA2.Domain.Entities.Correo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.EmailNotificacion
{
    public interface IEmailNotificacionRepository
    {
        Task EnviarAsync(Email request);
    }
}
