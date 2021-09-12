using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UrlShortening.DataAccess;
using UrlShortening.Model;
using UrlShortening.Service;
using UrlShortening.Service.Implementation;

namespace UrlShortening.API
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
            services.AddSingleton<MongoDBConfig>(Configuration.GetSection("MongoDBConfig").Get<MongoDBConfig>());
            services.AddControllers();
            services.AddHttpClient();
            services.AddTransient<IUrlValidationService, UrlValidationService>();
            services.AddTransient<IUrlDataRepository, UrlDataRepository>();
            services.AddTransient<IUrlDataContext, UrlDataContext>();
            services.AddTransient<IUrlDataManager, UrlDataManager>();
            services.AddTransient<IShortCodeGeneratorService, ShortCodeGeneratorService>();
            services.AddStackExchangeRedisCache(o =>
            {
                o.Configuration = Configuration.GetValue<string>("Redis:Configuration");
                o.InstanceName = Configuration.GetValue<string>("Redis:InstanceName");
            });

            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<Startup>>();
            services.AddSingleton(typeof(ILogger), logger);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
