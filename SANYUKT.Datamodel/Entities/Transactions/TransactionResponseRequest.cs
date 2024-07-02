using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.Entities.Transactions
{
    public class SevicechargeRequest
    { 
        public int AgencyId { get; set; }
        public int ServiceId { get; set;}
        public decimal Amount { get; set;}
    }
    public class SevicechargeResponse
    {
        public int CalculationType { get; set; }
        public int SlabType { get; set; }
        public string CalculationTypeName { get; set; }
        public decimal CalculationValue { get; set; }
    }
}
