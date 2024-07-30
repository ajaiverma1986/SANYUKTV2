﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.Masters
{
    public class CommissionDistributionRequest
    {
        public int? AgencyId { get; set; }
        public int? ServiceId { get; set; }
        public int? CalculationTypeId { get; set; }
        public double? amount { get; set; }
        public int? PlanId { get; set; }
    }
    public class TopupChargeRequest
    {
        public int? TopupChargeId { get; set; }
        public int? SlabTypeId { get; set; }
        public int? CalculationTypeId { get; set; }
        public double? Amount { get; set; }
    }
    public class TransactionslabRequest
    {
        public int? SlabId { get; set; }
        public int? SlabType { get; set; }
        public int? CalculationType { get; set; }
        public int? AgencyID { get; set; }
        public int? ServiceID { get; set; }
        public double? Amount { get; set; }
    }
}
