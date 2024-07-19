using SANYUKT.Datamodel.RblPayoutRequest;
using SANYUKT.Datamodel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.RblPayoutResponse
{

    public class GetSinglePaymentStatusCorpRes
    {
        public Header Header { get; set; }
        public Signature Signature { get; set; }
    }

    public class Header
    {
        public string TranID { get; set; }
        public string Corp_ID { get; set; }
        public string Maker_ID { get; set; }
        public string Checker_ID { get; set; }
        public string Approver_ID { get; set; }
        public string Status { get; set; }
        public string Error_Cde { get; set; }
        public string Error_Desc { get; set; }
    }
    public class SinglePaymentBodyResponseFT
    {
        public string RefNo { get; set; }
        public string Ben_Acct_No { get; set; }
        public string Amount { get; set; }
        public string BenIFSC { get; set; }
        public string Txn_Time { get; set; }

    }
    public class SinglePaymentResponseFT
    {
        public Single_Payment_Corp_RespFT Single_Payment_Corp_Resp { get; set; }

    }
    public class Single_Payment_Corp_RespFT
    {
        public Header Header { get; set; }
        public SinglePaymentBodyResponseFT Body { get; set; }
        public Signature Signature { get; set; }

    }
    public class SinlePaymentBodyResponseNEFT
    {
        public string RefNo { get; set; }
        public string UTRNo { get; set; }
        public string PONum { get; set; }
        public string Ben_Acct_No { get; set; }
        public string Amount { get; set; }
        public string BenIFSC { get; set; }
        public string Txn_Time { get; set; }

    }
    public class Single_Payment_Corp_RespNEFT
    {
        public Header Header { get; set; }
        public SinlePaymentBodyResponseNEFT Body { get; set; }
        public Signature Signature { get; set; }

    }
    public class SinglePaymentResponseNEFT
    {
        public Single_Payment_Corp_RespNEFT Single_Payment_Corp_Resp { get; set; }

    }
    public class SinlePaymentBodyResponseIMPS
    {
        public string RefNo { get; set; }
        public string channelpartnerrefno { get; set; }
        public string RRN { get; set; }
        public string Ben_Acct_No { get; set; }
        public string Amount { get; set; }
        public string BenIFSC { get; set; }
        public string Txn_Time { get; set; }

    }
    public class Single_Payment_Corp_RespIMPS
    {
        public Header Header { get; set; }
        public SinlePaymentBodyResponseIMPS Body { get; set; }
        public Signature Signature { get; set; }

    }
    public class SinglePaymentResponseIMPS
    {
        public Single_Payment_Corp_RespIMPS Single_Payment_Corp_Resp { get; set; }

    }
    public class GetsinglePaymentReponse
    {
        public GetSinglePaymentStatusCorpRes get_Single_Payment_Status_Corp_Res { get; set; }

    }

    public class Signature
    {
        public string Signature1 { get; set; }
    }
    public class RblResponse
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public string ErrorCode { get; set; }
        public string ErorrDescription { get; set; }
        public string ChanelPartnerRefNo { get; set; }
       
    }
    public class RblAccountBalalnceResponse :RblResponse
    {
        public string BalalnceAmount { get; set; }
      
    }
    public class RblTransactionResponse: RblResponse
    {
        public string BankRefNo { get; set; }
        public string RRN { get; set; }
        public string Ben_Acct_No { get; set; }
        public string Amount { get; set; }
        public string BenIFSC { get; set; }
        public string Txn_Time { get; set; }
        public string RefNo { get; set; }
        public string PoNum { get; set; }

    }
    public class SingleRblPaymentResponse
    {
        public SinglePaymentCorpResp Single_Payment_Corp_Resp { get; set; }
    }
    public class SinglePaymentCorpResp
    {
        public Header Header { get; set; }
        public BodySingle Body { get; set; }
        public Signature Signature { get; set; }
    }
    public class BodySingle
    {
        public string channelpartnerrefno { get; set; }
        public string RRN { get; set; }
        public string Ben_Acct_No { get; set; }
        public string Amount { get; set; }
        public string BenIFSC { get; set; }
        public string Txn_Time { get; set; }
        public string RefNo { get; set; }
    }
    public class GetAccbalResponse
    {
        public GetAccountBalanceRes getAccountBalanceRes { get; set; }
    }
    public class GetAccountBalanceRes
    {
        public Header Header { get; set; }
        public BodyBla Body { get; set; }
        public Signature Signature { get; set; }
    }
    public class BalAmt
    {
        public string amountValue { get; set; }
        public CurrencyCode currencyCode { get; set; }
    }

    public class BodyBla
    {
        public BalAmt BalAmt { get; set; }
    }

    public class CurrencyCode
    {
    }

}
