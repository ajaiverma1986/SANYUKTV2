using Microsoft.AspNetCore.Mvc;
using SANYUKT.API.Common;
using SANYUKT.API.Security;
using SANYUKT.Datamodel.Masters;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider;
using SANYUKT.Provider.Payout;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SANYUKT.API.Controllers
{
    [ResponseCache(Duration = -1, Location = ResponseCacheLocation.None, NoStore = true)]
    [ServiceFilter(typeof(SANYUKTExceptionFilterService))]
    public class MasterDataController : BaseApiController
    {
        public readonly MasterDataProvider _Provider;
        private AuthenticationHelper _callValidator = null;
        private readonly AuthenticationProvider _authenticationProvider;
        public MasterDataController()
        {
            _authenticationProvider = new AuthenticationProvider();
            _Provider = new MasterDataProvider();
            _callValidator = new AuthenticationHelper();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCompanyTypeMaster(int? CompanyTypeId)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetAllCompanyTypeMaster(CompanyTypeId);
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> GenderList()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetGender();
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> MaritalStatusList()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetMaritalStatus();
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> AdressTypeList()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetAdressTypeMaster();
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> AgencyList()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetAgencyTypeMaster();
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> BankList()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetBankList();
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> StateList()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetStateList();
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> DistrictList(int? StateId)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetDistrictList(StateId);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> DistrictMasterList([FromBody]DistrictListRequest request)
        {
            ListResponse response = new ListResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetDistrictMasterList(request);
            return Json(response);
        }

        [HttpGet]
        public async Task<IActionResult> KycTypeList(int? CompanyTypeId, int? UserTypeID)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetAllKycTypeMasterList(CompanyTypeId, UserTypeID);
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> UserTypeList()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetAllUserType();
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> UserTypeAdminList()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetAllUserAdminType();
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> DemographicDataListByPincode(string Pincode)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetDataByPincode(Pincode);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> DemographicDataListByPincodeList([FromBody]PincodeDataRequest request)
        {
            ListResponse response = new ListResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetDataByPincodeList(request);
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> ListLedegrType()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetallLedegrType();
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> ListPlanMaster()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetallPlanMaster();
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> ListServiceType(int? AgencyId)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetallServiceTypeList(AgencyId);
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> ListPaymentChanel()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetAllPaymentChanel();
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> ListPaymentModes(int? PaymentChanelId)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetAllPaymentModes(PaymentChanelId);
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> ListAllService(int? ServiceTypeId)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetAllService(ServiceTypeId);
            return Json(response);
        }
    }

}
