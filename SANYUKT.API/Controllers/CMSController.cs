using Audit.WebApi;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using SANYUKT.API.Common;
using SANYUKT.API.Security;
using SANYUKT.Datamodel.CMS;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace SANYUKT.API.Controllers
{
    [EnableCors("AllowAll")]
    [ResponseCache(Duration = -1, Location = ResponseCacheLocation.None, NoStore = true)]
    [ServiceFilter(typeof(SANYUKTExceptionFilterService))]
    public class CMSController : BaseApiController
    {

        private IHostingEnvironment _env;

        private readonly CMSProvider provider = null;
        private AuthenticationHelper _callValidator = null;
        //private readonly AgentProvider _provider = null;
        //private AgentHelper agentHelper = null;
        //private readonly AgentProvider agentprovider = null;

        public CMSController(IHostingEnvironment env)
        {
            _env = env;
            provider = new CMSProvider();
            _callValidator = new AuthenticationHelper();

            //agentHelper = new AgentHelper();
            //agentprovider = new AgentProvider();
            //_provider = new AgentProvider();
        }

        [HttpPost]
        [AuditApi(EventTypeName = "POST CMSController/CreateCmsUser", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> CreateCmsUser([FromBody] AddCMS1 request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (request == null)
            {
                response.SetError("Request Can't be null");
                return Json(response);
            }

            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

           // response = await provider.CreateCmsUser(request, this.CallerUser);

            return Json(response);

        }
        [HttpGet]
        [AuditApi(EventTypeName = "GET CMSController/getStateDetail", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> getStateDetail()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);

            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await provider.getStateDetail(this.CallerUser);

            return Json(response);

        }

        [HttpPost]
        [AuditApi(EventTypeName = "POST CMSController/sendOTP", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> sendOTP([FromBody] MerchantOTPRequest1 request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (request == null)
            {
                response.SetError("Request Can't be null");
                return Json(response);
            }

            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await provider.sendOTP(request, this.CallerUser);

            return Json(response);

        }

        [HttpPost]
        [AuditApi(EventTypeName = "POST CMSController/resendotp", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> resendotp([FromBody] resendOTPReq1 request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (request == null)
            {
                response.SetError("Request Can't be null");
                return Json(response);
            }

            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await provider.resendOTP(request, this.CallerUser);

            return Json(response);

        }

        [HttpPost]
        [AuditApi(EventTypeName = "POST CMSController/validateOTP", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> validateOTP([FromBody] validateOTPReq1 request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (request == null)
            {
                response.SetError("Request Can't be null");
                return Json(response);
            }

            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await provider.validateOTP(request, this.CallerUser);

            return Json(response);

        }

        [HttpPost]
        [AuditApi(EventTypeName = "POST CMSController/BiometricEKYC", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> BiometricEKYC([FromBody] BioEKYCReq1 request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (request == null)
            {
                response.SetError("Request Can't be null");
                return Json(response);
            }

            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await provider.BiometricEKYC(request, this.CallerUser);

            return Json(response);

        }


        [HttpPost]
        [AuditApi(EventTypeName = "POST CMSController/StatusCheck", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> StatusCheck([FromBody] Statuscheck request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (request == null)
            {
                response.SetError("Request Can't be null");
                return Json(response);
            }

            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await provider.StatusCheck(request, this.CallerUser);

            return Json(response);

        }

        [HttpGet]
        [AuditApi(EventTypeName = "GET CMSController/ListEntityUser", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> ListEntityUser(string Usercode)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (Usercode == null)
            {
                response.SetError("Usercode cant be null");
                return Json(response);
            }

            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await provider.ListEntityUser(Usercode, this.CallerUser);

            return Json(response);

        }

        [HttpPost]
        [AuditApi(EventTypeName = "POST CMSController/UpdateCmsUser", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> UpdateCmsUser()
        {
            SimpleResponse response = new SimpleResponse();
            //ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            //if (request == null)
            //{
            //    response.SetError("Request Can't be null");
            //    return Json(response);
            //}

            //if (error.HasError)
            //{
            //    response.SetError(error);
            //    return Json(response);
            //}

            return Json(response);

        }

        [HttpPost]
        [AuditApi(EventTypeName = "POST CMSController/singleSignOnURL", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> singleSignOnURL([FromBody] signonRequest1 request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (request == null)
            {
                response.SetError("Request Can't be null");
                return Json(response);
            }

            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await provider.singleSignOnURL(request, this.CallerUser);

            return Json(response);

        }
        /// <summary>
        /// CMS  API
        /// </summary>
        /// <param name="gIBLPremiumRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [AuditApi(EventTypeName = "POST CMSController/WalletDebit", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> WalletDebit([FromBody] WalletDebitRequest gIBLPremiumRequest)
        {
            CMSDebitResponse response = new CMSDebitResponse();
            //ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);

            //if (error.HasError)
            //{
            //    response.errorMessage = error.ErrorMessage;
            //    return Json(response);
            //}

            if (gIBLPremiumRequest == null)
            {
                response.errorMessage = "Request Can't be null";
                return Json(response);
            }

            response = await provider.WalletDebit(gIBLPremiumRequest);
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
           // await _provider.APIRequestRecord_Log("CMS API Wallet Debit", JsonConvert.SerializeObject(gIBLPremiumRequest, options), JsonConvert.SerializeObject(response, options), "", "");
            return Json(response);
        }

        [HttpGet]
        [AuditApi(EventTypeName = "GET CMSController/TransactionReciept", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> TransactionReciept(string TransactionID)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (TransactionID == null)
            {
                response.SetError("TransactionId cant be null");
                return Json(response);
            }

            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response = await provider.TransactionReciept(TransactionID);

            return Json(response);

        }
        [HttpPost]
        [AuditApi(EventTypeName = "POST CMSController/TransactionReport", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> TransactionReport([FromBody] TransactionReportRequest Request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);

            if (Request == null)
            {
                response.SetError("Request cant be null");
                return Json(response);
            }
            if (Request.Usercode == "")
            {
                response.SetError("Agent cant be null");
                return Json(response);
            }

            response = await provider.TransactionReport(Request);

            return Json(response);

        }

        [HttpGet]
        public async Task<IActionResult> CompanyTypes()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);

            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

           // response = await provider.getCompanyTypes(CallerUser);
            return Json(response);
        }

        [HttpPost]
        [AuditApi(EventTypeName = "POST CMSController/CashDropStatusCheck", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = false, IncludeModelState = false)]
        public async Task<IActionResult> CheckCashDropStatus([FromBody] CheckCashDropStatusRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(this.CallerUser, false);
            if (request == null)
            {
                response.SetError("Request Can't be null");
                return Json(response);
            }

            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

           // response = await provider.CheckCashDropStatus(request, this.CallerUser);

            return Json(response);

        }
    }
}
