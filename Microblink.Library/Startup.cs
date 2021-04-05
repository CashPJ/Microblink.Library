using Microblink.Common.Common.Middlewares;
using Microblink.Common.Middlewares;
using Microblink.Common.MrzValidator;
using Microblink.Library.Services.Context;
using Microblink.Library.Services.Context.Config.Interfaces;
using Microblink.Library.Services.Context.Interfaces;
using Microblink.Library.Services.Services.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;

namespace Microblink.Library
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c => {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddSingleton<IDataContextConfig, DataContextConfig>(_ =>
                new DataContextConfig
                {
                    DatabaseContextConfig = new DatabaseContextConfig
                    {
                        DatabaseConnectionString = Configuration["Contexts:DatabaseContext:DbConnectionString"]
                    },
                    MicroblinkContextConfig = new MicroblinkContextConfig
                    {
                        AuthorizationScheme = Configuration["Contexts:MicroblinkContext:AuthorizationScheme"],
                        AuthorizationToken = Configuration["Contexts:MicroblinkContext:AuthorizationToken"],
                        BlinkIdRecognizerEndpoint = Configuration["Contexts:MicroblinkContext:BlinkIdRecognizerEndpoint"]
                    }
                }
             );
            services.AddSingleton<IDataContext, DataContext>();
            services.AddSingleton<IDatabaseContext, DatabaseContext>();
            services.AddSingleton<IMicroblinkContext, MicroblinkContext>();           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
