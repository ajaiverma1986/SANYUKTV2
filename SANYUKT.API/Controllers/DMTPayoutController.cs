using Microsoft.AspNetCore.Mvc;
using SANYUKT.API.Common;
using SANYUKT.API.Security;
using SANYUKT.Provider.Payout;
using SANYUKT.Provider;
using Microsoft.AspNetCore.Hosting;
using Audit.WebApi;
using SANYUKT.Datamodel.DTO.Request;
using SANYUKT.Datamodel.DTO.Response;
using SANYUKT.Datamodel.Shared;
using System.Threading.Tasks;
using SANYUKT.Configuration;
using SANYUKT.Datamodel.Entities.Transactions;
using SANYUKT.Datamodel.Entities.Users;
using SANYUKT.Datamodel.RblPayoutRequest;
using SANYUKT.Datamodel.RblPayoutResponse;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace SANYUKT.API.Controllers
{
    [ResponseCache(Duration = -1, Location = ResponseCacheLocation.None, NoStore = true)]
    [ServiceFilter(typeof(SANYUKTExceptionFilterService))]
    public class DMTPayoutController : BaseApiController
    {
        private AuthenticationProvider _authenticationProvider;
        private AuthenticationHelper _callValidator = null;
        public readonly UserDetailsProvider _Provider;
        public readonly RblPayoutProvider _rblProvider;
        private IHostingEnvironment _env;
        public readonly TransactionProvider _TxnProvider;
        public DMTPayoutController(IHostingEnvironment env)
        {
            _env = env;
            _authenticationProvider = new AuthenticationProvider();
            _callValidator = new AuthenticationHelper();
            _Provider = new UserDetailsProvider();
            _rblProvider = new RblPayoutProvider();
            _TxnProvider = new TransactionProvider();
        }
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userLoginRequest"></param>
        /// <returns></returns>
        [HttpPost]
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

            return Ok(response);

        }
        /// <summary>
        /// Add Benficiary Request
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        [Route("DMT/AddBenficiary")]
        [HttpPost]
        public async Task<IActionResult> AddBenficiary([FromBody] AddBenficiaryRequest request)
        {
            long UserId = 0;
            SimpleResponse response = new SimpleResponse();
            UserId = await _Provider.AddNewBenficiary(request, this.CallerUser);
            if (UserId > 0)
            {
                response.Result = UserId;
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
        [Route("DMT/ListBenficiary")]
        [HttpPost]
        public async Task<IActionResult> ListBenficiary([FromBody] ListBenficaryRequest request)
        {
            List<BenficiaryResponse> benficiaries = new List<BenficiaryResponse>();
            SimpleResponse response = new SimpleResponse();
            benficiaries = await _Provider.GetAllBenficiary(request, this.CallerUser);
            if (benficiaries == null)
            {
                response = new SimpleResponse();
                response.SetError(ErrorCodes.NO_RECORD_FOUND);
                return Json(response);
            }
            if (benficiaries.Count > 0)
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
        [Route("DMT/Transaction")]
        [HttpPost]
        public async Task<IActionResult> DirectPay([FromBody] SinglePaymentRequestFT request)
        {
            UserLoginResponse response = new UserLoginResponse();

            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (error.HasError)
            {
                response.SetError(error);
                return Ok(response);
            }
            SimpleResponse response1 = new SimpleResponse();
            X509Certificate2 certificate2 = new X509Certificate2(System.IO.Path.Combine(_env.WebRootPath.ToString() + "/SSlCertificate", SANYUKTApplicationConfiguration.Instance.certisslName.ToString()), SANYUKTApplicationConfiguration.Instance.certisslpass.ToString());
            response1 = await _rblProvider.PayoutTransactionwithoutBen(request, certificate2, this.CallerUser);

            return Ok(response1);

        }
        /// <summary>
        /// Get transaction Status
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("DMT/TransactionStatus")]
        [HttpPost]
        public async Task<IActionResult> TransactionStatus([FromBody] SinglePaymentStatusNew request)
        {
            UserLoginResponse response = new UserLoginResponse();

            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (error.HasError)
            {
                response.SetError(error);
                return Ok(response);
            }
            RblStatusResponse response1 = new RblStatusResponse();
            X509Certificate2 certificate2 = new X509Certificate2(System.IO.Path.Combine(_env.WebRootPath.ToString() + "/SSlCertificate", SANYUKTApplicationConfiguration.Instance.certisslName.ToString()), SANYUKTApplicationConfiguration.Instance.certisslpass.ToString());
            response1 = await _rblProvider.PayoutTransactionStatus(request, certificate2, this.CallerUser);

            return Ok(response1);

        }
        /// <summary>
        /// Direct Payout Transaction List
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        [Route("DMT/TransactionList")]
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
            response = await _TxnProvider.GetPayoutTransactionList(request, CallerUser);
            return Json(response);
        }
        /// <summary>
        /// Balance Check API
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        [Route("DMT/GetBalalnce")]
        [HttpGet]
        public async Task<IActionResult> CheckBalance()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.CheckBalalnce(CallerUser);
            return Json(response);
        }
        /// <summary>
        /// Payout transaction API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("DMT/TransactionWithBenID")]
        [HttpPost]
        public async Task<IActionResult> PayoutTransaction([FromBody] PayoutTransaction request)
        {
            UserLoginResponse response = new UserLoginResponse();

            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (error.HasError)
            {
                response.SetError(error);
                return Ok(response);
            }
            SimpleResponse response1 = new SimpleResponse();
            X509Certificate2 certificate2 = new X509Certificate2(System.IO.Path.Combine(_env.WebRootPath.ToString() + "/SSlCertificate", SANYUKTApplicationConfiguration.Instance.certisslName.ToString()), SANYUKTApplicationConfiguration.Instance.certisslpass.ToString());
            response1 = await _rblProvider.PayoutTransaction(request, certificate2, this.CallerUser);

            return Ok(response1);

        }
    }
}

