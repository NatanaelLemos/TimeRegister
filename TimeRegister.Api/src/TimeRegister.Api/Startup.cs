using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TimeRegister.Domain.AppServices;
using TimeRegister.Domain.Data;
using TimeRegister.Infrastructure.Data;

namespace TimeRegister.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureInjection(services);

            services.AddCors(o =>
                o.AddPolicy("AllowAll", b =>
                    b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials()
                )
            );

            services.AddResponseCompression(opt =>
            {
                opt.Providers.Add<GzipCompressionProvider>();
                opt.EnableForHttps = true;
            });

            services
                .AddMvc(opt => opt.Filters.Add(new CorsAuthorizationFilterFactory("AllowAll")))
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .AddXmlDataContractSerializerFormatters();
        }

        protected virtual void ConfigureInjection(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("timeRegister");
            services.AddSingleton<MongoDbContext>(i => new MongoDbContext(connectionString));

            services
                .AddScoped<ITimeRepository, TimeRepository>()
                .AddScoped<TimeAppService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
                .UseResponseCompression()
                .UseCors("AllowAll")
                .UseMvc();
        }
    }
}
