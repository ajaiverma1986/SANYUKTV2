using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using SANYUKT.Configuration;
using SANYUKT.Datamodel.Entities.SysModel;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider.Shared;
using SANYUKT.Repository;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using SANYUKT.Commonlib.Security;
using System.Security.Cryptography;

namespace SANYUKT.Provider
{
    public class UtilityProvider : BaseProvider
    {
        public readonly SysMgrRepository _repository = null;
        public UtilityProvider()
        {
            _repository = new SysMgrRepository();
        }
        public async Task<OTPResponse> SendOTP(OTPRequest otpRequest)
        {
            string smscontent = "";
            OTPResponse response = new OTPResponse();

            if (otpRequest != null && !string.IsNullOrEmpty(otpRequest.mobileno))
            {
                string _otp = CommonHelper.RandomDigits(6);
                smscontent = "" + _otp + " is the reference no. for FIA Verification. Do Not share the reference no. with anyone other than the agent assisting.";

                response = await _repository.SendOTP(otpRequest.mobileno, _otp);

            }
            return response;
        }

        public async Task<SimpleResponse> ValidateOTP(OTPValidateRequest fIAOTPValidateRequest)
        {
            SimpleResponse response = new SimpleResponse();

            if (fIAOTPValidateRequest != null && !string.IsNullOrEmpty(fIAOTPValidateRequest.mobileno) && !string.IsNullOrEmpty(fIAOTPValidateRequest.otp))
            {
                response = await _repository.ValidateOTP(fIAOTPValidateRequest.mobileno, fIAOTPValidateRequest.otp);

            }
            return response;
        }

        public string GetHMACSHA256(string text, string key)
        {
            UTF8Encoding encoder = new UTF8Encoding();

            byte[] hashValue;
            byte[] keybyt = encoder.GetBytes(key);
            byte[] message = encoder.GetBytes(text);

            HMACSHA256 hashString = new HMACSHA256(keybyt);
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }
        public async Task<SimpleResponse> ValidateDocument(DocValidatorViewModelRequest request, ISANYUKTServiceUser serviceUser)
        {
            WebRequest objRequest;
            string jsondata = "";
            string jsons = "";
            string reqUrl = "";
            SimpleResponse response = new SimpleResponse();
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            if (request.veritype == 1)
            {
                reqUrl = SANYUKTApplicationConfiguration.Instance.digitapApiURL + "kyc/v1/pan_details";
                DocValidatorPanRequest request1 = new DocValidatorPanRequest();
                request1.client_ref_num = await _repository.GenerateServiceSessionCode(request.veritype, serviceUser);
                request1.Pan = request.DocumentNo;
                jsondata = JsonConvert.SerializeObject(request1, options);

            }
            else
            {
                reqUrl = SANYUKTApplicationConfiguration.Instance.digitapApiURL + "kyc/v1/basic_aadhaar";
                DocValidatorAadharRequest request1 = new DocValidatorAadharRequest();
                request1.client_ref_num = await _repository.GenerateServiceSessionCode(request.veritype, serviceUser);
                request1.aadhaar = request.DocumentNo;
                jsondata = JsonConvert.SerializeObject(request1, options);
            }
            string headerval = SANYUKTApplicationConfiguration.Instance.digitapClientID + ":" + SANYUKTApplicationConfiguration.Instance.digitapSecratekey;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(headerval);
            string headerauth = Convert.ToBase64String(plainTextBytes);


            if (!string.IsNullOrEmpty(reqUrl))
            {
                objRequest = WebRequest.Create(reqUrl);
                objRequest.ContentType = "application/json";
                objRequest.Method = "POST";
                objRequest.Headers.Add("authorization", headerauth);
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

                JObject jsonResponse = JObject.Parse(jsons);
               string anananc = jsonResponse["result"].ToString();
                string resp_Code= jsonResponse["http_response_code"].ToString();
             
                if (resp_Code=="200")
                {
                    string result_code = jsonResponse["result_code"].ToString();

                    if(result_code=="101")
                    {
                        JObject jsonResponse1 = JObject.Parse(anananc);
                        PanDocumentValidResponse response1 = new PanDocumentValidResponse();
                        response1.pan = jsonResponse1["pan"].ToString();
                        response1.pan_type = jsonResponse1["pan_type"].ToString();
                        response1.fullname = jsonResponse1["fullname"].ToString();
                        response1.first_name = jsonResponse1["first_name"].ToString();
                        response1.middle_name = jsonResponse1["middle_name"].ToString();
                        response1.last_name = jsonResponse1["last_name"].ToString();
                        response1.gender = jsonResponse1["gender"].ToString();
                        response1.dob = jsonResponse1["dob"].ToString();
                        response1.mobile = jsonResponse1["mobile"].ToString();
                        response1.email = jsonResponse1["email"].ToString();
                        JObject jsonaddress = JObject.Parse(jsonResponse1["address"].ToString());

                        response1.address = jsonaddress["building_name"].ToString() + " " + jsonaddress["locality"].ToString() + " " + jsonaddress["street_name"].ToString() + " " + jsonaddress["pincode"].ToString() + " " + jsonaddress["city"].ToString() + " " + jsonaddress["state"].ToString();
                        response.Result = response1;
                    }
                    else
                    {
                        response.SetError(ErrorCodes.NO_RECORD_FOUND);
                    }
                  
                }
                else {
                    response.SetError(ErrorCodes.NO_RECORD_FOUND);
                }

            }

            return response;
        }
    }
}
