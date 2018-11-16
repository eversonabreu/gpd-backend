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
            services.AddTransient<ICargoRepository, CargoRepository>();
            services.AddTransient<IProjetoRepository, ProjetoRepository>();
            services.AddTransient<IArvoreRepository, ArvoreRepository>();
            services.AddTransient<IUnidadeMedidaRepository, UnidadeMedidaRepository>();
            services.AddTransient<IUsuarioGrupoRepository, UsuarioGrupoRepository>();
            services.AddTransient<IIndicadorRepository, IndicadorRepository>();
            services.AddTransient<IMovimentoRepository, MovimentoRepository>();
        }
    }
}
