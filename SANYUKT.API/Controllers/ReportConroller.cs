using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SANYUKT.API.Common;
using SANYUKT.API.Security;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider;
using System.Threading.Tasks;

namespace SANYUKT.API.Controllers
{
    [EnableCors("AllowAll")]
    [ResponseCache(Duration = -1, Location = ResponseCacheLocation.None, NoStore = true)]
    [ServiceFilter(typeof(SANYUKTExceptionFilterService))]
    public class ReportConroller:BaseApiController
    {
        public readonly ReportProvider _provider = null;
        private AuthenticationHelper _callValidator = null;
        public ReportConroller()
        {
            _provider = new ReportProvider();
            _callValidator = new AuthenticationHelper();
        }
        [HttpGet]
        public async Task<IActionResult> GetTransactionSummaryByUserId(int UserID)
        {
            SimpleResponse response = new SimpleResponse();
            ErrorResponse error = await _callValidator.AuthenticateAndAuthorize(CallerUser, true);
            if (error.HasError)
            {
                response.SetError(error);
                return Json(response);
            }
            response = await _provider.GetTransactionSummaryByUserId(UserID,CallerUser);
            return Json(response);
        }
    }
}
