using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Repository;
using System.Collections.Generic;
using System.Net;

namespace SANYUKT.API.Common
{
    public class CustomIPWhitelistActionFilter: ActionFilterAttribute
    {
        private readonly IPOptions _ipWhitelistOptions;
        private readonly UsersRepository repository=null;

        public CustomIPWhitelistActionFilter(IOptions<IPOptions>
        whiteListOptions, ILogger<CustomIPWhitelistActionFilter> logger)
        {
            _ipWhitelistOptions = whiteListOptions.Value;
            repository = new UsersRepository();
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            ISANYUKTServiceUser serviceUser = context.HttpContext.RequestServices.GetService(typeof(ISANYUKTServiceUser)) as ISANYUKTServiceUser;
            if (serviceUser != null)
            {
                if( serviceUser.UserMasterID!=0)
                {
                    IPAddress remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;
                    _ipWhitelistOptions.Whitelist = await repository.GetallIPAddress(serviceUser);
                    List<string> whiteListIPList = _ipWhitelistOptions.Whitelist;

                    if (!whiteListIPList.Contains(remoteIpAddress.ToString()))
                    {
                        BaseResponse response = new BaseResponse();
                        response.SetError(ErrorCodes.SP_142);
                        context.Result = new ObjectResult(response);
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return;
                    }
                }
                else
                {
                    BaseResponse response = new BaseResponse();
                    response.SetError(ErrorCodes.SP_141);
                    context.Result = new ObjectResult(response);
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }
               
            }
            else
            {
                BaseResponse response = new BaseResponse();
                response.SetError(ErrorCodes.SP_141);
                context.Result = new ObjectResult(response);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }
                

            base.OnActionExecuting(context);
        }
    }
}
