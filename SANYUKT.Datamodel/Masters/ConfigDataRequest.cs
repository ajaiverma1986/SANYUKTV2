using System;
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
        public int? PlanId { get; set; }
        public int? SlabType { get; set; }
        public int? CalculationType { get; set; }
        public int? AgencyID { get; set; }
        public int? ServiceID { get; set; }
        public double? Amount { get; set; }
    }
    public class AddPaymentAccountMasterRequest
    {
        public int? BankID { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string Ifsccode { get; set; }
        public string BranchName { get; set; }
        public string Branchcode { get; set; }
        public string Micrcode { get; set; }
        public string BranchAddress { get; set; }
        public long CreatedBy { get; set; }

    }
    public class CreateapplicationRequest
    {
        public string ApplicationName { get; set; }
        public string ApplicationDescription { get; set; }

    }
    public class ChangePaymentAccStatusRequest
    {
        public int? PaymentAccountID { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; } 
    }
    public class GetServicePolicyRequest
    {
        public int ServiceId { get; set; }
        public int Agencyid { get; set; }
        public int PolicyId { get; set; }
        public string PolicyKey { get; set; }
    }
}
