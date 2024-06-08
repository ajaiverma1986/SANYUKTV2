using Newtonsoft.Json;
using SANYUKT.Commonlib.Cache;
using SANYUKT.Configuration;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.Entities.Authorization;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.RblPayoutRequest;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SANYUKT.Provider.Payout
{
    public class RblPayoutProvider:BaseProvider
    {
        public async Task<SimpleResponse> GetBalalce(AccountBalalnceRequest requestreq, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response=new SimpleResponse ();
            var _clientHandler = new HttpClientHandler();
            _clientHandler.ClientCertificates.Add(Certificatetext);
            _clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var client = new HttpClient(_clientHandler);
            string Url=SANYUKTApplicationConfiguration.Instance.RblPayoutBaseUrl.ToString();
            string fullurl = Url + "test/sb/rbl/v1/accounts/balance/query?client_id=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientId.ToString() + "&client_secret=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientSecrat.ToString();
            var request = new HttpRequestMessage(HttpMethod.Post, fullurl);
            string username = SANYUKTApplicationConfiguration.Instance.RblPayoutusername.ToString(); 
            string pass = SANYUKTApplicationConfiguration.Instance.RblPayoutPass.ToString();
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

        public async Task<SimpleResponse> PayoutTransaction(PaymentRequest requestreq, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            var _clientHandler = new HttpClientHandler();
            _clientHandler.ClientCertificates.Add(Certificatetext);
            _clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var client = new HttpClient(_clientHandler);
            string Url = SANYUKTApplicationConfiguration.Instance.RblPayoutBaseUrl.ToString();
            string fullurl = Url + "test/sb/rbl/v1/payments/corp/payment?client_id=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientId.ToString() + "&client_secret=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientSecrat.ToString();
            var request = new HttpRequestMessage(HttpMethod.Post, fullurl);
            string username = SANYUKTApplicationConfiguration.Instance.RblPayoutusername.ToString();
            string pass = SANYUKTApplicationConfiguration.Instance.RblPayoutPass.ToString();
            var authenticationString = $"{username}:{pass}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
            request.Headers.Add("Authorization", $"Basic {base64EncodedAuthenticationString}");


            //make the request
            string jsonstr = JsonConvert.SerializeObject(requestreq, Formatting.Indented);
            var content = new StringContent(jsonstr);
            request.Content = content;
            var response1 = await client.SendAsync(request);
            response.Result = await response1.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<SimpleResponse> PayoutTransactionStatus(PaymentStatusRequest requestreq, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            var _clientHandler = new HttpClientHandler();
            _clientHandler.ClientCertificates.Add(Certificatetext);
            _clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var client = new HttpClient(_clientHandler);
            string Url = SANYUKTApplicationConfiguration.Instance.RblPayoutBaseUrl.ToString();
            string fullurl = Url + "test/sb/rbl/v1/payments/corp/payment/query?client_id=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientId.ToString() + "&client_secret=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientSecrat.ToString();
            var request = new HttpRequestMessage(HttpMethod.Post, fullurl);
            string username = SANYUKTApplicationConfiguration.Instance.RblPayoutusername.ToString();
            string pass = SANYUKTApplicationConfiguration.Instance.RblPayoutPass.ToString();
            var authenticationString = $"{username}:{pass}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
            request.Headers.Add("Authorization", $"Basic {base64EncodedAuthenticationString}");


            //make the request
            string jsonstr = JsonConvert.SerializeObject(requestreq, Formatting.Indented);
            var content = new StringContent(jsonstr);
            request.Content = content;
            var response1 = await client.SendAsync(request);
            response.Result = await response1.Content.ReadAsStringAsync();

            return response;
        }
    }
    
}
