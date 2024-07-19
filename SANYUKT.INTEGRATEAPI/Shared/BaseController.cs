using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.Common;

namespace SANYUKT.INTEGRATEAPI.Shared
{
    public class BaseController:Controller
    {
        public SANYUKTLoggedInUser CurrentLoggedInUser
        {
            get
            {
                return this.HttpContext.RequestServices.GetService(typeof(ISANUKTLoggedInUser)) as SANYUKTLoggedInUser;
            }
        }

        protected void LogOutUser()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
