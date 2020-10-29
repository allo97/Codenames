using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TajniacyAPI.DataAccess.Implementations;
using TajniacyAPI.DataAccess.Interfaces;
using TajniacyAPI.DataAccess.Model;
using TajniacyAPI.Services.Implementations;
using TajniacyAPI.Services.Interfaces;

namespace TajniacyAPI
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", builder => builder
                .SetIsOriginAllowed(isOriginAllowed: _ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()));

            services.AddControllers();

            services.AddSwaggerGen(c => { 
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tajniacy API", Version = "V1" });
            });

            // Add framework services.
            services.AddMvc();
            services.Configure<MongoSettings>(options =>
            {
                options.ConnectionString
                    = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.MongoDBName
                    = Configuration.GetSection("MongoConnection:MongoDBName").Value;
            });

            services.AddSingleton<ITajniacyUnitOfWork, TajniacyUnitOfWork>();
            services.AddScoped<ICardsService, CardsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAll");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tajniacy API V1");
            });

            app.UseHttpsRedirection();
            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
