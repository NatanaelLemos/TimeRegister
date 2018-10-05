using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRegister.Api;
using TimeRegister.Domain.AppServices;
using TimeRegister.Domain.Data;

namespace TimeRegister.Tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void ConfigureInjection(IServiceCollection services)
        {
            services
                .AddScoped<ITimeRepository, FakeTimeRepository>()
                .AddScoped<TimeAppService>();
        }
    }
}
