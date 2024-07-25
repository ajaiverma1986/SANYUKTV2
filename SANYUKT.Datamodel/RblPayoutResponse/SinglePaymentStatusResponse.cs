using SANYUKT.Datamodel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.RblPayoutResponse
{

    public class RblStatusResponse
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public string ErrorCode { get; set; }
        public string ErorrDescription { get; set; }
        public string ChanelPartnerRefNo { get; set; }
        public string Amount { get; set; }
        public string REFNO { get; set; }
        public string Txntime { get; set; }
        public string BenaccountNo { get; set; }
        public string BenIfsccode { get; set; }
        public string TXNType { get; set; }

    }
    public class HeaderSatusResponse
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
    public class SignatureStatusResponse
    {
        public string Signature { get; set; }
    }
    public class BodyStatusResponseFT
    {
        public string ORGTRANSACTIONID { get; set; }
        public string AMOUNT { get; set; }
        public string REFNO { get; set; }
        public string BEN_ACCT_NO { get; set; }
        public string BENIFSC { get; set; }
        public string TXNSTATUS { get; set; }
        public string STATUSDESC { get; set; }
        public string TXNTIME { get; set; }
    }
    public class GetSinglePaymentStatusCorpResFT
    {
        public HeaderSatusResponse  Header { get; set; }
        public BodyStatusResponseFT Body { get; set; }
        public SignatureStatusResponse Signature { get; set; }
    }
    public class SinglePaymentStatusResponseFT
    {
        public GetSinglePaymentStatusCorpResFT get_Single_Payment_Status_Corp_Res { get; set; }
    }
    public class BodyStatusResponseNEFT
    {
        public string ORGTRANSACTIONID { get; set; }
        public string AMOUNT { get; set; }
        public string REFNO { get; set; }
        public string UTRNO { get; set; }
        public string PONUM { get; set; }
        public string BEN_ACCT_NO { get; set; }
        public string BENIFSC { get; set; }
        public string TXNSTATUS { get; set; }
        public string STATUSDESC { get; set; }
        public string BEN_CONF_RECEIVED { get; set; }
        public string TXNTIME { get; set; }
    }
    public class GetSinglePaymentStatusCorpResNEFT
    {
        public HeaderSatusResponse Header { get; set; }
        public BodyStatusResponseNEFT Body { get; set; }
        public SignatureStatusResponse Signature { get; set; }
    }
    public class SinglePaymentStatusResponseNEFT
    {
        public GetSinglePaymentStatusCorpResNEFT get_Single_Payment_Status_Corp_Res { get; set; }
    }

}
