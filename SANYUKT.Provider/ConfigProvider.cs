using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.Masters;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider.Shared;
using SANYUKT.Repository;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
        public async Task<SimpleResponse> AddPaymentAccounts(AddPaymentAccountMasterRequest request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            response.Result = await _repository.AddPaymentAccounts(request, serviceUser);
            return response;
        }
        public async Task<SimpleResponse> changesPaymentAccountsStatus(ChangePaymentAccStatusRequest request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            response.Result = await _repository.changesPaymentAccountsStatus(request, serviceUser);
            return response;
        }
        public async Task<SimpleResponse> CreateNewApplication(CreateapplicationRequest request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            if(request==null)
            {
                response.SetError(ErrorCodes.SP_133);
                return response;
            }
            if(request.ApplicationName=="")
            {
                response.SetError(ErrorCodes.SP_133);
                return response;
            }

            Guid specificGuid = Guid.NewGuid();
            string apiKey = specificGuid.ToString();

            response.Result = await _repository.CreateNewApplication(request, apiKey.ToUpper(), serviceUser);
            return response;
        }
        public async Task<SimpleResponse> GetServicePolicy(GetServicePolicyRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetServicePolicy(request);
            return response;
        }
    }
}
