using Audit.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SANYUKT.API.Common;
using SANYUKT.API.Security;
using SANYUKT.Configuration;
using SANYUKT.Datamodel.DTO.Request;
using SANYUKT.Datamodel.DTO.Response;
using SANYUKT.Datamodel.Entities.Users;
using SANYUKT.Datamodel.RblPayoutRequest;
using SANYUKT.Datamodel.RblPayoutResponse;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider;
using SANYUKT.Provider.Payout;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SANYUKT.API.Controllers
{
    [ResponseCache(Duration = -1, Location = ResponseCacheLocation.None, NoStore = true)]
    [ServiceFilter(typeof(SANYUKTExceptionFilterService))]
    public class RblPayoutController: BaseApiController
    {
        public readonly RblPayoutProvider _Provider;
        public readonly UserDetailsProvider _userProvider;
        private IHostingEnvironment _env;
        private AuthenticationHelper _callValidator = null;
        public RblPayoutController(IHostingEnvironment env)
        {
            _env= env;
            _Provider = new RblPayoutProvider();
            _callValidator = new AuthenticationHelper();
            _userProvider=new UserDetailsProvider();
        }

        ///// <summary>
        ///// Get Balalnce API
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[AuditApi(EventTypeName = "POST RblPayoutController/GetAccountBalalance", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        //public async Task<IActionResult> GetAccountBalalance([FromBody]RblPayoutRequest objp)
        //{
        //    UserLoginResponse response = new UserLoginResponse();

        //    ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
        //    if (error.HasError)
        //    {
        //        response.SetError(error);
        //        return Ok(response);
        //    }
        //    SimpleResponse response1 =new SimpleResponse();
        //    X509Certificate2 certificate2 = new X509Certificate2(System.IO.Path.Combine(_env.WebRootPath.ToString ()+ "/SSlCertificate", SANYUKTApplicationConfiguration.Instance.certisslName.ToString ()), SANYUKTApplicationConfiguration.Instance.certisslpass.ToString ());
        //    response1 = await _Provider.GetBalalce(objp, certificate2, this.CallerUser);

        //    return Ok(response1);

        //}



        /// <summary>
        /// Payout transaction API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AuditApi(EventTypeName = "POST RblPayoutController/PayoutTransactionwithoutBen", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> DirectPay([FromBody] SinglePaymentRequest request)
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
            response1 = await _Provider.PayoutTransactionwithoutBen(request, certificate2, this.CallerUser);

            return Ok(response1);

        }
        /// <summary>
        /// Payout transaction API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AuditApi(EventTypeName = "POST RblPayoutController/PayoutTransaction", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
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
            response1 = await _Provider.PayoutTransaction(request, certificate2, this.CallerUser);

            return Ok(response1);

        }
        /// <summary>
        /// Get transaction Status
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AuditApi(EventTypeName = "POST RblPayoutController/TransactionStatus", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> TransactionStatus([FromBody] SinglePaymentStatus request)
        {
            UserLoginResponse response = new UserLoginResponse();

            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (error.HasError)
            {
                response.SetError(error);
                return Ok(response);
            }
            RblResponse response1 = new RblResponse();
            X509Certificate2 certificate2 = new X509Certificate2(System.IO.Path.Combine(_env.WebRootPath.ToString() + "/SSlCertificate", SANYUKTApplicationConfiguration.Instance.certisslName.ToString()), SANYUKTApplicationConfiguration.Instance.certisslpass.ToString());
            response1 = await _Provider.PayoutTransactionStatus(request, certificate2, this.CallerUser);

            return Ok(response1);

        }
        /// <summary>
        /// Get transaction Status
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AuditApi(EventTypeName = "POST RblPayoutController/GetAccountStatement", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> GetAccountStatement([FromBody] RblPayoutStatementRequest request)
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
            response1 = await _Provider.AccountStatement(request, certificate2, this.CallerUser);

            return Ok(response1);

        }
        /// <summary>
        /// Add Beneficiary API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AuditApi(EventTypeName = "POST RblPayoutController/AddBenficiary", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> AddBenficiary([FromBody] AddBenficiaryRequest request)
        {
            UserLoginResponse response = new UserLoginResponse();

            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (error.HasError)
            {
                response.SetError(error);
                return Ok(response);
            }
            SimpleResponse response1 = new SimpleResponse();

            
            long userid = await _userProvider.AddNewBenficiary(request, this.CallerUser);

            response1.Result = userid;

            return Ok(response1);

        }
        /// <summary>
        /// Add Beneficiary API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AuditApi(EventTypeName = "POST RblPayoutController/GetAllBenficiary", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> GetAllBenficiary([FromBody] ListBenficaryRequest request)
        {
            UserLoginResponse response = new UserLoginResponse();

            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (error.HasError)
            {
                response.SetError(error);
                return Ok(response);
            }
            SimpleResponse response1 = new SimpleResponse();


            response1.Result = await _userProvider.GetAllBenficiary(request, this.CallerUser);

            return Ok(response1);

        }
    }
}
