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
            //ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            //if (error.HasError)
            //{
            //    response.SetError(error);
            //    return Json(response);
            //}
            response = await _provider.GenerateToken( CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetFinoCustomerDetail([FromBody] GetCustomerRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            //ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            //if (error.HasError)
            //{
            //    response.SetError(error);
            //    return Json(response);
            //}

            response = await _provider.GetFinoCustomerDetail(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterFinoCustomerKyc([FromBody] FinoEkycRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            //ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            //if (error.HasError)
            //{
            //    response.SetError(error);
            //    return Json(response);
            //}

            response = await _provider.FinoCustomerEkyc(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterFinoCustomer([FromBody] FinoRegCustomerRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            //ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            //if (error.HasError)
            //{
            //    response.SetError(error);
            //    return Json(response);
            //}

            response = await _provider.FinoRegisterCustomer(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> FinoRegisterBenficiary([FromBody] FinoRegBenRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            //ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            //if (error.HasError)
            //{
            //    response.SetError(error);
            //    return Json(response);
            //}

            response = await _provider.FinoRegisterBenficiary(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> FinoDeleteBenficiary([FromBody] FinoDeleteBenRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            //ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            //if (error.HasError)
            //{
            //    response.SetError(error);
            //    return Json(response);
            //}

            response = await _provider.FinoDeleteBenficiary(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> FinoFetchBenficiary([FromBody] FinofetchBenRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            //ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            //if (error.HasError)
            //{
            //    response.SetError(error);
            //    return Json(response);
            //}

            response = await _provider.FinoFetchBenficiary(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> FinoFetchBenficiaryByBenID([FromBody] FinofetchBenRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            //ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            //if (error.HasError)
            //{
            //    response.SetError(error);
            //    return Json(response);
            //}

            response = await _provider.FinoFetchBenficiaryByBenID(request, this.CallerUser);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> FinoPPenyDrop([FromBody] FinoTransactionRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            //ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            //if (error.HasError)
            //{
            //    response.SetError(error);
            //    return Json(response);
            //}

            response = await _provider.FinoPPenyDrop(request, this.CallerUser);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> FinoTransactionOTP([FromBody] FinoTransactionSendRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            //ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            //if (error.HasError)
            //{
            //    response.SetError(error);
            //    return Json(response);
            //}

            response = await _provider.FinoTransactionOTP(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> FinoTransaction([FromBody] FinoTransactionFinalRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            //ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            //if (error.HasError)
            //{
            //    response.SetError(error);
            //    return Json(response);
            //}

            response = await _provider.FinoTransaction(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> FinoTransactionStatus([FromBody] FinoTransactionStatusRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            //ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            //if (error.HasError)
            //{
            //    response.SetError(error);
            //    return Json(response);
            //}

            response = await _provider.FinoTransactionStatus(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> FinoTransactionRefundOTP([FromBody] FinoRefundOtpRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            //ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            //if (error.HasError)
            //{
            //    response.SetError(error);
            //    return Json(response);
            //}

            response = await _provider.FinoTransactionRefundOTP(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> FinoTransactionRefund([FromBody] FinoRefundRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            //ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            //if (error.HasError)
            //{
            //    response.SetError(error);
            //    return Json(response);
            //}

            response = await _provider.FinoTransactionRefund(request, this.CallerUser);
            return Json(response);
        }

    }
}
