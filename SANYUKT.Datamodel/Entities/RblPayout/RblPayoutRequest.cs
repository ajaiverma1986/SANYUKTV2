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
       public string description { get; set; }
       

    }
}
