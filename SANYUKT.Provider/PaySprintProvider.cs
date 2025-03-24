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
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using SANYUKT.Repository;
using Org.BouncyCastle.Ocsp;

namespace SANYUKT.Provider
{
    public class PaySprintProvider : BaseProvider
    {
        private readonly SysMgrProvider _sysprd = null;
        private readonly PaySprintIntegratorProvider _pro=null;
        private readonly PaySprintRepository _repository=null;
        public PaySprintProvider()
        {
            _sysprd = new SysMgrProvider();
            _pro=new PaySprintIntegratorProvider();
            _repository=new PaySprintRepository();
        }
        public async Task<SimpleResponse> GenerateToken(ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse xxx = new SimpleResponse();
            // xxx = await _sysprd.GenerateServiceSessionID(2, serviceUser);
            //string RequestID = xxx.Result.ToString();
            string RequestID = "1120001234";
            SimpleResponse response = new SimpleResponse();
            string key = SANYUKTApplicationConfiguration.Instance.PaysprintjwtToken;
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
            else
            {
                response.SetError("Token not Generated");
            }

            return response;
        }
       public static byte[] Encrypt(string piddata, string key, string iv)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(SANYUKTApplicationConfiguration.Instance.PaysprintAESENCRYPTIONKEY); // Make sure key is 16 bytes for AES-128
                aesAlg.IV = Encoding.UTF8.GetBytes(SANYUKTApplicationConfiguration.Instance.PaysprintAESENCRYPTIONIV); // Make sure IV is 16 bytes for AES-128
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                byte[] piddataBytes = Encoding.UTF8.GetBytes(piddata);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(piddataBytes, 0, piddataBytes.Length);

