// (C) 2019 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:        16.01.2019 15:26
// Developer:      Manuel Fasching
// Project         CADMeshConverter
//
// Released under GPL-3.0-only

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CMCCloud.SwaggerOperations;
using Swashbuckle.AspNetCore.Swagger;
using CMCCloud.Helper;

namespace CMCCloud
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Which Swagger Version to use
        /// </summary>
        private const string SwaggerVersion = "v3";

        public Startup(IConfiguration configuration)
        {
            
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(SwaggerVersion, new Info
                                             {
                                                 Title = "CADMeshConverter API",
                                                 Version = SwaggerVersion,
                                                 Description = "CADMeshConverter - Convert your meshes",
                                                 Contact = new Contact
                                                           {
                                                               Name = "BISS",
                                                               Email = "biss@fotec.at",
                                                               Url = "http://www.fotec.at"
                                                           }
                                             });
                c.MapType<Stream>(() => new Schema { Type = "file" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory,xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.OperationFilter<UploadJobOperation>();
            });

            services.Configure<FormOptions>(x => x.MultipartBodyLengthLimit = 1_074_790_400);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v3/swagger.json", "CADMeshConverter API V1");
            });

            app.UseMvc();
        }
    }
}
