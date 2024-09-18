using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto.Agreement.Srp;
using Org.BouncyCastle.Crypto.Tls;
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
        public readonly ConfigProvider _configprovider = null;
        public RblPayoutProvider() {
            _provider = new TransactionProvider();
            _userprovider= new UserDetailsProvider();
            _commonProvider= new CommonProvider();
            _masterdatarepos= new MasterDataRepository();
            _configprovider= new ConfigProvider();
        }

        public async Task<SimpleResponse> GetBalalceNew (RblPayoutRequest objp, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();

            // Check if the certificate is valid
            if (!Certificatetext.Verify())
            {
                response.Result = "The certificate is not valid.";
                return response;
            }

            // Check certificate expiration
            DateTime now = DateTime.Now;
            if (now < Certificatetext.NotBefore || now > Certificatetext.NotAfter)
            {
                response.Result = "The certificate is either not yet valid or has expired.";
                return response;
            }


            try
            {
                AccountBalalnceRequest requestreq = new AccountBalalnceRequest();
                RblAccountBalalnceResponse resp = new RblAccountBalalnceResponse();
                BaseTransactionRequest request1 = new BaseTransactionRequest();
                request1.agencyid = 1;
                request1.serviceid = 1;
                request1.TxnPlateForm = "API";
                request1.partnerreferenceno = objp.PartnerRefNo;
                request1.partnerretailorid = "";



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

                
                var _clientHandler = new HttpClientHandler();
                _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                _clientHandler.ClientCertificates.Add(Certificatetext);
                _clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
                
                var client = new HttpClient(_clientHandler);
                string Url = SANYUKTApplicationConfiguration.Instance.RblPayoutBaseUrl.ToString();
                //string fullurl = Url + "test/sb/rbl/v1/accounts/balance/query?client_id=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientId.ToString() + "&client_secret=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientSecrat.ToString();
                string fullurl = Url + "v1/accounts/balance/query?client_id=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientId.ToString() + "&client_secret=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientSecrat.ToString();
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

            }
            catch (Exception ex)
            {

                response.Result = ex.Message.ToString();
            }
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
            string fullurl = Url + "v1/accounts/balance/query?client_id=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientId.ToString() + "&client_secret=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientSecrat.ToString();
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

        public async Task<SimpleResponse> PayoutTransactionwithoutBenFT(SinglePaymentRequestFT req, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser,int Caaltypeid=0)
        {
            PaymentRequestFT requestreq = new PaymentRequestFT();
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
            if (Convert.ToDecimal(req.Amount) <= 0)
            {
                response.SetError(ErrorCodes.SP_131);
                return response;
            }

            RblAccountBalalnceResponse respbal = new RblAccountBalalnceResponse();
            RblPayoutRequest payoutRequest = new RblPayoutRequest();
            payoutRequest.ApproverId = "";
            payoutRequest.PartnerRetailorId = serviceUser.UserID.ToString();
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

            //user transaction configration
            UserConfigResponse userConfig = new UserConfigResponse();
            userConfig = await _userprovider.GetUserConfig(serviceUser);
            if (userConfig == null)
            {
                response.SetError(ErrorCodes.SP_137);
                return response;
            }
            if (userConfig.ChargeTypeOn == 0)
            {
                response.SetError(ErrorCodes.SP_137);
                return response;
            }
            if (userConfig.PlanId == 0)
            {
                response.SetError(ErrorCodes.SP_137);
                return response;
            }
            if (req.Amount > userConfig.MaxTxn)
            {
                response.SetError(ErrorCodes.SP_138);
                return response;
            }
            if (req.Amount < userConfig.MinTxn)
            {
                response.SetError(ErrorCodes.SP_139);
                return response;
            }

            if (userConfig.ChargeTypeOn == (int)ChargeDeductionType.FromTransaction)
            {
                //service charge calculation and validation
                SevicechargeByPlanRequest sevicechargeRequest = new SevicechargeByPlanRequest();
                sevicechargeRequest.ServiceId = 1;
                sevicechargeRequest.AgencyId = 1;
                sevicechargeRequest.Amount = Convert.ToDecimal(req.Amount);


                SevicechargeResponse sevicechargeResponse = new SevicechargeResponse();
                sevicechargeResponse = await _provider.GetServiceChargeDetailByPlan(sevicechargeRequest);
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
            request1.partnerretailorid = serviceUser.UserID.ToString();
            request1.TxnPlateForm = "API";
            request1.agencyid = 1;
            request1.serviceid = 1;
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
            if(Caaltypeid==0)
            {
            long benid = 0;

            AddBenficiaryRequest objb = new AddBenficiaryRequest();
            objb.BenBankcode = req.Ben_BankCd;
            objb.BenBranchCode = req.Ben_BranchCd;
            objb.BenAddress = "";
            objb.BenAccountNo = req.Ben_Acct_No;
            objb.BenbankName = req.Ben_BankName;
            objb.BenficiaryName = req.Ben_Name;
            objb.BenIfsccode = req.Ben_IFSC;
            objb.BenMobile = req.Ben_Mobile;
            benid = await _userprovider.AddNewBenficiary(objb, serviceUser);
            if (benid == 0)
            {
                response.SetError(ErrorCodes.SP_128);
                return response;
            }
            }


            //api payment initiations
            Single_Payment_Corp_ReqFT spr = new Single_Payment_Corp_ReqFT();
            SignatureFT si = new SignatureFT();
            si.Signature = "";
            spr.Signature = si;
            HeaderFT hp = new HeaderFT();
            hp.Approver_ID = "";
            hp.Checker_ID = "";
            hp.Corp_ID = SANYUKTApplicationConfiguration.Instance.RblPayoutCORPID.ToString();
            hp.TranID = transactionResponse.Transactioncode;
            hp.Maker_ID = "";
            spr.Header = hp;

            BodyFT bp = new BodyFT();
            bp.Amount = req.Amount.ToString();
            bp.Ben_BranchCd = req.Ben_BranchCd;
            bp.Ben_BankCd = req.Ben_BankCd;
            bp.Ben_Address = "";
            bp.Issue_BranchCd = "";
            bp.Ben_Acct_No = req.Ben_Acct_No;
            bp.Ben_TrnParticulars = req.Ben_TrnParticulars;
            bp.Ben_PartTrnRmks = "SINGLE PAYMENT";
            bp.Debit_PartTrnRmks = "";
            bp.Ben_BankName = req.Ben_BankName;
            bp.Remarks = "PAYEMNT QUEUE";
            bp.Ben_Email = "";
            bp.Ben_IFSC = req.Ben_IFSC;
            bp.Ben_Mobile = req.Ben_Mobile;
            bp.Ben_Name = req.Ben_Name;
            bp.Debit_Acct_Name = serviceList.ServiceAccName;
            bp.Debit_Acct_No = serviceList.ServiceAccountNo;
            bp.Debit_IFSC = serviceList.ServcieIfsccode;
            bp.Debit_Mobile = req.SenderMobile;
            bp.Debit_TrnParticulars = "Transaction By-"+ req.SenderMobile.ToString();
            bp.Mode_of_Pay ="FT";
            spr.Body = bp;
            requestreq.Single_Payment_Corp_Req = spr;


            //api execution 

            var _clientHandler = new HttpClientHandler();
            _clientHandler.ClientCertificates.Add(Certificatetext);
            _clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var client = new HttpClient(_clientHandler);
            string Url = SANYUKTApplicationConfiguration.Instance.RblPayoutBaseUrl.ToString();
            string fullurl = Url + "v1/payments/corp/payment?client_id=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientId.ToString() + "&client_secret=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientSecrat.ToString();
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
            SinglePaymentResponseFT nnn = new SinglePaymentResponseFT();
            UpdateTransactionStatusRequest request3 = new UpdateTransactionStatusRequest();
            if (response1.StatusCode == HttpStatusCode.OK)
            {
                string jsonData = JsonConvert.SerializeObject(response.Result);
                dynamic jsonn = JsonConvert.DeserializeObject(jsonData);



                string strRemSlash = jsonn.Replace("\"", "\'");
                string strRemNline = strRemSlash.Replace("\n", " ");
                // Time to desrialize it to convert it into an object class.
                nnn = JsonConvert.DeserializeObject<SinglePaymentResponseFT>(@strRemNline);
                resp.Status = nnn.Single_Payment_Corp_Resp.Header.Status;
                resp.ChanelPartnerRefNo = req.PartnerRefNo;
                resp.ErorrDescription = nnn.Single_Payment_Corp_Resp.Header.Error_Desc;
                resp.ErrorCode = nnn.Single_Payment_Corp_Resp.Header.Error_Cde;
                resp.TransactionId = nnn.Single_Payment_Corp_Resp.Header.TranID;
                if (resp.Status == "Success")
                {
                    resp.RefNo = nnn.Single_Payment_Corp_Resp.Body.RefNo;
                    resp.BenIFSC = nnn.Single_Payment_Corp_Resp.Body.BenIFSC;
                    resp.Ben_Acct_No = nnn.Single_Payment_Corp_Resp.Body.Ben_Acct_No;
                    resp.Amount = nnn.Single_Payment_Corp_Resp.Body.Amount;
                    resp.Txn_Time = nnn.Single_Payment_Corp_Resp.Body.Txn_Time;
                    resp.BankRefNo = "";
                }
                else
                {
                    resp.RefNo = "";
                }

                resp.RRN = "";
                

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
                request3.RefNo5 = req.Amount.ToString();
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
                request3.RefNo5 = req.Amount.ToString();
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

        public async Task<SimpleResponse> PayoutTransactionwithoutBenNEFT(SinglePaymentRequestFT req, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser, int Caaltypeid = 0)
        {
            PaymentRequestNEFT requestreq = new PaymentRequestNEFT();
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
            if (Convert.ToDecimal(req.Amount) <= 0)
            {
                response.SetError(ErrorCodes.SP_131);
                return response;
            }
            


            RblAccountBalalnceResponse respbal = new RblAccountBalalnceResponse();
            RblPayoutRequest payoutRequest = new RblPayoutRequest();
            payoutRequest.ApproverId = "";
            payoutRequest.PartnerRetailorId = req.SenderMobile;
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

            //user transaction configration
            UserConfigResponse userConfig = new UserConfigResponse();
            userConfig = await _userprovider.GetUserConfig(serviceUser);
            if (userConfig == null)
            {
                response.SetError(ErrorCodes.SP_137);
                return response;
            }
            if (userConfig.ChargeTypeOn == 0)
            {
                response.SetError(ErrorCodes.SP_137);
                return response;
            }
            if (userConfig.PlanId == 0)
            {
                response.SetError(ErrorCodes.SP_137);
                return response;
            }
            if (req.Amount > userConfig.MaxTxn)
            {
                response.SetError(ErrorCodes.SP_138);
                return response;
            }
            if (req.Amount < userConfig.MinTxn)
            {
                response.SetError(ErrorCodes.SP_139);
                return response;
            }

            if (userConfig.ChargeTypeOn == (int)ChargeDeductionType.FromTransaction)
            {
                //service charge calculation and validation
                SevicechargeByPlanRequest sevicechargeRequest = new SevicechargeByPlanRequest();
                sevicechargeRequest.ServiceId = 1;
                sevicechargeRequest.AgencyId = 1;
                sevicechargeRequest.Amount = Convert.ToDecimal(req.Amount);


                SevicechargeResponse sevicechargeResponse = new SevicechargeResponse();
                sevicechargeResponse = await _provider.GetServiceChargeDetailByPlan(sevicechargeRequest);
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
            request1.partnerretailorid = req.SenderMobile;
            request1.TxnPlateForm = "API";
            request1.agencyid = 1;
            request1.serviceid = 1;
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
            if (Caaltypeid == 0)
            {
                long benid = 0;

                AddBenficiaryRequest objb = new AddBenficiaryRequest();
                objb.BenBankcode = req.Ben_BankCd;
                objb.BenBranchCode = req.Ben_BranchCd;
                objb.BenAddress = "";
                objb.BenAccountNo = req.Ben_Acct_No;
                objb.BenbankName = req.Ben_BankName;
                objb.BenficiaryName = req.Ben_Name;
                objb.BenIfsccode = req.Ben_IFSC;
                objb.BenMobile = req.Ben_Mobile;
                benid = await _userprovider.AddNewBenficiary(objb, serviceUser);
                if (benid == 0)
                {
                    response.SetError(ErrorCodes.SP_128);
                    return response;
                }
            }


            //api payment initiations
            Single_Payment_Corp_ReqNEFT spr = new Single_Payment_Corp_ReqNEFT();
            SignatureNEFT si = new SignatureNEFT();
            si.Signature = "";
            spr.Signature = si;
            HeaderNEFT hp = new HeaderNEFT();
            hp.Approver_ID = "";
            hp.Checker_ID = "";
            hp.Corp_ID = SANYUKTApplicationConfiguration.Instance.RblPayoutCORPID.ToString();
            hp.TranID = transactionResponse.Transactioncode;
            hp.Maker_ID = "";
            spr.Header = hp;

            BodyNEFT bp = new BodyNEFT();
            bp.Amount = req.Amount.ToString();
            bp.Ben_BranchCd = req.Ben_BranchCd;
            bp.Ben_BankCd = req.Ben_BankCd;
            bp.Ben_Address = "";
            bp.Issue_BranchCd = "";
            bp.Ben_Acct_No = req.Ben_Acct_No;
            bp.Ben_TrnParticulars = req.Ben_TrnParticulars;
            bp.Ben_PartTrnRmks = "SINGLE PAYMENT";
            bp.Debit_PartTrnRmks = "";
            bp.Ben_BankName = req.Ben_BankName;
            bp.Remarks = "PAYEMNT QUEUE";
            bp.Ben_Email = "";
            bp.Ben_IFSC = req.Ben_IFSC;
            bp.Ben_Mobile = req.Ben_Mobile;
            bp.Ben_Name = req.Ben_Name;
            bp.Debit_Acct_Name = serviceList.ServiceAccName;
            bp.Debit_Acct_No = serviceList.ServiceAccountNo;
            bp.Debit_IFSC = serviceList.ServcieIfsccode;
            bp.Debit_Mobile = req.SenderMobile;
            bp.Debit_TrnParticulars= "Transaction By-" + req.SenderMobile.ToString();
            bp.Mode_of_Pay = "NEFT";
            spr.Body = bp;
            requestreq.Single_Payment_Corp_Req = spr;


            //api execution 

            var _clientHandler = new HttpClientHandler();
            _clientHandler.ClientCertificates.Add(Certificatetext);
            _clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var client = new HttpClient(_clientHandler);
            string Url = SANYUKTApplicationConfiguration.Instance.RblPayoutBaseUrl.ToString();
            string fullurl = Url + "v1/payments/corp/payment?client_id=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientId.ToString() + "&client_secret=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientSecrat.ToString();
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
            SinglePaymentResponseNEFT nnn = new SinglePaymentResponseNEFT();
            UpdateTransactionStatusRequest request3 = new UpdateTransactionStatusRequest();
            if (response1.StatusCode == HttpStatusCode.OK)
            {
                string jsonData = JsonConvert.SerializeObject(response.Result);
                dynamic jsonn = JsonConvert.DeserializeObject(jsonData);



                string strRemSlash = jsonn.Replace("\"", "\'");
                string strRemNline = strRemSlash.Replace("\n", " ");
                // Time to desrialize it to convert it into an object class.
                nnn = JsonConvert.DeserializeObject<SinglePaymentResponseNEFT>(@strRemNline);
                resp.Status = nnn.Single_Payment_Corp_Resp.Header.Status;
                resp.ChanelPartnerRefNo = req.PartnerRefNo;
                resp.ErorrDescription = nnn.Single_Payment_Corp_Resp.Header.Error_Desc;
                resp.ErrorCode = nnn.Single_Payment_Corp_Resp.Header.Error_Cde;
                resp.TransactionId = nnn.Single_Payment_Corp_Resp.Header.TranID;
                if (resp.Status == "Success")
                {
                    resp.RefNo = nnn.Single_Payment_Corp_Resp.Body.RefNo;
                    resp.RRN = "";
                    resp.BenIFSC = nnn.Single_Payment_Corp_Resp.Body.BenIFSC;
                    resp.Ben_Acct_No = nnn.Single_Payment_Corp_Resp.Body.Ben_Acct_No;
                    resp.Amount = nnn.Single_Payment_Corp_Resp.Body.Amount;
                    resp.Txn_Time = nnn.Single_Payment_Corp_Resp.Body.Txn_Time;
                    resp.BankRefNo = nnn.Single_Payment_Corp_Resp.Body.UTRNo;
                    resp.PoNum = nnn.Single_Payment_Corp_Resp.Body.PONum;
                }
                else
                {
                    resp.RefNo = "";
                }

               

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
                request3.RefNo8 = resp.PoNum;
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
                resp.PoNum = "";

                request3.RefNo = "";
                request3.RelatedReference = "";
                request3.RefNo1 = "";
                request3.RefNo2 = "";
                request3.RefNo3 = "";
                request3.BankTxnDatetime = "";
                request3.RefNo4 = "Unauthorized";
                request3.RefNo5 = req.Amount.ToString();
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
                resp.PoNum = "";

                request3.RefNo = "";
                request3.RelatedReference = "";
                request3.RefNo1 = "";
                request3.RefNo2 = "";
                request3.RefNo3 = "";
                request3.BankTxnDatetime = "";
                request3.RefNo4 = "InternalServerError";
                request3.RefNo5 = req.Amount.ToString();
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

        public async Task<SimpleResponse> PayoutTransactionwithoutBenRTGS(SinglePaymentRequestFT req, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser, int Caaltypeid = 0)
        {
            PaymentRequestRTGS requestreq = new PaymentRequestRTGS();
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
            if (Convert.ToDecimal(req.Amount) <= 0)
            {
                response.SetError(ErrorCodes.SP_131);
                return response;
            }
            if (Convert.ToDecimal(req.Amount) <= 200000)
            {
                response.SetError(ErrorCodes.SP_132);
                return response;
            }


            RblAccountBalalnceResponse respbal = new RblAccountBalalnceResponse();
            RblPayoutRequest payoutRequest = new RblPayoutRequest();
            payoutRequest.ApproverId = "";
            payoutRequest.PartnerRetailorId = req.SenderMobile;
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

            //user transaction configration
            UserConfigResponse userConfig = new UserConfigResponse();
            userConfig = await _userprovider.GetUserConfig(serviceUser);
            if (userConfig == null)
            {
                response.SetError(ErrorCodes.SP_137);
                return response;
            }
            if (userConfig.ChargeTypeOn == 0)
            {
                response.SetError(ErrorCodes.SP_137);
                return response;
            }
            if (userConfig.PlanId == 0)
            {
                response.SetError(ErrorCodes.SP_137);
                return response;
            }
            if (req.Amount > userConfig.MaxTxn)
            {
                response.SetError(ErrorCodes.SP_138);
                return response;
            }
            if (req.Amount < userConfig.MinTxn)
            {
                response.SetError(ErrorCodes.SP_139);
                return response;
            }

            if (userConfig.ChargeTypeOn == (int)ChargeDeductionType.FromTransaction)
            {
                //service charge calculation and validation
                SevicechargeByPlanRequest sevicechargeRequest = new SevicechargeByPlanRequest();
                sevicechargeRequest.ServiceId = 1;
                sevicechargeRequest.AgencyId = 1;
                sevicechargeRequest.Amount = Convert.ToDecimal(req.Amount);


                SevicechargeResponse sevicechargeResponse = new SevicechargeResponse();
                sevicechargeResponse = await _provider.GetServiceChargeDetailByPlan(sevicechargeRequest);
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
            request1.partnerretailorid = req.SenderMobile;
            request1.TxnPlateForm = "API";
            request1.agencyid = 1;
            request1.serviceid = 1;
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
            if (Caaltypeid == 0)
            {
                long benid = 0;

                AddBenficiaryRequest objb = new AddBenficiaryRequest();
                objb.BenBankcode = req.Ben_BankCd;
                objb.BenBranchCode = req.Ben_BranchCd;
                objb.BenAddress = "";
                objb.BenAccountNo = req.Ben_Acct_No;
                objb.BenbankName = req.Ben_BankName;
                objb.BenficiaryName = req.Ben_Name;
                objb.BenIfsccode = req.Ben_IFSC;
                objb.BenMobile = req.Ben_Mobile;
                benid = await _userprovider.AddNewBenficiary(objb, serviceUser);
                if (benid == 0)
                {
                    response.SetError(ErrorCodes.SP_128);
                    return response;
                }
            }


            //api payment initiations
            Single_Payment_Corp_ReqRTGS spr = new Single_Payment_Corp_ReqRTGS();
            SignatureRTGS si = new SignatureRTGS();
            si.Signature = "";
            spr.Signature = si;
            HeaderRTGS hp = new HeaderRTGS();
            hp.Approver_ID = "";
            hp.Checker_ID = "";
            hp.Corp_ID = SANYUKTApplicationConfiguration.Instance.RblPayoutCORPID.ToString();
            hp.TranID = transactionResponse.Transactioncode;
            hp.Maker_ID = "";
            spr.Header = hp;

            BodyRTGS bp = new BodyRTGS();
            bp.Amount = req.Amount.ToString();
            bp.Ben_BranchCd = req.Ben_BranchCd;
            bp.Ben_BankCd = req.Ben_BankCd;
            bp.Ben_Address = "";
            bp.Issue_BranchCd = "";
            bp.Ben_Acct_No = req.Ben_Acct_No;
            bp.Ben_TrnParticulars = req.Ben_TrnParticulars;
            bp.Ben_PartTrnRmks = "SINGLE PAYMENT";
            bp.Debit_PartTrnRmks = "";
            bp.Ben_BankName = req.Ben_BankName;
            bp.Remarks = "PAYEMNT QUEUE";
            bp.Ben_Email = "";
            bp.Ben_IFSC = req.Ben_IFSC;
            bp.Ben_Mobile = req.Ben_Mobile;
            bp.Ben_Name = req.Ben_Name;
            bp.Debit_Acct_Name = serviceList.ServiceAccName;
            bp.Debit_Acct_No = serviceList.ServiceAccountNo;
            bp.Debit_IFSC = serviceList.ServcieIfsccode;
            bp.Debit_Mobile = req.SenderMobile;
            bp.Debit_TrnParticulars = "Transaction By-" + req.SenderMobile.ToString();
            bp.Mode_of_Pay = "RTGS";
            spr.Body = bp;
            requestreq.Single_Payment_Corp_Req = spr;


            //api execution 

            var _clientHandler = new HttpClientHandler();
            _clientHandler.ClientCertificates.Add(Certificatetext);
            _clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var client = new HttpClient(_clientHandler);
            string Url = SANYUKTApplicationConfiguration.Instance.RblPayoutBaseUrl.ToString();
            string fullurl = Url + "v1/payments/corp/payment?client_id=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientId.ToString() + "&client_secret=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientSecrat.ToString();
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
            SinglePaymentResponseNEFT nnn = new SinglePaymentResponseNEFT();
            UpdateTransactionStatusRequest request3 = new UpdateTransactionStatusRequest();
            if (response1.StatusCode == HttpStatusCode.OK)
            {
                string jsonData = JsonConvert.SerializeObject(response.Result);
                dynamic jsonn = JsonConvert.DeserializeObject(jsonData);



                string strRemSlash = jsonn.Replace("\"", "\'");
                string strRemNline = strRemSlash.Replace("\n", " ");
                // Time to desrialize it to convert it into an object class.
                nnn = JsonConvert.DeserializeObject<SinglePaymentResponseNEFT>(@strRemNline);
                resp.Status = nnn.Single_Payment_Corp_Resp.Header.Status;
                resp.ChanelPartnerRefNo = req.PartnerRefNo;
                resp.ErorrDescription = nnn.Single_Payment_Corp_Resp.Header.Error_Desc;
                resp.ErrorCode = nnn.Single_Payment_Corp_Resp.Header.Error_Cde;
                resp.TransactionId = nnn.Single_Payment_Corp_Resp.Header.TranID;
                if (resp.Status == "Success")
                {
                    resp.RefNo = nnn.Single_Payment_Corp_Resp.Body.RefNo;
                    resp.RRN = "";
                    resp.BenIFSC = nnn.Single_Payment_Corp_Resp.Body.BenIFSC;
                    resp.Ben_Acct_No = nnn.Single_Payment_Corp_Resp.Body.Ben_Acct_No;
                    resp.Amount = nnn.Single_Payment_Corp_Resp.Body.Amount;
                    resp.Txn_Time = nnn.Single_Payment_Corp_Resp.Body.Txn_Time;
                    resp.BankRefNo = nnn.Single_Payment_Corp_Resp.Body.RefNo;
                    resp.PoNum = nnn.Single_Payment_Corp_Resp.Body.PONum;
                }
                else
                {
                    resp.RefNo = "";
                }

               

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
                request3.RefNo8 = resp.PoNum;
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
                request3.RefNo5 = req.Amount.ToString();
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
                request3.RefNo5 = req.Amount.ToString();
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

        public async Task<SimpleResponse> PayoutTransactionwithoutBenIMPS(SinglePaymentRequestFT req, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser, int Caaltypeid = 0)
        {
            PaymentRequestIMPS requestreq = new PaymentRequestIMPS();
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
            if (Convert.ToDecimal(req.Amount) <= 0)
            {
                response.SetError(ErrorCodes.SP_131);
                return response;
            }

            RblAccountBalalnceResponse respbal = new RblAccountBalalnceResponse();
            RblPayoutRequest payoutRequest = new RblPayoutRequest();
            payoutRequest.ApproverId = "";
            payoutRequest.PartnerRetailorId = req.SenderMobile;
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

            //user transaction configration
            UserConfigResponse userConfig = new UserConfigResponse();
            userConfig = await _userprovider.GetUserConfig(serviceUser);
            if (userConfig == null)
            {
                response.SetError(ErrorCodes.SP_137);
                return response;
            }
            if (userConfig.ChargeTypeOn == 0)
            {
                response.SetError(ErrorCodes.SP_137);
                return response;
            }
            if (userConfig.PlanId == 0)
            {
                response.SetError(ErrorCodes.SP_137);
                return response;
            }
            if (req.Amount > userConfig.MaxTxn)
            {
                response.SetError(ErrorCodes.SP_138);
                return response;
            }
            if (req.Amount < userConfig.MinTxn)
            {
                response.SetError(ErrorCodes.SP_139);
                return response;
            }

            if (userConfig.ChargeTypeOn == (int)ChargeDeductionType.FromTransaction)
            {
                //service charge calculation and validation
                SevicechargeByPlanRequest sevicechargeRequest = new SevicechargeByPlanRequest();
                sevicechargeRequest.ServiceId = 1;
                sevicechargeRequest.AgencyId = 1;
                sevicechargeRequest.Amount = Convert.ToDecimal(req.Amount);


                SevicechargeResponse sevicechargeResponse = new SevicechargeResponse();
                sevicechargeResponse = await _provider.GetServiceChargeDetailByPlan(sevicechargeRequest);
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
            request1.partnerretailorid = req.SenderMobile;
            request1.TxnPlateForm = "API";
            request1.agencyid = 1;
            request1.serviceid = 1;
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
            if (Caaltypeid == 0)
            {
                long benid = 0;

                AddBenficiaryRequest objb = new AddBenficiaryRequest();
                objb.BenBankcode = req.Ben_BankCd;
                objb.BenBranchCode = req.Ben_BranchCd;
                objb.BenAddress = "";
                objb.BenAccountNo = req.Ben_Acct_No;
                objb.BenbankName = req.Ben_BankName;
                objb.BenficiaryName = req.Ben_Name;
                objb.BenIfsccode = req.Ben_IFSC;
                objb.BenMobile = req.Ben_Mobile;
                benid = await _userprovider.AddNewBenficiary(objb, serviceUser);
                if (benid == 0)
                {
                    response.SetError(ErrorCodes.SP_128);
                    return response;
                }
            }


            //api payment initiations
            Single_Payment_Corp_ReqIMPS spr = new Single_Payment_Corp_ReqIMPS();
            SignatureIMPS si = new SignatureIMPS();
            si.Signature = "";
            spr.Signature = si;
            HeaderIMPS hp = new HeaderIMPS();
            hp.Approver_ID = "";
            hp.Checker_ID = "";
            hp.Corp_ID = SANYUKTApplicationConfiguration.Instance.RblPayoutCORPID.ToString();
            hp.TranID = transactionResponse.Transactioncode;
            hp.Maker_ID ="";
            spr.Header = hp;

            BodyIMPS bp = new BodyIMPS();
            bp.Amount = req.Amount.ToString();
            bp.Ben_BranchCd = req.Ben_BranchCd;
            bp.Ben_BankCd = req.Ben_BankCd;
            bp.Ben_Address = "";
            bp.Issue_BranchCd = "";
            bp.Ben_Acct_No = req.Ben_Acct_No;
            bp.Ben_TrnParticulars = req.Ben_TrnParticulars;
            bp.Ben_PartTrnRmks = "SINGLE PAYMENT";
            bp.Debit_PartTrnRmks = "";
            bp.Ben_BankName = req.Ben_BankName;
            bp.Remarks = "PAYEMNT QUEUE";
            bp.Ben_Email = "";
            bp.Ben_IFSC = req.Ben_IFSC;
            bp.Ben_Mobile = req.Ben_Mobile;
            bp.Ben_Name = req.Ben_Name;
            bp.Debit_Acct_Name = serviceList.ServiceAccName;
            bp.Debit_Acct_No = serviceList.ServiceAccountNo;
            bp.Debit_IFSC = serviceList.ServcieIfsccode;
            bp.Debit_Mobile = req.SenderMobile;
            bp.Debit_TrnParticulars = "Transaction By-" + req.SenderMobile.ToString();
            bp.Mode_of_Pay = "IMPS";
            spr.Body = bp;
            requestreq.Single_Payment_Corp_Req = spr;


            //api execution 

            var _clientHandler = new HttpClientHandler();
            _clientHandler.ClientCertificates.Add(Certificatetext);
            _clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var client = new HttpClient(_clientHandler);
            string Url = SANYUKTApplicationConfiguration.Instance.RblPayoutBaseUrl.ToString();
            string fullurl = Url + "v1/payments/corp/payment?client_id=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientId.ToString() + "&client_secret=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientSecrat.ToString();
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
            SinglePaymentResponseIMPS nnn = new SinglePaymentResponseIMPS();
            UpdateTransactionStatusRequest request3 = new UpdateTransactionStatusRequest();
            if (response1.StatusCode == HttpStatusCode.OK)
            {
                string jsonData = JsonConvert.SerializeObject(response.Result);
                dynamic jsonn = JsonConvert.DeserializeObject(jsonData);



                string strRemSlash = jsonn.Replace("\"", "\'");
                string strRemNline = strRemSlash.Replace("\n", " ");
                // Time to desrialize it to convert it into an object class.
                nnn = JsonConvert.DeserializeObject<SinglePaymentResponseIMPS>(@strRemNline);
                resp.Status = nnn.Single_Payment_Corp_Resp.Header.Status;
                resp.ChanelPartnerRefNo = req.PartnerRefNo;
                resp.ErorrDescription = nnn.Single_Payment_Corp_Resp.Header.Error_Desc;
                resp.ErrorCode = nnn.Single_Payment_Corp_Resp.Header.Error_Cde;
                resp.TransactionId = nnn.Single_Payment_Corp_Resp.Header.TranID;
                if (resp.Status == "Success")
                {
                    resp.RefNo = nnn.Single_Payment_Corp_Resp.Body.channelpartnerrefno;
                    resp.RRN = nnn.Single_Payment_Corp_Resp.Body.RRN;
                    resp.BenIFSC = nnn.Single_Payment_Corp_Resp.Body.BenIFSC;
                    resp.Ben_Acct_No = nnn.Single_Payment_Corp_Resp.Body.Ben_Acct_No;
                    resp.Amount = nnn.Single_Payment_Corp_Resp.Body.Amount;
                    resp.Txn_Time = nnn.Single_Payment_Corp_Resp.Body.Txn_Time;
                    resp.BankRefNo = nnn.Single_Payment_Corp_Resp.Body.RefNo;
                }
                else
                {
                    resp.RefNo = nnn.Single_Payment_Corp_Resp.Body.channelpartnerrefno;
                    resp.RRN = nnn.Single_Payment_Corp_Resp.Body.RRN;
                    resp.BenIFSC = nnn.Single_Payment_Corp_Resp.Body.BenIFSC;
                    resp.Ben_Acct_No = nnn.Single_Payment_Corp_Resp.Body.Ben_Acct_No;
                    resp.Amount = nnn.Single_Payment_Corp_Resp.Body.Amount;
                    resp.Txn_Time = nnn.Single_Payment_Corp_Resp.Body.Txn_Time;
                    resp.BankRefNo = nnn.Single_Payment_Corp_Resp.Body.RefNo;
                }

              

                request3.RefNo = resp.RRN;
                request3.RelatedReference = resp.RefNo;
                request3.RefNo1 = resp.BankRefNo;
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
                request3.RefNo5 = req.Amount.ToString();
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
                request3.RefNo5 = req.Amount.ToString();
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

        public async Task<SimpleResponse> PayoutTransactionwithoutBen(SinglePaymentRequestFT req, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
           SimpleResponse response=new SimpleResponse();
            if(req.Mode_of_Pay=="FT")
            {
                response=await PayoutTransactionwithoutBenFT(req, Certificatetext, serviceUser,0);
            }
            else if(req.Mode_of_Pay=="NEFT")
            {
                response=await PayoutTransactionwithoutBenNEFT(req, Certificatetext, serviceUser,0);  
            }
            else if(req.Mode_of_Pay=="RTGS")
            {
                response= await PayoutTransactionwithoutBenRTGS(req, Certificatetext, serviceUser,0);

            }
            else if( req.Mode_of_Pay=="IMPS")
            {
                response=await PayoutTransactionwithoutBenIMPS(req, Certificatetext, serviceUser,0);
            }
            else
            {
                response.SetError(ErrorCodes.SP_130);
            }

            return response;
        }
      
        public async Task<SimpleResponse> PayoutTransaction(PayoutTransaction req, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();

           

            BenficiaryResponse resben = new BenficiaryResponse();
            resben = await _userprovider.GetBenficiaryDetailsByID(Convert.ToInt64(req.BenficiaryID));
            if (resben == null)
            {
                response.SetError(ErrorCodes.SP_129);
                return response;
            }
            SinglePaymentRequestFT fT = new SinglePaymentRequestFT();
            fT.Amount =Convert.ToDecimal( req.Amount);
            fT.Ben_BankName=resben.BenbankName;
            fT.Ben_BankCd = resben.BenBankcode;
            fT.Ben_BranchCd=resben.BenBranchCode;
            fT.Mode_of_Pay=req.Mode_of_Pay;
            fT.Ben_TrnParticulars=req.Ben_TrnParticulars;
            fT.Ben_Acct_No = resben.BenAccountNo;
            fT.Ben_IFSC=resben.BenIfsccode;
            fT.Ben_Mobile=resben.BenMobile;
            fT.Ben_Name=resben.BenficiaryName;
            fT.PartnerRefNo=req.PartnerRefNo;
            fT.SenderMobile = req.SenderMobile;

            if (req.Mode_of_Pay == "FT")
            {
                response = await PayoutTransactionwithoutBenFT(fT, Certificatetext, serviceUser,1);
            }
            else if (req.Mode_of_Pay == "NEFT")
            {
                response = await PayoutTransactionwithoutBenNEFT(fT, Certificatetext, serviceUser,1);
            }
            else if (req.Mode_of_Pay == "RTGS")
            {
                response = await PayoutTransactionwithoutBenRTGS(fT, Certificatetext, serviceUser, 1);

            }
            else if (req.Mode_of_Pay == "IMPS")
            {
                response = await PayoutTransactionwithoutBenIMPS(fT, Certificatetext, serviceUser,1);
            }
            else
            {
                response.SetError(ErrorCodes.SP_130);
            }

            return response;
        }

      
        public async Task<RblStatusResponse> PayoutTransactionStatus(SinglePaymentStatusNew obbb, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            RblStatusResponse resp =new RblStatusResponse();
            TransactionDetailsRequest txndrequest=new TransactionDetailsRequest();
            txndrequest.TransactionCode=obbb.TransactionID;
            txndrequest.PartnerTransactionId = obbb.PartnerTxnId;
            TransactionDetailListResponse resptxn=new TransactionDetailListResponse();
            SinglePaymentStatus mm=new SinglePaymentStatus();
            mm.ApproverId = "";
            mm.TxnPlateForm = "API";
            mm.PartnerRetailorId = "";
            mm.PartnerRefNo = obbb .PartnerTxnId;
            mm.CheckedrId = "";
            mm.MakerId = "";


            resptxn = await _provider.GetTransactionDetail(txndrequest, serviceUser);
            if (resptxn != null)
            {
                if (resptxn.RefNo7  == "FT")
                {
                    mm.RefNo = resptxn.RefNo;
                    resp = await FTTransactionStatus(mm, Certificatetext, serviceUser);
                }
                else if(resptxn.RefNo7=="NEFT")
                {
                    mm.RefNo = resptxn.RefNo;
                    resp = await NEFTTransactionStatus(mm, Certificatetext, serviceUser);
                }
                else if (resptxn.RefNo7 == "RTGS")
                {
                    mm.RefNo = resptxn.RefNo;
                    resp = await RTGSTransactionStatus(mm, Certificatetext, serviceUser);
                }
                else if (resptxn.RefNo7 == "IMPS")
                {
                    mm.RefNo = resptxn.RefNo;
                    resp = await IMPSTransactionStatus(mm, Certificatetext, serviceUser);
                }
                else
                {
                    resp.Status = "";
                    resp.ChanelPartnerRefNo =obbb.PartnerTxnId;
                    resp.ErorrDescription = "Invalid Transaction Detail";
                    resp.ErrorCode =  "SP_9001";
                    resp.TransactionId = obbb.TransactionID;
                    resp.Amount = "";
                    resp.REFNO = "";
                    resp.Txntime = "";
                    resp.BenaccountNo = "";
                    resp.BenIfsccode ="";
                    resp.PONUM = "";
                    resp.RRN = "";
                }
            }
        
            return resp;
        }

        public async Task<RblStatusResponse> NEFTTransactionStatus(SinglePaymentStatus obbb, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();

            BaseTransactionRequest request1 = new BaseTransactionRequest();
            request1.agencyid = 1;
            request1.serviceid = 1;
            request1.TxnPlateForm = obbb.TxnPlateForm;
            request1.partnerreferenceno = obbb.PartnerRefNo;
            request1.partnerretailorid = obbb.PartnerRetailorId;
            request1.TxnType = "Transaction Status";

            RblStatusResponse resp = new RblStatusResponse();
            PaymentStatusRequestNEFT requestreq = new PaymentStatusRequestNEFT();
            GetSinglePaymentStatusCorpReqNEFT gsps = new GetSinglePaymentStatusCorpReqNEFT();
            Signature1 si = new Signature1();
            si.Signature = "";

            gsps.Signature = si;
            Headerstaus hd = new Headerstaus();
            hd.Approver_ID = obbb.ApproverId;
            hd.TranID = await _provider.NewNonFinacialTransaction(request1, serviceUser);
            hd.Corp_ID = SANYUKTApplicationConfiguration.Instance.RblPayoutCORPID;
            hd.Maker_ID = obbb.MakerId;
            hd.Checker_ID = obbb.CheckedrId;
            BodyStatusNEFT bs = new BodyStatusNEFT();
            bs.UTRNo = obbb.RefNo;
            gsps.Header = hd;
            gsps.Body = bs;
            requestreq.get_Single_Payment_Status_Corp_Req = gsps;

            var _clientHandler = new HttpClientHandler();
            _clientHandler.ClientCertificates.Add(Certificatetext);
            _clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var client = new HttpClient(_clientHandler);
            string Url = SANYUKTApplicationConfiguration.Instance.RblPayoutBaseUrl.ToString();
            string fullurl = Url + "v1/payments/corp/payment/query?client_id=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientId.ToString() + "&client_secret=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientSecrat.ToString();
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
            SinglePaymentStatusResponseNEFT nnn = new SinglePaymentStatusResponseNEFT();
            UpdateNonfinacialRequest request2 = new UpdateNonfinacialRequest();
            if (response1.StatusCode == HttpStatusCode.OK)
            {
                string jsonData = JsonConvert.SerializeObject(response.Result);
                dynamic jsonn = JsonConvert.DeserializeObject(jsonData);

                string strRemSlash = jsonn.Replace("\"", "\'");
                string strRemNline = strRemSlash.Replace("\n", " ");
                // Time to desrialize it to convert it into an object class.
                nnn = JsonConvert.DeserializeObject<SinglePaymentStatusResponseNEFT>(@strRemNline);
                resp.Status = nnn.get_Single_Payment_Status_Corp_Res.Body.TXNSTATUS;
                resp.ChanelPartnerRefNo = obbb.PartnerRefNo;
                resp.ErorrDescription = nnn.get_Single_Payment_Status_Corp_Res.Header.Error_Desc;
                resp.ErrorCode = nnn.get_Single_Payment_Status_Corp_Res.Header.Error_Cde;
                resp.TransactionId = nnn.get_Single_Payment_Status_Corp_Res.Body.ORGTRANSACTIONID;
                resp.Amount = nnn.get_Single_Payment_Status_Corp_Res.Body.AMOUNT;
                resp.REFNO = nnn.get_Single_Payment_Status_Corp_Res.Body.REFNO;
                resp.Txntime = nnn.get_Single_Payment_Status_Corp_Res.Body.TXNTIME;
                resp.BenaccountNo = nnn.get_Single_Payment_Status_Corp_Res.Body.BEN_ACCT_NO;
                resp.BenIfsccode = nnn.get_Single_Payment_Status_Corp_Res.Body.BENIFSC;
                resp.PONUM = nnn.get_Single_Payment_Status_Corp_Res.Body.PONUM;
                resp.UTRNO = nnn.get_Single_Payment_Status_Corp_Res.Body.UTRNO;
                resp.TXNType = "NEFT";

                request2.errorcode = resp.ErrorCode;
                request2.errorDescrtiopn = resp.ErorrDescription;
                request2.Txncode = nnn.get_Single_Payment_Status_Corp_Res.Header.TranID;
                await _provider.UpdateNonFinacialTransaction(request2, serviceUser);
            }
            else if (response1.StatusCode == HttpStatusCode.Unauthorized)
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

        public async Task<RblStatusResponse> RTGSTransactionStatus(SinglePaymentStatus obbb, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();

            BaseTransactionRequest request1 = new BaseTransactionRequest();
            request1.agencyid = 1;
            request1.serviceid = 1;
            request1.TxnPlateForm = obbb.TxnPlateForm;
            request1.partnerreferenceno = obbb.PartnerRefNo;
            request1.partnerretailorid = obbb.PartnerRetailorId;
            request1.TxnType = "Transaction Status";

            RblStatusResponse resp = new RblStatusResponse();
            PaymentStatusRequestNEFT requestreq = new PaymentStatusRequestNEFT();
            GetSinglePaymentStatusCorpReqNEFT gsps = new GetSinglePaymentStatusCorpReqNEFT();
            Signature1 si = new Signature1();
            si.Signature = "";

            gsps.Signature = si;
            Headerstaus hd = new Headerstaus();
            hd.Approver_ID = obbb.ApproverId;
            hd.TranID = await _provider.NewNonFinacialTransaction(request1, serviceUser);
            hd.Corp_ID = SANYUKTApplicationConfiguration.Instance.RblPayoutCORPID;
            hd.Maker_ID = obbb.MakerId;
            hd.Checker_ID = obbb.CheckedrId;
            BodyStatusNEFT bs = new BodyStatusNEFT();
            bs.UTRNo = obbb.RefNo;
            gsps.Header = hd;
            gsps.Body = bs;
            requestreq.get_Single_Payment_Status_Corp_Req = gsps;

            var _clientHandler = new HttpClientHandler();
            _clientHandler.ClientCertificates.Add(Certificatetext);
            _clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var client = new HttpClient(_clientHandler);
            string Url = SANYUKTApplicationConfiguration.Instance.RblPayoutBaseUrl.ToString();
            string fullurl = Url + "v1/payments/corp/payment/query?client_id=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientId.ToString() + "&client_secret=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientSecrat.ToString();
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
            SinglePaymentStatusResponseNEFT nnn = new SinglePaymentStatusResponseNEFT();
            UpdateNonfinacialRequest request2 = new UpdateNonfinacialRequest();
            if (response1.StatusCode == HttpStatusCode.OK)
            {
                string jsonData = JsonConvert.SerializeObject(response.Result);
                dynamic jsonn = JsonConvert.DeserializeObject(jsonData);

                string strRemSlash = jsonn.Replace("\"", "\'");
                string strRemNline = strRemSlash.Replace("\n", " ");
                // Time to desrialize it to convert it into an object class.
                nnn = JsonConvert.DeserializeObject<SinglePaymentStatusResponseNEFT>(@strRemNline);
                resp.Status = nnn.get_Single_Payment_Status_Corp_Res.Body.TXNSTATUS;
                resp.ChanelPartnerRefNo = obbb.PartnerRefNo;
                resp.ErorrDescription = nnn.get_Single_Payment_Status_Corp_Res.Header.Error_Desc;
                resp.ErrorCode = nnn.get_Single_Payment_Status_Corp_Res.Header.Error_Cde;
                resp.TransactionId = nnn.get_Single_Payment_Status_Corp_Res.Body.ORGTRANSACTIONID;
                resp.Amount = nnn.get_Single_Payment_Status_Corp_Res.Body.AMOUNT;
                resp.REFNO = nnn.get_Single_Payment_Status_Corp_Res.Body.REFNO;
                resp.Txntime = nnn.get_Single_Payment_Status_Corp_Res.Body.TXNTIME;
                resp.BenaccountNo = nnn.get_Single_Payment_Status_Corp_Res.Body.BEN_ACCT_NO;
                resp.BenIfsccode = nnn.get_Single_Payment_Status_Corp_Res.Body.BENIFSC;
                resp.PONUM = nnn.get_Single_Payment_Status_Corp_Res.Body.PONUM;
                resp.UTRNO = nnn.get_Single_Payment_Status_Corp_Res.Body.UTRNO;

                resp.TXNType = "RTGS";

                request2.errorcode = resp.ErrorCode;
                request2.errorDescrtiopn = resp.ErorrDescription;
                request2.Txncode = nnn.get_Single_Payment_Status_Corp_Res.Header.TranID;
                await _provider.UpdateNonFinacialTransaction(request2, serviceUser);
            }
            else if (response1.StatusCode == HttpStatusCode.Unauthorized)
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

        public async Task<RblStatusResponse> FTTransactionStatus(SinglePaymentStatus obbb, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();

            BaseTransactionRequest request1 = new BaseTransactionRequest();
            request1.agencyid = 1;
            request1.serviceid = 1;
            request1.TxnPlateForm = obbb.TxnPlateForm;
            request1.partnerreferenceno = obbb.PartnerRefNo;
            request1.partnerretailorid = obbb.PartnerRetailorId;
            request1.TxnType = "Transaction Status";

            RblStatusResponse resp = new RblStatusResponse();
            PaymentStatusRequestFT requestreq = new PaymentStatusRequestFT();
            GetSinglePaymentStatusCorpReqFT gsps = new GetSinglePaymentStatusCorpReqFT();
            Signature1 si = new Signature1();
            si.Signature = "";

            gsps.Signature = si;
            Headerstaus hd = new Headerstaus();
            hd.Approver_ID = obbb.ApproverId;
            hd.TranID = await _provider.NewNonFinacialTransaction(request1, serviceUser);
            hd.Corp_ID = SANYUKTApplicationConfiguration.Instance.RblPayoutCORPID;
            hd.Maker_ID = obbb.MakerId;
            hd.Checker_ID = obbb.CheckedrId;
            BodyStatusFT bs = new BodyStatusFT();
            bs.RefNo = obbb.RefNo;
            gsps.Header = hd;
            gsps.Body = bs;
            requestreq.get_Single_Payment_Status_Corp_Req = gsps;

            var _clientHandler = new HttpClientHandler();
            _clientHandler.ClientCertificates.Add(Certificatetext);
            _clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var client = new HttpClient(_clientHandler);
            string Url = SANYUKTApplicationConfiguration.Instance.RblPayoutBaseUrl.ToString();
            string fullurl = Url + "v1/payments/corp/payment/query?client_id=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientId.ToString() + "&client_secret=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientSecrat.ToString();
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
            SinglePaymentStatusResponseFT nnn = new SinglePaymentStatusResponseFT();
            UpdateNonfinacialRequest request2 = new UpdateNonfinacialRequest();
            if (response1.StatusCode == HttpStatusCode.OK)
            {
                string jsonData = JsonConvert.SerializeObject(response.Result);
                dynamic jsonn = JsonConvert.DeserializeObject(jsonData);

                string strRemSlash = jsonn.Replace("\"", "\'");
                string strRemNline = strRemSlash.Replace("\n", " ");
                // Time to desrialize it to convert it into an object class.
                nnn = JsonConvert.DeserializeObject<SinglePaymentStatusResponseFT>(@strRemNline);
                resp.Status = nnn.get_Single_Payment_Status_Corp_Res.Body.TXNSTATUS;
                resp.ChanelPartnerRefNo = obbb.PartnerRefNo;
                resp.ErorrDescription = nnn.get_Single_Payment_Status_Corp_Res.Header.Error_Desc;
                resp.ErrorCode = nnn.get_Single_Payment_Status_Corp_Res.Header.Error_Cde;
                resp.TransactionId = nnn.get_Single_Payment_Status_Corp_Res.Body.ORGTRANSACTIONID;
                resp.Amount = nnn.get_Single_Payment_Status_Corp_Res.Body.AMOUNT;
                resp.REFNO = nnn.get_Single_Payment_Status_Corp_Res.Body.REFNO;
                resp.Txntime = nnn.get_Single_Payment_Status_Corp_Res.Body.TXNTIME;
                resp.BenaccountNo = nnn.get_Single_Payment_Status_Corp_Res.Body.BEN_ACCT_NO;
                resp.BenIfsccode = nnn.get_Single_Payment_Status_Corp_Res.Body.BENIFSC;
                resp.TXNType = "FT";

                request2.errorcode = resp.ErrorCode;
                request2.errorDescrtiopn = resp.ErorrDescription;
                request2.Txncode = nnn.get_Single_Payment_Status_Corp_Res.Header.TranID;
                await _provider.UpdateNonFinacialTransaction(request2, serviceUser);
            }
            else if (response1.StatusCode == HttpStatusCode.Unauthorized)
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

        public async Task<RblStatusResponse> IMPSTransactionStatus(SinglePaymentStatus obbb, X509Certificate2 Certificatetext, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();

            BaseTransactionRequest request1 = new BaseTransactionRequest();
            request1.agencyid = 1;
            request1.serviceid = 1;
            request1.TxnPlateForm = obbb.TxnPlateForm;
            request1.partnerreferenceno = obbb.PartnerRefNo;
            request1.partnerretailorid = obbb.PartnerRetailorId;
            request1.TxnType = "Transaction Status";

            RblStatusResponse resp = new RblStatusResponse();
            PaymentStatusRequestIMPS requestreq = new PaymentStatusRequestIMPS();
            GetSinglePaymentStatusCorpReqIMPS gsps = new GetSinglePaymentStatusCorpReqIMPS();
            Signature1 si = new Signature1();
            si.Signature = "";

            gsps.Signature = si;
            Headerstaus hd = new Headerstaus();
            hd.Approver_ID = obbb.ApproverId;
            hd.TranID = await _provider.NewNonFinacialTransaction(request1, serviceUser);
            hd.Corp_ID = SANYUKTApplicationConfiguration.Instance.RblPayoutCORPID;
            hd.Maker_ID = obbb.MakerId;
            hd.Checker_ID = obbb.CheckedrId;
            BodyStatusIMPS bs = new BodyStatusIMPS();
            bs.RRN = obbb.RefNo;
            gsps.Header = hd;
            gsps.Body = bs;
            requestreq.get_Single_Payment_Status_Corp_Req = gsps;

            var _clientHandler = new HttpClientHandler();
            _clientHandler.ClientCertificates.Add(Certificatetext);
            _clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var client = new HttpClient(_clientHandler);
            string Url = SANYUKTApplicationConfiguration.Instance.RblPayoutBaseUrl.ToString();
            string fullurl = Url + "v1/payments/corp/payment/query?client_id=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientId.ToString() + "&client_secret=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientSecrat.ToString();
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
            SinglePaymentStatusResponseIMPS nnn = new SinglePaymentStatusResponseIMPS();
            UpdateNonfinacialRequest request2 = new UpdateNonfinacialRequest();
            if (response1.StatusCode == HttpStatusCode.OK)
            {
                string jsonData = JsonConvert.SerializeObject(response.Result);
                dynamic jsonn = JsonConvert.DeserializeObject(jsonData);

                string strRemSlash = jsonn.Replace("\"", "\'");
                string strRemNline = strRemSlash.Replace("\n", " ");
                // Time to desrialize it to convert it into an object class.
                nnn = JsonConvert.DeserializeObject<SinglePaymentStatusResponseIMPS>(@strRemNline);
                if(nnn.get_Single_Payment_Status_Corp_Res.Body.PAYMENTSTATUS=="7")
                {
                    resp.Status = "Success";
                }
                else if(nnn.get_Single_Payment_Status_Corp_Res.Body.PAYMENTSTATUS == "8")
                {
                    resp.Status = "Failure";
                }
                else if (nnn.get_Single_Payment_Status_Corp_Res.Body.PAYMENTSTATUS == "9")
                {
                    resp.Status = "In Progress";
                }

                resp.ChanelPartnerRefNo = obbb.PartnerRefNo;
                resp.ErorrDescription = nnn.get_Single_Payment_Status_Corp_Res.Header.Error_Desc;
                resp.ErrorCode = nnn.get_Single_Payment_Status_Corp_Res.Header.Error_Cde;
                resp.TransactionId = nnn.get_Single_Payment_Status_Corp_Res.Body.ORGTRANSACTIONID;
                resp.Amount = nnn.get_Single_Payment_Status_Corp_Res.Body.AMOUNT;
                resp.REFNO = nnn.get_Single_Payment_Status_Corp_Res.Body.REFNO;
                resp.Txntime = nnn.get_Single_Payment_Status_Corp_Res.Body.TXNTIME;
                resp.BenaccountNo = nnn.get_Single_Payment_Status_Corp_Res.Body.BEN_ACCT_NO;
                resp.BenIfsccode = nnn.get_Single_Payment_Status_Corp_Res.Body.IFSCCODE;
                resp.RRN = nnn.get_Single_Payment_Status_Corp_Res.Body.RRN;
                //resp.REMITTERNAME = nnn.get_Single_Payment_Status_Corp_Res.Body.REMITTERNAME;
                //resp.REMITTERMBLNO = nnn.get_Single_Payment_Status_Corp_Res.Body.REMITTERMBLNO;
                //resp.BANK = nnn.get_Single_Payment_Status_Corp_Res.Body.BANK;
                resp.TXNType = "IMPS";

                request2.errorcode = resp.ErrorCode;
                request2.errorDescrtiopn = resp.ErorrDescription;
                request2.Txncode = nnn.get_Single_Payment_Status_Corp_Res.Header.TranID;
                await _provider.UpdateNonFinacialTransaction(request2, serviceUser);
            }
            else if (response1.StatusCode == HttpStatusCode.Unauthorized)
            {

                resp.Status = "Unauthorized";
                resp.ChanelPartnerRefNo = obbb.PartnerRefNo;
                resp.ErorrDescription = "";
                resp.ErrorCode = response1.ReasonPhrase;
                resp.TransactionId = requestreq.get_Single_Payment_Status_Corp_Req.Header.TranID;

                request2.errorcode = response1.ReasonPhrase;
                request2.errorDescrtiopn = response1.ReasonPhrase;
                request2.Txncode = nnn.get_Single_Payment_Status_Corp_Res.Header.TranID;
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
            string fullurl = Url + "v1/payments/corp/payment/query?client_id=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientId.ToString() + "&client_secret=" + SANYUKTApplicationConfiguration.Instance.RblPayoutclientSecrat.ToString();
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
