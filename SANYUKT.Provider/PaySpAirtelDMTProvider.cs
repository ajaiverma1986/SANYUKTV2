using SANYUKT.Configuration;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.Paysprint;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider.Shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SANYUKT.Provider
{
    public class PaySpAirtelDMTProvider:BaseProvider
    {
        private readonly SysMgrProvider _sysprd = null;
        private readonly PaySprintIntegratorProvider _pro = null;
        public PaySpAirtelDMTProvider()
        {
            _sysprd = new SysMgrProvider();
            _pro = new PaySprintIntegratorProvider();
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


        public async Task<SpBaseResponse> GetAirtelCustomerDetail(GetCustomerRequestView request, ISANYUKTServiceUser serviceUser)
        {
            GetCustomerRequest request1 = new GetCustomerRequest();
            SpBaseResponse resp = new SpBaseResponse();
            request1.mobile = request.Mobile;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 14);
            return resp;
        }

        public async Task<SpBaseResponse> AirtelVerifyAadhar(AirtelVerifyAadharRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();

            AirtelVerifyAadharRequest request1 = new AirtelVerifyAadharRequest();
            request1.mobile = request.mobile;
            request1.aadhaar_no = request.aadhaar_no;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 15);
            return resp;
        }

        public async Task<SpBaseResponse> AirtelRegisterCustomer(AirtelRegCustomerRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            AirtelRegCustomerRequest request1 = new AirtelRegCustomerRequest();

            string piddata = request.data;
            byte[] key = Convert.FromBase64String(SANYUKTApplicationConfiguration.Instance.PaysprintAESENCRYPTIONKEY);
            byte[] iv = Convert.FromBase64String(SANYUKTApplicationConfiguration.Instance.PaysprintAESENCRYPTIONIV);

            byte[] ciphertext_raw = Encrypt(piddata, key, iv);
            string enctoken = Convert.ToBase64String(ciphertext_raw);

            request1.mobile = request.mobile;
            request1.otp = request.otp;
            request1.data = enctoken;
            request1.stateresp = request.stateresp;
            request1.accessmode = request.accessmode;
            request1.is_iris = request.is_iris;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 16);
            return resp;
        }

        public async Task<SpBaseResponse> AirtelRegisterBenficiary(FinoRegBenRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinoRegBenRequest request1 = new FinoRegBenRequest();
            request1.mobile = request.mobile;
            request1.benename = request.benename;
            request1.verified = request.verified;
            request1.bankid = request.bankid;
            request1.accno = request.accno;
            request1.ifsccode = request.ifsccode;

            resp = await _pro.GenericIntegrator(request1, request.TokenData, 17);
            return resp;
        }

        public async Task<SpBaseResponse> AirtelDeleteBenficiary(FinoDeleteBenRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinoDeleteBenRequest request1 = new FinoDeleteBenRequest();
            request1.mobile = request.mobile;
            request1.bene_id = request.bene_id;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 18);
            return resp;
        }

        public async Task<SpBaseResponse> AirtelFetchBenficiary(FinofetchBenRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinofetchBenRequest request1 = new FinofetchBenRequest();
            request1.mobile = request.mobile;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 19);
            return resp;
        }

        public async Task<SpBaseResponse> AirtelFetchBenficiaryByBenID(FinofetchBenRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinofetchBenbybenIDRequest request1 = new FinofetchBenbybenIDRequest();
            request1.beneid = request.beneid;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 20);
            return resp;
        }

        public async Task<SpBaseResponse> AirtelPPenyDrop(FinoTransactionRequestView request, ISANYUKTServiceUser serviceUser)
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
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 21);
            return resp;
        }

        public async Task<SpBaseResponse> AirtelTransactionOTP(FinoTransactionSendRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinoTransactionSendRequestView request1 = new FinoTransactionSendRequestView();
            request1.bene_id = request.bene_id;
            request1.referenceid = request.referenceid;
            request1.txntype = request.txntype;
            request1.mobile = request.mobile;
            request1.amount = request.amount;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 22);
            return resp;
        }

        public async Task<SpBaseResponse> AirtelTransaction(FinoTransactionFinalRequestView request, ISANYUKTServiceUser serviceUser)
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
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 23);
            return resp;
        }

        public async Task<SpBaseResponse> AirtelTransactionStatus(FinoTransactionStatusRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinoTransactionStatusRequest request1 = new FinoTransactionStatusRequest();
            request1.referenceid = request.referenceid;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 24);
            return resp;
        }

        public async Task<SpBaseResponse> AirtelTransactionRefundOTP(FinoRefundOtpRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinoRefundOtpRequest request1 = new FinoRefundOtpRequest();
            request1.referenceid = request.referenceid;
            request1.ackno = request.ackno;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 25);
            return resp;
        }

        public async Task<SpBaseResponse> AirtelTransactionRefund(FinoRefundRequestView request, ISANYUKTServiceUser serviceUser)
        {
            SpBaseResponse resp = new SpBaseResponse();
            FinoRefundRequest request1 = new FinoRefundRequest();
            request1.referenceid = request.referenceid;
            request1.ackno = request.ackno;
            request1.otp = request.otp;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 26);
            return resp;
        }
        public async Task<SpBaseResponse> AirtelCMSGenerateURL(AirtelCMSLNKGENRequestView request, ISANYUKTServiceUser serviceUser)
        {
            AirtelCMSLNKGENRequest request1 = new AirtelCMSLNKGENRequest();
            SpBaseResponse resp = new SpBaseResponse();
            request1.latitude = request.latitude;
            request1.refid = request.refid;
            request1.latitude = request.latitude;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 32);
            return resp;
        }
    }
}
