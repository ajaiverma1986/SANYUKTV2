using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider.Shared;
using System;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using SANYUKT.Configuration;
using SANYUKT.Datamodel.Paysprint;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.Interfaces;
using System.Net;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using SANYUKT.Commonlib.Utility;
using System.Collections.Generic;

namespace SANYUKT.Provider
{
    public class PaySprintProvider:BaseProvider
    {
       private readonly SysMgrProvider _sysprd=null;
        public PaySprintProvider() {
            _sysprd = new  SysMgrProvider();
        }
        public async Task<SimpleResponse> GenerateToken(ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse xxx=new SimpleResponse ();
            xxx = await _sysprd.GenerateServiceSessionID(2, serviceUser);
            string RequestID = xxx.Result.ToString();
            SimpleResponse response = new SimpleResponse();
            string key = SANYUKTApplicationConfiguration.Instance.PaysprintAuthKey;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, "HS256");
            var header = new JwtHeader(credentials);

            var payload = new JwtPayload
           {
                {"timestamp", DateTimeOffset.Now.ToUnixTimeMilliseconds()},
                {"partnerId",SANYUKTApplicationConfiguration.Instance.PaysprintPartnerId },
                { "reqid",RequestID},
            };
            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(secToken);
            if (tokenString != "")
            {
                response.Result = tokenString;
            }
            else {
                response.SetError("Token not Generated");
            }

