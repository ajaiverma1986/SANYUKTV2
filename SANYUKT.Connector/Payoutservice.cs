using SANYUKT.Connector.Shared;
using SANYUKT.Datamodel.DTO.Request;
using SANYUKT.Datamodel.Entities.Users;
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

       
        /// <summary>
        /// Add beficiary  Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="SanyuktLoggedInUser"></param>
        /// <returns>SimpleResponse</returns>
        public async Task<SimpleResponse> AddBenficiary(AddBenficiaryRequest request, ISANUKTLoggedInUser SanyuktLoggedInUser)
        {
            return (await apiHelper.PostAsync<SimpleResponse>("RblPayout/AddBenficiary", request, SanyuktLoggedInUser));
        }

        /// <summary>
        /// List beficiary 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="SanyuktLoggedInUser"></param>
        /// <returns>SimpleResponse</returns>
        public async Task<SimpleResponse> ListBenficiary(ListBenficaryRequest request, ISANUKTLoggedInUser SanyuktLoggedInUser)
        {
            return (await apiHelper.PostAsync<SimpleResponse>("RblPayout/GetAllBenficiary", request, SanyuktLoggedInUser));
        }
    }
}
