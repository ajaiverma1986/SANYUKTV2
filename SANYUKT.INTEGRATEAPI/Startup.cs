using Audit.SqlServer.Providers;
using SANYUKT.Configuration;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Logging;
using SANYUKT.INTEGRATEAPI.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Text.Json.Serialization;

namespace SANYUKT.INTEGRATEAPI
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            System.Net.ServicePointManager.DefaultConnectionLimit = 10000;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddMvcCore().AddApiExplorer();

             services.AddSession();
            //services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddScoped<ISANUKTLoggedInUser, SANYUKTLoggedInUser>();
            services.AddSingleton<SANYUKTExceptionFilterService>();
            services.AddSingleton<ILoggingService, LoggingService>();

            int cookieExpiryInMinutes = int.Parse(Configuration["cookieExpiryInMinutes"]);

            services.AddAuthentication(authenticationOptions =>
            {
                authenticationOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(o =>
            {
                o.SlidingExpiration = true;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(cookieExpiryInMinutes);
            });

           


            Audit.Core.Configuration.DataProvider = new SqlDataProvider()
            {
                ConnectionString = SANYUKTApplicationConfiguration.Instance.AuditingDB,
                Schema = "dbo",
                TableName = "Event",
                IdColumnName = "EventId",
                JsonColumnName = "Data",
                LastUpdatedDateColumnName = "LastUpdatedDate"
            };

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")),
                RequestPath = "/Images"
            });
            app.UseRouting();
            app.Use(async (context, next) =>
            {
                ISANUKTLoggedInUser user = context.RequestServices.GetRequiredService<ISANUKTLoggedInUser>();
                user.ApiToken = Configuration["APIToken"];

                user.UserToken = context.Request.Headers["usertoken"];

                if (string.IsNullOrEmpty(user.UserToken))
                {
                    user.UserToken = context.Request.Query["usertoken"];
                }

                if (context.User.Identity.IsAuthenticated)
                {
                    /* Claim userTokenClaim = context.User.Claims.FirstOrDefault(c => (c.Type.Equals(FIAClaimTypes.UserToken, StringComparison.InvariantCultureIgnoreCase)));
                     if (userTokenClaim != default(Claim))
                         user.UserToken = userTokenClaim.Value;

                     Claim UserMasterIDClaim = context.User.Claims.FirstOrDefault(c => (c.Type.Equals(FIAClaimTypes.UserMasterID, StringComparison.InvariantCultureIgnoreCase)));
                     if (UserMasterIDClaim != default(Claim))
                         user.UserMasterID = Convert.ToInt64(UserMasterIDClaim.Value);

                     //Claim UserTypeClaim = context.User.Claims.FirstOrDefault(c => (c.Type.Equals(FIAClaimTypes.UserType, StringComparison.InvariantCultureIgnoreCase)));
                     //if(UserTypeClaim != default(Claim))
                     //    user.UserTypeID = Convert.ToInt32(UserTypeClaim.Value);

                     user.DisplayName = context.User.Claims.FirstOrDefault(c => (c.Type.Equals(ClaimTypes.Name, StringComparison.InvariantCultureIgnoreCase))).Value;

                     user.RolePermissions = context.User.Claims.Where(c => c.Type.Equals(ClaimTypes.Role, StringComparison.InvariantCultureIgnoreCase)).Select(c => c.Value).ToList();*/
                }

                //Get client ip address
                user.IPAddress = context.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                if (string.IsNullOrEmpty(user.IPAddress))
                {
                    user.IPAddress = context.Request.Headers["X-Forwarded-For"];
                    if (!string.IsNullOrEmpty(user.IPAddress))
                    {
                        user.IPAddress = user.IPAddress.Split(',')[0].Split(';')[0];
                        if (user.IPAddress.Contains(":"))
                        {
                            user.IPAddress = user.IPAddress.Substring(0, user.IPAddress.LastIndexOf(':'));
                        }
                    }
                }

                await next();
            });

            app.UseSession();
            app.UseCors(cors => { cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }



    
}
