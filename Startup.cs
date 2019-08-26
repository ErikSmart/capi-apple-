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
            services.AddCors();
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

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

            services.AddAutoMapper();
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
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCors(builder => builder.WithOrigins("http://www.apirequest.io").WithMethods("*").WithHeaders("*"));
            app.UseMvc();
        }
    }
}
