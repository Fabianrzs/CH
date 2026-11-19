using Microsoft.Extensions.Configuration;

namespace GA2.Infraestructure.Repositories
{
    public class AcreditacionRepository : DapperGenericRepository
    {
        public AcreditacionRepository(IConfiguration configuration) : base(configuration) {}

    }
}
