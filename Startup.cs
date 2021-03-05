using _1eraAPI.AutoMapper;
using _1eraAPI.Data;
using _1eraAPI.Includes;
using _1eraAPI.Repo;
using _1eraAPI.Repo.IRepo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace _1eraAPI
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
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AutosAPI")));
            services.AddScoped<IMarcaRepo, MarcaRepo>();
            services.AddScoped<IModeloRepo, ModeloRepo>();
            services.AddScoped<IUsuarioRepo, UsuarioRepo>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAutoMapper(typeof(AutosMappers));

            services.AddControllers(); 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("APIMarcas", new OpenApiInfo 
                { 
                    Title = "APIMarcas", 
                    Version = "v1",
                    Description = "Backend Autos",

                });

                c.SwaggerDoc("APIModelos", new OpenApiInfo
                {
                    Title = "APIModelos",
                    Version = "v1",
                    Description = "Backend Autos",

                });

                c.SwaggerDoc("APIUsuarios", new OpenApiInfo
                {
                    Title = "APIUsuaros",
                    Version = "v1",
                    Description = "Backend Autos",

                });

                var fileXMLCommments = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var rutaAPIComments = Path.Combine(AppContext.BaseDirectory, fileXMLCommments);
                c.IncludeXmlComments(rutaAPIComments);

                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "Autenticación JWT(Beare)",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",                    
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement 
                { 
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });

            services.AddCors();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/APIMarcas/swagger.json", "API Marcas v1");
                    c.SwaggerEndpoint("/swagger/APIModelos/swagger.json", "API Modelos v1");
                    c.SwaggerEndpoint("/swagger/APIUsuarios/swagger.json", "API Usuarios v1");
                });
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            app.UseRouting();

            
            app.UseAuthentication();
            app.UseAuthorization();

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
    }
}
