using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using SANYUKT.Datamodel.CMS;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider.Shared;
using SANYUKT.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SANYUKT.Configuration;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.Interfaces;

namespace SANYUKT.Provider
{
    public class CMSProvider : BaseProvider
    {
        private readonly CMSRepository repository = null;
        private readonly AgentProvider _provider = null;
        public CMSProvider()
        {
            repository = new CMSRepository();
            _provider = new AgentProvider();
        }

        // sha256
        public static string algoSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hashBytes);
            }
        }

        //MD5
        public static string getMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        //AES-128
        public static string GenerateSessionKeyString(string plainText)
        {
            byte[] Key = Encoding.UTF8.GetBytes("8080808080808080");
            byte[] IV = Encoding.UTF8.GetBytes("8080808080808080");

            Aes aesAlg = Aes.Create();
            aesAlg.KeySize = 128; // Set the key size to 128 bits
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            System.IO.MemoryStream msEncrypt = new System.IO.MemoryStream();
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            System.IO.StreamWriter swEncrypt = new System.IO.StreamWriter(csEncrypt);

            swEncrypt.Write(plainText);
            swEncrypt.Close();
            csEncrypt.Close();
            msEncrypt.Close();

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        //RSA/ECB/PKCS1Padding 
        public static string RSAEncryption(string Sessionkey)
        {
            try
            {
                X509Certificate2 certificate2 = new X509Certificate2(Path.GetFullPath(SANYUKTApplicationConfiguration.Instance.CMSCertificate));
                var testData = Encoding.UTF8.GetBytes(Sessionkey);
                using (var rsa = (RSACng)certificate2.PublicKey.Key)
                {
                    var encryptedData = rsa.Encrypt(testData, RSAEncryptionPadding.Pkcs1);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    return base64Encrypted;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string EncodeJsonToBase64(string jsonString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
            return Convert.ToBase64String(bytes);
        }

        public static string EncryptUsingSessionKey(string skey, string Jdata)
        {

            byte[] sessionKey = Encoding.UTF8.GetBytes(skey);
            byte[] data = Encoding.UTF8.GetBytes(Jdata);
            using (Aes aes = Aes.Create())
            {
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.ECB;
                aes.Key = sessionKey;
                aes.GenerateIV();
                ICryptoTransform encryptor = aes.CreateEncryptor();
                byte[] encryptedData;

                using (var ms = new System.IO.MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                    }
                    encryptedData = ms.ToArray();
                }
                return Convert.ToBase64String(encryptedData);
            }
        }

        //AES Encrypt for SignOn
        public static string EncryptStringToBytes(string plaintext, byte[] key)
        {
            Aes aes = Aes.Create();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            aes.Key = key;
            aes.GenerateIV();
            byte[] ciphertext;
            string encryptedData;
            using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                byte[] plaintextBytes = System.Text.Encoding.UTF8.GetBytes(plaintext);
                ciphertext = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);
            }
            string base64dd = Convert.ToBase64String(ciphertext);
            encryptedData = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(base64dd));

            return encryptedData;
        }


        public async Task<SimpleResponse> CreateCmsUser(AddCMS1 objp, SANYUKTServiceUser FIAAPIUser)
        {
            SimpleResponse response = new SimpleResponse();
            string jsondata = "";
            string jsons = "";
            string datetimest = "";
            string Headrevalue = "";
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            try
            {
                AddCMS objn = new AddCMS();
                objn.latitude = objp.latitude;
                objn.password = SANYUKTApplicationConfiguration.Instance.CMSpassword;
                objn.ipAddress = objp.ipAddress;
                objn.longitude = objp.longitude;
                objn.username = SANYUKTApplicationConfiguration.Instance.CMSMerchantLoginId;
                objn.supermerchantId = Convert.ToInt32(SANYUKTApplicationConfiguration.Instance.CMSMerchantId);
                Merchant mm = new Merchant();
                mm.cancelledChequeImages = objp.merchant.cancelledChequeImages;
                mm.emailId = objp.merchant.emailId;
                mm.merchantPhoneNumber = objp.merchant.merchantPhoneNumber;
                mm.lastName = objp.merchant.lastName;
                mm.firstName = objp.merchant.firstName;
                mm.tradeBusinessProof = objp.merchant.tradeBusinessProof;
                mm.certificateOfIncorporationImage = objp.merchant.certificateOfIncorporationImage;
                mm.companyLegalName = objp.merchant.companyLegalName;
                mm.companyType = objp.merchant.companyType;
                mm.merchantLoginId = objp.merchant.merchantLoginId;
                mm.merchantLoginPin = objp.merchant.merchantLoginPin;
                mm.middleName = objp.merchant.middleName;
                mm.physicalVerification = objp.merchant.physicalVerification;
                mm.termsConditionCheck = objp.merchant.termsConditionCheck;
                mm.userType = objp.merchant.userType;
                mm.videoKycWithLatLongData = objp.merchant.videoKycWithLatLongData;

                MerchantKyc mkyc = new MerchantKyc();
                mkyc.aadhaarNumber = objp.merchant.kyc.aadhaarNumber;
                mkyc.userPan = objp.merchant.kyc.userPan;
                mkyc.merchantPanImage = objp.merchant.kyc.merchantPanImage;
                mkyc.companyOrShopPan = objp.merchant.kyc.companyOrShopPan;
                mkyc.gstinNumber = objp.merchant.kyc.gstinNumber;
                mkyc.maskedAadharImage = objp.merchant.kyc.maskedAadharImage;
                mkyc.shopAndPanImage = objp.merchant.kyc.shopAndPanImage;

                mm.kyc = mkyc;

                SettlementV1 sv = new SettlementV1();
                sv.companyBankName = objp.merchant.settlementV1.companyBankName;
                sv.companyBankAccountNumber = objp.merchant.settlementV1.companyBankAccountNumber;
                sv.bankAccountName = objp.merchant.settlementV1.bankAccountName;
                sv.bankIfscCode = objp.merchant.settlementV1.bankIfscCode;
                mm.settlementV1 = sv;

                MerchantAddress ma = new MerchantAddress();
                ma.merchantAddress1 = objp.merchant.merchantAddress.merchantAddress1;
                ma.merchantAddress2 = objp.merchant.merchantAddress.merchantAddress2;
                ma.merchantPinCode = objp.merchant.merchantAddress.merchantPinCode;
                ma.merchantCityName = objp.merchant.merchantAddress.merchantCityName;
                ma.merchantState = objp.merchant.merchantAddress.merchantState;
                ma.merchantDistrictName = objp.merchant.merchantAddress.merchantDistrictName;
                mm.merchantAddress = ma;

                MerchantKycAddressData mad = new MerchantKycAddressData();
                mad.shopAddress = objp.merchant.merchantKycAddressData.shopAddress;
                mad.backgroundImageOfShop = objp.merchant.merchantKycAddressData.backgroundImageOfShop;
                mad.shopLatitude = objp.merchant.merchantKycAddressData.shopLatitude;
                mad.shopCity = objp.merchant.merchantKycAddressData.shopCity;
                mad.shopLongitude = objp.merchant.merchantKycAddressData.shopLongitude;
                mad.shopDistrict = objp.merchant.merchantKycAddressData.shopDistrict;
                mad.shopPincode = objp.merchant.merchantKycAddressData.shopPincode;
                mad.shopState = objp.merchant.merchantKycAddressData.shopState;

                mm.merchantKycAddressData = mad;

                objn.merchant = mm;


                string sessionID = repository.GenerateSessioncode(objp.merchant.merchantLoginId.ToString(), "CC", "2", "1").Result;

                string SessionKey = GenerateSessionKeyString(sessionID);
                string RSAEncrdata = RSAEncryption(SessionKey);
                string jsonstring = JsonConvert.SerializeObject(objn);
                string hashValue = algoSHA256(jsonstring);

                string Url;
                WebRequest objRequest;

                Url = SANYUKTApplicationConfiguration.Instance.CMSAPIMerURL.ToString() + "onboarding/merchant/creation/v2";
                datetimest = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                objRequest = WebRequest.Create(Url);
                objRequest.ContentType = "application/json";
                objRequest.Method = "POST";
                objRequest.Headers.Add("trnTimestamp", datetimest);
                objRequest.Headers.Add("eskey", RSAEncrdata);
                objRequest.Headers.Add("hash", hashValue);

                Headrevalue = "ContentType|application/json|Method|POST|trnTimestamp|" + datetimest + "|eskey|" + RSAEncrdata + "|hash|" + hashValue;
                string encryptedData = EncryptUsingSessionKey(SessionKey, jsonstring);

                using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
                {
                    streamWriter.Write(encryptedData);
                    streamWriter.Flush();
                    streamWriter.Close();
                }


                jsondata = JsonConvert.SerializeObject(objn, options);

                var httpResponse = (WebResponse)objRequest.GetResponse();
                using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
                {
                    jsons = await streamReader.ReadToEndAsync();
                    response.Result = jsons.ToString();
                    AddCMSAgentRequest obm = new AddCMSAgentRequest();
                    obm.agentcode = objn.merchant.merchantLoginId;
                    //obm.agentname = (objn.merchant.firstName)+" "+(objn.merchant.middleName)+" " + (objn.merchant.lastName);

                    JObject jsonResponse = JObject.Parse(jsons);
                    bool status = (bool)jsonResponse["status"];

                    if (status == true)
                    {
                        obm.status = 0;
                        response = await repository.CreateCMSAgent(obm, FIAAPIUser);
                    }
                    else
                    {
                        // string errMsg = (string)jsonResponse["message"];
                        //string errCode = (string)jsonResponse["statusCode"];
                        response.Result = jsons;
                        //response.SetError(errMsg);
                    }
                }

                await _provider.APIRequestRecord_Log("CMS API Merchant Onboarding", jsondata, JsonConvert.SerializeObject(jsons, options), Headrevalue, "");
            }
            catch (Exception ex)
            {
                response.SetError(ex.Message.ToString());
                await _provider.APIRequestRecord_Log("CMS API Merchant Onboarding", jsondata, ex.Message.ToString(), Headrevalue, "");
            }
            return response;
        }

        public async Task<SimpleResponse> getStateDetail(ISANYUKTServiceUser FIAAPIUser)
        {
            string Url;
            SimpleResponse response = new SimpleResponse();
            WebRequest objRequest;
            string jsons = "";
            Url = "https://fingpayap.tapits.in/fpaepsweb/api/onboarding/getstates";

            objRequest = WebRequest.Create(Url);
            objRequest.ContentType = "application/json";
            objRequest.Method = "GET";

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
                response.Result = jsons.ToString();
            }

            return response;

        }

        public async Task<SimpleResponse> sendOTP(MerchantOTPRequest1 objp, ISANYUKTServiceUser FIAAPIUser)
        {
            SimpleResponse response = new SimpleResponse();

            if (objp == null)
            {
                response.SetError(ErrorCodes.INVALID_REQUEST);
                return response;
            }

            if (objp.merchantLoginId == "")
            {
                response.SetError("Merchant Login ID is Required");
                return response;
            }
            if (objp.transactionType == "")
            {
                response.SetError("Transaction type is Required");
                return response;
            }
            if (objp.mobileNumber == "")
            {
                response.SetError("Mobile No. is Required");
                return response;
            }
            if (objp.aadharNumber == "")
            {
                response.SetError("Aadhar no. is Required");
                return response;
            }
            if (objp.panNumber == "")
            {
                response.SetError("PAN no. is Required");
                return response;
            }

            if (objp.latitude == 0)
            {
                response.SetError("Latitude is missing");
                return response;
            }
            if (objp.longitude == 0)
            {
                response.SetError("Longitude is missing");
                return response;
            }

            string sessionID = repository.GenerateSessioncode(objp.merchantLoginId, "CC", "2", "1").Result;
            string SessionKey = GenerateSessionKeyString(sessionID);
            string RSAEncrdata = RSAEncryption(SessionKey);

            string Url;

            MerchantOTPRequest objn = new MerchantOTPRequest();
            objn.aadharNumber = objp.aadharNumber;
            objn.panNumber = objp.panNumber;
            objn.matmSerialNumber = objp.matmSerialNumber;
            objn.mobileNumber = objp.mobileNumber;
            objn.superMerchantId = SANYUKTApplicationConfiguration.Instance.CMSMerchantId;
            objn.transactionType = objp.transactionType;
            objn.latitude = objp.latitude;
            objn.longitude = objp.longitude;
            objn.merchantLoginId = objp.merchantLoginId;

            WebRequest objRequest;
            string jsons = "";
            Url = SANYUKTApplicationConfiguration.Instance.CMSAPIURL.ToString() + "ekyc/merchant/sendotp";

            objRequest = WebRequest.Create(Url);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("trnTimestamp", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            objRequest.Headers.Add("deviceIMEI", objp.deviceIMEI);
            objRequest.Headers.Add("eskey", RSAEncrdata);

            string jsonstring = JsonConvert.SerializeObject(objn);

            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            string jsondata = JsonConvert.SerializeObject(objn, options);

            string hashValue = algoSHA256(jsonstring);
            objRequest.Headers.Add("hash", hashValue);

            string encryptedData = EncryptUsingSessionKey(SessionKey, jsonstring);

            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(encryptedData);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
                response.Result = jsons.ToString();
            }
            await _provider.APIRequestRecord_Log("CMS API OTP", jsondata, JsonConvert.SerializeObject(jsons, options), "", "");
            return response;
        }

        public async Task<SimpleResponse> resendOTP(resendOTPReq1 objp, ISANYUKTServiceUser FIAAPIUser)
        {
            SimpleResponse response = new SimpleResponse();
            if (objp == null)
            {
                response.SetError(ErrorCodes.INVALID_REQUEST);
                return response;
            }

            if (objp.merchantLoginId == "")
            {
                response.SetError("Merchant Login ID is Required");
                return response;
            }
            if (objp.primaryKeyId == 0)
            {
                response.SetError("Primary Key ID is missing");
                return response;
            }
            if (objp.encodeFPTxnId == "")
            {
                response.SetError("FP Txn ID is missing");
                return response;
            }

            string sessionID = repository.GenerateSessioncode(objp.merchantLoginId, "CC", "2", "1").Result;
            string SessionKey = GenerateSessionKeyString(sessionID);
            string RSAEncrdata = RSAEncryption(SessionKey);
            //string dIMEI = deviceIMEI;

            string Url;

            WebRequest objRequest;
            string jsons = "";
            Url = SANYUKTApplicationConfiguration.Instance.CMSAPIURL.ToString() + "ekyc/merchant/resendotp";

            resendOTPReq objn = new resendOTPReq();
            objn.encodeFPTxnId = objp.encodeFPTxnId;
            objn.primaryKeyId = objp.primaryKeyId;
            objn.merchantLoginId = objp.merchantLoginId;
            objn.superMerchantId = SANYUKTApplicationConfiguration.Instance.CMSMerchantId;

            objRequest = WebRequest.Create(Url);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("trnTimestamp", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            objRequest.Headers.Add("eskey", RSAEncrdata);
            objRequest.Headers.Add("deviceIMEI", objp.deviceIMEI);

            string jsonstring = JsonConvert.SerializeObject(objn);

            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            string jsondata = JsonConvert.SerializeObject(objn, options);

            string hashValue = algoSHA256(jsonstring);
            objRequest.Headers.Add("hash", hashValue);

            string encryptedData = EncryptUsingSessionKey(SessionKey, jsonstring);

            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(encryptedData);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
                response.Result = jsons.ToString();
            }
            await _provider.APIRequestRecord_Log("CMS API Resend OTP", jsondata, JsonConvert.SerializeObject(jsons, options), "", "");
            return response;
        }

        public async Task<SimpleResponse> validateOTP(validateOTPReq1 objp, ISANYUKTServiceUser FIAAPIUser)
        {
            SimpleResponse response = new SimpleResponse();

            if (objp == null)
            {
                response.SetError(ErrorCodes.INVALID_REQUEST);
                return response;
            }

            if (objp.merchantLoginId == "")
            {
                response.SetError("Merchant Login ID is Required");
                return response;
            }
            if (objp.otp == "")
            {
                response.SetError("OTP is Required");
                return response;
            }
            if (objp.primaryKeyId == 0)
            {
                response.SetError("Primary Key ID is missing");
                return response;
            }
            if (objp.encodeFPTxnId == "")
            {
                response.SetError("FP Txn ID is missing");
                return response;
            }
            string sessionID = repository.GenerateSessioncode(objp.merchantLoginId, "CC", "2", "1").Result;
            string SessionKey = GenerateSessionKeyString(sessionID);
            string RSAEncrdata = RSAEncryption(SessionKey);
            //string dIMEI = deviceIMEI;

            string Url;

            WebRequest objRequest;
            string jsons = "";
            Url = SANYUKTApplicationConfiguration.Instance.CMSAPIURL.ToString() + "ekyc/merchant/validateotp";

            validateOTPReq onjn = new validateOTPReq();
            onjn.otp = objp.otp;
            onjn.primaryKeyId = objp.primaryKeyId;
            onjn.merchantLoginId = objp.merchantLoginId;
            onjn.superMerchantId = SANYUKTApplicationConfiguration.Instance.CMSMerchantId;
            onjn.encodeFPTxnId = objp.encodeFPTxnId;

            objRequest = WebRequest.Create(Url);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("trnTimestamp", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            objRequest.Headers.Add("Eskey", RSAEncrdata);
            objRequest.Headers.Add("deviceIMEI", objp.deviceIMEI);

            string jsonstring = JsonConvert.SerializeObject(onjn);

            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            string jsondata = JsonConvert.SerializeObject(objp, options);

            string hashValue = algoSHA256(jsonstring);
            objRequest.Headers.Add("hash", hashValue);

            string encryptedData = EncryptUsingSessionKey(SessionKey, jsonstring);

            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(encryptedData);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
                response.Result = jsons.ToString();
            }
            await _provider.APIRequestRecord_Log("CMS API Validate OTP", jsondata, JsonConvert.SerializeObject(jsons, options), "", "");
            return response;
        }

        public async Task<SimpleResponse> BiometricEKYC(BioEKYCReq1 objp, ISANYUKTServiceUser FIAAPIUser)
        {
            SimpleResponse response = new SimpleResponse();

            if (objp == null)
            {
                response.SetError(ErrorCodes.INVALID_REQUEST);
                return response;
            }

            if (objp.merchantLoginId == "")
            {
                response.SetError("Merchant Login ID is Required");
                return response;
            }
            if (objp.cardnumberORUID.adhaarNumber == "")
            {
                response.SetError("Aadhar No. is Required");
                return response;
            }
            if (objp.cardnumberORUID.indicatorforUID == "")
            {
                response.SetError("Indicator for UID is missing");
                return response;
            }
            if (objp.primaryKeyId == 0)
            {
                response.SetError("Primary Key ID is missing");
                return response;
            }
            if (objp.encodeFPTxnId == "")
            {
                response.SetError("FP Txn ID is missing");
                return response;
            }
            string sessionID = repository.GenerateSessioncode(objp.merchantLoginId, "CC", "2", "1").Result;
            string SessionKey = GenerateSessionKeyString(sessionID);
            string RSAEncrdata = RSAEncryption(SessionKey);
            //string dIMEI = deviceIMEI;

            BioEKYCReq objn = new BioEKYCReq();
            objn.merchantLoginId = objp.merchantLoginId;
            objn.primaryKeyId = objp.primaryKeyId;
            objn.encodeFPTxnId = objp.encodeFPTxnId;
            objn.requestRemarks = objp.requestRemarks;
            objn.captureResponse = objp.captureResponse;
            objn.cardnumberORUID = objp.cardnumberORUID;
            objn.superMerchantId = SANYUKTApplicationConfiguration.Instance.CMSMerchantId;

            string Url;

            WebRequest objRequest;
            string jsons = "";
            Url = SANYUKTApplicationConfiguration.Instance.CMSAPIURL.ToString() + "ekyc/merchant/biometric";

            objRequest = WebRequest.Create(Url);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("trnTimestamp", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            objRequest.Headers.Add("Eskey", RSAEncrdata);
            objRequest.Headers.Add("deviceIMEI", objp.deviceIMEI);

            string jsonstring = JsonConvert.SerializeObject(objn);

            string hashValue = algoSHA256(jsonstring);
            objRequest.Headers.Add("hash", hashValue);

            string encryptedData = EncryptUsingSessionKey(SessionKey, jsonstring);

            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(encryptedData);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
                response.Result = jsons.ToString();
            }
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            await _provider.APIRequestRecord_Log("Biometrict API Merchant Onboarding", JsonConvert.SerializeObject(objn, options), JsonConvert.SerializeObject(jsons, options), "", "");
            return response;
        }

        public async Task<SimpleResponse> StatusCheck(Statuscheck objp, ISANYUKTServiceUser FIAAPIUser)
        {
            SimpleResponse response = new SimpleResponse();

            string securityKey = SANYUKTApplicationConfiguration.Instance.CMSMerchantSecurityKey;
            string jsonstring = JsonConvert.SerializeObject(objp);
            string trnTimestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            string hash = algoSHA256(jsonstring + securityKey + trnTimestamp);

            string Url;

            WebRequest objRequest;
            string jsons = "";
            Url = SANYUKTApplicationConfiguration.Instance.CMSAPIURL.ToString() + "ekyc/status/check";

            objRequest = WebRequest.Create(Url);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";
            objRequest.Headers.Add("trnTimestamp", trnTimestamp);
            objRequest.Headers.Add("hash", hash);

            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonstring);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
                response.Result = jsons.ToString();
            }
            return response;

        }

        public async Task<SimpleResponse> ListEntityUser(string Usercode, ISANYUKTServiceUser serviceUser)
        {
            return await repository.ListEntityuser(Usercode, serviceUser);
        }
        public async Task<SimpleResponse> TransactionReciept(string TransactionID)
        {
            return await repository.TransactionReciept(TransactionID);
        }
        public async Task<SimpleResponse> TransactionReport(TransactionReportRequest Request)
        {
            return await repository.TransactionReport(Request);
        }

        public async Task<SimpleResponse> singleSignOnURL(signonRequest1 objp, ISANYUKTServiceUser FIAAPIUser)
        {
            SimpleResponse response = new SimpleResponse();
            string Url;

            signonRequest request = new signonRequest();
            request.additionalParams = objp.additionalParams;
            request.amount = objp.amount;
            request.latitude = objp.latitude;
            request.longitude = objp.longitude;
            request.loginType = objp.loginType;
            request.merchantId = objp.merchantId;
            request.merchantPin = objp.merchantPin;
            request.mobileNumber = objp.mobileNumber;
            request.supermerchantId = SANYUKTApplicationConfiguration.Instance.CMSMerchantId;
            request.superMerchantSkey = SANYUKTApplicationConfiguration.Instance.CMSMerchantSecurityKey;

            string jsons = JsonConvert.SerializeObject(request);
            string baseUrl = SANYUKTApplicationConfiguration.Instance.CMSSingleSignOnURL;
            string secretkey = SANYUKTApplicationConfiguration.Instance.CMSMerchantSecurityKey.Substring(0, 32);

            byte[] parsedBase64Keybyt = System.Text.Encoding.UTF8.GetBytes(secretkey);

            string key = System.Convert.ToBase64String(parsedBase64Keybyt);
            string encryptInfo = EncryptStringToBytes(jsons, parsedBase64Keybyt);

            Url = $"{baseUrl}?data={encryptInfo}&skey={key}";
            await _provider.APIRequestRecord_Log("Single signon CMS", jsons, Url, encryptInfo, key);
            response.Result = Url;
            return response;

        }
        public async Task<CMSDebitResponse> WalletDebit(WalletDebitRequest request)
        {
            return await repository.WalletDebit(request);
        }

        public async Task<SimpleResponse> getCompanyTypes(SANYUKTServiceUser FIAAPIUser)
        {
            string Url;
            SimpleResponse response = new SimpleResponse();
            WebRequest objRequest;
            string jsons = "";
            Url = "https://fingpayap.tapits.in/fpaepsweb/api/onboarding/get/companyType/master";

            objRequest = WebRequest.Create(Url);
            objRequest.ContentType = "application/json";
            objRequest.Method = "GET";

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
                response.Result = jsons.ToString();
            }
            return response;

        }

        public async Task<SimpleResponse> CheckCashDropStatus(CheckCashDropStatusRequest objp, SANYUKTServiceUser FIAAPIUser)
        {
            SimpleResponse response = new SimpleResponse();
            CashDropStatusCheck obj = new CashDropStatusCheck();

            int superMerchantId = Convert.ToInt32(SANYUKTApplicationConfiguration.Instance.CMSMerchantId);

            obj.superMerchantId = superMerchantId;
            obj.merchantTransactionId = objp.BillpayTxnId;
            obj.bcLoginId = objp.AgentCode;

            string hash = algoSHA256(string.Concat(objp.BillpayTxnId, objp.AgentCode, superMerchantId).ToLower());

            obj.hash = hash;
            string jsonstring = JsonConvert.SerializeObject(obj);

            string Url;

            WebRequest objRequest;
            string jsons = "";
            Url = SANYUKTApplicationConfiguration.Instance.CMSAPIMerCorporateServiceURL.ToString() + "cms/status/check";

            objRequest = WebRequest.Create(Url);
            objRequest.ContentType = "application/json";
            objRequest.Method = "POST";

            using (var streamWriter = new System.IO.StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonstring);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (WebResponse)objRequest.GetResponse();
            using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                jsons = await streamReader.ReadToEndAsync();
                response.Result = jsons.ToString();
            }
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            await _provider.APIRequestRecord_Log("Checking CMS Status - Cash Drop", JsonConvert.SerializeObject(obj, options), JsonConvert.SerializeObject(jsons, options), "", "");
            return response;
        }
    }
}
