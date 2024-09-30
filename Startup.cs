using AwsDotnetCsharp.Application.Interface;
using AwsDotnetCsharp.Application.Services;
using AwsDotnetCsharp.Domain.Interface;
using AwsDotnetCsharp.Domain.Services;
using AwsDotnetCsharp.Infrastructure.Config;
using AwsDotnetCsharp.Infrastructure.Persistence.Interface;
using AwsDotnetCsharp.Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsDotnetCsharp
{
    internal class Startup
    {
        public static ServiceProvider Services { get; private set; }
        public static void ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IInmuebleApplication, InmuebleApplication>();
            serviceCollection.AddSingleton<IInmuebleDomain, InmuebleDomain>();
            serviceCollection.AddSingleton<IInmuebleRepository, InmuebleRepository>();
            serviceCollection.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });

            //serviceCollection.AddInmuebleDependencyInjection();
            //serviceCollection.AddInmuebleCORS();
            Services = serviceCollection.BuildServiceProvider();
            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();
            app.UseCors();
        }
    }
}
