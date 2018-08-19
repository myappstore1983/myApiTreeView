using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using myApiTreeView.API.Utilities;
using myApiTreeView.API.Data;
using myApiTreeView.DataSeed;
using myApiTreeView.Services;
using AutoMapper;
using Swashbuckle.AspNetCore.Swagger;

namespace myApiTreeView
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
            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
         
            services.AddMvc().AddJsonOptions(opt => {
                opt.SerializerSettings.ReferenceLoopHandling =
                Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            //.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<IDataRepo,DataRepo>();
            services.AddScoped<IFolderService,FolderService>();
            services.AddScoped<ITestCaseService,TestCaseService>();

            services.AddTransient<Seed>();
            services.AddCors();

           services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Tree View Web API",
                    Description = "This Tree View Api is used to manage the testcase files in the folders."
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,Seed seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else 
            { 
                app.UseExceptionHandler(builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        context.Response.AddApplicationError(error.Error.Message);
                        await context.Response.WriteAsync(error.Error.Message);
                       
                    });
                });
            }

             app.UseSwagger();

             app.UseSwaggerUI(c =>
             {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My TreeView API V1");
                // c.RoutePrefix = string.Empty;
             });

           // seeder.SeedFolders();
            app.UseStatusCodePagesWithReExecute("/Errors/Index", "?statusCode={0}");
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            app.UseMvc();
        }
    }
}
