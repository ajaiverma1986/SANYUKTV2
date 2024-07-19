using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SANYUKT.Logging;
using Newtonsoft.Json;
using SANYUKT.Datamodel.Interfaces;

namespace SANYUKT.Connector.Shared
{
    /// <summary>
    /// API Connector class to interact with APIs, to call any method of the APIs this class should be called 
    /// </summary>
    public class APIConnector
    {
        #region Declaration
        public static HttpClient client = new HttpClient();
        private readonly LoggingService _Log;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        #region Constructor
        public APIConnector()
        {
            this._Log = new LoggingService();
        }
        #endregion

        /// <summary>
        /// Base URL property, to set the base URL of API
        /// </summary>
        #region BaseUrl
        private static string _BaseUrl = "";
        public string BaseUrl
        {
            get
            {
                return _BaseUrl;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;
                if (client.BaseAddress != null)
                    return;
                _BaseUrl = value;

                client.BaseAddress = new Uri(value);
            }
        }
        #endregion

        /// <summary>
        /// Log Method for logging request and 
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        #region Log Method
        private async Task Log(string Message)
        {
            //Uncomment below line to stop logging Requests,
            return;
            //await _Log.LogMessage(Message, null);
        }
        #endregion

        /// <summary>
        /// Get Async method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="queryStringParams"></param>
        /// <param name="fiaUser"></param>
        /// <returns></returns>
        #region Get Async Method
        public async Task<T> GetAsync<T>(string apiPath, string queryStringParams, ISANUKTLoggedInUser fiaUser)
        {
            T apiResult = default(T);

            if (!string.IsNullOrEmpty(queryStringParams))
            {
                if (apiPath.Contains("?"))
                    apiPath = apiPath + "&" + queryStringParams;
                else if (queryStringParams.Contains("?"))
                    apiPath = apiPath + queryStringParams;
                else
                    apiPath = apiPath + "?" + queryStringParams;
            }

            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, apiPath))
            {
                msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.ApiToken))
                    msg.Headers.Add("ApiToken", fiaUser.ApiToken);
                else
                {
                    //TODO://
                    //msg.Headers.Add("ApiToken", AppSettings.APIToken);
                }

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.UserToken))
                    msg.Headers.Add("UserToken", fiaUser.UserToken);

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.IPAddress))
                    msg.Headers.Add("ClientIPAddress", fiaUser.IPAddress);

                await Log("Sending to API - " + apiPath);
                using (HttpResponseMessage httpResponse = await client.SendAsync(msg))
                {
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        string result = httpResponse.Content.ReadAsStringAsync().Result;
                        apiResult = JsonConvert.DeserializeObject<T>(result);
                    }
                }
                await Log("Sending to API complete - " + apiPath + " - " + apiResult);
            }

            return apiResult;
        }
        #endregion

        /// <summary>
        /// Get Async method
        /// </summary>
        /// <param name="apiPath"></param>
        /// <param name="queryStringParams"></param>
        /// <param name="fiaUser"></param>
        /// <returns></returns>
        #region Get Async method
        public async Task<string> GetAsync(string apiPath, string queryStringParams, ISANUKTLoggedInUser fiaUser)
        {
            string apiResult = string.Empty;

            if (!string.IsNullOrEmpty(queryStringParams))
            {
                if (apiPath.Contains("?"))
                    apiPath = apiPath + "&" + queryStringParams;
                else if (queryStringParams.Contains("?"))
                    apiPath = apiPath + queryStringParams;
                else
                    apiPath = apiPath + "?" + queryStringParams;
            }

            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, apiPath))
            {
                msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.ApiToken))
                    msg.Headers.Add("ApiToken", fiaUser.ApiToken);
                else
                {
                    //TODO:
                    ///msg.Headers.Add("ApiToken", AppSettings.APIToken);
                }

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.UserToken))
                    msg.Headers.Add("UserToken", fiaUser.UserToken);

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.IPAddress))
                    msg.Headers.Add("ClientIPAddress", fiaUser.IPAddress);

                await Log("Sending to API - " + apiPath);
                using (HttpResponseMessage httpResponse = await client.SendAsync(msg))
                {
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        apiResult = httpResponse.Content.ReadAsStringAsync().Result;
                    }
                }
                await Log("Sending to API complete - " + apiPath + " - " + apiResult);
            }
            return apiResult;
        }
        #endregion

        /// <summary>
        /// Get Async Bytes
        /// </summary>
        /// <param name="apiPath"></param>
        /// <param name="queryStringParams"></param>
        /// <param name="fiaUser"></param>
        /// <returns></returns>
        #region Get Async Bytes
        public async Task<byte[]> GetAsyncBytes(string apiPath, string queryStringParams, ISANUKTLoggedInUser fiaUser)
        {
            byte[] apiResult = null;

            if (!string.IsNullOrEmpty(queryStringParams))
            {
                if (apiPath.Contains("?"))
                    apiPath = apiPath + "&" + queryStringParams;
                else if (queryStringParams.Contains("?"))
                    apiPath = apiPath + queryStringParams;
                else
                    apiPath = apiPath + "?" + queryStringParams;
            }

            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, apiPath))
            {
                msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.ApiToken))
                    msg.Headers.Add("ApiToken", fiaUser.ApiToken);
                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.UserToken))
                    msg.Headers.Add("UserToken", fiaUser.UserToken);

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.IPAddress))
                    msg.Headers.Add("ClientIPAddress", fiaUser.IPAddress);

                await Log("Sending to API - " + apiPath);
                using (HttpResponseMessage httpResponse = await client.SendAsync(msg))
                {
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        apiResult = httpResponse.Content.ReadAsByteArrayAsync().Result;
                    }
                    else
                        apiResult = new byte[0];
                }
                await Log("Sending to API complete - " + apiPath);
            }
            return apiResult;
        }
        #endregion

        /// <summary>
        /// Get Async Method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="fiaUser"></param>
        /// <returns></returns>
        #region Get Async Method
        public async Task<T> GetAsync<T>(string apiPath, ISANUKTLoggedInUser fiaUser)
        {
            return await GetAsync<T>(apiPath, string.Empty, fiaUser);
        }
        #endregion

        /// <summary>
        /// HTTP GET API Helper without user token requirement
        /// specifically created for the static data pull methods
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="drOmniUser"></param>
        /// <returns></returns>
        #region Get Async Method
        public async Task<string> GetAsync(string apiPath, ISANUKTLoggedInUser fiaUser)
        {
            return await GetAsync(apiPath, string.Empty, fiaUser);
        }
        #endregion

        /// <summary>
        /// Post Async Method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="requestData"></param>
        /// <param name="fiaUser"></param>
        /// <returns></returns>
        #region Post Async Method
        public async Task<T> PostAsync<T>(string apiPath, object requestData, ISANUKTLoggedInUser fiaUser)
        {
            T apiResult = default(T);

            string jsonStr = (requestData != null ? JsonConvert.SerializeObject(requestData) : "");


            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, apiPath))
            {
                msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.ApiToken))
                    msg.Headers.Add("ApiToken", fiaUser.ApiToken);
                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.UserToken))
                    msg.Headers.Add("UserToken", fiaUser.UserToken);

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.IPAddress))
                    msg.Headers.Add("ClientIPAddress", fiaUser.IPAddress);

                HttpContent content = new StringContent(jsonStr);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                msg.Content = content;

                string result = "";

                await Log("Sending to API Post - " + apiPath + " - payload = " + jsonStr);
                using (HttpResponseMessage httpResponse = await client.SendAsync(msg))
                {

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        result = httpResponse.Content.ReadAsStringAsync().Result;
                        apiResult = JsonConvert.DeserializeObject<T>(result);
                    }
                }
                await Log("Sending to API complete with result - " + apiPath + " - " + result);
            }

            return apiResult;
        }
        #endregion

        /// <summary>
        /// Post Async Method For Form Data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="requestData"></param>
        /// <param name="fiaUser"></param>
        /// <returns></returns>
        #region Post Async Method For Form Data
        public async Task<T> PostAsyncFormData<T>(string apiPath, string requestData, ISANUKTLoggedInUser fiaUser)
        {
            T apiResult = default(T);

            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, apiPath))
            {
                msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.ApiToken))
                    msg.Headers.Add("ApiToken", fiaUser.ApiToken);
                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.UserToken))
                    msg.Headers.Add("UserToken", fiaUser.UserToken);

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.IPAddress))
                    msg.Headers.Add("ClientIPAddress", fiaUser.IPAddress);

                HttpContent content = new StringContent(requestData);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                msg.Content = content;
                string result = string.Empty;
                await Log("Sending to API Post - " + apiPath + " - payload = " + requestData);
                using (HttpResponseMessage httpResponse = await client.SendAsync(msg))
                {
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        result = httpResponse.Content.ReadAsStringAsync().Result;
                        apiResult = JsonConvert.DeserializeObject<T>(result);
                    }
                }
                await Log("Sending to API complete with result - " + apiPath + " - " + result);
            }
            return apiResult;
        }
        #endregion

        /// <summary>
        /// Post Async Method
        /// </summary>
        /// <param name="apiPath"></param>
        /// <param name="requestData"></param>
        /// <param name="fiaUser"></param>
        /// <returns></returns>
        #region Post Async Method
        public async Task<string> PostAsync(string apiPath, object requestData, ISANUKTLoggedInUser fiaUser)
        {
            string apiResult = string.Empty;
            string jsonStr = JsonConvert.SerializeObject(requestData);

            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, apiPath))
            {
                msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.ApiToken))
                    msg.Headers.Add("ApiToken", fiaUser.ApiToken);
                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.UserToken))
                    msg.Headers.Add("UserToken", fiaUser.UserToken);

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.IPAddress))
                    msg.Headers.Add("ClientIPAddress", fiaUser.IPAddress);

                HttpContent content = new StringContent(jsonStr);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                msg.Content = content;

                string httpStr = "";

                await Log("Sending to API Post - " + apiPath + " - payload = " + jsonStr);
                using (HttpResponseMessage httpResponse = await client.SendAsync(msg))
                {
                    httpStr = httpResponse.IsSuccessStatusCode.ToString();
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        apiResult = await httpResponse.Content.ReadAsStringAsync();
                    }
                }
                await Log("Sending to API complete with result - " + apiPath + " - " + apiResult);
            }

            return apiResult;
        }
        #endregion

        /// <summary>
        /// Post Async Method 
        /// This method only used for sending extra header values with PostAsyn method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="requestData"></param>
        /// <param name="headerValues"></param>
        /// <param name="fiaUser"></param>
        /// <returns></returns>
        #region Post Async Method
        public async Task<string> PostAsync<T>(string apiPath, object requestData, List<KeyValuePair<string, string>> headerValues, ISANUKTLoggedInUser fiaUser)
        {
            string apiResult = string.Empty;
            string jsonStr = JsonConvert.SerializeObject(requestData);

            //client.BaseAddress = new Uri(BaseUrl);

            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, apiPath))
            {
                msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.ApiToken))
                    msg.Headers.Add("ApiToken", fiaUser.ApiToken);
                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.UserToken))
                    msg.Headers.Add("UserToken", fiaUser.UserToken);
                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.IPAddress))
                    msg.Headers.Add("ClientIPAddress", fiaUser.IPAddress);

                if (headerValues != null)
                {
                    foreach (KeyValuePair<string, string> header in headerValues)
                    {
                        msg.Headers.Add(header.Key, header.Value);
                    }
                }

                HttpContent content = new StringContent(jsonStr);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                msg.Content = content;
                await Log("Sending to API Post - " + apiPath + " - payload = " + jsonStr);
                using (HttpResponseMessage httpResponse = await client.SendAsync(msg))
                {
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        apiResult = await httpResponse.Content.ReadAsStringAsync();
                    }
                }
                await Log("Sending to API complete with result - " + apiPath + " - " + apiResult);
            }
            return apiResult;
        }
        #endregion

        /// <summary>
        /// Post Async Method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="requestData"></param>
        /// <param name="timeout"></param>
        /// <param name="fiaUser"></param>
        /// <returns></returns>
        #region Post Async Method
        public async Task<T> PostAsync<T>(string apiPath, object requestData, TimeSpan timeout, ISANUKTLoggedInUser fiaUser)
        {
            T apiResult = default(T);
            string jsonStr = JsonConvert.SerializeObject(requestData);

            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, apiPath))
            {
                msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.ApiToken))
                    msg.Headers.Add("ApiToken", fiaUser.ApiToken);
                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.UserToken))
                    msg.Headers.Add("UserToken", fiaUser.UserToken);

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.IPAddress))
                    msg.Headers.Add("ClientIPAddress", fiaUser.IPAddress);

                HttpContent content = new StringContent(jsonStr);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                msg.Content = content;

                string result = string.Empty;
                await Log("Sending to API Post - " + apiPath + " - payload = " + jsonStr);
                using (HttpResponseMessage httpResponse = await client.SendAsync(msg))
                {
                    //s = httpResponse.IsSuccessStatusCode.ToString();
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        result = httpResponse.Content.ReadAsStringAsync().Result;
                        apiResult = JsonConvert.DeserializeObject<T>(result);
                    }
                }
                await Log("Sending to API complete with result - " + apiPath + " - " + result);
            }
            return apiResult;
        }
        #endregion

        /// <summary>
        /// Delete Async Method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="queryStringParams"></param>
        /// <param name="fiaUser"></param>
        /// <returns></returns>
        #region Delete Async Method
        public async Task<T> DeleteAsync<T>(string apiPath, string queryStringParams, ISANUKTLoggedInUser fiaUser)
        {
            T apiResult = default(T);

            if (queryStringParams != null)
            {
                if (apiPath.Contains("?"))
                    apiPath = apiPath + "&" + queryStringParams;
                else if (queryStringParams.Contains("?"))
                    apiPath = apiPath + queryStringParams;
                else
                    apiPath = apiPath + "?" + queryStringParams;
            }

            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Delete, apiPath))
            {
                msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.ApiToken))
                    msg.Headers.Add("ApiToken", fiaUser.ApiToken);
                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.UserToken))
                    msg.Headers.Add("UserToken", fiaUser.UserToken);

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.IPAddress))
                    msg.Headers.Add("ClientIPAddress", fiaUser.IPAddress);

                string result = string.Empty;
                await Log("Sending to API Post - " + apiPath);
                using (HttpResponseMessage httpResponse = await client.SendAsync(msg))
                {
                    //s = httpResponse.IsSuccessStatusCode.ToString();
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        result = httpResponse.Content.ReadAsStringAsync().Result;
                        apiResult = JsonConvert.DeserializeObject<T>(result);
                    }
                }
                await Log("Sending to API complete - " + apiPath + " - " + result);
            }

            return apiResult;
        }
        #endregion

        /// <summary>
        /// Delete Async Method
        /// </summary>
        /// <param name="apiPath"></param>
        /// <param name="queryStringParams"></param>
        /// <param name="fiaUser"></param>
        /// <returns></returns>
        #region Delete Async Method
        public async Task<string> DeleteAsync(string apiPath, string queryStringParams, ISANUKTLoggedInUser fiaUser)
        {
            string apiResult = string.Empty;

            if (queryStringParams != null)
            {
                if (apiPath.Contains("?"))
                    apiPath = apiPath + "&" + queryStringParams;
                else if (queryStringParams.Contains("?"))
                    apiPath = apiPath + queryStringParams;
                else
                    apiPath = apiPath + "?" + queryStringParams;
            }

            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Delete, apiPath))
            {
                msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.ApiToken))
                    msg.Headers.Add("ApiToken", fiaUser.ApiToken);
                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.UserToken))
                    msg.Headers.Add("UserToken", fiaUser.UserToken);

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.IPAddress))
                    msg.Headers.Add("ClientIPAddress", fiaUser.IPAddress);

                await Log("Sending to API Post - " + apiPath);
                using (HttpResponseMessage httpResponse = await client.SendAsync(msg))
                {
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        apiResult = httpResponse.Content.ReadAsStringAsync().Result;
                    }
                }
                await Log("Sending to API complete - " + apiPath + " - " + apiResult);
            }

            return apiResult;
        }
        #endregion

        /// <summary>
        /// Put Async Method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="requestData"></param>
        /// <param name="fiaUser"></param>
        /// <returns></returns>
        #region Put Async Method
        public async Task<T> PutAsync<T>(string apiPath, object requestData, ISANUKTLoggedInUser fiaUser)
        {
            T apiResult = default(T);
            string jsonStr = JsonConvert.SerializeObject(requestData);


            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Put, apiPath))
            {
                msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.ApiToken))
                    msg.Headers.Add("ApiToken", fiaUser.ApiToken);
                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.UserToken))
                    msg.Headers.Add("UserToken", fiaUser.UserToken);

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.IPAddress))
                    msg.Headers.Add("ClientIPAddress", fiaUser.IPAddress);

                HttpContent content = new StringContent(jsonStr);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                msg.Content = content;

                string result = string.Empty;
                await Log("Sending to API Post - " + apiPath + " - payload = " + jsonStr);
                using (HttpResponseMessage httpResponse = await client.SendAsync(msg))
                {
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        result = httpResponse.Content.ReadAsStringAsync().Result;
                        apiResult = JsonConvert.DeserializeObject<T>(result);
                    }
                }
                await Log("Sending to API complete - " + apiPath + " - " + result);
            }

            return apiResult;
        }
        #endregion

        /// <summary>
        /// Put Async Method
        /// </summary>
        /// <param name="apiPath"></param>
        /// <param name="RequestData"></param>
        /// <param name="fiaUser"></param>
        /// <returns></returns>
        #region Put Async Method
        public async Task<string> PutAsync(string apiPath, object RequestData, ISANUKTLoggedInUser fiaUser)
        {
            string apiResult = string.Empty;
            string jsonStr = JsonConvert.SerializeObject(RequestData);

            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Put, apiPath))
            {
                msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.ApiToken))
                    msg.Headers.Add("ApiToken", fiaUser.ApiToken);
                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.UserToken))
                    msg.Headers.Add("UserToken", fiaUser.UserToken);

                if (fiaUser != null && !string.IsNullOrEmpty(fiaUser.IPAddress))
                    msg.Headers.Add("ClientIPAddress", fiaUser.IPAddress);

                HttpContent content = new StringContent(jsonStr);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                msg.Content = content;

                await Log("Sending to API Post - " + apiPath + " - payload = " + jsonStr);
                using (HttpResponseMessage httpResponse = await client.SendAsync(msg))
                {
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        apiResult = httpResponse.Content.ReadAsStringAsync().Result;
                    }
                }
                await Log("Sending to API complete - " + apiPath + " - " + apiResult);
            }
            return apiResult;
        }
        #endregion
    }
}
