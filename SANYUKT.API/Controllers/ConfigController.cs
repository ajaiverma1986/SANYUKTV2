using Microsoft.AspNetCore.Mvc;
using SANYUKT.API.Common;
using SANYUKT.API.Security;
using SANYUKT.Datamodel.Masters;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider;
using System.Threading.Tasks;

namespace SANYUKT.API.Controllers
{
    [ResponseCache(Duration = -1, Location = ResponseCacheLocation.None, NoStore = true)]
    [ServiceFilter(typeof(SANYUKTExceptionFilterService))]
    public class ConfigController : BaseApiController
    {
        
        public readonly ConfigProvider _Provider;
        private AuthenticationHelper _callValidator = null;
        private readonly AuthenticationProvider _authenticationProvider;
        public ConfigController()
        {
            _authenticationProvider = new AuthenticationProvider();
            _Provider = new ConfigProvider();
            _callValidator = new AuthenticationHelper();
        }
        [HttpGet]
        public async Task<IActionResult> ListCalCulationType()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetallCalculationTypeMaster();
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> ListChargeDedcutionType()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetAllChargeDeductionType();
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> ListPlanMaster(int? PlanId)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetallPlanList(PlanId);
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetallSlabType()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetallSlabType();
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> ListCommissionDistribution([FromBody] CommissionDistributionRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetallCommissionDistribution(request);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> ListTopupCharge([FromBody] TopupChargeRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetAllTopupCharge(request);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> ListTransactionSlab([FromBody] TransactionslabRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetallTransactionSlab(request);
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> ListPaymentAccounts(int? BankId)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetAllPaymentAccounts(BankId);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> AddPaymentAccounts([FromBody] AddPaymentAccountMasterRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.AddPaymentAccounts(request, CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewApplication([FromBody] CreateapplicationRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.CreateNewApplication(request, CallerUser);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> changesPaymentAccountsStatus([FromBody] ChangePaymentAccStatusRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.changesPaymentAccountsStatus(request, CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetServicePolicy(GetServicePolicyRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetServicePolicy(request);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> AddTransacttionSlab([FromBody] AddPaymentAccountMasterRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.AddPaymentAccounts(request, CallerUser);
            return Json(response);
        }
    }
}
