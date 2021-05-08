using BL.Services;
using BL.Services.Impl;
using DAL_EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SpendingTracker.Config;
using SpendingTracker.Middleware;
using SpendingTracker.Services;
using SpendingTracker.Services.Impl;
using System.Collections.Generic;
using System.Text;

namespace SpendingTracker
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
            services.AddDbContext<AppDbContext>(x =>
            {
                x.UseSqlServer(Configuration.GetConnectionString("SpendingTracker"));
            });

            services.AddControllers(x =>
            {
                x.Filters.Add<HttpStatusExceptionFilter>();
            });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.TokenValidationParameters.ValidateActor = false;
                    x.TokenValidationParameters.ValidateAudience = false;
                    x.TokenValidationParameters.ValidateIssuer = false;
                    //x.TokenValidationParameters.ValidateLifetime = true;
                    x.TokenValidationParameters.ValidateLifetime = false;
                    x.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(Configuration.GetSection(nameof(JWTSettings)).Get<JWTSettings>().AccessTokenSecret));
                });

            services.AddAuthorization();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SpendingTracker", Version = "v1" });

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme = JwtBearerDefaults.AuthenticationScheme,
                            Name = JwtBearerDefaults.AuthenticationScheme,
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            services.Configure<GoogleSettings>(Configuration.GetSection(nameof(GoogleSettings)));
            services.Configure<JWTSettings>(Configuration.GetSection(nameof(JWTSettings)));

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {

            if (env.IsDevelopment())
            {
                app.UseCors(x =>
                {
                    x.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
                });

                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpendingTracker v1"));

                app.UseSpaDevServerProxy("http://localhost:5000");
            }
            else
            {
                app.UseDefaultFiles();
                app.UseStaticFiles();
            }

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
