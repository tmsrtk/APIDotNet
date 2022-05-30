using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDotNet.Data;
using APIDotNet.Mapper;
using APIDotNet.Repository;
using APIDotNet.Repository.IRepository;
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

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(_config.GetConnectionString("DefaultConnection"));

            });

            services.AddControllers();
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddAutoMapper(profileAssemblyMarkerTypes: typeof(ParkyMappings));
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ParkyAPISpec", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "ParkyAPI",
                    Version = "v1",
                    Description = "Udemy Parky API",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "srt.karunarathna@gmail.com",
                        Name = "Sanjaya"
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License"
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/ParkyAPISpec/swagger.json", "Parky API");
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
