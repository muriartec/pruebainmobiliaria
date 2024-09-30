using AwsDotnetCsharp.Application.Interface;
using AwsDotnetCsharp.Application.Services;
using AwsDotnetCsharp.Domain.Interface;
using AwsDotnetCsharp.Domain.Services;
using AwsDotnetCsharp.Infrastructure.Persistence.Interface;
using AwsDotnetCsharp.Infrastructure.Persistence.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsDotnetCsharp.Infrastructure.Config
{
    public static class ServiceCollectionExtension
    {
        public static void AddInmuebleDependencyInjection(this IServiceCollection services)
        {
            //services.AddTransient<IInmuebleApplication, InmuebleApplication>();
            //services.AddTransient<IInmuebleDomain, InmuebleDomain>();
            //services.AddTransient<IInmuebleRepository, InmuebleRepository>();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IInmuebleApplication, InmuebleApplication>();
            serviceCollection.AddSingleton<IInmuebleDomain, InmuebleDomain>();
            serviceCollection.AddSingleton<IInmuebleRepository, InmuebleRepository>();
        }
        public static void AddInmuebleCORS(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });
        }
    }
}
