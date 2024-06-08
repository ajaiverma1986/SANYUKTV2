using Audit.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SANYUKT.API.Common;
using SANYUKT.API.Security;
using SANYUKT.Configuration;
using SANYUKT.Datamodel.DTO.Request;
using SANYUKT.Datamodel.DTO.Response;
using SANYUKT.Datamodel.RblPayoutRequest;
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
        private IHostingEnvironment _env;
        public RblPayoutController(IHostingEnvironment env)
        {
            _env= env;
            _Provider = new RblPayoutProvider();
        }

        /// <summary>
        /// Get Balalnce API
        /// </summary>
        /// <param name="request"></param>
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
            X509Certificate2 certificate2 = new X509Certificate2(System.IO.Path.Combine(_env.WebRootPath.ToString ()+ "/SSlCertificate", SANYUKTApplicationConfiguration.Instance.certisslName.ToString ()), SANYUKTApplicationConfiguration.Instance.certisslpass.ToString ());
            response1 = await _Provider.GetBalalce(request, certificate2, this.CallerUser);

            return Ok(response1);

        }
        /// <summary>
        /// Payout transaction API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AuditApi(EventTypeName = "POST RblPayoutController/PayoutTransaction", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> Transaction([FromBody] PaymentRequest request)
        {
            //    UserLoginResponse response = new UserLoginResponse();

            //    ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            //    if (error.HasError)
            //    {
            //        response.SetError(error);
            //        return Ok(response);
            //    }
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
        [AuditApi(EventTypeName = "POST RblPayoutController/PayoutTransaction", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> TransactionStatus([FromBody] PaymentStatusRequest request)
        {
            //    UserLoginResponse response = new UserLoginResponse();

            //    ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            //    if (error.HasError)
            //    {
            //        response.SetError(error);
            //        return Ok(response);
            //    }
            SimpleResponse response1 = new SimpleResponse();
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
        [AuditApi(EventTypeName = "POST RblPayoutController/PayoutTransaction", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> GetAccountStatement([FromBody] AccountstatementRequest request)
        {
            //    UserLoginResponse response = new UserLoginResponse();

            //    ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            //    if (error.HasError)
            //    {
            //        response.SetError(error);
            //        return Ok(response);
            //    }
            SimpleResponse response1 = new SimpleResponse();
            X509Certificate2 certificate2 = new X509Certificate2(System.IO.Path.Combine(_env.WebRootPath.ToString() + "/SSlCertificate", SANYUKTApplicationConfiguration.Instance.certisslName.ToString()), SANYUKTApplicationConfiguration.Instance.certisslpass.ToString());
            response1 = await _Provider.AccountStatement(request, certificate2, this.CallerUser);

            return Ok(response1);

        }
    }
}
