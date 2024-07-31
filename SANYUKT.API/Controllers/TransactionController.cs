using Microsoft.AspNetCore.Mvc;
using SANYUKT.API.Common;
using SANYUKT.API.Security;
using SANYUKT.Datamodel.Entities.Transactions;
using SANYUKT.Datamodel.Masters;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider;
using System.Threading.Tasks;

namespace SANYUKT.API.Controllers
{
    [ResponseCache(Duration = -1, Location = ResponseCacheLocation.None, NoStore = true)]
    [ServiceFilter(typeof(SANYUKTExceptionFilterService))]
    public class TransactionController : BaseApiController
    {
        public readonly TransactionProvider _Provider;
        private AuthenticationHelper _callValidator = null;
        private readonly AuthenticationProvider _authenticationProvider;

        public TransactionController()
        {
            _Provider = new TransactionProvider();
            _callValidator = new AuthenticationHelper();
            _authenticationProvider = new AuthenticationProvider();
        }
        [HttpPost]
        public async Task<IActionResult> NewPayinRequest([FromBody] AddPaymentRequestRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.AddNewPayinRequest(request, CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePayinRequestStatus([FromBody] ApproveRejectPayinRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.ApproveRejectPayinRequest(request, CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> ListPayinRequest([FromBody] ListPayinRequestRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetallPayinRequest(request);
            return Json(response);
        }
    }
}
