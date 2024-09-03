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
    public class SinglePaymentStatus: RblPayoutBasicchild
    {
        public string RefNo { get; set; }
    }
    public class SinglePaymentStatusNew
    {
        public string TransactionID { get; set; }
        public string PartnerTxnId { get; set; }
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

    public class HeaderFT
    {
        public string TranID { get; set; }
        public string Corp_ID { get; set; }
        public string Maker_ID { get; set; }
        public string Checker_ID { get; set; }
        public string Approver_ID { get; set; }

    }
    public class BodyFT
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
    public class SignatureFT
    {
        public string Signature { get; set; }

    }
    public class Single_Payment_Corp_ReqFT
    {
        public HeaderFT Header { get; set; }
        public BodyFT Body { get; set; }
        public SignatureFT Signature { get; set; }

    }
    public class PaymentRequestFT
    {
        public Single_Payment_Corp_ReqFT Single_Payment_Corp_Req { get; set; }

    }

    public class HeaderNEFT
    {
        public string TranID { get; set; }
        public string Corp_ID { get; set; }
        public string Maker_ID { get; set; }
        public string Checker_ID { get; set; }
        public string Approver_ID { get; set; }

    }
    public class BodyNEFT
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
    public class SignatureNEFT
    {
        public string Signature { get; set; }

    }
    public class Single_Payment_Corp_ReqNEFT
    {
        public HeaderNEFT Header { get; set; }
        public BodyNEFT Body { get; set; }
        public SignatureNEFT Signature { get; set; }

    }
    public class PaymentRequestNEFT
    {
        public Single_Payment_Corp_ReqNEFT Single_Payment_Corp_Req { get; set; }

    }

    public class HeaderRTGS
    {
        public string TranID { get; set; }
        public string Corp_ID { get; set; }
        public string Maker_ID { get; set; }
        public string Checker_ID { get; set; }
        public string Approver_ID { get; set; }

    }
    public class BodyRTGS
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
    public class SignatureRTGS
    {
        public string Signature { get; set; }

    }
    public class Single_Payment_Corp_ReqRTGS
    {
        public HeaderRTGS Header { get; set; }
        public BodyRTGS Body { get; set; }
        public SignatureRTGS Signature { get; set; }

    }
    public class PaymentRequestRTGS
    {
        public Single_Payment_Corp_ReqRTGS Single_Payment_Corp_Req { get; set; }

    }
    public class HeaderIMPS
    {
        public string TranID { get; set; }
        public string Corp_ID { get; set; }
        public string Maker_ID { get; set; }
        public string Checker_ID { get; set; }
        public string Approver_ID { get; set; }

    }
    public class BodyIMPS
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
    public class SignatureIMPS
    {
        public string Signature { get; set; }

    }
    public class Single_Payment_Corp_ReqIMPS
    {
        public HeaderIMPS Header { get; set; }
        public BodyIMPS Body { get; set; }
        public SignatureIMPS Signature { get; set; }

    }
    public class PaymentRequestIMPS
    {
        public Single_Payment_Corp_ReqIMPS Single_Payment_Corp_Req { get; set; }

    }
    public class SinglePaymentRequestFT 
    {
        public string PartnerRefNo { get; set; }
        public string PartnerRetailorId { get; set; }
        public decimal  Amount { get; set; }
        public string Ben_IFSC { get; set; }
        public string Ben_Acct_No { get; set; }
        public string Ben_Name { get; set; }
        public string Ben_BankName { get; set; }
        public string Ben_BankCd { get; set; }
        public string Ben_BranchCd { get; set; }
        public string Ben_Mobile { get; set; }
        public string Ben_TrnParticulars { get; set; }
        public string Mode_of_Pay { get; set; }

    }
}
