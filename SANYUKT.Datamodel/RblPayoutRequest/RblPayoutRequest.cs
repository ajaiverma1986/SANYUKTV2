using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.RblPayoutRequest
{
    public class RblPayoutBasic
    {
        public string ApproverId { get; set; }
        public string PartnerRefNo { get; set; }
        public string PartnerRetailorId {  get; set; }
        public string TxnPlateForm {  get; set; }
    }
    public class RblPayoutBasicchild : RblPayoutBasic
    {
        public string CheckedrId { get; set; }
        public string MakerId { get; set; }
    }
    public class RblPayoutRequest : RblPayoutBasic
    {
        public string AccountNo { get; set; }
    }
    public class SinglePaymentStatus : RblPayoutBasicchild
    {
        public string UTRNo { get; set; }
    }
    public class RblPayoutStatementRequest : RblPayoutBasic
    {
        public string Acc_No { get; set; }
        public string Tran_Type { get; set; }
        public string From_Dt { get; set; }
        public PaginationDetails Pagination_Details { get; set; }
        public string To_Dt { get; set; }
    }

    public class SinglePaymentRequest : RblPayoutBasicchild
    {
        public string Amount { get; set; }
        public string Debit_TrnParticulars { get; set; }
        public string Ben_IFSC { get; set; }
        public string Ben_Acct_No { get; set; }
        public string Ben_Name { get; set; }
        public string Ben_BankName { get; set; }
        public string Ben_BankCd { get; set; }
        public string Ben_BranchCd { get; set; }
        public string Ben_Mobile { get; set; }
        public string Ben_TrnParticulars { get; set; }
        public string Mode_of_Pay { get; set; }
        public string Remarks { get; set; }

    }
    public class PayoutTransaction
    {
        public string Amount { get; set; }
        public string Debit_TrnParticulars { get; set; }
        public string BenficiaryID { get; set; }
        public string Ben_TrnParticulars { get; set; }
        public string Mode_of_Pay { get; set; }
        public string Remarks { get; set; }
        public string PartnerRefNo { get; set; }
        public string PartnerRetailorId { get; set; }
       
    }
}
