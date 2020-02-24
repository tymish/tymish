using System.Reflection;

using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Tymish.Domain.Interfaces;
using Tymish.Persistence;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            // TODO: use dependency injection library
            services.AddMediatR(typeof(Tymish.Application.Employees.Commands.CreateEmployeeHandler).Assembly);
            services.AddMediatR(typeof(Tymish.Application.Employees.Queries.GetEmployeeListHandler).Assembly);

            services.AddScoped<ITymishDbContext>(s => s.GetService<TymishDbContext>());

            services.AddDbContext<TymishDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("TymishContext"))
            );

            services.AddSwaggerGen(options => 
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Tymish Api", Version = "v1" })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyHeader();
                options.AllowAnyMethod();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options => 
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tymish Api V1")
                );
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
