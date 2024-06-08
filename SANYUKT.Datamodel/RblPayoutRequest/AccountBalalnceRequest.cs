using SANYUKT.Datamodel.RblPayoutResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.RblPayoutRequest
{
    public class AccountBalalnceRequest
    {
        public GetAccountBalanceReq getAccountBalanceReq { get; set; }
    }
    public class BodyAcc
    {
        public string AcctId { get; set; }
    }

    public class GetAccountBalanceReq
    {
        public HeaderAcc Header { get; set; }
        public BodyAcc Body { get; set; }
        public Signature1 Signature { get; set; }
    }

    public class HeaderAcc
    {
        public string TranID { get; set; }
        public string Corp_ID { get; set; }
        public string Approver_ID { get; set; }
    }
    public class Signature1
    {
        public string Signature { get; set; }
    }
    //payment
    public class BodyPayment
    {
        public string Amount { get; set; }
        public string Debit_Acct_No { get; set; }
        public string Debit_Acct_Name { get; set; }
        public string Debit_IFSC { get; set; }
        public string Debit_Mobile { get; set; }
        public string Debit_TrnParticulars { get; set; }
        public string Debit_PartTrnRmks { get; set; }
        public string Ben_IFSC { get; set; }
        public string Ben_Acct_No { get; set; }
        public string Ben_Name { get; set; }
        public string Ben_Address { get; set; }
        public string Ben_BankName { get; set; }
        public string Ben_BankCd { get; set; }
        public string Ben_BranchCd { get; set; }
        public string Ben_Email { get; set; }
        public string Ben_Mobile { get; set; }
        public string Ben_TrnParticulars { get; set; }
        public string Ben_PartTrnRmks { get; set; }
        public string Issue_BranchCd { get; set; }
        public string Mode_of_Pay { get; set; }
        public string Remarks { get; set; }
    }

    public class HeaderPayment
    {
        public string TranID { get; set; }
        public string Corp_ID { get; set; }
        public string Maker_ID { get; set; }
        public string Checker_ID { get; set; }
        public string Approver_ID { get; set; }
    }

    public class PaymentRequest
    {
        public SinglePaymentCorpReq Single_Payment_Corp_Req { get; set; }
    }
    public class SinglePaymentCorpReq
    {
        public HeaderPayment Header { get; set; }
        public BodyPayment Body { get; set; }
        public Signature1 Signature { get; set; }
    }

    //statuys

    public class BodyStatus
    {
        public string UTRNo { get; set; }
    }

    public class GetSinglePaymentStatusCorpReq
    {
        public Headerstaus Header { get; set; }
        public BodyStatus Body { get; set; }
        public Signature1 Signature { get; set; }
    }

    public class Headerstaus
    {
        public string TranID { get; set; }
        public string Corp_ID { get; set; }
        public string Maker_ID { get; set; }
        public string Checker_ID { get; set; }
        public string Approver_ID { get; set; }
    }

    public class PaymentStatusRequest
    {
        public GetSinglePaymentStatusCorpReq get_Single_Payment_Status_Corp_Req { get; set; }
    }

   


}
