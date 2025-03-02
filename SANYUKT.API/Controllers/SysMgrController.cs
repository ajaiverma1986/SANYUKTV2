using Audit.WebApi;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SANYUKT.API.Common;
using SANYUKT.API.Security;
using SANYUKT.Datamodel.Entities.SysModel;
using SANYUKT.Datamodel.Entities.Users;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider;
using System.Threading.Tasks;

namespace SANYUKT.API.Controllers
{
    [EnableCors("AllowAll")]
    [ResponseCache(Duration = -1, Location = ResponseCacheLocation.None, NoStore = true)]
    [ServiceFilter(typeof(SANYUKTExceptionFilterService))]
    public class SysMgrController :BaseApiController
    {
        public readonly SysMgrProvider _Provider;
        private AuthenticationHelper _callValidator = null;
        private  readonly UtilityProvider _utilityProvider = null;
        public SysMgrController() {

            _Provider = new SysMgrProvider();
            _callValidator = new AuthenticationHelper();
            _utilityProvider=new UtilityProvider();
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewRole([FromBody] CreateRoleRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response.Result = await _Provider.CreateNewRole(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetAllRoles([FromBody] GetRoleRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetAllRoles(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> ValidateDocument([FromBody] DocValidatorViewModelRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _utilityProvider.ValidateDocument(request, this.CallerUser);
            return Json(response);
        }

        [HttpPost]
       
        public async Task<IActionResult> SendOTP([FromBody] OTPRequest request)
        {
            OTPResponse response = new OTPResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (error.HasError)
            {
                response.response_message = "Authentication Fail";
                return Json(response);
            }
            response = await _utilityProvider.SendOTP(request);
            return Json(response);
        }

        [HttpPost]
      
        public async Task<IActionResult> ValidateOTP([FromBody] OTPValidateRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _utilityProvider.ValidateOTP(request);
            return Json(response);
        }
    }
}
