using Audit.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SANYUKT.API.Common;
using SANYUKT.API.Security;
using SANYUKT.Configuration;
using SANYUKT.Datamodel.DTO.Request;
using SANYUKT.Datamodel.DTO.Response;
using SANYUKT.Datamodel.Entities.Transactions;
using SANYUKT.Datamodel.Entities.Users;
using SANYUKT.Datamodel.RblPayoutRequest;
using SANYUKT.Datamodel.RblPayoutResponse;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider;
using SANYUKT.Provider.Payout;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SANYUKT.API.Controllers
{
    [ResponseCache(Duration = -1, Location = ResponseCacheLocation.None, NoStore = true)]
    [ServiceFilter(typeof(SANYUKTExceptionFilterService))]

    public class PayoutController : BaseApiController
    {
        private AuthenticationProvider _authenticationProvider;
        private AuthenticationHelper _callValidator = null;
        public readonly UserDetailsProvider _Provider;
        public readonly RblPayoutProvider _rblProvider;
        private IHostingEnvironment _env;
        public readonly TransactionProvider _TxnProvider;
        public PayoutController(IHostingEnvironment env)
        {
            _env = env;
            _authenticationProvider = new AuthenticationProvider();
            _callValidator = new AuthenticationHelper();
            _Provider = new UserDetailsProvider();
            _rblProvider=new RblPayoutProvider();
            _TxnProvider=new TransactionProvider();
        }
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userLoginRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [AuditApi(EventTypeName = "POST PayoutController/GenerateToken", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> GenerateToken([FromBody] UserLoginRequest userLoginRequest)
        {
            UserLoginResponse response = new UserLoginResponse();

            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (error.HasError)
            {
                response.SetError(error);
                return Ok(response);
            }
            
            response = await _authenticationProvider.Login(userLoginRequest, this.CallerUser);

            ErrorResponse errorResponse = await _authenticationProvider.GetAllIPAddressDetails(CallerUser);
            if (errorResponse.HasError)
            {
                response.SetError(ErrorCodes.SP_142);
                return Ok(response);
            }
            if (this.CallerUser.UserTypeId != 2)
            {
                response.SetError(ErrorCodes.SP_140);
                return Ok(response);
            }


            return Ok(response);

        }
        /// <summary>
        /// Add Benficiary Request
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        [Route("Payout/AddBenficiary")]
        [HttpPost]
        public async Task<IActionResult> AddBenficiary([FromBody] AddBenficiaryRequest request)
        {
            long UserId = 0;
            SimpleResponse response=new SimpleResponse();

            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (error.HasError)
            {
                response.SetError(error);
                return Ok(response);
            }

            ErrorResponse errorResponse = await _authenticationProvider.GetAllIPAddressDetails(CallerUser);
            if (errorResponse.HasError)
            {
                response.SetError(ErrorCodes.SP_142);
                return Ok(response);
            }
            if (this.CallerUser.UserTypeId != 2)
            {
                response.SetError(ErrorCodes.SP_140);
                return Ok(response);
            }
            UserId = await _Provider.AddNewBenficiary(request,this.CallerUser);
            if(UserId>0)
            {
                response.Result=UserId;
            }
            if (response == null)
            {
                response = new SimpleResponse();
                response.SetError(ErrorCodes.SERVER_ERROR);
                return Json(response);
            }

            return Json(response);
        }
        /// <summary>
        /// Add Benficiary Request
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        [Route("Payout/ListBenficiary")]
        [HttpPost]
        public async Task<IActionResult> ListBenficiary([FromBody] ListBenficaryRequest request)
        {
            List<BenficiaryResponse> benficiaries=new List<BenficiaryResponse>();
            SimpleResponse response = new SimpleResponse();

            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (error.HasError)
            {
                response.SetError(error);
                return Ok(response);
            }

            ErrorResponse errorResponse = await _authenticationProvider.GetAllIPAddressDetails(CallerUser);
            if (errorResponse.HasError)
            {
                response.SetError(ErrorCodes.SP_142);
                return Ok(response);
            }
            if (this.CallerUser.UserTypeId != 2)
            {
                response.SetError(ErrorCodes.SP_140);
                return Ok(response);
            }
            benficiaries = await _Provider.GetAllBenficiary(request,this.CallerUser);
            if (benficiaries == null)
            {
                response = new SimpleResponse();
                response.SetError(ErrorCodes.NO_RECORD_FOUND);
                return Json(response);
            }
            if(benficiaries.Count>0)
            {
                response.Result = benficiaries;
            }
            if (response == null)
            {
                response = new SimpleResponse();
                response.SetError(ErrorCodes.SERVER_ERROR);
                return Json(response);
            }

            return Json(response);
        }
        /// <summary>
        /// Payout transaction API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("Payout/Transaction")]
        [HttpPost]
        [AuditApi(EventTypeName = "POST Payout/DirectPay", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> DirectPay([FromBody] SinglePaymentRequestFT request)
        {
            SimpleResponse response = new SimpleResponse();
           

            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (error.HasError)
            {
                response.SetError(error);
                return Ok(response);
            }
            ErrorResponse errorResponse = await _authenticationProvider.GetAllIPAddressDetails(CallerUser);
            if (errorResponse.HasError)
            {
                response.SetError(ErrorCodes.SP_142);
                return Ok(response);
            }
            if (this.CallerUser.UserTypeId != 2)
            {
                response.SetError(ErrorCodes.SP_140);
                return Ok(response);
            }
            if (request == null)
            {
                response.SetError(ErrorCodes.INVALID_PARAMETERS);
                return Ok(response);
            }
            if (request.SenderMobile == "")
            {
                response.SetError(ErrorCodes.SP_143);
                return Ok(response);
            }
            if (request.Mode_of_Pay == "")
            {
                response.SetError(ErrorCodes.SP_144);
                return Ok(response);
            }
           
            if (request.Amount == 0)
            {
                response.SetError(ErrorCodes.SP_131);
                return Ok(response);
            }
            if (Convert.ToDecimal(request.Amount) <= 0)
            {
                response.SetError(ErrorCodes.SP_131);
                return Ok(response);
            }
            if (request.PartnerRefNo == "")
            {
                response.SetError(ErrorCodes.SP_147);
                return Ok(response);
            }
            if (request.Ben_Name == "")
            {
                response.SetError(ErrorCodes.SP_148);
                return Ok(response);
            }
            if (request.Ben_Mobile == "")
            {
                response.SetError(ErrorCodes.SP_149);
                return Ok(response);
            }
            if (request.Ben_Acct_No == "")
            {
                response.SetError(ErrorCodes.SP_150);
                return Ok(response);
            }
            if (request.Ben_BankName == "")
            {
                response.SetError(ErrorCodes.SP_151);
                return Ok(response);
            }
            if (request.Ben_IFSC == "")
            {
                response.SetError(ErrorCodes.SP_152);
                return Ok(response);
            }
            
            X509Certificate2 certificate2 = new X509Certificate2(System.IO.Path.Combine(_env.WebRootPath.ToString() + "\\SSlCertificate", SANYUKTApplicationConfiguration.Instance.certisslName.ToString()), SANYUKTApplicationConfiguration.Instance.certisslpass.ToString());
            response = await _rblProvider.PayoutTransactionwithoutBen(request, certificate2, this.CallerUser);

            return Ok(response);

        }
        /// <summary>
        /// Get transaction Status
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("Payout/TransactionStatus")]
        [HttpPost]
        [AuditApi(EventTypeName = "POST RblPayoutController/TransactionStatus", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> TransactionStatus([FromBody] SinglePaymentStatusNew request)
        {
            SimpleResponse response = new SimpleResponse();

            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (error.HasError)
            {
                response.SetError(error);
                return Ok(response);
            }
            ErrorResponse errorResponse = await _authenticationProvider.GetAllIPAddressDetails(CallerUser);
            if (errorResponse.HasError)
            {
                response.SetError(ErrorCodes.SP_142);
                return Ok(response);
            }
            if (this.CallerUser.UserTypeId != 2)
            {
                response.SetError(ErrorCodes.SP_140);
                return Ok(response);
            }
           
            RblStatusResponse response1 = new RblStatusResponse();
            X509Certificate2 certificate2 = new X509Certificate2(System.IO.Path.Combine(_env.WebRootPath.ToString() + "\\SSlCertificate", SANYUKTApplicationConfiguration.Instance.certisslName.ToString()), SANYUKTApplicationConfiguration.Instance.certisslpass.ToString());
            response1 = await _rblProvider.PayoutTransactionStatus(request, certificate2, this.CallerUser);

            return Ok(response1);

        }
        /// <summary>
        /// Direct Payout Transaction List
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        [Route("Payout/TransactionList")]
        [HttpPost]
        public async Task<IActionResult> TransactionList([FromBody] TransactionDetailsPayoutRequest request)
        {
            SimpleResponse response = new SimpleResponse();

            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            ErrorResponse errorResponse = await _authenticationProvider.GetAllIPAddressDetails(CallerUser);
            if (errorResponse.HasError)
            {
                response.SetError(ErrorCodes.SP_142);
                return Ok(response);
            }
            if (this.CallerUser.UserTypeId != 2)
            {
                response.SetError(ErrorCodes.SP_140);
                return Ok(response);
            }
            response = await _TxnProvider.GetPayoutTransactionList(request, CallerUser);
            return Json(response);
        }
        /// <summary>
        /// Balance Check API
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        [Route("Payout/GetBalalnce")]
        [HttpGet]
        //[AuditApi(EventTypeName = "POST Payout/CheckBalance", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = true, IncludeModelState = true)]
        public async Task<IActionResult> CheckBalance()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
          
            ErrorResponse errorResponse =await _authenticationProvider.GetAllIPAddressDetails(CallerUser);
            if(errorResponse.HasError)
            {
                response.SetError(ErrorCodes.SP_142);
                return Ok(response);
            }
            if (this.CallerUser.UserTypeId != 2)
            {
                response.SetError(ErrorCodes.SP_140);
                return Ok(response);
            }
            response = await _Provider.CheckBalalnce(CallerUser);
            return Json(response);
        }
        /// <summary>
        /// Payout transaction API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("Payout/TransactionWithBenID")]
        [HttpPost]
        [AuditApi(EventTypeName = "POST Payout/TransactionWithBenID", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> PayoutTransaction([FromBody] PayoutTransaction request)
        {
            SimpleResponse response = new SimpleResponse();

            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (error.HasError)
            {
                response.SetError(error);
                return Ok(response);
            }
            ErrorResponse errorResponse = await _authenticationProvider.GetAllIPAddressDetails(CallerUser);
            if (errorResponse.HasError)
            {
                response.SetError(ErrorCodes.SP_142);
                return Ok(response);
            }
            if (this.CallerUser.UserTypeId != 2)
            {
                response.SetError(ErrorCodes.SP_140);
                return Ok(response);
            }
            if(request==null)
            {
                response.SetError(ErrorCodes.INVALID_PARAMETERS);
                return Ok(response);
            }
            if (request.SenderMobile == "")
            {
                response.SetError(ErrorCodes.SP_143);
                return Ok(response);
            }
            if (request.Mode_of_Pay == "")
            {
                response.SetError(ErrorCodes.SP_144);
                return Ok(response);
            }
            if (request.BenficiaryID == null)
            {
                response.SetError(ErrorCodes.SP_145);
                return Ok(response);
            }
            if (request.BenficiaryID == "0")
            {
                response.SetError(ErrorCodes.SP_145);
                return Ok(response);
            }
            if (request.BenficiaryID == "")
            {
                response.SetError(ErrorCodes.SP_145);
                return Ok(response);
            }
            if (request.Amount == "")
            {
                response.SetError(ErrorCodes.SP_131);
                return Ok(response);
            }
            if (Convert.ToDecimal( request.Amount) <=0)
            {
                response.SetError(ErrorCodes.SP_131);
                return Ok(response);
            }
            if (request.PartnerRefNo=="")
            {
                response.SetError(ErrorCodes.SP_147);
                return Ok(response);
            }
           
            X509Certificate2 certificate2 = new X509Certificate2(System.IO.Path.Combine(_env.WebRootPath.ToString() + "\\SSlCertificate", SANYUKTApplicationConfiguration.Instance.certisslName.ToString()), SANYUKTApplicationConfiguration.Instance.certisslpass.ToString());
            response = await _rblProvider.PayoutTransaction(request, certificate2, this.CallerUser);

            return Ok(response);

        }
    }
}
