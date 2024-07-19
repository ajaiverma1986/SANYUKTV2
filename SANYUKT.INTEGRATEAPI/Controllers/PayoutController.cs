using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SANYUKT.Connector;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.DTO.Request;
using SANYUKT.Datamodel.DTO.Response;
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
        [Route("Payout/Login")]
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
            UserLoginResponse userLoginResponse = JsonConvert.DeserializeObject<UserLoginResponse>(await _Service.Login(userLoginRequest, this.CurrentLoggedInUser));
            if (userLoginResponse != null && !userLoginResponse.HasError)
            {
                this.CurrentLoggedInUser.UserToken = userLoginResponse.UserToken;
            }
            return userLoginResponse;
        }
    }
}
