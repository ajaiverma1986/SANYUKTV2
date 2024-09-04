using Audit.Core;
using Audit.SqlServer.Providers;
using Audit.WebApi;
using SANYUKT.Commonlib.Cache;
using SANYUKT.Configuration;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Logging;
using SANYUKT.Provider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using SANYUKT.API.Common;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using SANYUKT.Datamodel.Entities.Application;
using SANYUKT.Datamodel.Entities.Authorization;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

namespace SANYUKT.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private readonly AuthenticationProvider _authenticationProvider = null;
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 10000;
            Configuration = configuration;
            _authenticationProvider = new AuthenticationProvider();
        }



        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
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
            
            
            services.AddSingleton<SANYUKTExceptionFilterService>();
            services.AddSingleton<ILoggingService, LoggingService>();
            services.AddScoped<ISANYUKTServiceUser, SANYUKTServiceUser>();
            services.AddMemoryCache();
            //my data
            Audit.Core.Configuration.DataProvider = new SqlDataProvider()
            {
                ConnectionString = SANYUKTApplicationConfiguration.Instance.FIADB,
                Schema = "dbo",
                TableName = "Event",
                IdColumnName = "EventId",
                JsonColumnName = "Data",
                LastUpdatedDateColumnName = "LastUpdatedDate"
            };

            Audit.Core.Configuration.AddCustomAction(ActionType.OnEventSaving, scope =>
            {
                if (scope != null && scope.Event != null)
                {
                    if (scope.Event is AuditEventWebApi)
                    {
                        AuditApiAction mvc = (scope.Event as AuditEventWebApi).GetWebApiAuditAction();
                        if (mvc != null && mvc.ActionParameters != null)
                        {
                            if (mvc.ActionParameters.ContainsKey("userLoginRequest"))
                            {
                                if (mvc.ActionParameters["userLoginRequest"] is Datamodel.DTO.Request.UserLoginRequest)
                                    (mvc.ActionParameters["userLoginRequest"] as Datamodel.DTO.Request.UserLoginRequest).Password = "***NOT-LOGGED***";
                            }
                        }
                    }
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMemoryCache memoryCache)
        {
            MemoryCachingService.memoryCache = memoryCache;

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
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.Use(async (context, next) =>
            {
                ISANYUKTServiceUser serviceUser = context.RequestServices.GetRequiredService<ISANYUKTServiceUser>();
                serviceUser.ApiToken = context.Request.Headers["apitoken"];
                serviceUser.UserToken = context.Request.Headers["usertoken"];

                if (string.IsNullOrEmpty(serviceUser.ApiToken))
                {
                    serviceUser.ApiToken = context.Request.Query["apitoken"];
                }

                if (string.IsNullOrEmpty(serviceUser.UserToken))
                {
                    serviceUser.UserToken = context.Request.Query["usertoken"];
                }

                Int64 UserMasterID = 0;
                Int32 UserId = 0;
                Int32 UserTypeID =0;

                ApplicationUserMappingResponse applicationUserDetails = null;
                if (!string.IsNullOrEmpty(serviceUser.UserToken))
                    applicationUserDetails = await MemoryCachingService.Get<ApplicationUserMappingResponse>(string.Format(CacheKeys.APPLICATION_USER_DETAIL, serviceUser.UserToken));

                if (!string.IsNullOrEmpty(serviceUser.UserToken))
                {
                    UserMasterID = await MemoryCachingService.Get<Int32>(string.Format(CacheKeys.USERMASTER_ID, serviceUser.UserToken));
                    UserId = await MemoryCachingService.Get<Int32>(string.Format(CacheKeys.USER_ID, serviceUser.UserToken));
                }

                if (applicationUserDetails == null || (!string.IsNullOrEmpty(serviceUser.UserToken) && UserMasterID == 0))
                {
                    applicationUserDetails = await _authenticationProvider.GetApplicationAndUserDetails(serviceUser);
                    await MemoryCachingService.Put(string.Format(CacheKeys.APPLICATION_USER_DETAIL, serviceUser.UserToken), applicationUserDetails);

                    if (applicationUserDetails != null && (applicationUserDetails.UserMasterID.HasValue))
                    {
                        await MemoryCachingService.Put(string.Format(CacheKeys.USERMASTER_ID, serviceUser.UserToken), applicationUserDetails.UserMasterID);
                        UserMasterID = applicationUserDetails.UserMasterID.Value;

                        await MemoryCachingService.Put(string.Format(CacheKeys.USER_ID, serviceUser.UserToken), applicationUserDetails.UserID);
                      
                            UserId = applicationUserDetails.UserID.Value;
                        
                        await MemoryCachingService.Put(string.Format(CacheKeys.USER_Type, serviceUser.UserToken), applicationUserDetails.UserTypeID);
                        UserTypeID = applicationUserDetails.UserTypeID.Value;
                    }
                }

                if (applicationUserDetails != null)
                {
                    serviceUser.ApplicationID = applicationUserDetails.ApplicationId;
                    serviceUser.ApplicationName = applicationUserDetails.ApplicationName;
                    serviceUser.AppType = applicationUserDetails.AppType;
                    serviceUser.UserMasterID = UserMasterID;
                    serviceUser.OrganizationID = applicationUserDetails.OrganizationID;
                    serviceUser.WorkOrganizationID = applicationUserDetails.OrganizationID;
                    serviceUser.UserID = applicationUserDetails.UserID;
                    serviceUser.UserTypeId = applicationUserDetails.UserTypeID;

                    serviceUser.RequestUrl = string.Concat(context.Request.Scheme,
                        "://",
                        context.Request.Host.ToUriComponent(),
                        context.Request.PathBase.ToUriComponent(),
                        context.Request.Path.ToUriComponent(),
                        context.Request.QueryString.ToUriComponent());

                    //Get http request referer url
                    serviceUser.ReferrerUrl = context.Request.Headers["Referer"];

                    //Get http request headers
                    StringBuilder headers = new StringBuilder();
                    foreach (var header in context.Request.Headers)
                    {
                        headers.AppendLine(string.Format("{0}:{1}", header.Key, header.Value));
                    }
                    serviceUser.Headers = headers.ToString();

                    serviceUser.IPAddress = context.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
                    if (string.IsNullOrEmpty(serviceUser.IPAddress))
                    {
                        serviceUser.IPAddress = context.Request.Headers["X-Forwarded-For"];
                        if (!string.IsNullOrEmpty(serviceUser.IPAddress))
                        {
                            serviceUser.IPAddress = serviceUser.IPAddress.Split(',')[0].Split(';')[0];
                            if (serviceUser.IPAddress.Contains(":"))
                            {
                                serviceUser.IPAddress = serviceUser.IPAddress.Substring(0, serviceUser.IPAddress.LastIndexOf(':'));
                            }
                        }
                    }
                    //Get client IP address
                    serviceUser.ClientIPAddress = context.Request.Headers["ClientIPAddress"];
                }

                if (applicationUserDetails != null && applicationUserDetails.UserMasterID > 0)
                {
                    if (serviceUser.UserMasterID == 0)
                        serviceUser.UserMasterID = applicationUserDetails.UserMasterID;

                    if (serviceUser.UserMasterID > 0 && !string.IsNullOrEmpty(serviceUser.UserToken))
                    {
                        List<UserApplicationAccessPermissions> userPermissions = await MemoryCachingService.Get<List<UserApplicationAccessPermissions>>(string.Format(CacheKeys.USER_ROLES_API, serviceUser.ApplicationID, serviceUser.UserToken));

                        if (userPermissions == null || userPermissions.Count == 0)
                            await MemoryCachingService.Put(string.Format(CacheKeys.USER_ROLES_API, serviceUser.ApplicationID, serviceUser.UserToken), _authenticationProvider.GetUserAccessPermissions(serviceUser).Result);
                    }
                }
                await next();
            });
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

              

            });
        }
    }
}
