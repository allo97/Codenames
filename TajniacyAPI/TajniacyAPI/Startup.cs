using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using TajniacyAPI.CardsManagement.DataAccess.Implementations;
using TajniacyAPI.CardsManagement.DataAccess.Interfaces;
using TajniacyAPI.CardsManagement.Services.Implementations;
using TajniacyAPI.CardsManagement.Services.Interfaces;
using TajniacyAPI.JWTAuthentication.Helpers;
using TajniacyAPI.JWTAuthentication.Services.Implementations;
using TajniacyAPI.JWTAuthentication.Services.Interfaces;
using TajniacyAPI.MongoAPI.Implementations;
using TajniacyAPI.UserManagement.DataAccess.Implementations;
using TajniacyAPI.UserManagement.DataAccess.Interfaces;
using TajniacyAPI.UserManagement.Services.Implementations;
using TajniacyAPI.UserManagement.Services.Interfaces;

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

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Tajniacy API", Version = "V1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { In = ParameterLocation.Header, Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = SecuritySchemeType.ApiKey, Scheme = "Bearer" });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() } });
            });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                };
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

            // configure DI for application services
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddSingleton<ICardsUnitOfWork, CardsUnitOfWork>();
            services.AddScoped<ICardsService, CardsService>();
            services.AddSingleton<IUsersUnitOfWork, UsersUnitOfWork>();
            services.AddScoped<IUsersService, UsersService>();
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

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
