using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Capi.Modelos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Swagger;
using Capi.Entities;

namespace Capi
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
            //Confiracion corse poner esto en el controller donde quiera aplicarse [EnableCors("PermitirApiRequest")]
            services.AddCors(options =>
           {
               options.AddPolicy("PermitirApiRequest",
                    //builder => builder.WithOrigins("http://www.apirequest.io").WithMethods("*").AllowAnyHeader());
                    builder => builder.WithOrigins("http://localhost:4200").WithMethods("*").AllowAnyHeader());
           });

            //Poner esto en el controller para permitir acceso [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

            //Swagger 
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Info { Title = "Mi web api ", Version = "v1" });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = false,
           ValidateAudience = false,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
           ClockSkew = TimeSpan.Zero
       });
            //Agregando DTO POST en Mapper
            services.AddAutoMapper(options =>
            {
                options.CreateMap<CrearProductoDTO, Producto>();
                options.CreateMap<ActualizarProductoDTO, Producto>();
            });

            //Evitar que llegue a /Account/Login
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                //options.Cookie.HttpOnly = true;
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                //options.LoginPath = "/Account/Login";
                //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                //options.SlidingExpiration = true;

                options.Events.OnRedirectToLogin = context => {
                    context.Response.Headers["Location"] = context.RedirectUri;
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Conexion")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //Swagger 
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi api V1");
            });
            app.UseHttpsRedirection();
            app.UseAuthentication();
            //Corse por Middelware no recomenble se pone de manera global
            //app.UseCors(builder => builder.WithOrigins("http://www.apirequest.io").WithMethods("*").WithHeaders("*"));
            //app.UseStatusCodePages();
            app.UseMvc();
        }
    }
}
