
using Acesso.Service;
using Acesso.Service.Interfaces;
using ApiExterna.Services;
using Application.Service;
using Application.Service.Interface;
using AutoMapper;
using CrossCutting.Mapper;
using DataAccess.Redis;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NHTSAVehicleApi.Service;


namespace CrossCutting.Config
{
    public static class ApiConfig
    {
        public static IServiceCollection ApiConfigServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRedisCacheService, RedisCacheService>();

            services.AddScoped<IFabricanteRepository, FabricanteRepository>();
            services.AddScoped<IVeiculoRepository, VeiculoRepository>();
            services.AddScoped<IConcessionariaRepository, ConcessionariaRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IVendasRepository, VendasRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            services.AddScoped<IFabricanteService, FabricanteService>();
            services.AddScoped<IVeiculoService, VeiculoService>();
            services.AddScoped<IConcessionariaService, ConcessionariaService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IVendasService, VendasService>();
            services.AddScoped<IUsuarioService, UsuarioService>();

            services.AddScoped<INHTSARecallService, NHTSARecallService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ICorreiosService, CorreiosService>();

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<ApiMapper>();

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ApiMapper>();
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
