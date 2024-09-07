using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.Masters
{
    public class CalculationMasterResponse
    {
        public int CalculationTypeId { get; set; }
        public string CalculationTypeName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
    }
    public class ChargedeductionTypeListResponse
    {
        public int ChargeDeductionId { get; set; }
        public string ChargeDeductionType { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
    }
    public class PlanMasterListDataResponse
    {
        public int PlanId { get; set; }
        public string PlanCode { get; set; }
        public string PlanName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
    }
    public class SlabTypeListResponse
    {
        public int SlabTypId { get; set; }
        public string SlabTypeName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
    }
    public class CommissionDistributionResponse
    {
        public int MarginConfigrationID { get; set; }
        public int AgencyId { get; set; }
        public int ServiceId { get; set; }
        public int PlanId { get; set; }
        public decimal FromAmount { get; set; }
        public decimal Toamount { get; set; }
        public int CalculationTypeId { get; set; }
        public decimal CalculationValue { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string AgencyName { get; set; }
        public string ServiceName { get; set; }
        public string PlanName { get; set; }
        public string CalculationTypeName { get; set; }

    }

    public class TopupChargeResponse
    {
        public int TopupChargeId { get; set; }
       
        public decimal FromAmount { get; set; }
        public decimal Toamount { get; set; }
        public int SlabTypeId { get; set; }
        public int CalculationTypeId { get; set; }
        public decimal CalculationValue { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string SlabTypeName { get; set; }
        public string CalculationTypeName { get; set; }
    }
    public class TransactionslabResponse
    {
        public int SlabId { get; set; }
        public int PlanId { get; set; }
        public int AgencyID { get; set; }
        public int ServiceID { get; set; }
        public int SlabType { get; set; }
        public int CalculationType { get; set; }
        public decimal FromAmount { get; set; }
        public decimal Toamount { get; set; }
        public decimal CalculationValue { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string SlabTypeName { get; set; }
        public string CalculationTypeName { get; set; }
        public string AgencyName { get; set; }
        public string ServiceName { get; set; }
        public string PlanName { get; set; }
    }

    public class PaymentAccountsListResponse
    {
        public int PaymentAccountID { get; set; }
        public int BankID { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string Ifsccode { get; set; }
        public string BranchName { get; set; }
        public string Branchcode { get; set; }
        public string Micrcode { get; set; }
        public string BranchAddress { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string BankName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }

    public class GetservicePolicyResponse
    {
        public int SysPolicyId { get; set; }
        public string PolicyKey { get; set; }
        public string PolicyValue { get; set; }
       
    }

}
