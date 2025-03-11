using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using SANYUKT.Configuration;
using SANYUKT.Datamodel.Paysprint;
using SANYUKT.Provider.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SANYUKT.Provider
{
    public class PaySprintIntegratorProvider:BaseProvider
    {
        public PaySprintIntegratorProvider()
        {

        }
        public async Task<SpBaseResponse> GenericIntegrator<T>(T jsonRequest, string TokenData, int Apiid)
        {
            WebRequest objRequest;
            string jsondata = "";
            string jsons = "";
            string APIURL = "";
            SpBaseResponse resp = new SpBaseResponse();
            try
            {
                if (Apiid == 1)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/remitter/queryremitter";
                }
                else if (Apiid == 2)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/remitter/queryremitter/kyc";
                }
                else if (Apiid == 3)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/remitter/registerremitter";
                }
                else if (Apiid == 4)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/beneficiary/registerbeneficiary";
                }
                else if (Apiid == 5)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/beneficiary/registerbeneficiary/deletebeneficiary";
                }
                else if (Apiid == 6)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/beneficiary/registerbeneficiary/fetchbeneficiary";
                }
                else if (Apiid == 7)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/beneficiary/registerbeneficiary/fetchbeneficiary";
                }
                else if (Apiid == 8)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/beneficiary/registerbeneficiary/benenameverify";
                }
                else if (Apiid == 9)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/transact/transact/send_otp";
                }
                else if (Apiid == 10)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/transact/transact";
                }
                else if (Apiid == 11)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/transact/transact/querytransact";
                }
                else if (Apiid == 12)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/refund/refund/resendotp";
                }
                else if (Apiid == 13)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt/kyc/refund/refund/resendotp";
                }
                else if (Apiid == 14)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt-v2/remitter/queryremitter";
                }
                else if (Apiid == 15)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt-v2/remitter/queryremitter/aadhar_verify";
                }
                else if (Apiid == 16)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt-v2/remitter/registerremitter";
                }
                else if (Apiid == 17)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt-v2/beneficiary/registerbeneficiary";
                }
                else if (Apiid == 18)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt-v2/beneficiary/registerbeneficiary/deletebeneficiary";
                }
                else if (Apiid == 19)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt-v2/beneficiary/registerbeneficiary/fetchbeneficiary";
                }
                else if (Apiid == 20)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt-v2/beneficiary/registerbeneficiary/fetchbeneficiarybybeneid";
                }
                else if (Apiid == 21)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt-v2/beneficiary/registerbeneficiary/benenameverify";
                }
                else if (Apiid == 22)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt-v2/transact/transact/send_otp";
                }
                else if (Apiid == 23)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt-v2/transact/transact";
                }
                else if (Apiid == 24)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt-v2/transact/transact/querytransact";
                }
                else if (Apiid == 25)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt-v2/refund/refund/resendotp";
                }
                else if (Apiid == 26)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/dmt-v2/refund/refund";
                }
                else if (Apiid == 27)
                {
                    APIURL = SANYUKTApplicationConfiguration.Instance.PaysprintBaseUrl + "service-api/api/v1/service/cc-payment/ccpayment/generateotp";
                }

                objRequest = WebRequest.Create(APIURL);
                objRequest.ContentType = "application/json";
                objRequest.Method = "POST";
                objRequest.Headers.Add("Token", TokenData);
                objRequest.Headers.Add("Authorisedkey", SANYUKTApplicationConfiguration.Instance.PaysprintAuthKey);
                var options = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                jsondata = JsonConvert.SerializeObject(jsonRequest, options);
                using (var streamWriter = new StreamWriter(objRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsondata);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (WebResponse)objRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    jsons = await streamReader.ReadToEndAsync();
                }
                dynamic json = JsonConvert.DeserializeObject(jsons);
                resp.status = json.status;
                resp.message = json.message;
                resp.response_code = json.response_code;
                object innerjson = json.data;
                dynamic json2 = JsonConvert.DeserializeObject(innerjson.ToString());
                if (Apiid == 1)
                {
                    FinoCustomerLimitResponse resp2 = new FinoCustomerLimitResponse();
                    resp2.mobile = json2.mobile;
                    resp2.limit = json2.limit ?? "0";
                    resp.data = resp2;
                }
                else if (Apiid == 2)
                {
                    FinoCustomerEkycResponse resp2 = new FinoCustomerEkycResponse();
                    resp2.mobile = json2.mobile;
                    resp2.ekyc_id = json2.limit ?? "";
                    resp2.stateresp = json2.stateresp ?? "";
                    resp.data = resp2;
                }
                else if (Apiid == 3)
                {
                    FinoCustomerLimitResponse resp2 = new FinoCustomerLimitResponse();
                    resp2.mobile = json2.mobile;
                    resp2.limit = json2.limit ?? "0";
                    resp.data = resp2;
                }
                else if (Apiid == 4)
                {
                    FinoRegisterBenResponse resp2 = new FinoRegisterBenResponse();
                    resp2.paytm = json2.paytm;
                    resp2.ifsc = json2.ifsc;
                    resp2.accno = json2.accno;
                    resp2.bene_id = json2.bene_id;
                    resp2.bankid = json2.bankid;
                    resp2.banktype = json2.banktype;
                    resp2.bankname = json2.bankname;
                    resp2.name = json2.name;
                    resp2.verified = json2.verified;
                    resp.data = resp2;
                }
                else if (Apiid == 6)
                {
                    FinoRegisterBenResponse resp2 = new FinoRegisterBenResponse();
                    resp2.paytm = json2.paytm;
                    resp2.ifsc = json2.ifsc;
                    resp2.accno = json2.accno;
                    resp2.bene_id = json2.bene_id;
                    resp2.bankid = json2.bankid;
                    resp2.banktype = json2.banktype;
                    resp2.bankname = json2.bankname;
                    resp2.name = json2.name;
                    resp2.verified = json2.verified;
                    resp.data = resp2;
                }
                else if (Apiid == 7)
                {
                    FinoRegisterBenResponse resp2 = new FinoRegisterBenResponse();
                    resp2.paytm = json2.paytm;
                    resp2.ifsc = json2.ifsc;
                    resp2.accno = json2.accno;
                    resp2.bene_id = json2.bene_id;
                    resp2.bankid = json2.bankid;
                    resp2.banktype = json2.banktype;
                    resp2.bankname = json2.bankname;
                    resp2.name = json2.name;
                    resp2.verified = json2.verified;
                    resp.data = resp2;
                }
                else if (Apiid == 8)
                {
                    FinoTransactionResponse resp2 = new FinoTransactionResponse();
                    resp2.utr = json.utr;
                    resp2.account_number = json.account_number;
                    resp2.txn_amount = json.txn_amount;
                    resp2.txn_status = json.txn_status;
                    resp2.paysprint_share = json.paysprint_share;
                    resp2.ackno = json.ackno;
                    resp2.balance = json.balance;
                    resp2.benename = json.benename;
                    resp2.customercharge = json.customercharge;
                    resp2.netcommission = json.netcommission;
                    resp2.gst = json.gst;
                    resp2.tds = json.tds;
                    resp2.remarks = json.remarks;
                    resp2.remitter = json.remitter;
                    resp.data = resp2;
                }
                else if (Apiid == 9)
                {
                    FinoTransactionOTPResponse resp2 = new FinoTransactionOTPResponse();
                    resp2.stateresp = json.stateresp;
                    resp.data = resp2;
                }
                else if (Apiid == 10)
                {
                    FinoTransactionResponse resp2 = new FinoTransactionResponse();
                    resp2.utr = json.utr;
                    resp2.account_number = json.account_number;
                    resp2.txn_amount = json.txn_amount;
                    resp2.txn_status = json.txn_status;
                    resp2.paysprint_share = json.paysprint_share;
                    resp2.ackno = json.ackno;
                    resp2.balance = json.balance;
                    resp2.benename = json.benename;
                    resp2.customercharge = json.customercharge;
                    resp2.netcommission = json.netcommission;
                    resp2.gst = json.gst;
                    resp2.tds = json.tds;
                    resp2.remarks = json.remarks;
                    resp2.remitter = json.remitter;
                    resp.data = resp2;
                }
                else if (Apiid == 11)
                {
                    FinoTransactionStatusResponse resp2 = new FinoTransactionStatusResponse();
                    resp2.utr = json.utr;
                    resp2.account = json.account;
                    resp2.amount = json.amount;
                    resp2.referenceid = json.referenceid;
                    resp2.txn_status = json.txn_status;
                    resp2.ackno = json.ackno;
                    resp2.customercharge = json.customercharge;
                    resp2.netcommission = json.netcommission;
                    resp2.gst = json.gst;
                    resp2.tds = json.tds;
                    resp2.refundtxnid = json.refundtxnid;
                    resp2.refundtxnid = json.refundtxnid;
                    resp.data = resp2;
                }
                else if (Apiid == 14)
                {
                    FinoCustomerLimitResponse resp2 = new FinoCustomerLimitResponse();
                    resp2.mobile = json2.mobile;
                    resp2.limit = json2.limit ?? "0";
                    resp.data = resp2;
                }
                else if (Apiid == 15)
                {
                    FinoTransactionOTPResponse resp2 = new FinoTransactionOTPResponse();
                    resp2.stateresp = json.stateresp;
                    resp.data = resp2;
                }
                else if (Apiid == 16)
                {
                    FinoCustomerLimitResponse resp2 = new FinoCustomerLimitResponse();
                    resp2.mobile = json2.mobile;
                    resp2.limit = json2.limit ?? "0";
                    resp.data = resp2;
                }
                else if (Apiid == 17)
                {
                    FinoRegisterBenResponse resp2 = new FinoRegisterBenResponse();
                    resp2.paytm = json2.paytm;
                    resp2.ifsc = json2.ifsc;
                    resp2.accno = json2.accno;
                    resp2.bene_id = json2.bene_id;
                    resp2.bankid = json2.bankid;
                    resp2.banktype = json2.banktype;
                    resp2.bankname = json2.bankname;
                    resp2.name = json2.name;
                    resp2.verified = json2.verified;
                    resp.data = resp2;
                }
                else if (Apiid == 19)
                {
                    FinoRegisterBenResponse resp2 = new FinoRegisterBenResponse();
                    resp2.paytm = json2.paytm;
                    resp2.ifsc = json2.ifsc;
                    resp2.accno = json2.accno;
                    resp2.bene_id = json2.bene_id;
                    resp2.bankid = json2.bankid;
                    resp2.banktype = json2.banktype;
                    resp2.bankname = json2.bankname;
                    resp2.name = json2.name;
                    resp2.verified = json2.verified;
                    resp.data = resp2;
                }
                else if (Apiid == 20)
                {
                    FinoRegisterBenResponse resp2 = new FinoRegisterBenResponse();
                    resp2.paytm = json2.paytm;
                    resp2.ifsc = json2.ifsc;
                    resp2.accno = json2.accno;
                    resp2.bene_id = json2.bene_id;
                    resp2.bankid = json2.bankid;
                    resp2.banktype = json2.banktype;
                    resp2.bankname = json2.bankname;
                    resp2.name = json2.name;
                    resp2.verified = json2.verified;
                    resp.data = resp2;
                }
                else if (Apiid==21)
                {

                    FinoTransactionResponse resp2 = new FinoTransactionResponse();
                    resp2.utr = json.utr;
                    resp2.account_number = json.account_number;
                    resp2.txn_amount = json.txn_amount;
                    resp2.txn_status = json.txn_status;
                    resp2.paysprint_share = json.paysprint_share;
                    resp2.ackno = json.ackno;
                    resp2.balance = json.balance;
                    resp2.benename = json.benename;
                    resp2.customercharge = json.customercharge;
                    resp2.netcommission = json.netcommission;
                    resp2.gst = json.gst;
                    resp2.tds = json.tds;
                    resp2.remarks = json.remarks;
                    resp2.remitter = json.remitter;
                    resp.data = resp2;
                }
                else if (Apiid == 22)
                {

                    FinoTransactionOTPResponse resp2 = new FinoTransactionOTPResponse();
                    resp2.stateresp = json.stateresp;
                    resp.data = resp2;
                }
                else if(Apiid==23)
                {
                    FinoTransactionResponse resp2 = new FinoTransactionResponse();
                    resp2.utr = json.utr;
                    resp2.account_number = json.account_number;
                    resp2.txn_amount = json.txn_amount;
                    resp2.txn_status = json.txn_status;
                    resp2.paysprint_share = json.paysprint_share;
                    resp2.ackno = json.ackno;
                    resp2.balance = json.balance;
                    resp2.benename = json.benename;
                    resp2.customercharge = json.customercharge;
                    resp2.netcommission = json.netcommission;
                    resp2.gst = json.gst;
                    resp2.tds = json.tds;
                    resp2.remarks = json.remarks;
                    resp2.remitter = json.remitter;
                    resp.data = resp2;
                }
                else if (Apiid == 24)
                {
                    FinoTransactionStatusResponse resp2 = new FinoTransactionStatusResponse();
                    resp2.utr = json.utr;
                    resp2.account = json.account;
                    resp2.amount = json.amount;
                    resp2.referenceid = json.referenceid;
                    resp2.txn_status = json.txn_status;
                    resp2.ackno = json.ackno;
                    resp2.customercharge = json.customercharge;
                    resp2.netcommission = json.netcommission;
                    resp2.gst = json.gst;
                    resp2.tds = json.tds;
                    resp2.refundtxnid = json.refundtxnid;
                    resp2.refundtxnid = json.refundtxnid;
                    resp.data = resp2;
                }
            }
            catch (Exception ex)
            {
                resp.status = false;
                resp.message = ex.Message;
            }
            return resp;
        }
    }
}
