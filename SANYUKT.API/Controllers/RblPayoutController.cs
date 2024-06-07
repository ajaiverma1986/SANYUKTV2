using Audit.WebApi;
using Microsoft.AspNetCore.Mvc;
using SANYUKT.API.Common;
using SANYUKT.API.Security;
using SANYUKT.Datamodel.DTO.Request;
using SANYUKT.Datamodel.DTO.Response;
using SANYUKT.Datamodel.RblPayoutRequest;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider;
using SANYUKT.Provider.Payout;
using System.Threading.Tasks;

namespace SANYUKT.API.Controllers
{
    [ResponseCache(Duration = -1, Location = ResponseCacheLocation.None, NoStore = true)]
    [ServiceFilter(typeof(SANYUKTExceptionFilterService))]
    public class RblPayoutController: BaseApiController
    {
        public readonly RblPayoutProvider _Provider;
        
        public RblPayoutController()
        {
            _Provider = new RblPayoutProvider();
        }
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userLoginRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [AuditApi(EventTypeName = "POST RblPayoutController/GetAccountBalalance", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> GetAccountBalalance([FromBody] AccountBalalnceRequest request)
        {
        //    UserLoginResponse response = new UserLoginResponse();

        //    ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
        //    if (error.HasError)
        //    {
        //        response.SetError(error);
        //        return Ok(response);
        //    }
        SimpleResponse response1 =new SimpleResponse();
            response1 = await _Provider.GetBalalce(request, this.CallerUser);

            return Ok(response1);

        }
    }
}
