using SANYUKT.Datamodel.Entities.Transactions;
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
    public class MasterDataProvider:BaseProvider
    {
        public readonly MasterDataRepository _repository = null;
        public MasterDataProvider()
        {
            _repository = new MasterDataRepository();
        }
        public async Task<SimpleResponse> GetAllCompanyTypeMaster(int? CompanyTypeId)
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetAllCompanyTypeMaster(CompanyTypeId);
            return response;
        }
        public async Task<SimpleResponse> GetGender()
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetGender();
            return response;
        }
        public async Task<SimpleResponse> GetMaritalStatus()
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetMaritalStatus();
            return response;
        }
        public async Task<SimpleResponse> GetAdressTypeMaster()
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetAdressTypeMaster();
            return response;
        }
        public async Task<SimpleResponse> GetAgencyTypeMaster()
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetAgencyTypeMaster();
            return response;
        }
        public async Task<SimpleResponse> GetBankList()
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetBankList();
            return response;
        }
        public async Task<SimpleResponse> GetStateList()
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetAllstate();
            return response;
        }
        public async Task<SimpleResponse> GetDistrictList( int? StateId)
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetAllDistrict(StateId);
            return response;
        }
        public async Task<SimpleResponse> GetAllKycTypeMasterList(int? CompanyTypeId,int? UserTypeID)
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetAllKycTypeMasterList(CompanyTypeId, UserTypeID);
            return response;
        }
        public async Task<SimpleResponse> GetAllUserType()
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetUserTypeList();
            return response;
        }
        public async Task<SimpleResponse> GetAllUserAdminType()
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetAllUserAdminType();
            return response;
        }
        public async Task<SimpleResponse> GetDataByPincode(string Pincode)
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetDataByPincode(Pincode);
            return response;
        }
        public async Task<SimpleResponse> GetallLedegrType()
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetallLedegrType();
            return response;
        }
        public async Task<SimpleResponse> GetallPlanMaster()
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetallPlanMaster();
            return response;
        }
        public async Task<SimpleResponse> GetallServiceTypeList(int ? AgencyId)
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetAllServiceType(AgencyId);
            return response;
        }
        public async Task<SimpleResponse> GetAllService(int? ServiceTypeId)
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetAllService(ServiceTypeId);
            return response;
        }
        public async Task<SimpleResponse> GetAllPaymentChanel()
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetAllPaymentChanel();
            return response;
        }
        public async Task<SimpleResponse> GetAllPaymentModes(int? PaymentChanelId)
        {
            SimpleResponse response = new SimpleResponse();
            response = await _repository.GetAllPaymentModes(PaymentChanelId);
            return response;
        }
    }
}
