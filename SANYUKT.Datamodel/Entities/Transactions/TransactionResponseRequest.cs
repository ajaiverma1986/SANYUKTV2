using SANYUKT.Datamodel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.Entities.Transactions
{
    public class SevicechargeRequest
    { 
        public int AgencyId { get; set; }
        public int ServiceId { get; set;}
        public decimal Amount { get; set;}
    }
    public class SevicechargeByPlanRequest
    {
        public int AgencyId { get; set; }
        public int ServiceId { get; set; }
        public decimal Amount { get; set; }
        public int PlanId { get; set; }
    }
    public class TransactionDetailsRequest
    {
        public int AgencyId { get; set; }
        public int ServiceId { get; set; }
        public string TransactionCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string TxnType { get; set; }
        public string PartnerTransactionId { get; set; }
       
    }
    public class TransactionDetailsPayoutRequest
    {
        public string TransactionCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string TxnType { get; set; }
        public string PartnerTransactionId { get; set; }

    }
    public class SevicechargeResponse
    {
        public int CalculationType { get; set; }
        public int SlabType { get; set; }
        public string CalculationTypeName { get; set; }
        public decimal CalculationValue { get; set; }
    }
    public class TransactionDetailListResponse
    {
        public long TransactionId { get; set; }
        public string Transactioncode { get; set; }
        public long PartnerId { get; set; }
        public string PartnerTxnId { get; set; }
        public int ServiceId { get; set; }
        public int AgencyId { get; set; }
        public string PartnerRetailorId { get; set; }
        public string RefNo { get; set; }
        public string RelatedReference { get; set; }
        public string BankTxnDatetime { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TxnFee { get; set; }
        public string RefNo1 { get; set; }
        public string RefNo2 { get; set; }
        public string RefNo3 { get; set; }
        public string RefNo4 { get; set; }
        public string RefNo5 { get; set; }
        public string RefNo6 { get; set; }
        public string RefNo7 { get; set; }
        public string RefNo8 { get; set; }
        public string RefNo9 { get; set; }
        public string RefNo10 { get; set; }
        public string FailureReason { get; set; }
        public int Status { get; set; }
        public string PartnerName { get; set; }


    }

    public class TransactionDetailListPayoutResponse
    {
        public long TransactionId { get; set; }
        public string Transactioncode { get; set; }
        public string PartnerTxnId { get; set; }
        public string RefNo { get; set; }
        public string RelatedReference { get; set; }
        public string BankTxnDatetime { get; set; }
        public decimal? Amount { get; set; }
        //public string RefNo1 { get; set; }
        //public string RefNo2 { get; set; }
        //public string RefNo3 { get; set; }
        //public string RefNo4 { get; set; }
        //public string RefNo5 { get; set; }
        //public string RefNo6 { get; set; }
        //public string RefNo7 { get; set; }
        //public string RefNo8 { get; set; }
        //public string RefNo9 { get; set; }
        //public string RefNo10 { get; set; }
        public string FailureReason { get; set; }
        public int Status { get; set; }
        public string PartnerName { get; set; }


    }

    public class AddPaymentRequestRequest
    {
        public int PaymentChanelID { get; set; }
        public int PaymentModeId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Charge { get; set; }
        public long OriginatorAccountId { get; set; }
        public int BenficiaryAccountId { get; set; }
        public DateTime? DepositDate { get; set; }
        public string RefNo1 { get; set; }
        public string RefNo2 { get; set; }
        public string Remarks { get; set; }
        
    }
    public class ApproveRejectPayinRequest
    {
        public long RequestID { get; set; }
        public string RejectedReason { get; set; }
        public int Status { get; set; }

    }
    public class ListPayinRequestRequest:ListRequest
    {
        public int  PaymentChanelID { get; set; }
        public int PaymentModeId { get; set; }
        public int Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
    public class PayinRecieptRequest
    {
        public long RequestID { get; set; }
        public string RecieptFile { get; set; }
     
    }
    public class TxnListRequest : ListRequest
    {
        public string TransactionCode { get; set; }
        public string TxnType { get; set; }
        public string PartnerTransactionId { get; set; }
        public int Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
    public class UserStatementRequest : ListRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
