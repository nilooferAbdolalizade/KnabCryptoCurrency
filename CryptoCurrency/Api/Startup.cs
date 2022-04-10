using Application.Interface;
using ApplicationService.Service;
using Domain.IExternalService;
using Domain.IRepository;
using Infrastructure;
using Infrastructure.Context;
using Infrastructure.ExternalService;
using Infrastructure.ExternalService.Entity;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Api
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
            var coinMarketCapOptions = new CoinMarketCapConfig();
            Configuration.Bind(nameof(CoinMarketCapConfig), coinMarketCapOptions);
            services.AddSingleton(coinMarketCapOptions);
            services.AddDbContext<CryptoCurrencyContext>(option => option.UseInMemoryDatabase("InMemoryDbForKnabCryptoCurrencyProject"));

            services.AddRazorPages();
            services.AddControllersWithViews();
            ConfigInternalServices(services);
            ConfigureSwagger(services);
        }

        public static void ConfigInternalServices(IServiceCollection services)
        {
            services.AddTransient<ICryptoCurrencyRepository, CryptoCurrencyRepository>();
            services.AddTransient<ICryptoService, CryptoService>();
            services.AddTransient<ICoinMarketCapCallService, CoinMarketCapCallService>();
            services.AddSingleton<ICoinMarketCapService, CoinMarketCapService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpionts => endpionts.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}"));

            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/CryptoCurrency/swagger.json", "CryptoCurrency Service");
                setupAction.RoutePrefix = "";

                setupAction.DefaultModelExpandDepth(2);
                setupAction.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
                setupAction.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                setupAction.EnableDeepLinking();
                setupAction.DisplayOperationId();

            });
        }

        public void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                    name: "CryptoCurrency",
                    info: new OpenApiInfo()
                    {
                        Title = "CryptoCurrency Service",
                        Version = "1",
                        Description = "CryptoCurrency Service.",
                        Contact = new OpenApiContact()
                        {
                            Email = "Knab@CryptoCurrency.com",
                            Name = "Support",
                        },
                        License = new OpenApiLicense()
                        {
                            Name = "CryptoCurrency License",
                        }
                    });

                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
                xmlFiles.ForEach(xmlFile => setupAction.IncludeXmlComments(xmlFile));

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                setupAction.IncludeXmlComments(xmlCommentsFullPath);
            });
        }
    }
}