                // Convert the encrypted bytes to a Base64 string
               // return Convert.ToBase64String(encryptedBytes);
               return encryptedBytes;
            }
        }


        public async Task<SpBaseResponse> GetFinoCustomerDetail(GetCustomerRequestView request, ISANYUKTServiceUser serviceUser)
        {
           
            GetCustomerRequest request1 = new GetCustomerRequest();
            SpBaseResponse resp = new SpBaseResponse();
            request1.mobile = request.Mobile;
            resp = await _pro.GenericIntegrator(request1, request.TokenData,1);
            return resp;
        }

        public async Task<SpBaseResponse> FinoCustomerEkyc(FinoEkycRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            SpBaseResponse resp2 = new SpBaseResponse();
            string piddata = request.PidData;
            if (piddata == null) {
                resp2.response_code = "100";
                resp2.message = ErrorCodes.PID_DATA_REQ.ToString();
                resp2.SetError(ErrorCodes.PID_DATA_REQ);
                return resp2;
            }
            byte[] ciphertext_raw=  Encrypt(piddata, "", "");
            string enctoken = Convert.ToBase64String(ciphertext_raw);
            FinoEkycRequest request1 = new FinoEkycRequest();
            request1.mobile = request.Mobile;
            request1.aadhaar_number = request.AadharNo;
            request1.data = enctoken;
            request1.accessmode = request.AccessMode;
            request1.is_iris = request.isIris;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 2);
            //if (resp != null) { 
            //    if(resp.response_code=="1")
            //    {
                    
            //        FinoRegCustomerRequestView objp = new FinoRegCustomerRequestView();
            //        resp2= await FinoRegisterCustomer(objp, serviceUser);
            //        if (resp2 != null)
            //        {
            //            if (resp2.response_code == "1") {
            //                var regdata = (FinoCustomerEkycResponse)resp.data;
            //                PaySprintCreateCustomerRequest req = new PaySprintCreateCustomerRequest();
            //                req.AadharNo = request.AadharNo;
            //                req.FinoKYCId = regdata.ekyc_id;
            //                req.FirstName = request.FirstName;
            //                req.LastName = request.LastName;
            //                req.LastName = request.LastName;
            //                req.MobileNo = request.Mobile;
            //                long custid = await CreateFinoCustomer(req, serviceUser);
            //            }
            //        }
            //        else { 
            //            resp2.SetError(ErrorCodes.INVALID_PARAMETERS);
            //        }
                  

            //    }
            //}
            return resp;
        }

        public async Task<SpBaseResponse> FinoRegisterCustomer(FinoRegCustomerRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinoRegCustomerRequest request1 = new FinoRegCustomerRequest();
            request1.mobile = request.mobile;
            request1.otp = request.otp;
            request1.ekyc_id = request.ekyc_id;
            request1.stateresp = request.stateresp;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 3);
            return resp;
        }

        public async Task<SpBaseResponse> FinoRegisterBenficiary(FinoRegBenRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinoRegBenRequest request1 = new FinoRegBenRequest();
            request1.mobile = request.mobile;
            request1.benename = request.benename;
            request1.verified = request.verified;
            request1.bankid = request.bankid;
            request1.accno = request.accno;
            request1.ifsccode = request.ifsccode;

            resp = await _pro.GenericIntegrator(request1, request.TokenData,4);
            return resp;
        }

        public async Task<SpBaseResponse> FinoDeleteBenficiary(FinoDeleteBenRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinoDeleteBenRequest request1 = new FinoDeleteBenRequest();
            request1.mobile = request.mobile;
            request1.bene_id = request.bene_id;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 5);
            return resp;
        }

        public async Task<SpBaseResponse> FinoFetchBenficiary(FinofetchBenRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinofetchBenRequest request1 = new FinofetchBenRequest();
            request1.mobile = request.mobile;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 6);
            return resp;
        }

        public async Task<SpBaseResponse> FinoFetchBenficiaryByBenID(FinofetchBenRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinofetchBenbybenIDRequest request1 = new FinofetchBenbybenIDRequest();
            request1.beneid = request.beneid;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 7);
            return resp;
        }

        public async Task<SpBaseResponse> FinoPPenyDrop(FinoTransactionRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinoTransactionRequest request1 = new FinoTransactionRequest();
            request1.dob = request.dob;
            request1.benename = request.benename;
            request1.address = request.address;
            request1.bene_id = request.bene_id;
            request1.referenceid = "SP02003034"; // transaction id generated by system
            request1.accno = request.accno;
            request1.mobile = request.mobile;
            request1.gst_state = request.gst_state;
            request1.pincode = request.pincode;
            request1.bankid = request.bankid;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 8);
            return resp;
        }

        public async Task<SpBaseResponse> FinoTransactionOTP(FinoTransactionSendRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinoTransactionSendRequestView request1 = new FinoTransactionSendRequestView();
            request1.bene_id = request.bene_id;
            request1.referenceid = request.referenceid;
            request1.txntype = request.txntype;
            request1.mobile = request.mobile;
            request1.amount = request.amount;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 9);
            return resp;
        }

        public async Task<SpBaseResponse> FinoTransaction(FinoTransactionFinalRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinoTransactionFinalRequest request1 = new FinoTransactionFinalRequest();
            request1.bene_id = request.bene_id;
            request1.referenceid = request.referenceid;
            request1.txntype = request.txntype;
            request1.mobile = request.mobile;
            request1.amount = request.amount;
            request1.otp = request.otp;
            request1.stateresp = request.stateresp;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 10);
            return resp;
        }

        public async Task<SpBaseResponse> FinoTransactionStatus(FinoTransactionStatusRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinoTransactionStatusRequest request1 = new FinoTransactionStatusRequest();
            request1.referenceid = request.referenceid;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 11);
            return resp;
        }

        public async Task<SpBaseResponse> FinoTransactionRefundOTP(FinoRefundOtpRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinoRefundOtpRequest request1 = new FinoRefundOtpRequest();
            request1.referenceid = request.referenceid;
            request1.ackno = request.ackno;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 12);
            return resp;
        }

        public async Task<SpBaseResponse> FinoTransactionRefund(FinoRefundRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinoRefundRequest request1 = new FinoRefundRequest();
            request1.referenceid = request.referenceid;
            request1.ackno = request.ackno;
            request1.otp = request.otp;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 13);
            return resp;
        }

        public async Task<long> CreateFinoCustomer(PaySprintCreateCustomerRequest request, ISANYUKTServiceUser serviceUser)
        {
            long CustomerId = 0;
            CustomerId = await _repository.CreateNewCustomer(request, serviceUser);
            return CustomerId;
        }
    }
}
