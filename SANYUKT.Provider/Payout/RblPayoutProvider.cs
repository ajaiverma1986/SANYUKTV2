using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Agreement.Srp;
using SANYUKT.Commonlib.Cache;
using SANYUKT.Configuration;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.Entities.Authorization;
using SANYUKT.Datamodel.Entities.RblPayout;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.RblPayoutRequest;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider.Shared;
using SANYUKT.Repository;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SANYUKT.Provider.Payout
{
    public class RblPayoutProvider : BaseProvider
    {
        public readonly RblPayoutRepository _repository = null;
        public RblPayoutProvider() {
            _repository=new RblPayoutRepository ();
        }
        public async Task<SimpleResponse> GetBalalce(RblPayoutRequest objp, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            AccountBalalnceRequest requestreq = new AccountBalalnceRequest();

            NonFinancialTxnRequest request1 =new NonFinancialTxnRequest();
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
            ha.TranID = await _repository.NewNonFinacialTransaction(request1, serviceUser);
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
            SinglePaymentCorpReq spr = new SinglePaymentCorpReq();
            Signature1 si = new Signature1();
            si.Signature = "";
            spr.Signature = si;
            HeaderPayment hp = new HeaderPayment();
            hp.Approver_ID = req.ApproverId;
            hp.Checker_ID = req.CheckedrId;
            hp.Corp_ID = SANYUKTApplicationConfiguration.Instance.RblPayoutCORPID.ToString();
            hp.TranID = req.SessionTransactionID.ToString();
            hp.Maker_ID = req.MakerId.ToString();
            spr.Header = hp;

            BodyPayment bp = new BodyPayment();
            bp.Amount = req.Amount;
            bp.Remarks = req.Remarks;
            bp.Ben_BranchCd = req.Ben_BranchCd;
            bp.Ben_BankCd = req.Ben_BankCd;
            bp.Ben_Address = req.Ben_Address;
            bp.Issue_BranchCd = req.Issue_BranchCd;
            bp.Ben_Acct_No = req.Ben_Acct_No;
            bp.Ben_TrnParticulars = req.Ben_TrnParticulars;
            bp.Ben_PartTrnRmks = req.Ben_PartTrnRmks;
            bp.Debit_PartTrnRmks = req.Debit_PartTrnRmks;
            bp.Ben_BankName = req.Ben_BankName;
            bp.Remarks = req.Remarks;
            bp.Ben_Email = req.Ben_Email;
            bp.Ben_IFSC = req.Ben_IFSC;
            bp.Ben_Mobile = req.Ben_Mobile;
            bp.Ben_Name = req.Ben_Name;
            bp.Debit_Acct_Name = req.Debit_Acct_Name;
            bp.Debit_Acct_No = req.Debit_Acct_No;
            bp.Debit_IFSC = req.Debit_IFSC;
            bp.Debit_Mobile = req.Debit_Mobile;
            bp.Debit_TrnParticulars = req.Debit_TrnParticulars;
            bp.Mode_of_Pay = req.Mode_of_Pay;
            spr.Body = bp;
            requestreq.Single_Payment_Corp_Req = spr;



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

        public async Task<SimpleResponse> PayoutTransactionStatus(SinglePaymentStatus obbb, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();

            PaymentStatusRequest requestreq = new PaymentStatusRequest();
            GetSinglePaymentStatusCorpReq gsps = new GetSinglePaymentStatusCorpReq();
            Signature1 si = new Signature1();
            si.Signature = "";

            gsps.Signature = si;
            Headerstaus hd = new Headerstaus();
            hd.Approver_ID = obbb.ApproverId;
            hd.TranID = obbb.SessionTransactionID;
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

            return response;
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
            hs.TranID = obbb.SessionTransactionID;
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
