using Newtonsoft.Json;
using SANYUKT.Commonlib.Cache;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.Entities.Authorization;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.RblPayoutRequest;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SANYUKT.Provider.Payout
{
    public class RblPayoutProvider:BaseProvider
    {
        public async Task<SimpleResponse> GetBalalce(AccountBalalnceRequest requestreq, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response=new SimpleResponse ();
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://apideveloper.rblbank.com/test/sb/rbl/v1/accounts/balance/query?client_id=5b6aa5e8e2ca77ba9af0c19305663f98&client_secret=c1ae9fe7e68451ee53e25fa2e4bb4c69");
            string username = "FINDOORPAY";
            string pass = "Findoor@12@#";
            var authenticationString = $"{username}:{pass}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
            request.Headers.Add("Authorization", $"Basic {base64EncodedAuthenticationString}");

            //make the request
            string jsonstr= JsonConvert.SerializeObject(requestreq, Formatting.Indented);
            var content =new  StringContent(jsonstr);
            request.Content = content;
            var response1 = await client.SendAsync(request);
           response.Result= await response1.Content.ReadAsStringAsync();

            return response;
        }
    }
    
}
