using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WebApi.Helpers;
using WebApi.Services;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System; 
using Microsoft.OpenApi.Models;
using WebApi.Entities;
namespace WebApi
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
            //services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("TestDb"));
            services.AddDbContext<DataContext>(x => x.UseMySql(AppSettings.DB_CONN_STRING));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper();


            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>(); 
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetById(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.SecretJWT)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();


         
            services.AddSwaggerGen(c =>
            {
            c.SwaggerDoc("v1", new OpenApiInfo { 
                Title = "My API", 
                Version = "v1" 
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                In = ParameterLocation.Header, 
                Description = "Please insert JWT with <b>Bearer</b> into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey 
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            { 
                new OpenApiSecurityScheme 
                { 
                Reference = new OpenApiReference 
                { 
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" 
                } 
                },
                new string[] { } 
                } 
            });
            });
                        
        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DataContext _, IUserService Us)
        {
            // global cors policy
            
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc(); 
            _.Database.Migrate(); 
        


            app.UseSwagger();

            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api");
                c.RoutePrefix = string.Empty;
                c.DefaultModelExpandDepth(2);
                c.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);
                c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.EnableDeepLinking();
                c.EnableFilter();
                c.ShowExtensions();
                c.EnableValidator();
            });
        }
    }
}
