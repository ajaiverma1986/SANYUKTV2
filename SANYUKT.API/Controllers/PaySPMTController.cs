using Microsoft.AspNetCore.Mvc;
using SANYUKT.API.Common;
using SANYUKT.API.Security;
using SANYUKT.Datamodel.Entities;
using SANYUKT.Datamodel.Paysprint;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider;
using System.Threading.Tasks;

namespace SANYUKT.API.Controllers
{
    public class PaySPMTController : BaseApiController
    {
        public readonly PaySprintProvider _provider=null;
        private AuthenticationHelper _callValidator = null;
        public PaySPMTController() {
            _provider=new PaySprintProvider();
            _callValidator = new AuthenticationHelper();
        }

        [HttpGet]
        public async Task<IActionResult> GenerateToken()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _provider.GenerateToken( CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetFinoCustomerDetail([FromBody] GetCustomerRequestView request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _provider.GetFinoCustomerDetail(request, this.CallerUser);
            return Json(response);
        }

    }
}
