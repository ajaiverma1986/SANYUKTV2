using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto.Agreement.Srp;
using Org.BouncyCastle.Ocsp;
using SANYUKT.Commonlib.Cache;
using SANYUKT.Configuration;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.Entities;
using SANYUKT.Datamodel.Entities.Authorization;
using SANYUKT.Datamodel.Entities.RblPayout;
using SANYUKT.Datamodel.Entities.Transactions;
using SANYUKT.Datamodel.Entities.Users;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.Masters;
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
        public readonly CommonProvider _commonProvider = null;
        public readonly MasterDataRepository _masterdatarepos = null;
        public RblPayoutProvider() {
            _provider = new TransactionProvider();
            _userprovider= new UserDetailsProvider();
            _commonProvider= new CommonProvider();
            _masterdatarepos= new MasterDataRepository();
        }

        public async Task<SimpleResponse> GetBalalceNew (RblPayoutRequest objp, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            AccountBalalnceRequest requestreq = new AccountBalalnceRequest();
            RblAccountBalalnceResponse resp = new RblAccountBalalnceResponse();
            BaseTransactionRequest request1 = new BaseTransactionRequest();
            request1.agencyid = 1;
            request1.serviceid = 1;
            request1.TxnPlateForm = "API";
            request1.partnerid = serviceUser.UserMasterID;
            request1.partnerreferenceno = "";
            request1.partnerretailorid ="";



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
            await _commonProvider.ApilogResponse("RBL Payout Balalnce Enq", fullurl, "", jsonstr, response.Result.ToString());
            GetAccbalResponse nnn = new GetAccbalResponse();
            UpdateNonfinacialRequest request2 = new UpdateNonfinacialRequest();
            if (response1.StatusCode == HttpStatusCode.OK)
            {
                string jsonData = JsonConvert.SerializeObject(response.Result);
                dynamic jsonn = JsonConvert.DeserializeObject(jsonData);

                string strRemSlash = jsonn.Replace("\"", "\'");
                string strRemNline = strRemSlash.Replace("\n", " ");
                // Time to desrialize it to convert it into an object class.
                nnn = JsonConvert.DeserializeObject<GetAccbalResponse>(@strRemNline);
                resp.Status = nnn.getAccountBalanceRes.Header.Status;
                resp.ChanelPartnerRefNo = objp.PartnerRefNo;
                resp.ErorrDescription = nnn.getAccountBalanceRes.Header.Error_Desc;
                resp.ErrorCode = nnn.getAccountBalanceRes.Header.Error_Cde;
                resp.TransactionId = nnn.getAccountBalanceRes.Header.TranID;
                if (resp.Status == "FAILED")
                {
                    resp.BalalnceAmount = "0.00";
                }
                else
                {
                    resp.BalalnceAmount = nnn.getAccountBalanceRes.Body.BalAmt.amountValue;
                }

                request2.errorcode = resp.ErrorCode;
                request2.errorDescrtiopn = resp.ErorrDescription;
                request2.Txncode = resp.TransactionId;
                await _provider.UpdateNonFinacialTransaction(request2, serviceUser);
            }
            else if (response1.StatusCode == HttpStatusCode.Unauthorized)
            {

                resp.Status = "Unauthorized";
                resp.ChanelPartnerRefNo = objp.PartnerRefNo;
                resp.ErorrDescription = "";
                resp.ErrorCode = response1.ReasonPhrase;
                resp.TransactionId = ha.TranID;
                resp.BalalnceAmount = "0.00";

                request2.errorcode = response1.ReasonPhrase;
                request2.errorDescrtiopn = response1.ReasonPhrase;
                request2.Txncode = ha.TranID;
                await _provider.UpdateNonFinacialTransaction(request2, serviceUser);

            }
            else if (response1.StatusCode == HttpStatusCode.InternalServerError)
            {

                resp.Status = "InternalServerError";
                resp.ChanelPartnerRefNo = objp.PartnerRefNo;
                resp.ErorrDescription = "";
                resp.ErrorCode = response1.ReasonPhrase;
                resp.TransactionId = ha.TranID;
                resp.BalalnceAmount = "0.00";

                request2.errorcode = response1.ReasonPhrase;
                request2.errorDescrtiopn = response1.ReasonPhrase;
                request2.Txncode = ha.TranID;
                await _provider.UpdateNonFinacialTransaction(request2, serviceUser);

            }
            response.Result = resp;

            return response;
        }
        public async Task<SimpleResponse> GetBalalce(RblPayoutRequest objp, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            AccountBalalnceRequest requestreq = new AccountBalalnceRequest();
            RblAccountBalalnceResponse resp =new RblAccountBalalnceResponse();
            BaseTransactionRequest request1 =new BaseTransactionRequest();
            request1.agencyid = 1;
            request1.serviceid = 1;
            request1.TxnPlateForm = "API";
            request1.partnerid = serviceUser.UserMasterID;
            request1.partnerreferenceno = objp.PartnerRefNo;
            request1.partnerretailorid = objp.PartnerRetailorId;
            


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
            await _commonProvider.ApilogResponse("RBL Payout Balalnce Enq", fullurl, "", jsonstr, response.Result.ToString());
            GetAccbalResponse nnn = new GetAccbalResponse();
            UpdateNonfinacialRequest request2 = new UpdateNonfinacialRequest();
            if (response1.StatusCode == HttpStatusCode.OK)
            {
                string jsonData = JsonConvert.SerializeObject(response.Result);
                dynamic jsonn = JsonConvert.DeserializeObject(jsonData);

                string strRemSlash = jsonn.Replace("\"", "\'");
                string strRemNline = strRemSlash.Replace("\n", " ");
                // Time to desrialize it to convert it into an object class.
                nnn = JsonConvert.DeserializeObject<GetAccbalResponse>(@strRemNline);
                resp.Status = nnn.getAccountBalanceRes.Header.Status;
                resp.ChanelPartnerRefNo = objp.PartnerRefNo;
                resp.ErorrDescription = nnn.getAccountBalanceRes.Header.Error_Desc;
                resp.ErrorCode = nnn.getAccountBalanceRes.Header.Error_Cde;
                resp.TransactionId = nnn.getAccountBalanceRes.Header.TranID;
                if(resp.Status== "FAILED")
                {
                    resp.BalalnceAmount = "0.00";
                }
                else
                {
                    resp.BalalnceAmount = nnn.getAccountBalanceRes.Body.BalAmt.amountValue;
                }

                request2.errorcode = resp.ErrorCode;
                request2.errorDescrtiopn = resp.ErorrDescription;
                request2.Txncode = resp.TransactionId;
                await _provider.UpdateNonFinacialTransaction(request2, serviceUser);
            }
            else if (response1.StatusCode == HttpStatusCode.Unauthorized)
            {

                resp.Status = "Unauthorized";
                resp.ChanelPartnerRefNo = objp.PartnerRefNo;
                resp.ErorrDescription = "";
                resp.ErrorCode = response1.ReasonPhrase;
               resp.TransactionId = ha.TranID;
                resp.BalalnceAmount = "0.00";

                request2.errorcode = response1.ReasonPhrase;
                request2.errorDescrtiopn = response1.ReasonPhrase;
                request2.Txncode = ha.TranID;
                await _provider.UpdateNonFinacialTransaction(request2, serviceUser);

            }
            else if (response1.StatusCode == HttpStatusCode.InternalServerError)
            {

                resp.Status = "InternalServerError";
                resp.ChanelPartnerRefNo = objp.PartnerRefNo;
                resp.ErorrDescription = "";
                resp.ErrorCode = response1.ReasonPhrase;
                resp.TransactionId = ha.TranID;
                resp.BalalnceAmount = "0.00";

                request2.errorcode = response1.ReasonPhrase;
                request2.errorDescrtiopn = response1.ReasonPhrase;
                request2.Txncode = ha.TranID;
                await _provider.UpdateNonFinacialTransaction(request2, serviceUser);

            }
            response .Result = resp;

            return response;
        }

        public async Task<SimpleResponse> PayoutTransactionwithoutBen(SinglePaymentRequest req, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            PaymentRequest requestreq = new PaymentRequest();
            SimpleResponse response = new SimpleResponse();
            RblTransactionResponse resp = new RblTransactionResponse();
            decimal txnFee = 0;
            decimal Margin=0;

            //service list validation
            ServiceListResponse serviceList = new ServiceListResponse();

            serviceList = await _masterdatarepos.GetAllServcieList(1);

            if (serviceList == null)
            {
                response.SetError(ErrorCodes.SP_123);
                return response;
            }
            if (serviceList.ServiceAccountNo == null)
            {
                response.SetError(ErrorCodes.SP_124);
                return response;
            }
            if (serviceList.ServiceAccountNo == "")
            {
                response.SetError(ErrorCodes.SP_124);
                return response;
            }
            if (serviceList.ServcieIfsccode == null)
            {
                response.SetError(ErrorCodes.SP_125);
                return response;
            }
            if (serviceList.ServcieIfsccode == "")
            {
                response.SetError(ErrorCodes.SP_125);
                return response;
            }
            if (serviceList.ServiceAccName == null)
            {
                response.SetError(ErrorCodes.SP_126);
                return response;
            }
            if (serviceList.ServiceAccName == "")
            {
                response.SetError(ErrorCodes.SP_126);
                return response;
            }
            if (serviceList.ServiceMobileNo == null)
            {
                response.SetError(ErrorCodes.SP_127);
                return response;
            }
            if (serviceList.ServiceMobileNo == "")
            {
                response.SetError(ErrorCodes.SP_127);
                return response;
            }


            RblAccountBalalnceResponse respbal = new RblAccountBalalnceResponse();
            RblPayoutRequest payoutRequest = new RblPayoutRequest();
            payoutRequest.ApproverId = "";
            payoutRequest.PartnerRetailorId=req.PartnerRetailorId;
            payoutRequest.AccountNo = serviceList.ServiceAccountNo;
            
            

            //api balance chaeck validation
            SimpleResponse resbal = new SimpleResponse();

            resbal = await GetBalalceNew(payoutRequest, Certificatetext, serviceUser);
            if (resbal == null)
            {
                response.SetError(ErrorCodes.ACC_BAL_INSUFCIANT);
                return response;
            }
            respbal = resbal.Result as RblAccountBalalnceResponse;
            if (respbal == null)
            {
                response.SetError(ErrorCodes.ACC_BAL_INSUFCIANT);
                return response;
            }
            if (respbal.BalalnceAmount =="0.00")
            {
                response.SetError(ErrorCodes.ACC_BAL_INSUFCIANT);
                return response;
            }
            if (Convert.ToDecimal( respbal.BalalnceAmount) <=0)
            {
                response.SetError(ErrorCodes.ACC_BAL_INSUFCIANT);
                return response;
            }
            if (Convert.ToDecimal(respbal.BalalnceAmount) <=Convert.ToDecimal( req.Amount))
            {
                response.SetError(ErrorCodes.ACC_BAL_INSUFCIANT);
                return response;
            }

            //service charge calculation and validation
            SevicechargeRequest sevicechargeRequest = new SevicechargeRequest();
            sevicechargeRequest.ServiceId = 1;
            sevicechargeRequest.AgencyId = 1;
            sevicechargeRequest.Amount = Convert.ToDecimal(req.Amount);


            SevicechargeResponse sevicechargeResponse = new SevicechargeResponse();
            sevicechargeResponse = await _provider.GetServiceChargeDetail(sevicechargeRequest);
            if(sevicechargeResponse == null)
            {
                response.SetError(ErrorCodes.SERVICE_CHARGE_NOT_DEFINE);
                return response;
            }
            if(sevicechargeResponse.SlabType == 1)
            {
                if (sevicechargeResponse.CalculationType == 1)
                {
                    txnFee = sevicechargeResponse.CalculationValue;
                    Margin = 0;
                }
                else
                {
                    txnFee = sevicechargeResponse.CalculationValue*Convert.ToDecimal(req.Amount);
                    Margin = 0;
                }
            }
           else
            {
                if (sevicechargeResponse.CalculationType == 1)
                {
                    txnFee = 0;
                    Margin = sevicechargeResponse.CalculationValue;
                }
                else
                {
                    txnFee = 0;
                    Margin = sevicechargeResponse.CalculationValue * Convert.ToDecimal(req.Amount);
                }
            }

            //wallet balance check and validation

            bool isavaillimit = false;
            isavaillimit =await _userprovider.CheckAvailableBalance(Convert.ToDecimal( req.Amount), txnFee, serviceUser);
            if (isavaillimit)
            {
                response.SetError(ErrorCodes.INSUFFICIENT_LIMIT);
                return response;

            }

            //initiate transaction
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
            request1.txnFee = txnFee;
            request1.margincom = Margin;
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
            long benid = 0;
           
                AddBenficiaryRequest objb= new AddBenficiaryRequest ();
                objb.BenBankcode = req.Ben_BankCd;
                objb.BenBranchCode = req.Ben_BranchCd;
                objb.BenAddress = "";
                objb.BenAccountNo = req.Ben_Acct_No;
                objb.BenbankName = req.Ben_BankName;
                objb.BenficiaryName = req.Ben_Name;
                objb.BenIfsccode = req.Ben_IFSC;
                objb.BenMobile=req.Ben_Mobile;
                benid = await _userprovider.AddNewBenficiary(objb, serviceUser);
                if(benid == 0)
                {
                    response.SetError(ErrorCodes.SP_128);
                    return response;
                }
            

            //api payment initiations
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
            bp.Issue_BranchCd ="";
            bp.Ben_Acct_No = req.Ben_Acct_No;
            bp.Ben_TrnParticulars = req.Ben_TrnParticulars;
            bp.Ben_PartTrnRmks = "SINGLE PAYMENT";
            bp.Debit_PartTrnRmks = "";
            bp.Ben_BankName = req.Ben_BankName;
            bp.Remarks = req.Remarks;
            bp.Ben_Email = "";
            bp.Ben_IFSC = req.Ben_IFSC;
            bp.Ben_Mobile = req.Ben_Mobile;
            bp.Ben_Name = req.Ben_Name;
            bp.Debit_Acct_Name = serviceList.ServiceAccName;
            bp.Debit_Acct_No = serviceList.ServiceAccountNo;
            bp.Debit_IFSC = serviceList.ServcieIfsccode;
            bp.Debit_Mobile = serviceList.ServiceMobileNo;
            bp.Debit_TrnParticulars = req.Debit_TrnParticulars;
            bp.Mode_of_Pay = req.Mode_of_Pay;
            spr.Body = bp;
            requestreq.Single_Payment_Corp_Req = spr;


            //api execution 
           
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

            //response generation
            string jsonstr = JsonConvert.SerializeObject(requestreq, Formatting.Indented);
            var content = new StringContent(jsonstr);
            request.Content = content;
            var response1 = await client.SendAsync(request);
            response.Result = await response1.Content.ReadAsStringAsync();
            await _commonProvider.ApilogResponse("RBL Payout Transaction", fullurl, "", jsonstr, response.Result.ToString());
            SingleRblPaymentResponse nnn = new SingleRblPaymentResponse();
            UpdateTransactionStatusRequest request3 = new UpdateTransactionStatusRequest();
            if (response1.StatusCode == HttpStatusCode.OK)
            {
                string jsonData = JsonConvert.SerializeObject(response.Result);
                dynamic jsonn = JsonConvert.DeserializeObject(jsonData);



                string strRemSlash = jsonn.Replace("\"", "\'");
                string strRemNline = strRemSlash.Replace("\n", " ");
                // Time to desrialize it to convert it into an object class.
                nnn = JsonConvert.DeserializeObject<SingleRblPaymentResponse>(@strRemNline);
                resp.Status = nnn.Single_Payment_Corp_Resp.Header.Status;
                resp.ChanelPartnerRefNo = req.PartnerRefNo;
                resp.ErorrDescription = nnn.Single_Payment_Corp_Resp.Header.Error_Desc;
                resp.ErrorCode = nnn.Single_Payment_Corp_Resp.Header.Error_Cde;
                resp.TransactionId = nnn.Single_Payment_Corp_Resp.Header.TranID;
                if(resp.Status== "Success")
                {
                    resp.RefNo = nnn.Single_Payment_Corp_Resp.Body.channelpartnerrefno;
                }
                else
                {
                    resp.RefNo = "";
                }
               
                resp.RRN = nnn.Single_Payment_Corp_Resp.Body.RRN;
                resp.BenIFSC = nnn.Single_Payment_Corp_Resp.Body.BenIFSC;
                resp.Ben_Acct_No = nnn.Single_Payment_Corp_Resp.Body.Ben_Acct_No;
                resp.Amount = nnn.Single_Payment_Corp_Resp.Body.Amount;
                resp.Txn_Time = nnn.Single_Payment_Corp_Resp.Body.Txn_Time;
                resp.BankRefNo = nnn.Single_Payment_Corp_Resp.Body.RefNo;

                request3.RefNo = resp.BankRefNo;
                request3.RelatedReference = resp.RefNo;
                request3.RefNo1 = resp.RRN;
                request3.RefNo2 = resp.Ben_Acct_No;
                request3.RefNo3 = resp.BenIFSC;
                request3.BankTxnDatetime = resp.Txn_Time;
                request3.RefNo4 = resp.Status;
                request3.RefNo5 = resp.Amount;
                request3.RefNo6= resp.ErrorCode;
                request3.RefNo7 = req.Mode_of_Pay;
                request3.RefNo10 = resp.ErrorCode;
                request3.FailureReason = resp.ErorrDescription;
                request3.Transactioncode = resp.TransactionId;
                if(resp.Status== "Failure")
                {
                    request3.status = 4;
                }
                else if(resp.Status == "In Progress")
                {
                    request3.status = 3;
                }
                else if (resp.Status == "Success")
                {
                    request3.status = 2;
                }
                await _provider.NewTransactionUpdateStatus(request3, serviceUser);
            }
            else if (response1.StatusCode == HttpStatusCode.Unauthorized)
            {

                
                resp.Status = "Unauthorized";
                resp.ChanelPartnerRefNo = req.PartnerRefNo;
                resp.ErorrDescription = "";
                resp.ErrorCode = response1.ReasonPhrase;
                resp.TransactionId = hp.TranID;
                resp.RefNo = "";
                resp.RRN ="";
                resp.BenIFSC = "";
                resp.Ben_Acct_No = "";
                resp.Amount = "";
                resp.Txn_Time = "";
                resp.BankRefNo = "";

                request3.RefNo = "";
                request3.RelatedReference ="";
                request3.RefNo1 = "";
                request3.RefNo2 ="";
                request3.RefNo3 = "";
                request3.BankTxnDatetime = "";
                request3.RefNo4 = "Unauthorized";
                request3.RefNo5 = req.Amount;
                request3.RefNo6 = "Unauthorized";
                request3.RefNo7 = req.Mode_of_Pay;
                request3.RefNo10 = "Unauthorized";
                request3.FailureReason = "";
                request3.Transactioncode = resp.TransactionId;
                request3.status = 4;
                await _provider.NewTransactionUpdateStatus(request3, serviceUser);

            }
            else if (response1.StatusCode == HttpStatusCode.InternalServerError)
            {


                resp.Status = "InternalServerError";
                resp.ChanelPartnerRefNo = req.PartnerRefNo;
                resp.ErorrDescription = "";
                resp.ErrorCode = response1.ReasonPhrase;
                resp.TransactionId = hp.TranID;
                resp.RefNo = "";
                resp.RRN = "";
                resp.BenIFSC = "";
                resp.Ben_Acct_No = "";
                resp.Amount = "";
                resp.Txn_Time = "";
                resp.BankRefNo = "";

                request3.RefNo = "";
                request3.RelatedReference = "";
                request3.RefNo1 = "";
                request3.RefNo2 = "";
                request3.RefNo3 = "";
                request3.BankTxnDatetime = "";
                request3.RefNo4 = "InternalServerError";
                request3.RefNo5 = req.Amount;
                request3.RefNo6 = "InternalServerError";
                request3.RefNo7 = req.Mode_of_Pay;
                request3.RefNo10 = "InternalServerError";
                request3.FailureReason = "";
                request3.Transactioncode = resp.TransactionId;
                request3.status = 4;
                await _provider.NewTransactionUpdateStatus(request3, serviceUser);

            }
            response.Result = resp;

            return response;
        }

        public async Task<SimpleResponse> PayoutTransaction(PayoutTransaction req, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            PaymentRequest requestreq = new PaymentRequest();
            SimpleResponse response = new SimpleResponse();
            RblTransactionResponse resp = new RblTransactionResponse();
            decimal txnFee = 0;
            decimal Margin = 0;

            //service list validation
            ServiceListResponse serviceList = new ServiceListResponse();

            serviceList = await _masterdatarepos.GetAllServcieList(1);

            if (serviceList == null)
            {
                response.SetError(ErrorCodes.SP_123);
                return response;
            }
            if (serviceList.ServiceAccountNo == null)
            {
                response.SetError(ErrorCodes.SP_124);
                return response;
            }
            if (serviceList.ServiceAccountNo == "")
            {
                response.SetError(ErrorCodes.SP_124);
                return response;
            }
            if (serviceList.ServcieIfsccode == null)
            {
                response.SetError(ErrorCodes.SP_125);
                return response;
            }
            if (serviceList.ServcieIfsccode == "")
            {
                response.SetError(ErrorCodes.SP_125);
                return response;
            }
            if (serviceList.ServiceAccName == null)
            {
                response.SetError(ErrorCodes.SP_126);
                return response;
            }
            if (serviceList.ServiceAccName == "")
            {
                response.SetError(ErrorCodes.SP_126);
                return response;
            }
            if (serviceList.ServiceMobileNo == null)
            {
                response.SetError(ErrorCodes.SP_127);
                return response;
            }
            if (serviceList.ServiceMobileNo == "")
            {
                response.SetError(ErrorCodes.SP_127);
                return response;
            }


            RblAccountBalalnceResponse respbal = new RblAccountBalalnceResponse();
            RblPayoutRequest payoutRequest = new RblPayoutRequest();
            payoutRequest.ApproverId = "";
            payoutRequest.PartnerRetailorId = req.PartnerRetailorId;
            payoutRequest.AccountNo = serviceList.ServiceAccountNo;



            //api balance chaeck validation
            SimpleResponse resbal = new SimpleResponse();

            resbal = await GetBalalceNew(payoutRequest, Certificatetext, serviceUser);
            if (resbal == null)
            {
                response.SetError(ErrorCodes.ACC_BAL_INSUFCIANT);
                return response;
            }
            respbal = resbal.Result as RblAccountBalalnceResponse;
            if (respbal == null)
            {
                response.SetError(ErrorCodes.ACC_BAL_INSUFCIANT);
                return response;
            }
            if (respbal.BalalnceAmount == "0.00")
            {
                response.SetError(ErrorCodes.ACC_BAL_INSUFCIANT);
                return response;
            }
            if (Convert.ToDecimal(respbal.BalalnceAmount) <= 0)
            {
                response.SetError(ErrorCodes.ACC_BAL_INSUFCIANT);
                return response;
            }
            if (Convert.ToDecimal(respbal.BalalnceAmount) <= Convert.ToDecimal(req.Amount))
            {
                response.SetError(ErrorCodes.ACC_BAL_INSUFCIANT);
                return response;
            }

            //service charge calculation and validation
            SevicechargeRequest sevicechargeRequest = new SevicechargeRequest();
            sevicechargeRequest.ServiceId = 1;
            sevicechargeRequest.AgencyId = 1;
            sevicechargeRequest.Amount = Convert.ToDecimal(req.Amount);


            SevicechargeResponse sevicechargeResponse = new SevicechargeResponse();
            sevicechargeResponse = await _provider.GetServiceChargeDetail(sevicechargeRequest);
            if (sevicechargeResponse == null)
            {
                response.SetError(ErrorCodes.SERVICE_CHARGE_NOT_DEFINE);
                return response;
            }
            if (sevicechargeResponse.SlabType == 1)
            {
                if (sevicechargeResponse.CalculationType == 1)
                {
                    txnFee = sevicechargeResponse.CalculationValue;
                    Margin = 0;
                }
                else
                {
                    txnFee = sevicechargeResponse.CalculationValue * Convert.ToDecimal(req.Amount);
                    Margin = 0;
                }
            }
            else
            {
                if (sevicechargeResponse.CalculationType == 1)
                {
                    txnFee = 0;
                    Margin = sevicechargeResponse.CalculationValue;
                }
                else
                {
                    txnFee = 0;
                    Margin = sevicechargeResponse.CalculationValue * Convert.ToDecimal(req.Amount);
                }
            }

            //wallet balance check and validation

            bool isavaillimit = false;
            isavaillimit = await _userprovider.CheckAvailableBalance(Convert.ToDecimal(req.Amount), txnFee, serviceUser);
            if (isavaillimit)
            {
                response.SetError(ErrorCodes.INSUFFICIENT_LIMIT);
                return response;

            }

            //initiate transaction
            NewTransactionRequest request1 = new NewTransactionRequest();
            request1.description = "Payout Transaction";
            request1.amount = Convert.ToDecimal(req.Amount);
            request1.partnerretailorid = req.PartnerRetailorId;
            request1.TxnPlateForm = "API";
            request1.agencyid = 1;
            request1.serviceid = 1;
            request1.partnerid = serviceUser.UserMasterID;
            request1.partnerreferenceno = req.PartnerRefNo;
            request1.TxnType = "D";
            request1.txnFee = txnFee;
            request1.margincom = Margin;
            TransactionResponse transactionResponse = new TransactionResponse();

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

            BenficiaryResponse resben=new BenficiaryResponse ();
            resben = await _userprovider.GetBenficiaryDetailsByID(Convert.ToInt64(req.BenficiaryID));
            if (resben == null)
            {
                response.SetError(ErrorCodes.SP_129);
                return response;
            }

            //api payment initiations
            SinglePaymentCorpReq spr = new SinglePaymentCorpReq();
            Signature1 si = new Signature1();
            si.Signature = "";
            spr.Signature = si;
            HeaderPayment hp = new HeaderPayment();
            hp.Approver_ID ="";
            hp.Checker_ID = "";
            hp.Corp_ID = SANYUKTApplicationConfiguration.Instance.RblPayoutCORPID.ToString();
            hp.TranID = transactionResponse.Transactioncode;
            hp.Maker_ID = "";
            spr.Header = hp;

            BodyPayment bp = new BodyPayment();
            bp.Amount = req.Amount;
            bp.Ben_BranchCd = resben.BenBranchCode;
            bp.Ben_BankCd = resben.BenBankcode;
            bp.Ben_Address = "";
            bp.Issue_BranchCd = "";
            bp.Ben_Acct_No = resben.BenAccountNo;
            bp.Ben_TrnParticulars = req.Ben_TrnParticulars;
            bp.Ben_PartTrnRmks = "SINGLE PAYMENT";
            bp.Debit_PartTrnRmks = "";
            bp.Ben_BankName = resben.BenbankName;
            bp.Remarks = req.Remarks;
            bp.Ben_Email = "";
            bp.Ben_IFSC = resben.BenIfsccode;
            bp.Ben_Mobile = resben.BenMobile;
            bp.Ben_Name = resben.BenficiaryName;
            bp.Debit_Acct_Name = serviceList.ServiceAccName;
            bp.Debit_Acct_No = serviceList.ServiceAccountNo;
            bp.Debit_IFSC = serviceList.ServcieIfsccode;
            bp.Debit_Mobile = serviceList.ServiceMobileNo;
            bp.Debit_TrnParticulars = req.Debit_TrnParticulars;
            bp.Mode_of_Pay = req.Mode_of_Pay;
            spr.Body = bp;
            requestreq.Single_Payment_Corp_Req = spr;


            //api execution 

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

            //response generation
            string jsonstr = JsonConvert.SerializeObject(requestreq, Formatting.Indented);
            var content = new StringContent(jsonstr);
            request.Content = content;
            var response1 = await client.SendAsync(request);
            response.Result = await response1.Content.ReadAsStringAsync();
            await _commonProvider.ApilogResponse("RBL Payout Transaction", fullurl, "", jsonstr, response.Result.ToString());
            SingleRblPaymentResponse nnn = new SingleRblPaymentResponse();
            UpdateTransactionStatusRequest request3 = new UpdateTransactionStatusRequest();
            if (response1.StatusCode == HttpStatusCode.OK)
            {
                string jsonData = JsonConvert.SerializeObject(response.Result);
                dynamic jsonn = JsonConvert.DeserializeObject(jsonData);



                string strRemSlash = jsonn.Replace("\"", "\'");
                string strRemNline = strRemSlash.Replace("\n", " ");
                // Time to desrialize it to convert it into an object class.
                nnn = JsonConvert.DeserializeObject<SingleRblPaymentResponse>(@strRemNline);
                resp.Status = nnn.Single_Payment_Corp_Resp.Header.Status;
                resp.ChanelPartnerRefNo = req.PartnerRefNo;
                resp.ErorrDescription = nnn.Single_Payment_Corp_Resp.Header.Error_Desc;
                resp.ErrorCode = nnn.Single_Payment_Corp_Resp.Header.Error_Cde;
                resp.TransactionId = nnn.Single_Payment_Corp_Resp.Header.TranID;
                if (resp.Status == "Success")
                {
                    resp.RefNo = nnn.Single_Payment_Corp_Resp.Body.channelpartnerrefno;
                }
                else
                {
                    resp.RefNo = "";
                }

                resp.RRN = nnn.Single_Payment_Corp_Resp.Body.RRN;
                resp.BenIFSC = nnn.Single_Payment_Corp_Resp.Body.BenIFSC;
                resp.Ben_Acct_No = nnn.Single_Payment_Corp_Resp.Body.Ben_Acct_No;
                resp.Amount = nnn.Single_Payment_Corp_Resp.Body.Amount;
                resp.Txn_Time = nnn.Single_Payment_Corp_Resp.Body.Txn_Time;
                resp.BankRefNo = nnn.Single_Payment_Corp_Resp.Body.RefNo;

                request3.RefNo = resp.BankRefNo;
                request3.RelatedReference = resp.RefNo;
                request3.RefNo1 = resp.RRN;
                request3.RefNo2 = resp.Ben_Acct_No;
                request3.RefNo3 = resp.BenIFSC;
                request3.BankTxnDatetime = resp.Txn_Time;
                request3.RefNo4 = resp.Status;
                request3.RefNo5 = resp.Amount;
                request3.RefNo6 = resp.ErrorCode;
                request3.RefNo7 = req.Mode_of_Pay;
                request3.RefNo10 = resp.ErrorCode;
                request3.FailureReason = resp.ErorrDescription;
                request3.Transactioncode = resp.TransactionId;
                if (resp.Status == "Failure")
                {
                    request3.status = 4;
                }
                else if (resp.Status == "In Progress")
                {
                    request3.status = 3;
                }
                else if (resp.Status == "Success")
                {
                    request3.status = 2;
                }
                await _provider.NewTransactionUpdateStatus(request3, serviceUser);
            }
            else if (response1.StatusCode == HttpStatusCode.Unauthorized)
            {


                resp.Status = "Unauthorized";
                resp.ChanelPartnerRefNo = req.PartnerRefNo;
                resp.ErorrDescription = "";
                resp.ErrorCode = response1.ReasonPhrase;
                resp.TransactionId = hp.TranID;
                resp.RefNo = "";
                resp.RRN = "";
                resp.BenIFSC = "";
                resp.Ben_Acct_No = "";
                resp.Amount = "";
                resp.Txn_Time = "";
                resp.BankRefNo = "";

                request3.RefNo = "";
                request3.RelatedReference = "";
                request3.RefNo1 = "";
                request3.RefNo2 = "";
                request3.RefNo3 = "";
                request3.BankTxnDatetime = "";
                request3.RefNo4 = "Unauthorized";
                request3.RefNo5 = req.Amount;
                request3.RefNo6 = "Unauthorized";
                request3.RefNo7 = req.Mode_of_Pay;
                request3.RefNo10 = "Unauthorized";
                request3.FailureReason = "";
                request3.Transactioncode = resp.TransactionId;
                request3.status = 4;
                await _provider.NewTransactionUpdateStatus(request3, serviceUser);

            }
            else if (response1.StatusCode == HttpStatusCode.InternalServerError)
            {


                resp.Status = "InternalServerError";
                resp.ChanelPartnerRefNo = req.PartnerRefNo;
                resp.ErorrDescription = "";
                resp.ErrorCode = response1.ReasonPhrase;
                resp.TransactionId = hp.TranID;
                resp.RefNo = "";
                resp.RRN = "";
                resp.BenIFSC = "";
                resp.Ben_Acct_No = "";
                resp.Amount = "";
                resp.Txn_Time = "";
                resp.BankRefNo = "";

                request3.RefNo = "";
                request3.RelatedReference = "";
                request3.RefNo1 = "";
                request3.RefNo2 = "";
                request3.RefNo3 = "";
                request3.BankTxnDatetime = "";
                request3.RefNo4 = "InternalServerError";
                request3.RefNo5 = req.Amount;
                request3.RefNo6 = "InternalServerError";
                request3.RefNo7 = req.Mode_of_Pay;
                request3.RefNo10 = "InternalServerError";
                request3.FailureReason = "";
                request3.Transactioncode = resp.TransactionId;
                request3.status = 4;
                await _provider.NewTransactionUpdateStatus(request3, serviceUser);

            }
            response.Result = resp;

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
            bs.RRN = obbb.UTRNo;
            gsps.Header = hd;
            gsps .Body = bs;
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
            await _commonProvider.ApilogResponse("RBL Payout Transaction Status", fullurl, "", jsonstr, response.Result.ToString());
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
            else if (response1.StatusCode == HttpStatusCode.InternalServerError)
            {

                resp.Status = "InternalServerError";
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
