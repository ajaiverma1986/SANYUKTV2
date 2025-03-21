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
        public readonly PaySpAirtelDMTProvider _airtelProvider = null;
        public readonly SPayCreditCardProvider _credProvider = null;
        public PaySPMTController() {
            _provider=new PaySprintProvider();
            _callValidator = new AuthenticationHelper();
            _airtelProvider=new PaySpAirtelDMTProvider();
            _credProvider=new SPayCreditCardProvider();
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
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _provider.GetFinoCustomerDetail(request, this.CallerUser);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterFinoCustomerKyc([FromBody] FinoEkycRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _provider.FinoCustomerEkyc(request, this.CallerUser);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterFinoCustomer([FromBody] FinoRegCustomerRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _provider.FinoRegisterCustomer(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> FinoRegisterBenficiary([FromBody] FinoRegBenRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _provider.FinoRegisterBenficiary(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> FinoDeleteBenficiary([FromBody] FinoDeleteBenRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _provider.FinoDeleteBenficiary(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> FinoFetchBenficiary([FromBody] FinofetchBenRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _provider.FinoFetchBenficiary(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> FinoFetchBenficiaryByBenID([FromBody] FinofetchBenRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _provider.FinoFetchBenficiaryByBenID(request, this.CallerUser);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> FinoPPenyDrop([FromBody] FinoTransactionRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _provider.FinoPPenyDrop(request, this.CallerUser);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> FinoTransactionOTP([FromBody] FinoTransactionSendRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _provider.FinoTransactionOTP(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> FinoTransaction([FromBody] FinoTransactionFinalRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _provider.FinoTransaction(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> FinoTransactionStatus([FromBody] FinoTransactionStatusRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _provider.FinoTransactionStatus(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> FinoTransactionRefundOTP([FromBody] FinoRefundOtpRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

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

        [HttpPost]
        public async Task<IActionResult> GetAirtelCustomerDetail([FromBody] GetCustomerRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _airtelProvider.GetAirtelCustomerDetail(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> AirtelVerifyAadhar([FromBody] AirtelVerifyAadharRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _airtelProvider.AirtelVerifyAadhar(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> AirtelRegisterCustomer([FromBody] AirtelRegCustomerRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _airtelProvider.AirtelRegisterCustomer(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> AirtelRegisterBenficiary([FromBody] FinoRegBenRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _airtelProvider.AirtelRegisterBenficiary(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> AirtelDeleteBenficiary([FromBody] FinoDeleteBenRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _airtelProvider.AirtelDeleteBenficiary(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> AirtelFetchBenficiary([FromBody] FinofetchBenRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _airtelProvider.AirtelFetchBenficiary(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> AirtelFetchBenficiaryByBenID([FromBody] FinofetchBenRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _airtelProvider.AirtelFetchBenficiaryByBenID(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> AirtelPPenyDrop([FromBody] FinoTransactionRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _airtelProvider.AirtelPPenyDrop(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> AirtelTransactionOTP([FromBody] FinoTransactionSendRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _airtelProvider.AirtelTransactionOTP(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> AirtelTransaction([FromBody] FinoTransactionFinalRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _airtelProvider.AirtelTransaction(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> AirtelTransactionStatus([FromBody] FinoTransactionStatusRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _airtelProvider.AirtelTransactionStatus(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> AirtelTransactionRefundOTP([FromBody] FinoRefundOtpRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _airtelProvider.AirtelTransactionRefundOTP(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> AirtelTransactionRefund([FromBody] FinoRefundRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _airtelProvider.AirtelTransactionRefund(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> AirtelCMSTransactionInq([FromBody] AirtelCMSLNKGENRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _airtelProvider.AirtelCMSTransactionInq(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> GenerateOTPForCreditCard([FromBody] CCGenerateOTPView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _credProvider.GenerateOTP(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreditCardBillpayment([FromBody] CCPaybillRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _credProvider.Billpayment(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetCCPaymentStatus([FromBody] CCPaybillStatusRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _credProvider.GetCCPaymentStatus(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetCCRefundOTP([FromBody] CCRefundOTPRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _credProvider.GetCCRefundOTP(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetCCRefund([FromBody] CCRefundRequestView request)
        {
            SpBaseResponse response = new SpBaseResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await _credProvider.GetCCRefund(request, this.CallerUser);
            return Json(response);
        }

    }
}
