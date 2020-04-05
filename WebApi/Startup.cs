using System.Net;
using System.Reflection;

using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Tymish.Application.Interfaces;
using Tymish.Gateways;
using Tymish.Persistence;
using Tymish.WebApi.Middleware;

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

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
            });

            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddControllers();
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // TODO: use dependency injection library
            services.AddMediatR(Assembly.Load("Application"));

            services.AddScoped<ITymishDbContext>(s => s.GetService<TymishDbContext>());

            services.AddScoped<IEmailGateway>(
                s => { 
                    var enableEmail = Configuration.GetValue<bool>("EnableEmail");
                    var apiKey = Configuration.GetValue<string>("ApiKeys:SendGrid");
                    return enableEmail && apiKey != string.Empty
                        ? new SendGridEmailGateway(apiKey) as IEmailGateway
                        : new NoEmailGateway() as IEmailGateway;
                });

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
            else
            {
                app.UseMiddleware<ExceptionHandler>();
            }

            app.UseRouting();

            // Nginx to Kestrel
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
