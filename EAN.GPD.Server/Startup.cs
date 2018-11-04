using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace EAN.GPD.Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "GPD - Gerenciamento Pelas Diretrizes - API",
                        Version = "v1",
                        Description = "API - GPD",
                        Contact = new Contact
                        {
                            Name = "Everson",
                            Url = "https://github.com/eversonean"
                        }
                    });

                //[FIXO]
                const string caminhoXmlDoc = @"C:\projetos\EAN.GPD\EAN.GPD.Server\bin\Debug\netcoreapp2.1\EAN.GPD.Server.xml";
                c.IncludeXmlComments(caminhoXmlDoc);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GPD - Gerenciamento Pelas Diretrizes");
            });
        }
    }
}
