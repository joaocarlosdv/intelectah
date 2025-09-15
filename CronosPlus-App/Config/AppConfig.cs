using CronosPlus_App.Services;
using CronosPlus_App.Services.Interfaces;

namespace CronosPlus_App.Config
{
    public static class AppConfig
    {
        public static IServiceCollection AppConfigServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IFabricanteServiceApp, FabricanteServiceApp>();
            services.AddScoped<IVeiculoServiceApp, VeiculoServiceApp>();
            services.AddScoped<IConcessionariaServiceApp, ConcessionariaServiceApp>();
            services.AddScoped<IClienteServiceApp, ClienteServiceApp>();
            services.AddScoped<IVendasServiceApp, VendasServiceApp>();
            services.AddScoped<IUsuarioServiceApp, UsuarioServiceApp>();
            services.AddScoped<IAcessoServiceApp, AcessoServiceApp>();
            

            return services;
        }
    }
}
