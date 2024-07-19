using SANYUKT.Connector.Shared;
using SANYUKT.Datamodel.DTO.Request;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.Shared;
using System;
using System.Threading.Tasks;

namespace SANYUKT.Connector
{
    public class Payoutservice:BaseService
    {
       
        public async Task<string> Login(Object userLoginRequest, ISANUKTLoggedInUser loggedInUser)
        {
            return await apiHelper.PostAsync("AA/Login", userLoginRequest, loggedInUser);
        }

        //public async Task<SimpleResponse> User_GetByID(long UserID, IFIALoggedInUser FIALoggedInUser)
        //{
        //    return (await apiHelper.GetAsync<SimpleResponse>("User/User_GetByID?UserID=" + UserID, FIALoggedInUser));
        //}
    }
}
