using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SANYUKT.Datamodel.Entities.RblPayout
{
    public class BaseTransactionRequest
    {
        public int agencyid { get; set; }
        public int serviceid { get; set; }
        public long? partnerid { get; set; }
        public string partnerretailorid { get; set; }
        public string partnerreferenceno { get; set; }
        public string TxnPlateForm { get; set; }
        public string TxnType { get; set; }

    }

   
    public class UpdateNonfinacialRequest
    {
        public string Txncode { get; set; }
        public string errorcode { get; set; }
        public string errorDescrtiopn { get; set; }
        
    }
    public class NewTransactionRequest: BaseTransactionRequest
    {
        public decimal amount { get; set; }
        public decimal txnFee { get; set; }
        public decimal margincom { get; set; }
        public string description { get; set; }
       

    }
    public class UpdateTransactionStatusRequest 
    {
        public string Transactioncode { get; set; }
        public string RefNo { get; set; }
        public string RelatedReference { get; set; }
        public string BankTxnDatetime { get; set; }
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
        public int status { get; set; }

    }
}
