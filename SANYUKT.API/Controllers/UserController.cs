using Audit.WebApi;
using Microsoft.AspNetCore.Mvc;
using SANYUKT.API.Common;
using SANYUKT.API.Security;
using SANYUKT.Configuration;
using SANYUKT.Datamodel.DTO.Response;
using SANYUKT.Datamodel.Entities.Users;
using SANYUKT.Datamodel.RblPayoutRequest;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider;
using SANYUKT.Provider.Shared;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SANYUKT.API.Controllers
{
    [ResponseCache(Duration = -1, Location = ResponseCacheLocation.None, NoStore = true)]
    [ServiceFilter(typeof(SANYUKTExceptionFilterService))]
    public class UserController : BaseApiController
    {
        public readonly UserDetailsProvider _Provider;
        private AuthenticationHelper _callValidator = null;
        private readonly AuthenticationProvider _authenticationProvider;
        public UserController() {
            _authenticationProvider = new AuthenticationProvider();
            _Provider = new UserDetailsProvider();
            _callValidator = new AuthenticationHelper();
        }
        
        [HttpPost]
        [AuditApi(EventTypeName = "POST UserController/CreateNewUser", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = true, IncludeModelState = true)]
        public async Task<IActionResult> CreateNewUser([FromBody] CreateUserWithlogoRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }


            FileManager obj = new FileManager();
            string filename = obj.SaveFile(request.FileBytes, request.MobileNo, request.FileName);
            response.Result = await _Provider.CreateNewUserRequest(request, filename, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        [AuditApi(EventTypeName = "POST UserController/AddOriginatorAccounts", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = true, IncludeModelState = true)]
        public async Task<IActionResult> AddOriginatorAccounts([FromBody] CreateOriginatorAccountRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response.Result = await _Provider.AddOriginatorAccounts(request, this.CallerUser);
            return Json(response);
        }
    }
}
