using Azure.Storage.Blobs;
using Back_Market_Vinci.Api;
using Back_Market_Vinci.DataServices;
using Back_Market_Vinci.DataServices.ProductDAO;
using Back_Market_Vinci.Domaine.Other;
using Back_Market_Vinci.Uc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci
{
    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddSingleton<IDalServices, DalServices>();
            services.AddSingleton<IUserDAO, UserDAO>();
            services.AddSingleton<IUserUCC, UserUCC>();
            services.AddSingleton<IProductDAO, ProductDAO>();
            services.AddSingleton<IProductUCC, ProductUCC>();
            services.AddSingleton<IRatingsDAO, RatingsDAO>();
            services.AddSingleton(x => new BlobServiceClient(Configuration.GetValue<string>("AzureBlobStorageConnectionString")));
            services.AddSingleton<IBlobService, BlobService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var options = new ExceptionHandlerOptions();
            options.AllowStatusCode404Response = true;
            options.ExceptionHandlingPath = "/error";
            app.UseExceptionHandler(options);

            app.UseRouting();
            
            app.UseCors(
                options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()

            );

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
