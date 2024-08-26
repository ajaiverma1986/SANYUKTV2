using Audit.WebApi;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using SANYUKT.API.Common;
using SANYUKT.API.Security;
using SANYUKT.Configuration;
using SANYUKT.Datamodel.DTO.Response;
using SANYUKT.Datamodel.Entities.Users;
using SANYUKT.Datamodel.RblPayoutRequest;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider;
using SANYUKT.Provider.Shared;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SANYUKT.API.Controllers
{
    [EnableCors("AllowAll")]
    [ResponseCache(Duration = -1, Location = ResponseCacheLocation.None, NoStore = true)]
    [ServiceFilter(typeof(SANYUKTExceptionFilterService))]
    public class UserController : BaseApiController
    {
        public readonly UserDetailsProvider _Provider;
        private AuthenticationHelper _callValidator = null;
        private readonly AuthenticationProvider _authenticationProvider;
        public UserController() {
            _authenticationProvider = new AuthenticationProvider();
            _Provider = new UserDetailsProvider();
            _callValidator = new AuthenticationHelper();
        }

        /// <summary>
        /// User Creation
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        //[AuditApi(EventTypeName = "POST UserController/CreateNewUser", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = true, IncludeModelState = true)]
        public async Task<IActionResult> CreateNewUser([FromBody] CreateUserWithlogoRequest request)
        {
            string filename = "";
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }

            response.Result = await _Provider.CreateNewUserRequest(request, this.CallerUser);
            return Json(response);
        }
        /// <summary>
        /// Upload Logo
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        
        [HttpPost]
       
        //[AuditApi(EventTypeName = "POST UserController/CreateNewUser", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = true, IncludeModelState = true)]
        public async Task<IActionResult> UploadUserLogo()
        {
            string filename = "";
          IFormFile newfile=  Request.Form.Files[0];
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }


            FileManager obj = new FileManager();
            UploadOrgLogo logore=new UploadOrgLogo ();
            
            filename = obj.SaveFile(GetStreamBytes(newfile.OpenReadStream()), CallerUser.UserID.ToString(), newfile.FileName);
            logore.FileName = filename;
            logore .UserId = CallerUser.UserID;
            logore.FileBytes = GetStreamBytes(newfile.OpenReadStream());



            response.Result = await _Provider.UpdateUserOrgLogo(logore, filename, this.CallerUser);
            return Json(response);
        }
        public byte[] GetStreamBytes(Stream inputStream)
        {
            using (inputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                return memoryStream.ToArray();
            }
        }

        [HttpPost]
        //[AuditApi(EventTypeName = "POST UserController/AddOriginatorAccounts", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = true, IncludeModelState = true)]
        public async Task<IActionResult> AddOriginatorAccounts([FromBody] CreateOriginatorAccountRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response.Result = await _Provider.AddOriginatorAccounts(request, this.CallerUser);
            return Json(response);
        }
        [HttpGet]
       // [AuditApi(EventTypeName = "POST UserController/ListOriginatorAccounts", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = true, IncludeModelState = true)]
        public async Task<IActionResult> ListOriginatorAccounts()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetallOriginatorsAccount(CallerUser);
            return Json(response);
        }
        [HttpPost]
       // [AuditApi(EventTypeName = "POST UserController/AddUserAddress", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = true, IncludeModelState = true)]
        public async Task<IActionResult> AddUserAddress([FromBody] CreateUserDetailAddressRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response.Result = await _Provider.AddUserAddress(request, this.CallerUser);
            return Json(response);
        }
        [HttpGet]
       // [AuditApi(EventTypeName = "POST UserController/ListUserAddresses", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = true, IncludeModelState = true)]
        public async Task<IActionResult> ListUserAddresses()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetAllUserAddress(CallerUser);
            return Json(response);
        }
        [HttpPost]
        //[AuditApi(EventTypeName = "POST UserController/AddUserDeatilKYC", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = true, IncludeModelState = true)]
        public async Task<IActionResult> AddUserDeatilKYC([FromBody] CreateUserDetailKyc1 request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }


            FileManager obj = new FileManager();
            string filename = obj.SaveFile(request.FileBytes, request.DocumentNo, request.FileName);
            response.Result = await _Provider.AddUserDeatilKYC(request, filename, this.CallerUser);
            return Json(response);
        }
        [HttpGet]
        //[AuditApi(EventTypeName = "POST UserController/ListUserKYC", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = true, IncludeModelState = true)]
        public async Task<IActionResult> ListUserKYC()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetAllUserKyc(CallerUser);
            return Json(response);
        }
        [HttpGet]
        // [AuditApi(EventTypeName = "POST UserController/ListUserAddresses", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = true, IncludeModelState = true)]
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
        [HttpPost]
        // [AuditApi(EventTypeName = "POST UserController/CreateOrgAPIPartner", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = true, IncludeModelState = true)]
        public async Task<IActionResult> CreateOrgAPIPartner([FromBody] CreateNewPartnerRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            if (request == null)
            {
                response.SetError(ErrorCodes.INVALID_PARAMETERS);
                return Json(response);
            }
            if (request.Password.ToString()=="")
            {
                response.SetError(ErrorCodes.SP_135);
                return Json(response);
            }
            if (request.EmailId.ToString() == "")
            {
                response.SetError(ErrorCodes.SP_135);
                return Json(response);
            }
            response.Result = await _Provider.CreateOrgAPIPartner(request, this.CallerUser);
            return Json(response);
        }
        [HttpPost]
        // [AuditApi(EventTypeName = "POST UserController/CreateOrgAPIPartner", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = true, IncludeModelState = true)]
        public async Task<IActionResult> CreateNewAPIUser([FromBody] CreateNewUserRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            if (request == null)
            {
                response.SetError(ErrorCodes.INVALID_PARAMETERS);
                return Json(response);
            }
            if (request.Password.ToString() == "")
            {
                response.SetError(ErrorCodes.SP_135);
                return Json(response);
            }
           
            response.Result = await _Provider.CreateNewUser(request, this.CallerUser);
            return Json(response);
        }
        [HttpGet]
        // [AuditApi(EventTypeName = "POST UserController/ListUserAddresses", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = true, IncludeModelState = true)]
        public async Task<IActionResult> GetAllapplication()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response.Result = await _Provider.Getallapplication(CallerUser);
            return Json(response);
        }
        [HttpGet]
        // [AuditApi(EventTypeName = "POST UserController/ListUserAddresses", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = true, IncludeModelState = true)]
        public async Task<IActionResult> GetallUserByOrg()
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response.Result = await _Provider.GetallUserByOrg(CallerUser);
            return Json(response);
        }
        public async Task<IActionResult> UploadUserKYC(int kycTypeId,string DocumentNo)
        {
            string filename = "";
            IFormFile newfile = Request.Form.Files[0];
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            FileManager obj = new FileManager();
            UploadUserKYCFileRequest request1 = new UploadUserKYCFileRequest();
            request1.DocumentNo = DocumentNo;
            request1.KycID = kycTypeId;
            string Fullfilename = "";
            if(DocumentNo!="")
            {
                Fullfilename = CallerUser.UserID.ToString() + "_" + kycTypeId.ToString() + "_" + DocumentNo;
            }
            else if(DocumentNo!=null)
            {
                Fullfilename = CallerUser.UserID.ToString() + "_" + kycTypeId.ToString() + "_" + DocumentNo;
            }
            else
            {
                Fullfilename = CallerUser.UserID.ToString() + "_" + kycTypeId.ToString() ;
            }
             
            filename = obj.SaveKYCDocument(GetStreamBytes(newfile.OpenReadStream()), CallerUser.UserID.ToString(), newfile.FileName, Fullfilename);
            response.Result = await _Provider.UploadUserKYC(request1, filename, this.CallerUser);
            return Json(response);
        }
        [HttpGet]
        //[AuditApi(EventTypeName = "POST UserController/ListUserKYC", IncludeHeaders = true, IncludeResponseBody = true, IncludeRequestBody = true, IncludeModelState = true)]
        public async Task<IActionResult> ListUserKYCById(long KycId)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetAllUserKycById(KycId,CallerUser);
            return Json(response);
        }
    }
}
