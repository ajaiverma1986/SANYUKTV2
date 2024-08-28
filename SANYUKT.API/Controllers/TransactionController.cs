using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SANYUKT.API.Common;
using SANYUKT.API.Security;
using SANYUKT.Datamodel.Entities.Transactions;
using SANYUKT.Datamodel.Entities.Users;
using SANYUKT.Datamodel.Masters;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider;
using SANYUKT.Provider.Shared;
using System.IO;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace SANYUKT.API.Controllers
{
    [ResponseCache(Duration = -1, Location = ResponseCacheLocation.None, NoStore = true)]
    [ServiceFilter(typeof(SANYUKTExceptionFilterService))]
    public class TransactionController : BaseApiController
    {
        public readonly TransactionProvider _Provider;
        private AuthenticationHelper _callValidator = null;
        private readonly AuthenticationProvider _authenticationProvider;

        public TransactionController()
        {
            _Provider = new TransactionProvider();
            _callValidator = new AuthenticationHelper();
            _authenticationProvider = new AuthenticationProvider();
        }
        [HttpPost]
        public async Task<IActionResult> NewPayinRequest([FromBody] AddPaymentRequestRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.AddNewPayinRequest(request, CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePayinRequestStatus([FromBody] ApproveRejectPayinRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.ApproveRejectPayinRequest(request, CallerUser);
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> ListPayinRequest([FromBody] ListPayinRequestRequest request)
        {
            ListResponse response = new ListResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _Provider.GetallPayinRequest(request, CallerUser);
            return Json(response);
        }
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
            response = await _Provider.GetPayoutTransactionList(request, CallerUser);
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
        public async Task<IActionResult> UpdatePayinRecieptFile(long RequestId)
        {
            SimpleResponse response = new SimpleResponse();
            string filename = "";
            IFormFile newfile = Request.Form.Files[0];
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            FileManager obj = new FileManager();
            PayinRecieptRequest request1 = new PayinRecieptRequest();
            request1.RequestID = RequestId;
          
            string Fullfilename ="" ;
            Fullfilename = RequestId.ToString()+"_"+this.CallerUser.UserID.ToString();

            filename = obj.SaveOtherDocument(GetStreamBytes(newfile.OpenReadStream()), "Wallet", newfile.FileName, Fullfilename, RequestId.ToString());
            request1.RecieptFile = filename;
           response = await _Provider.UpdatePayinRecieptFile(request1, CallerUser);
            return Json(response);
        }
    }
}
