using System;
using System.Data.SqlClient;


namespace SANYUKT.Datamodel.Entities.Transactions
{
    public class PayinRequestListResponse
    {
        public long RequestID { get; set; }
        public long UserId { get; set; }
        public int PaymentChanelID { get; set; }
        public int PaymentModeId { get; set; }
        public int OriginatorAccountId { get; set; }
        public int BenficiaryAccountId { get; set; }
        public decimal Amount { get; set; }
        public decimal? Charge { get; set; }
        public string RefNo1 { get; set; }
        public string RefNo2 { get; set; }
        public string Remarks { get; set; }
        public string RejectedReason { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string OriginatorBank { get; set; }
        public string OrgAccountName { get; set; }
        public string OrgAccountNo { get; set; }
        public string OrgIfsccode { get; set; }
        public string OrgBranchAddress { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string BranchName { get; set; }
        public string Ifsccode { get; set; }
        public string Branchcode { get; set; }
        public string BranchAddress { get; set; }
        public string PaymentChanelName { get; set; }
        public string PaymentModeName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DepositDate { get; set; }

    }

    public class PayinRequestReciptListResponse
    {
        public long RequestID { get; set; }
        public string RecieptFile { get; set; }
     
    }
    public class PayinRequestReciptDownloadResponse
    {
        public long RequestID { get; set; }
        public string RecieptFile { get; set; }
        public Byte[] FileBytes { get; set; }
        public string Base64String { get; set; }
        public string MediaExtension { get; set; }

    }
}
