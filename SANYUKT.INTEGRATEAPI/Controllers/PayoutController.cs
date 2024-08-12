using Audit.WebApi;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SANYUKT.Commonlib.Utility;
using SANYUKT.Connector;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.DTO.Request;
using SANYUKT.Datamodel.DTO.Response;
using SANYUKT.Datamodel.Entities.Transactions;
using SANYUKT.Datamodel.Entities.Users;
using SANYUKT.Datamodel.RblPayoutRequest;
using SANYUKT.Datamodel.Shared;
using SANYUKT.INTEGRATEAPI.Shared;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SANYUKT.INTEGRATEAPI.Controllers
{
    /// <summary>
    /// Authentication and Authorization controller
    /// </summary>
    [EnableCors("AllowAll")]
    [ResponseCache(Duration = -1, Location = ResponseCacheLocation.None, NoStore = true)]
    [ServiceFilter(typeof(SANYUKTExceptionFilterService))]
    public class PayoutController:BaseController
    {
        private readonly Payoutservice _Service = null;
        public PayoutController()
        {
            _Service = new Payoutservice();
           
        }
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userLoginRequest"></param>
        /// <returns></returns>
        [Route("Payout/GenerateToken")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
        {
            UserLoginResponse userLoginResponse = null;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (!claimsIdentity.IsAuthenticated)
            {
                userLoginResponse = await ValidateUser(userLoginRequest);
            }
            else
            {
                userLoginResponse = new UserLoginResponse();
                userLoginResponse.UserToken = claimsIdentity.Claims.Where(c => c.Type == SANYUKTClaimTypes.UserToken).Select(c => c.Value).SingleOrDefault();
            }
            return Ok(userLoginResponse);
        }
        private async Task<UserLoginResponse> ValidateUser(UserLoginRequest userLoginRequest)
        {
            //UserLoginResponse userLoginResponse = null;
            UserLoginResponse userLoginResponse = JsonConvert.DeserializeObject<UserLoginResponse>(await _Service.Login(userLoginRequest, this.CurrentLoggedInUser));
            if (userLoginResponse != null && !userLoginResponse.HasError)
            {
                this.CurrentLoggedInUser.UserToken = userLoginResponse.UserToken;
            }
            return userLoginResponse;
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
            SimpleResponse response = (await _Service.AddBenficiary(request, this.CurrentLoggedInUser));

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
            SimpleResponse response = (await _Service.ListBenficiary(request, this.CurrentLoggedInUser));

            if (response == null)
            {
                response = new SimpleResponse();
                response.SetError(ErrorCodes.SERVER_ERROR);
                return Json(response);
            }

            return Json(response);
        }
        /// <summary>
        /// Direct Payout Transaction
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        [Route("Payout/Transaction")]
        [HttpPost]
        public async Task<IActionResult> DirectPay([FromBody] SinglePaymentRequestFT request)
        {
            SimpleResponse response = (await _Service.DirectPay(request, this.CurrentLoggedInUser));

            if (response == null)
            {
                response = new SimpleResponse();
                response.SetError(ErrorCodes.SERVER_ERROR);
                return Json(response);
            }

            return Json(response);
        }
        /// <summary>
        /// Direct Payout Transaction Status
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        [Route("Payout/TransactionStatus")]
        [HttpPost]
        public async Task<IActionResult> TransactionStatus([FromBody] SinglePaymentStatus request)
        {
            SimpleResponse response = (await _Service.TransactionStatus(request, this.CurrentLoggedInUser));

            if (response == null)
            {
                response = new SimpleResponse();
                response.SetError(ErrorCodes.SERVER_ERROR);
                return Json(response);
            }

            return Json(response);
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
            response = (await _Service.TransactionList(request, this.CurrentLoggedInUser)).Deserialize<SimpleResponse>();

            if (response == null)
            {
                response = new SimpleResponse();
                response.SetError(ErrorCodes.SERVER_ERROR);
                return Json(response);
            }

            return Json(response);
        }
        /// <summary>
        /// Balance Check API
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        [Route("Payout/GetBalalnce")]
        [HttpGet]
        public async Task<IActionResult> GetBalalnce(long PartnerId)
        {
            SimpleResponse response = new SimpleResponse();
            response = (await _Service.GetBalalnce(PartnerId, this.CurrentLoggedInUser)).Deserialize<SimpleResponse>();
           

            if (response == null)
            {
                response = new SimpleResponse();
                response.SetError(ErrorCodes.SERVER_ERROR);
                return Json(response);
            }

            return Json(response);
        }
    }
}
