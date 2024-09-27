using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.Entities
{
    public class TransactionSummaryByUserResponse
    {
        public string ServiceName {  get; set; }    
        public string RepType { get; set; }
        public int TxnCount { get; set; }
        public decimal TotalAmount { get; set; }

    }
    public class GetDayBookRequest
    {
        public long? UserID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
    public class GetDayBookResponse
    {
        public string ServiceName { get; set; }
        public long PartnerId { get; set; }
        public string OrganisationName { get; set; }
        public int txnTotalcount { get; set; }
        public decimal txntotalAmt { get; set; }
        public int txnSuccescount { get; set; }
        public decimal txnSuccesAmt { get; set; }
        public int txnPendingcount { get; set; }
        public decimal txnPendingAmt { get; set; }
        public int txnFailurecount { get; set; }
        public decimal txnFailureAmt { get; set; }
        public decimal Surcharge { get; set; }
        public decimal Commission { get; set; }

    }

    public class GetFirmDetailByFirmId
    {
        public string Usercode { get; set; }
        public long UserId { get; set; }
        public string OrganisationName { get; set; }
        public string ContactPersonName { get; set; }
        public decimal? AvailableLimit { get; set; }
        public decimal? ThresoldLimit { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string LogoUrl { get; set; }
        public string RemarkReason { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string UserPermaAddress { get; set; }
        public decimal? MaxPayinamount { get; set; }
        public int? MaxNoofcountPayin { get; set; }
        public int SameAmountPayinAllowed { get; set; }
        public string SameAmountPayinAllowedText { get; set; }
        public string UserOfficeAddress { get; set; }
        public decimal? MinTxn { get; set; }
        public decimal? MaxTxn { get; set; }
        public int ChargeTypeOn { get; set; }
        public string ChargeDeductionType { get; set; }
        public int? PlanId { get; set; }
        public string PlanName { get; set; }
        public string Pancard { get; set; }
        public string AadharCard { get; set; }
        public string MaskedPan { get; set; }
        public string MaskedAadhar { get; set; }
        public string GSTNo { get; set; }

    }
}
