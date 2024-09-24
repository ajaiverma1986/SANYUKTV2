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
}
