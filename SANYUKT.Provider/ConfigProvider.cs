using SANYUKT.Datamodel.Masters;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider.Shared;
using SANYUKT.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SANYUKT.Provider
{
    public class ConfigProvider:BaseProvider
    {
        public readonly ConfigRepository _repository = null;
        public ConfigProvider()
        {
            _repository = new ConfigRepository();
        }
        public async Task<SimpleResponse> GetallCalculationTypeMaster()
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetAllCalculationTypeMaster();
            return response;
        }
        public async Task<SimpleResponse> GetAllChargeDeductionType()
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetAllChargeDeductionType();
            return response;
        }
        public async Task<SimpleResponse> GetallPlanList(int? PlanID)
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetallPlanList(PlanID);
            return response;
        }
        public async Task<SimpleResponse> GetallSlabType()
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetallSlabType();
            return response;
        }
        public async Task<SimpleResponse> GetallCommissionDistribution(CommissionDistributionRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetallCommissionDistribution(request);
            return response;
        }
        public async Task<SimpleResponse> GetAllTopupCharge(TopupChargeRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetAllTopupCharge(request);
            return response;
        }
        public async Task<SimpleResponse> GetallTransactionSlab(TransactionslabRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetallTransactionSlab(request);
            return response;
        }
        public async Task<SimpleResponse> GetAllPaymentAccounts(int? Bankid)
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetAllPaymentAccounts(Bankid);
            return response;
        }
    }
}
