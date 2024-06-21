using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SANYUKT.Datamodel.Entities.RblPayout
{
    
    public class NonFinancialTxnRequest
    {
        public int agencyid {  get; set; }
        public int serviceid { get; set; }
        public int partnerid { get; set; }
        public string partnerretailorid { get; set; }
        public string partnerreferenceno { get; set; }
        public string TxnPlateForm { get; set; }

    }
}
