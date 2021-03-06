using System.Reflection;
using System.Text.Json.Serialization;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Tymish.Application.Dtos;
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
            // This must happen first for Nginx forwarding to Kestrel
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                
                // note sure if this is needed
                // options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
            });

            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            
            // This was used for circlar dependencies with Newtonsoft
            // Maybe this isn't required for System.Text.Json
            //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            services.AddMediatR(Assembly.GetExecutingAssembly());
            
            services.AddAutoMapper(typeof(DefaultProfile).Assembly);

            // TODO: use dependency injection library
            services.AddMediatR(Assembly.Load("Application"));

            services.AddScoped<ITymishDbContext>(s => s.GetService<TymishDbContext>());
            services.AddScoped<IAuthGateway, AuthGateway>();

            services.AddScoped<IEmailGateway>(
                s => { 
                    var enableEmail = Configuration.GetValue<bool>("EnableEmail");
                    var apiKey = Configuration.GetValue<string>("ApiKeys:SendGrid");
                    return enableEmail && apiKey != string.Empty
                        ? new SendGridEmailGateway(apiKey) as IEmailGateway
                        : new NoEmailGateway() as IEmailGateway;
                });

            // Configuration Options
            IConfigurationSection authOptions = Configuration.GetSection("Jwt");
            services.Configure<AuthOptions>(authOptions);

            // DB
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
                app.UseForwardedHeaders(); // Nginx to Kestrel
                app.UseMiddleware<ExceptionHandler>();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
