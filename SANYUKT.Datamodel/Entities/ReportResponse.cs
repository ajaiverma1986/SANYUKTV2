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
}
