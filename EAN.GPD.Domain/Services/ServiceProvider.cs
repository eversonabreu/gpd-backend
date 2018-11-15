using EAN.GPD.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EAN.GPD.Domain.Services
{
    public class ServiceProvider
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IDepartamentoRepository, DepartamentoRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
        }
    }
}
