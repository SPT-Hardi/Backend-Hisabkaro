using DinkToPdf;
using DinkToPdf.Contracts;
using HIsabKaro.Cores.Common;
using HIsabKaro.Cores.Common.Contact;
using HIsabKaro.Cores.Common.File;
using HIsabKaro.Cores.Common.MailService;
using HIsabKaro.Cores.Common.Shift;
using HIsabKaro.Cores.Developer.Subscriber;
using HIsabKaro.Cores.Employee.Job;
using HIsabKaro.Cores.Employee.Staff;
using HIsabKaro.Cores.Employer.Organization;
using HIsabKaro.Cores.Employer.Organization.Branch;
using HIsabKaro.Cores.Employer.Organization.Job;
using HIsabKaro.Cores.Employer.Organization.Staff;
using HIsabKaro.Cores.Employer.Organization.Staff.Attendance;
using HIsabKaro.Middleware;
using HIsabKaro.Models.Common.MailService;
using HIsabKaro.Services;
using HIsabKaro.Services.PDF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro
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


            HisabKaroContext.DBContext db = new HisabKaroContext.DBContext("data source=20.219.49.104;initial catalog=HisabKaro;integrated security=False;persist security info=True;user id=HisabKaroDB;password=Hisabkaro@2022@@#;License Key=qHnH5wx/L422kFN4WQussVkqbelF0xGMaZi+DGL6lhFu+VTasW/ZRA22+dVoDbuQ64trDZsBMziLDE9kumHeTDKlcRSCvsotqn7rHn9VHFXS3Jmh/rFBVSxav6UlKmT4POdU+hnX8ACaigXhFdBiZ4NeHNVRNTqJ4fUTou0czKt8ATWxOB2MjUrprbYTV2ECFJOo2uLgwGzqeEpv1gGPLKR3p5DOKdeMu61FRAak23fmjt8PPQpz50o1E0r0FFdoQrJIYKkMxqRiD2IhVxlcVCvpIqR31rWwKJ1sNquGBMU=;");
            //HisabKaroContext.DBContext db = new HisabKaroContext.DBContext("data source = DESKTOP - D9KFJSI; initial catalog = HisabKaroDB; integrated security = False; persist security info = True; user id = rj; password = 12345;License Key=qHnH5wx/L422kFN4WQussVkqbelF0xGMaZi+DGL6lhFu+VTasW/ZRA22+dVoDbuQ64trDZsBMziLDE9kumHeTDKlcRSCvsotqn7rHn9VHFXS3Jmh/rFBVSxav6UlKmT4POdU+hnX8ACaigXhFdBiZ4NeHNVRNTqJ4fUTou0czKt8ATWxOB2MjUrprbYTV2ECFJOo2uLgwGzqeEpv1gGPLKR3p5DOKdeMu61FRAak23fmjt8PPQpz50o1E0r0FFdoQrJIYKkMxqRiD2IhVxlcVCvpIqR31rWwKJ1sNquGBMU=;");
            //data source = 20.219.49.104; initial catalog = HisabKaro; integrated security = False; persist security info = True; user id = HisabKaroDB; password = *****************

                              services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy",
                                  builder =>
                                  {
                                      builder.SetIsOriginAllowed(host => true)
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials();
                                  });
            });

            //Mail Service Configure
            services.Configure<MailSetting>(Configuration.GetSection("MailSettings"));
            
            //For PDF Service
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            //JSON Serializer
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
                = new DefaultContractResolver());

            services.AddControllers().AddNewtonsoftJson();
            //------------------------------------register services----------------------------//
            services.AddSingleton<Users>();
            
            services.AddTransient<ITokenServices, TokenServices>();
            services.AddTransient<Tokens>();
            services.AddTransient<Uploads>();
            services.AddTransient<ER_JobDetails>();
            services.AddSingleton<Users>();
            services.AddTransient<ITokenServices, TokenServices>();
            services.AddTransient<Tokens>();
            services.AddTransient<Uploads>();
            services.AddTransient<EE_AppliedJobs>();
            services.AddTransient<BranchDetails>();
            services.AddTransient<ER_AppliedJobs>();
            services.AddTransient<OrganizationDetails>();
            services.AddTransient<OrganizationProfiles>();
            services.AddTransient<StaffDetails>();
            services.AddTransient<ContactAddress>();
            services.AddTransient<ShiftTimes>();
            services.AddTransient<Statistics>();
            services.AddTransient<Submits>();
            services.AddTransient<StaffPersonalDetails>();
            services.AddTransient<MailServices>();
            //services.AddTransient<Cores.Common.Contact.Current>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<Services.PDF.HTMLString>();
            services.AddTransient<CustomAssemblyLoadContext>();
            var context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));

            //---------------------------------------------------------------------------------//
            services.AddControllers();
            services.AddAuthentication(option =>
            {

                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(option =>
                {
                    option.SaveToken = true;
                    option.RequireHttpsMetadata = false;
                    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = Configuration["JWT:ValidAudience"],
                        ValidIssuer = Configuration["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["JWT:secret"]))
                    };
                });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HIsabKaro", Version = "v1" });
                //c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            HIsabKaro.Cores.Common.Contact.Current.SetHttpContextAccessor(accessor); 
            app.UseCors("MyPolicy");
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HIsabKaro v1"));
            }
            /*            app.UseStaticFiles();
                        app.UseDirectoryBrowser(new DirectoryBrowserOptions
                        {
                            FileProvider = new PhysicalFileProvider
                        (
                            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload")
                        ),
                            RequestPath = "/Upload"
                        });*/
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                   Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload")),
                RequestPath = "/Upload",
                EnableDefaultFiles = true
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            
            app.UseAuthorization();
            app.UseMiddleware<JwtHandler>();
            //app.UseMiddleware<CustomeException>();
            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
