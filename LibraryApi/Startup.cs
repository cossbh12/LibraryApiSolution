using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using LibraryApi.Domain;
using LibraryApi.Profiles;
using LibraryApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LibraryApi
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
          //  services.AddTransient<ISystemTime, SystemTime>(); Will add one per call for it
          //  services.AddSingleton<ISystemTime, SystemTime>(); Will spin one up the first time its called
            services.AddScoped<ISystemTime, SystemTime>();
            services.AddDbContext<LibraryDataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("LibraryDatabase"));
            });
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Books());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton<IMapper>(mapper);
            services.AddSingleton<MapperConfiguration>(mappingConfig);

            services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
               {
                   Title = "Library API",
                   Version = "1.0",
                   Contact = new Microsoft.OpenApi.Models.OpenApiContact
                   {
                       Name = "Sonya",
                       Email = "Wildebrownfox@gmail.com"
                   },
                   Description = "An API for the BES 100 Class"
               });
               var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
               var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
               c.IncludeXmlComments(xmlPath);
           });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API");
                c.RoutePrefix = "";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
