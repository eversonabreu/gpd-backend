using EAN.GPD.Domain.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace EAN.GPD.Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSwaggerGen(swg =>
            {
                swg.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "GPD - Gerenciamento Pelas Diretrizes - API",
                        Version = "v1",
                        Description = "API - GPD",
                        Contact = new Contact
                        {
                            Name = "Everson",
                            Url = "http://eversonabreu.blogspot.com"
                        }
                    });

                swg.IncludeXmlComments(Path.ChangeExtension(Assembly.GetAssembly(typeof(Startup)).Location, "xml"));
                swg.DescribeAllEnumsAsStrings();
                swg.OperationFilter<AddRequiredHeaderParameter>();
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            Domain.Services.ServiceProvider.Register(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvc();
            app.UseStatusCodePages();

            app.UseSwagger();
            app.UseSwaggerUI(sgw =>
            {
                sgw.SwaggerEndpoint("/swagger/v1/swagger.json", "GPD - Gerenciamento Pelas Diretrizes");
            });

            //[FIXO]
            DatabaseConfiguration.ConnectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=GPD;Pooling=true;";
            DatabaseConfiguration.Migrate();
        }
    }

    internal class AddRequiredHeaderParameter : IOperationFilter
    {
        void IOperationFilter.Apply(Operation operation, OperationFilterContext context)
        {
            var param = new Param
            {
                Name = "authorization",
                In = "header",
                Description = "Token",
                Required = true,
                Type = "string"
            };
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }
            operation.Parameters.Add(param);
        }
    }

    internal class Param : IParameter
    {
        public string Description { get; set; }

        public Dictionary<string, object> Extensions { get { return new Dictionary<string, object> { { "test", true } }; } }

        public string In { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public bool Required { get; set; }
    }
}