            return response;
        }
        public static byte[] Encrypt(string plainText, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    return ms.ToArray();
                }
            }
        }
       
        public async Task<SimpleResponse> GetFinoCustomerDetail(GetCustomerRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response=new SimpleResponse();
            HttpWebRequest objRequest;
            string reqUrl = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl+ "service-api/api/v1/service/dmt/kyc/remitter/queryremitter";
            string jsondata = "";
            string jsons = "";
            GetCustomerRequest request1=new GetCustomerRequest();
            request1.mobile=request.Mobile;
            objRequest = (HttpWebRequest)HttpWebRequest.Create(reqUrl);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("Token", request.TokenData);
            objRequest.Headers.Add("Authorisedkey", SANYUKTApplicationConfiguration.Instance.PaysprintAuthKey);
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsondata = JsonConvert.SerializeObject(request1, options);
            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(jsondata);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)objRequest.GetResponse();

            
            if(httpResponse.StatusCode == HttpStatusCode.OK)
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    jsons = await streamReader.ReadToEndAsync();
                }
                SpCustomerResponse resp = jsons.Deserialize<SpCustomerResponse>();
                response.Result = resp;
            }
            else if (httpResponse.StatusCode==HttpStatusCode.Unauthorized)
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    jsons = await streamReader.ReadToEndAsync();
                }
                SpBaseResponse resp=jsons.Deserialize<SpBaseResponse>();
                response.SetError(resp.message);
            }
           
            return response;

        }

        public async Task<SimpleResponse> FinoCustomerEkyc(FinoEkycRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            WebRequest objRequest;
            string reqUrl = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/remitter/queryremitter/kyc";
            string jsondata = "";
            string jsons = "";

            string piddata = request.PidData;
            byte[] key = Convert.FromBase64String(SANYUKTApplicationConfiguration.Instance.PaysprintAESENCRYPTIONKEY);
            byte[] iv = Convert.FromBase64String(SANYUKTApplicationConfiguration.Instance.PaysprintAESENCRYPTIONIV); 

            byte[] ciphertext_raw = Encrypt(piddata, key, iv);
            string enctoken = Convert.ToBase64String(ciphertext_raw);


            FinoEkycRequest request1 = new FinoEkycRequest();
            request1.mobile = request.Mobile;
            request1.aadhaar_number = request.AadharNo;
            request1.piddata = enctoken;
            request1.accessmode = request.AccessMode;
            request1.is_iris = request.isIris;
            objRequest = WebRequest.Create(reqUrl);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("Token", request.TokenData);
            objRequest.Headers.Add("Authorisedkey", SANYUKTApplicationConfiguration.Instance.PaysprintAuthKey);
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsondata = JsonConvert.SerializeObject(request1, options);
            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(jsondata);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
            }
            response.Result = jsons;
            return response;

        }

        public async Task<SimpleResponse> FinoRegisterCustomer(FinoRegCustomerRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            WebRequest objRequest;
            string reqUrl = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/remitter/registerremitter";
            string jsondata = "";
            string jsons = "";


            FinoRegCustomerRequest request1 = new FinoRegCustomerRequest();
            request1.mobile = request.mobile;
            request1.otp = request.otp;
            request1.ekyc_id = request.ekyc_id;
            request1.stateresp = request.stateresp;
 
            objRequest = WebRequest.Create(reqUrl);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("Token", request.TokenData);
            objRequest.Headers.Add("Authorisedkey", SANYUKTApplicationConfiguration.Instance.PaysprintAuthKey);
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsondata = JsonConvert.SerializeObject(request1, options);
            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(jsondata);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
            }
            response.Result = jsons;
            return response;

        }

        public async Task<SimpleResponse> FinoRegisterBenficiary(FinoRegBenRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            WebRequest objRequest;
            string reqUrl = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/beneficiary/registerbeneficiary";
            string jsondata = "";
            string jsons = "";


            FinoRegBenRequest request1 = new FinoRegBenRequest();
            request1.mobile = request.mobile;
            request1.benename = request.benename;
            request1.verified = request.verified;
            request1.bankid = request.bankid;
            request1.accno = request.accno;
            request1.ifsccode = request.ifsccode;

            objRequest = WebRequest.Create(reqUrl);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("Token", request.TokenData);
            objRequest.Headers.Add("Authorisedkey", SANYUKTApplicationConfiguration.Instance.PaysprintAuthKey);
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsondata = JsonConvert.SerializeObject(request1, options);
            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(jsondata);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
            }
            response.Result = jsons;
            return response;

        }

        public async Task<SimpleResponse> FinoDeleteBenficiary(FinoDeleteBenRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            WebRequest objRequest;
            string reqUrl = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/beneficiary/registerbeneficiary/deletebeneficiary";
            string jsondata = "";
            string jsons = "";


            FinoDeleteBenRequest request1 = new FinoDeleteBenRequest();
            request1.mobile = request.mobile;
            request1.bene_id = request.bene_id;
          
            objRequest = WebRequest.Create(reqUrl);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("Token", request.TokenData);
            objRequest.Headers.Add("Authorisedkey", SANYUKTApplicationConfiguration.Instance.PaysprintAuthKey);
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsondata = JsonConvert.SerializeObject(request1, options);
            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(jsondata);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
            }
            response.Result = jsons;
            return response;

        }

        public async Task<SimpleResponse> FinoFetchBenficiary(FinofetchBenRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            WebRequest objRequest;
            string reqUrl = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/beneficiary/registerbeneficiary/fetchbeneficiary";
            string jsondata = "";
            string jsons = "";


            FinofetchBenRequest request1 = new FinofetchBenRequest();
            request1.mobile = request.mobile;

            objRequest = WebRequest.Create(reqUrl);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("Token", request.TokenData);
            objRequest.Headers.Add("Authorisedkey", SANYUKTApplicationConfiguration.Instance.PaysprintAuthKey);
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsondata = JsonConvert.SerializeObject(request1, options);
            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(jsondata);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
            }
            response.Result = jsons;
            return response;

        }

        public async Task<SimpleResponse> FinoFetchBenficiaryByBenID(FinofetchBenRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            WebRequest objRequest;
            string reqUrl = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/beneficiary/registerbeneficiary/fetchbeneficiary";
            string jsondata = "";
            string jsons = "";

            FinofetchBenbybenIDRequest request1 = new FinofetchBenbybenIDRequest();
            request1.beneid = request.beneid;

            objRequest = WebRequest.Create(reqUrl);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("Token", request.TokenData);
            objRequest.Headers.Add("Authorisedkey", SANYUKTApplicationConfiguration.Instance.PaysprintAuthKey);
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsondata = JsonConvert.SerializeObject(request1, options);
            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(jsondata);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
            }
            response.Result = jsons;
            return response;

        }

        public async Task<SimpleResponse> FinoPPenyDrop(FinoTransactionRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            WebRequest objRequest;
            string reqUrl = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/beneficiary/registerbeneficiary/benenameverify";
            string jsondata = "";
            string jsons = "";

            FinoTransactionRequest request1 = new FinoTransactionRequest();
            request1.dob = request.dob;
            request1.benename = request.benename;
            request1.address = request.address;
            request1.bene_id = request.bene_id;
            request1.referenceid = request.referenceid;
            request1.accno = request.accno;
            request1.mobile = request.mobile;
            request1.gst_state = request.gst_state;
            request1.pincode = request.pincode;
            request1.bankid = request.bankid;

            objRequest = WebRequest.Create(reqUrl);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("Token", request.TokenData);
            objRequest.Headers.Add("Authorisedkey", SANYUKTApplicationConfiguration.Instance.PaysprintAuthKey);
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsondata = JsonConvert.SerializeObject(request1, options);
            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(jsondata);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
            }
            response.Result = jsons;
            return response;

        }

        public async Task<SimpleResponse> FinoTransactionOTP(FinoTransactionSendRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            WebRequest objRequest;
            string reqUrl = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/transact/transact/send_otp";
            string jsondata = "";
            string jsons = "";

            FinoTransactionSendRequestView request1 = new FinoTransactionSendRequestView();
            request1.bene_id = request.bene_id;
            request1.referenceid = request.referenceid;
            request1.txntype = request.txntype;
            request1.mobile = request.mobile;
            request1.amount = request.amount;
           
            objRequest = WebRequest.Create(reqUrl);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("Token", request.TokenData);
            objRequest.Headers.Add("Authorisedkey", SANYUKTApplicationConfiguration.Instance.PaysprintAuthKey);
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsondata = JsonConvert.SerializeObject(request1, options);
            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(jsondata);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
            }
            response.Result = jsons;
            return response;

        }

        public async Task<SimpleResponse> FinoTransaction(FinoTransactionFinalRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            WebRequest objRequest;
            string reqUrl = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/transact/transact";
            string jsondata = "";
            string jsons = "";

            FinoTransactionFinalRequest request1 = new FinoTransactionFinalRequest();
            request1.bene_id = request.bene_id;
            request1.referenceid = request.referenceid;
            request1.txntype = request.txntype;
            request1.mobile = request.mobile;
            request1.amount = request.amount;
            request1.otp = request.otp;
            request1.stateresp = request.stateresp;

            objRequest = WebRequest.Create(reqUrl);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("Token", request.TokenData);
            objRequest.Headers.Add("Authorisedkey", SANYUKTApplicationConfiguration.Instance.PaysprintAuthKey);
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsondata = JsonConvert.SerializeObject(request1, options);
            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(jsondata);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
            }
            response.Result = jsons;
            return response;

        }

        public async Task<SimpleResponse> FinoTransactionStatus(FinoTransactionStatusRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            WebRequest objRequest;
            string reqUrl = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/transact/transact/querytransact";
            string jsondata = "";
            string jsons = "";

            FinoTransactionStatusRequest request1 = new FinoTransactionStatusRequest();
            request1.referenceid = request.referenceid;
          

            objRequest = WebRequest.Create(reqUrl);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("Token", request.TokenData);
            objRequest.Headers.Add("Authorisedkey", SANYUKTApplicationConfiguration.Instance.PaysprintAuthKey);
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsondata = JsonConvert.SerializeObject(request1, options);
            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(jsondata);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
            }
            response.Result = jsons;
            return response;

        }

        public async Task<SimpleResponse> FinoTransactionRefundOTP(FinoRefundOtpRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            WebRequest objRequest;
            string reqUrl = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/refund/refund/resendotp";
            string jsondata = "";
            string jsons = "";

            FinoRefundOtpRequest request1 = new FinoRefundOtpRequest();
            request1.referenceid = request.referenceid;
            request1.ackno = request.ackno;


            objRequest = WebRequest.Create(reqUrl);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("Token", request.TokenData);
            objRequest.Headers.Add("Authorisedkey", SANYUKTApplicationConfiguration.Instance.PaysprintAuthKey);
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsondata = JsonConvert.SerializeObject(request1, options);
            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(jsondata);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
            }
            response.Result = jsons;
            return response;

        }

        public async Task<SimpleResponse> FinoTransactionRefund(FinoRefundRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            WebRequest objRequest;
            string reqUrl = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/refund/refund/resendotp";
            string jsondata = "";
            string jsons = "";

            FinoRefundRequest request1 = new FinoRefundRequest();
            request1.referenceid = request.referenceid;
            request1.ackno = request.ackno;
            request1.otp = request.otp;

            objRequest = WebRequest.Create(reqUrl);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("Token", request.TokenData);
            objRequest.Headers.Add("Authorisedkey", SANYUKTApplicationConfiguration.Instance.PaysprintAuthKey);
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jsondata = JsonConvert.SerializeObject(request1, options);
            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(jsondata);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
            }
            response.Result = jsons;
            return response;

        }
    }
}
