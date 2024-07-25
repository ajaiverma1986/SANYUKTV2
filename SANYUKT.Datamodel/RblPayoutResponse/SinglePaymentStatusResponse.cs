using SANYUKT.Datamodel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.RblPayoutResponse
{

    public class RblStatusResponse
    {
        public string TransactionId { get; set; }=string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ErrorCode { get; set; }=string.Empty;
        public string ErorrDescription { get; set; } = string.Empty;
        public string ChanelPartnerRefNo { get; set; } = string.Empty;
        public string Amount { get; set; } =string.Empty;
        public string REFNO { get; set; }=string.Empty;
        public string RRN { get; set; }=string.Empty;
        public string Txntime { get; set; }=string.Empty ;
        public string BenaccountNo { get; set; } = string.Empty;
        public string BenIfsccode { get; set; }=string.Empty;
        public string PONUM { get; set; } = string.Empty;
        public string UTRNO { get; set; } = string.Empty;
        //public string REMITTERNAME { get; set; } = string.Empty;
        //public string REMITTERMBLNO { get; set; } = string.Empty;
        //public string BANK { get; set; } = string.Empty;
        public string TXNType { get; set; } = string.Empty;

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
    public class BodyStatusResponseIMPS
    {
        public string ORGTRANSACTIONID { get; set; }
        public string REFNO { get; set; }
        public string RRN { get; set; }
        public string AMOUNT { get; set; }
        public string PAYMENTSTATUS { get; set; }
        public string REMITTERNAME { get; set; }
        public string REMITTERMBLNO { get; set; }
        public string BENEFICIARYNAME { get; set; }
        public string BANK { get; set; }
        public string IFSCCODE { get; set; }
        public string BEN_ACCT_NO { get; set; }
        public string TXNTIME { get; set; }
    }
    public class GetSinglePaymentStatusCorpResIMPS
    {
        public HeaderSatusResponse Header { get; set; }
        public BodyStatusResponseIMPS Body { get; set; }
        public SignatureStatusResponse Signature { get; set; }
    }
    public class SinglePaymentStatusResponseIMPS
    {
        public GetSinglePaymentStatusCorpResIMPS get_Single_Payment_Status_Corp_Res { get; set; }
    }


}
