using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

using AutoMapper;

using Serilog;

using ArcherMobilApp.DAL.MsSql;
using ArcherMobilApp.DAL.MsSql.Contract;
using ArcherMobilApp.Data;
using ArcherMobilApp.Infrastracture;
using ArcherMobilApp.Infrastracture.Security.Jwt;
using ArcherMobilApp.Middlewares;
using ArcherMobilApp.Models;
using Archer.AMA.Bootstrapper;
using Archer.AMA.WebApi.Security.Authorize;
using Archer.AMA.Entity;
using Microsoft.AspNetCore.HttpOverrides;
using ArcherMobilApp.Core.Filter;
using Hellang.Middleware.ProblemDetails;
using System.Net.Http;

namespace ArcherMobilApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        private IWebHostEnvironment _environment;
        public IWebHostEnvironment CurrentEnvironment { get; }


        private void ConfigureSecurity(IServiceCollection services)
        {
            services.AddScoped<IArcherAuthorizationRequirement, ArcherAuthorizationRequirement>();
            services.AddScoped<IArcherSecurityTokenValidator, ArcherSecurityTokenValidator>();
            var serviceProvider = services.BuildServiceProvider();

            services.AddAuthentication(obj =>
            {
                obj.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                obj.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RequireSignedTokens = true,
                        ValidIssuer = Configuration.GetValue<string>("Jwt:Issuer"),
                        ValidAudience = Configuration.GetValue<string>("Jwt:Audience"),
                        IssuerSigningKey = JwtSecurityKey.Create(Configuration.GetValue<string>("Jwt:SecurityKey"))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            if (!serviceProvider.GetService<IArcherSecurityTokenValidator>().ValidateToken(context.SecurityToken, context.Principal.Identity.Name))
                                context.Fail("invalid_token");
                            return Task.CompletedTask;
                        }
                    };

                });

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy(ArcherAuthorizeAttibute.PolicyName,
                    policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.Requirements.Add(serviceProvider.GetService<IArcherAuthorizationRequirement>());
                    });
            }).AddSingleton<IAuthorizationHandler, ArcherAuthorizationHandler>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(); // Make sure you call this previous to AddMvc
            services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, AppClaimsPrincipalFactory>();
            // Auto Mapper Configurations   
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<DocumentEntity, DocumentViewModel>().ReverseMap();
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            IdentityModelEventSource.ShowPII = true;

            services.AddAntiforgery(options =>
            {
                options.FormFieldName = "AntiforgeryFieldname";
                options.HeaderName = "X-CSRF-TOKEN-HEADERNAME"; 
                options.SuppressXFrameOptionsHeader = false;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            Bootstraper.Init(services, Configuration);
            services.AddProblemDetails(setup =>
            {
                setup.IncludeExceptionDetails = (ctx, ex) => CurrentEnvironment.IsDevelopment();
                // This will map NotImplementedException to the 501 Not Implemented status code.
                setup.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);

                // This will map HttpRequestException to the 503 Service Unavailable status code.
                setup.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);

                // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
                // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
                setup.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IRepositoryContextFactory, RepositoryContextFactory>();
            services.AddScoped<IUserRepository>(provider => new UsersRepository(Configuration.GetConnectionString("DefaultConnection"), provider.GetService<IRepositoryContextFactory>()));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            }).AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.Configure<PasswordHasherOptions>(options =>
                options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
            );
            services.AddSingleton<IConfiguration>(Configuration);


            ConfigureSecurity(services);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //Add versioned
            services.AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                opt.SubstituteApiVersionInUrl = true;
            });
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            //Add swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Archer Mobile API", 
                    Version = "v1",
                    Description = "A simple example Archer ASP.NET Core Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "Archer Co",
                        Email = "archer-company@gmail.com",
                        Url = new Uri("https://archer-soft.com/ua")
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
//            services.AddSwaggerGen(o =>
//            {
//                //o.SchemaFilter<ApiSchemaFilter>();
//                o.OperationFilter<ApiSwaggerOperationFilter>();
//                o.SwaggerDoc(SwaggerDocumentName, Configuration.Load<Info>("SwaggerInfo"));
//            });

            //https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders
            //https://jeremylindsayni.wordpress.com/author/jeremylindsayni/
            services.Configure<IdentityOptions>(options =>
            {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            });

            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(Configuration);

            Log.Logger = logger.CreateLogger();
            Log.Information("web site is started.");
            services.AddControllers(options => options.Filters.Add(new HttpResponseExceptionFilter()));

            //services.AddControllers(options => options.Filters.Add(new HttpResponseExceptionFilter()));
            //Notifier
            services.AddMvc().AddNToastNotifyToastr();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Make sure you call this before calling app.UseMvc()
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());


            //Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            //specifying the Swagger JSON endpoint.
            app.UseSwagger();
                         
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;

            });
            // Error handler
            app.UseProblemDetails();

            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            //app.UseHttpContextMiddleware();

            //Configuration for ngnix
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseNToastNotify();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute("users", "{controller=Users}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
