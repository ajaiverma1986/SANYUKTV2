using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Agreement.Srp;
using SANYUKT.Commonlib.Cache;
using SANYUKT.Configuration;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.Entities;
using SANYUKT.Datamodel.Entities.Authorization;
using SANYUKT.Datamodel.Entities.RblPayout;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.RblPayoutRequest;
using SANYUKT.Datamodel.RblPayoutResponse;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider.Shared;
using SANYUKT.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SANYUKT.Provider.Payout
{
    public class RblPayoutProvider : BaseProvider
    {
        public readonly TransactionProvider _provider = null;
        public readonly UserDetailsProvider _userprovider= null;
        public RblPayoutProvider() {
            _provider = new TransactionProvider();
            _userprovider= new UserDetailsProvider();
        }
        public async Task<SimpleResponse> GetBalalce(RblPayoutRequest objp, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            AccountBalalnceRequest requestreq = new AccountBalalnceRequest();

            BaseTransactionRequest request1 =new BaseTransactionRequest();
            request1.agencyid = 1;
            request1.serviceid = 1;
            request1.TxnPlateForm = "API";
            request1.partnerid = 1;
            request1.partnerreferenceno = "ABC000001";
            request1.partnerretailorid = "C10000001";
            


            GetAccountBalanceReq aa = new GetAccountBalanceReq();
            HeaderAcc ha = new HeaderAcc();
            ha.Approver_ID = objp.ApproverId;
            ha.Corp_ID = SANYUKTApplicationConfiguration.Instance.RblPayoutCORPID.ToString();
            ha.TranID = await _provider.NewNonFinacialTransaction(request1, serviceUser);
            aa.Header = ha;
            BodyAcc ba = new BodyAcc();
            ba.AcctId = objp.AccountNo;
            aa.Body = ba;
            Signature1 si = new Signature1();
            si.Signature = "";
            aa.Signature = si;
            requestreq.getAccountBalanceReq = aa;

            SimpleResponse response = new SimpleResponse();
            var _clientHandler = new HttpClientHandler();
            _clientHandler.ClientCertificates.Add(Certificatetext);
            _clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var client = new HttpClient(_clientHandler);
            string Url = SANYUKTApplicationConfiguration.Instance.RblPayoutBaseUrl.ToString();
            string fullurl = Url + "test/sb/rbl/v1/accounts/balance/query?client_id=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientId.ToString() + "&client_secret=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientSecrat.ToString();
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

        public async Task<SimpleResponse> PayoutTransaction(SinglePaymentRequest req, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            PaymentRequest requestreq = new PaymentRequest();
            SimpleResponse response = new SimpleResponse();


            bool isavaillimit = false;
            isavaillimit =await _userprovider.CheckAvailableBalance(Convert.ToDecimal( req.Amount), 0, serviceUser);
            if (isavaillimit)
            {
                response.SetError(ErrorCodes.INSUFFICIENT_LIMIT);
                return response;

            }


            NewTransactionRequest request1= new NewTransactionRequest();
            request1.description = "Payout Transaction";
            request1.amount =Convert.ToDecimal ( req.Amount);
            request1.partnerretailorid = req.PartnerRetailorId;
            request1.TxnPlateForm= req.TxnPlateForm;
            request1.agencyid = 1;
            request1.serviceid = 1;
            request1.partnerid = serviceUser.UserMasterID;
            request1.partnerreferenceno = req.PartnerRefNo;
            request1.TxnType = "D";
            TransactionResponse transactionResponse=new TransactionResponse ();

             transactionResponse = await _provider.NewTransaction(request1, serviceUser);
            if (transactionResponse == null)
            {
                response.SetError(ErrorCodes.TRANSACTION_NOT_DONE);
                return response;
            }
            if (transactionResponse.Transactioncode == null)
            {
                response.SetError(ErrorCodes.TRANSACTION_NOT_DONE);
                return response;
            }
            if (transactionResponse.Transactioncode == "")
            {
                response.SetError(ErrorCodes.TRANSACTION_NOT_DONE);
                return response;
            }


            SinglePaymentCorpReq spr = new SinglePaymentCorpReq();
            Signature1 si = new Signature1();
            si.Signature = "";
            spr.Signature = si;
            HeaderPayment hp = new HeaderPayment();
            hp.Approver_ID = req.ApproverId;
            hp.Checker_ID = req.CheckedrId;
            hp.Corp_ID = SANYUKTApplicationConfiguration.Instance.RblPayoutCORPID.ToString();
            hp.TranID = transactionResponse.Transactioncode;
            hp.Maker_ID = req.MakerId.ToString();
            spr.Header = hp;

            BodyPayment bp = new BodyPayment();
            bp.Amount = req.Amount;
            bp.Ben_BranchCd = req.Ben_BranchCd;
            bp.Ben_BankCd = req.Ben_BankCd;
            bp.Ben_Address = "";
            bp.Issue_BranchCd = req.Issue_BranchCd;
            bp.Ben_Acct_No = req.Ben_Acct_No;
            bp.Ben_TrnParticulars = req.Ben_TrnParticulars;
            bp.Ben_PartTrnRmks = req.Ben_PartTrnRmks;
            bp.Debit_PartTrnRmks = "";
            bp.Ben_BankName = req.Ben_BankName;
            bp.Remarks = req.Remarks;
            bp.Ben_Email = "";
            bp.Ben_IFSC = req.Ben_IFSC;
            bp.Ben_Mobile = req.Ben_Mobile;
            bp.Ben_Name = req.Ben_Name;
            bp.Debit_Acct_Name = SANYUKTApplicationConfiguration.Instance.RblAccountName ;
            bp.Debit_Acct_No = SANYUKTApplicationConfiguration.Instance.RblAccountNo;
            bp.Debit_IFSC =  SANYUKTApplicationConfiguration.Instance.RblPayoutIfsccode;
            bp.Debit_Mobile = SANYUKTApplicationConfiguration.Instance.RblPayoutIfsccode;
            bp.Debit_TrnParticulars = req.Debit_TrnParticulars;
            bp.Mode_of_Pay = req.Mode_of_Pay;
            spr.Body = bp;
            requestreq.Single_Payment_Corp_Req = spr;



           
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

        public async Task<RblResponse> PayoutTransactionStatus(SinglePaymentStatus obbb, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();

            BaseTransactionRequest request1 = new BaseTransactionRequest();
            request1.agencyid = 1;
            request1.serviceid = 1;
            request1.TxnPlateForm = obbb.TxnPlateForm;
            request1.partnerid = serviceUser.UserMasterID;
            request1.partnerreferenceno =obbb.PartnerRefNo;
            request1.partnerretailorid = obbb.PartnerRetailorId;
            request1.TxnType = "Transaction Status";

            RblResponse resp =new RblResponse();
            PaymentStatusRequest requestreq = new PaymentStatusRequest();
            GetSinglePaymentStatusCorpReq gsps = new GetSinglePaymentStatusCorpReq();
            Signature1 si = new Signature1();
            si.Signature = "";

            gsps.Signature = si;
            Headerstaus hd = new Headerstaus();
            hd.Approver_ID = obbb.ApproverId;
            hd.TranID = await _provider.NewNonFinacialTransaction(request1, serviceUser);
            hd.Corp_ID = SANYUKTApplicationConfiguration.Instance.RblPayoutCORPID;
            hd.Maker_ID = obbb.MakerId;
            hd.Checker_ID = obbb.CheckedrId;
            BodyStatus bs = new BodyStatus();
            bs.UTRNo = obbb.UTRNo;
            gsps.Header = hd;
            requestreq.get_Single_Payment_Status_Corp_Req = gsps;

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
            GetsinglePaymentReponse nnn=new GetsinglePaymentReponse ();
            UpdateNonfinacialRequest request2 =new UpdateNonfinacialRequest ();
            if (response1.StatusCode == HttpStatusCode.OK)
            {
                string jsonData = JsonConvert.SerializeObject(response.Result);
                dynamic jsonn = JsonConvert.DeserializeObject(jsonData);
             
                string strRemSlash = jsonn.Replace("\"", "\'");
                string strRemNline = strRemSlash.Replace("\n", " ");
                // Time to desrialize it to convert it into an object class.
                 nnn = JsonConvert.DeserializeObject<GetsinglePaymentReponse>(@strRemNline);
                resp.Status = nnn.get_Single_Payment_Status_Corp_Res.Header.Status;
                resp.ChanelPartnerRefNo = obbb.PartnerRefNo;
                resp.ErorrDescription = nnn.get_Single_Payment_Status_Corp_Res.Header.Error_Desc;
                resp.ErrorCode = nnn.get_Single_Payment_Status_Corp_Res.Header.Error_Cde;
                resp.TransactionId = nnn.get_Single_Payment_Status_Corp_Res.Header.TranID;

                request2.errorcode = resp.ErrorCode;
                request2.errorDescrtiopn = resp.ErorrDescription;
                request2.Txncode = resp.TransactionId;
              await _provider.UpdateNonFinacialTransaction(request2, serviceUser);
            }
            else if (response1.StatusCode==HttpStatusCode.Unauthorized)
            {
               
                resp.Status = "Unauthorized";
                resp.ChanelPartnerRefNo = obbb.PartnerRefNo;
                resp.ErorrDescription = "";
                resp.ErrorCode = response1.ReasonPhrase;
                resp.TransactionId = requestreq.get_Single_Payment_Status_Corp_Req.Header.TranID;

                request2.errorcode = response1.ReasonPhrase;
                request2.errorDescrtiopn = response1.ReasonPhrase;
                request2.Txncode = requestreq.get_Single_Payment_Status_Corp_Req.Header.TranID;
                await _provider.UpdateNonFinacialTransaction(request2, serviceUser);

            }

            return resp;
        }

        public async Task<SimpleResponse> AccountStatement(RblPayoutStatementRequest obbb, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();

            AccountstatementRequest requestreq = new AccountstatementRequest();
            AccStmtDtRngReq asdt = new AccStmtDtRngReq();
            Signature1 si = new Signature1();
            asdt.Signature = si;
            HeaderStatement hs = new HeaderStatement();
            hs.Approver_ID = obbb.ApproverId;
            hs.Corp_ID = SANYUKTApplicationConfiguration.Instance.RblPayoutCORPID;
            hs.TranID = obbb.PartnerRefNo;
            asdt.Header = hs;
            BodyStatement bs = new BodyStatement();
            bs.From_Dt = obbb.From_Dt;
            bs.To_Dt = obbb.To_Dt;
            bs.Tran_Type = obbb.Tran_Type;
            PaginationDetails pg = new PaginationDetails();
            pg.Last_Balance = obbb.Pagination_Details.Last_Balance;
            pg.Last_Pstd_Date = obbb.Pagination_Details.Last_Pstd_Date;
            pg.Last_Txn_Date = obbb.Pagination_Details.Last_Txn_Date;
            pg.Last_Txn_Id = obbb.Pagination_Details.Last_Txn_Id;
            pg.Last_Txn_SrlNo = obbb.Pagination_Details.Last_Txn_SrlNo;
            bs.Pagination_Details = pg;
            asdt.Body = bs;
            requestreq.Acc_Stmt_DtRng_Req = asdt;


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
