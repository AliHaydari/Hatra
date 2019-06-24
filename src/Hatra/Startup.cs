using CacheManager.Core;
using DNTCaptcha.Core;
using DNTCommon.Web.Core;
using EFSecondLevelCache.Core;
using Hatra.Common.WebToolkit;
using Hatra.DataLayer.Context;
using Hatra.Elastic;
using Hatra.FileUpload;
using Hatra.IocConfig;
using Hatra.ViewModels.Identity.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;

namespace Hatra
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
            services.AddEFSecondLevelCache();
            addInMemoryCacheServiceProvider(services);
            //addRedisCacheServiceProvider(services);

            services.Configure<SiteSettings>(options => Configuration.Bind(options));

            // Adds all of the ASP.NET Core Identity related services and configurations at once.
            services.AddCustomIdentityServices();

            // blueimp/jQuery-File-Upload service
            services.AddScoped<FilesHelper, FilesHelper>();
            services.AddScoped<FileUploadUtilities, FileUploadUtilities>();

            var siteSettings = services.GetSiteSettings();
            services.AddRequiredEfInternalServices(siteSettings); // It's added to access services from the dbcontext, remove it if you are using the normal `AddDbContext` and normal constructor dependency injection.
            services.AddDbContextPool<ApplicationDbContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.SetDbContextOptions(siteSettings);
                optionsBuilder.UseInternalServiceProvider(serviceProvider); // It's added to access services from the dbcontext, remove it if you are using the normal `AddDbContext` and normal constructor dependency injection.
            });

            services.AddMvc(options =>
            {
                options.UseYeKeModelBinder();
                options.AllowEmptyInputInBodyModelBinding = true;
                // options.Filters.Add(new NoBrowserCacheAttribute());
            }).AddJsonOptions(jsonOptions =>
            {
                jsonOptions.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDNTCommonWeb();
            services.AddDNTCaptcha();
            services.AddCloudscribePagination();


            // Get the connection settings from appsettings.json and inject them into ElasticConnectionSettings
            services.AddOptions();
            services.Configure<ElasticConnectionSettings>(Configuration.GetSection("ElasticConnectionSettings"));

            services.AddSingleton(typeof(ElasticClientProvider));
            services.AddTransient(typeof(DataIndexer));

            services.AddTransient(typeof(SearchService));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseExceptionHandler("/error/index/500");
            app.UseStatusCodePagesWithReExecute("/error/index/{0}");

            app.UseContentSecurityPolicy();

            // Serve wwwroot as root
            app.UseFileServer(new FileServerOptions
            {
                // Don't expose file system
                EnableDirectoryBrowsing = false
            });

            app.UseAuthentication();
            // app.UseNoBrowserCache();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Account}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void addInMemoryCacheServiceProvider(IServiceCollection services)
        {
            var jss = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            services.AddSingleton(typeof(ICacheManagerConfiguration),
                new CacheManager.Core.ConfigurationBuilder()
                    .WithJsonSerializer(serializationSettings: jss, deserializationSettings: jss)
                    .WithMicrosoftMemoryCacheHandle(instanceName: "MemoryCache1")
                    .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(10))
                    .DisablePerformanceCounters()
                    .DisableStatistics()
                    .Build());
            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));
        }

        //private static void addRedisCacheServiceProvider(IServiceCollection services)
        //{
        //    var jss = new JsonSerializerSettings
        //    {
        //        NullValueHandling = NullValueHandling.Ignore,
        //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //    };

        //    const string redisConfigurationKey = "redis";
        //    services.AddSingleton(typeof(ICacheManagerConfiguration),
        //        new CacheManager.Core.ConfigurationBuilder()
        //            .WithJsonSerializer(serializationSettings: jss, deserializationSettings: jss)
        //            .WithUpdateMode(CacheUpdateMode.Up)
        //            .WithRedisConfiguration(redisConfigurationKey, config =>
        //            {
        //                config.WithAllowAdmin()
        //                    .WithDatabase(0)
        //                    .WithEndpoint("localhost", 6379);
        //            })
        //            .WithMaxRetries(100)
        //            .WithRetryTimeout(50)
        //            .WithRedisCacheHandle(redisConfigurationKey)
        //            .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(10))
        //            .Build());
        //    services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));
        //}
    }
}