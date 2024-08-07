using SANYUKT.Connector.Shared;
using SANYUKT.Datamodel.DTO.Request;
using SANYUKT.Datamodel.Entities.Transactions;
using SANYUKT.Datamodel.Entities.Users;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.RblPayoutRequest;
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
        /// <summary>
        /// Direct Payout Transaction 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="SanyuktLoggedInUser"></param>
        /// <returns>SimpleResponse</returns>
        public async Task<SimpleResponse> DirectPay(SinglePaymentRequestFT request, ISANUKTLoggedInUser SanyuktLoggedInUser)
        {
            return (await apiHelper.PostAsync<SimpleResponse>("RblPayout/DirectPay", request, SanyuktLoggedInUser));
        }
        /// <summary>
        ///  Payout Transaction Status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="SanyuktLoggedInUser"></param>
        /// <returns>SimpleResponse</returns>
        public async Task<SimpleResponse> TransactionStatus(SinglePaymentStatus request, ISANUKTLoggedInUser SanyuktLoggedInUser)
        {
            return (await apiHelper.PostAsync<SimpleResponse>("RblPayout/TransactionStatus", request, SanyuktLoggedInUser));
        }
        /// <summary>
        ///  Payout Transaction List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="SanyuktLoggedInUser"></param>
        /// <returns>SimpleResponse</returns>
        public async Task<SimpleResponse> TransactionList(TransactionDetailsPayoutRequest request, ISANUKTLoggedInUser SanyuktLoggedInUser)
        {
            return (await apiHelper.PostAsync<SimpleResponse>("Transaction/TransactionList", request, SanyuktLoggedInUser));
        }
        /// <summary>
        ///  Blalnce Check AI
        /// </summary>
        /// <param name="PartnerID"></param>
        /// <param name="SanyuktLoggedInUser"></param>
        /// <returns>SimpleResponse</returns>
        public async Task<SimpleResponse> GetBalalnce(long PartnerID, ISANUKTLoggedInUser SanyuktLoggedInUser)
        {
            return (await apiHelper.GetAsync<SimpleResponse>("User/CheckBalance?PartnerID=" + PartnerID, SanyuktLoggedInUser));
        }
    }
}
