using SANYUKT.Database;
using SANYUKT.Datamodel.Entities.Transactions;
using SANYUKT.Datamodel.Masters;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SANYUKT.Repository
{
    public class MasterDataRepository:BaseRepository
    {
        public readonly ISANYUKTDatabase _database = null;
        public MasterDataRepository()
        {
            _database = new SANYUKTDatabase();
        }
        public async Task<ServiceListResponse> GetAllServcieList(int ServiceId)
        {
            ServiceListResponse response = null;
            var dbCommand = _database.GetStoredProcCommand("[MDM].GetAllServiceList");
            _database.AddInParameter(dbCommand, "@ServiceId", ServiceId);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                dataReader.Read();
                if (dataReader.HasRows)
                {
                    response = new ServiceListResponse();
                    response.ServiceId = GetInt32Value(dataReader, "ServiceId").Value;
                    response.ServiceTypeId = GetInt32Value(dataReader, "ServiceTypeId").Value;
                    response.ServiceCode = GetStringValue(dataReader, "ServiceCode");
                    response.ServiceName = GetStringValue(dataReader, "ServiceName");
                    response.ServcieIfsccode = GetStringValue(dataReader, "ServcieIfsccode");
                    response.ServiceAccountNo = GetStringValue(dataReader, "ServiceAccountNo");
                    response.ServiceAccName = GetStringValue(dataReader, "ServiceAccName");
                    response.ServiceMobileNo = GetStringValue(dataReader, "ServiceMobileNo");

                }
            }

            return response;
        }

       
        public async Task<SimpleResponse> GetAllCompanyTypeMaster(int? CompanyTypeId)
        {
            SimpleResponse response = new SimpleResponse();
            List<CompanyTypeMasterResponse> objMaster = new List<CompanyTypeMasterResponse>();

            var dbCommand = _database.GetStoredProcCommand("[MDM].ListCompanyTypeMaster");
            _database.AddInParameter(dbCommand, "@CompanyTypeId", CompanyTypeId);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    CompanyTypeMasterResponse obj = new CompanyTypeMasterResponse();

                    obj.CompnayTypeId = GetInt32Value(dataReader, "CompnayTypeId").Value;
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.CompanyTypeName = GetStringValue(dataReader, "CompanyTypeName");
                    obj.StatusName = GetStringValue(dataReader, "StatusName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }

        public async Task<SimpleResponse> GetGender()
        {
            SimpleResponse response = new SimpleResponse();
            List<GenderResponse> objMaster = new List<GenderResponse>();

            var dbCommand = _database.GetStoredProcCommand("usp_GetGender");
           
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    GenderResponse obj = new GenderResponse();

                    obj.GenderId = GetInt32Value(dataReader, "GenderId").Value;
                   
                    obj.GenderName = GetStringValue(dataReader, "GenderName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }

        public async Task<SimpleResponse> GetMaritalStatus()
        {
            SimpleResponse response = new SimpleResponse();
            List<MaritalStatusResponse> objMaster = new List<MaritalStatusResponse>();

            var dbCommand = _database.GetStoredProcCommand("usp_GetMaritalStatus");

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    MaritalStatusResponse obj = new MaritalStatusResponse();

                    obj.MaritalStatusID = GetInt32Value(dataReader, "MaritalStatusID").Value;

                    obj.MaritalStatusName = GetStringValue(dataReader, "MaritalStatusName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }

        public async Task<SimpleResponse> GetAdressTypeMaster()
        {
            SimpleResponse response = new SimpleResponse();
            List<AddressTypeListResponse> objMaster = new List<AddressTypeListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[MDM].AddressTypeMasterList");

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    AddressTypeListResponse obj = new AddressTypeListResponse();

                    obj.AddressTypeId = GetInt32Value(dataReader, "AddressTypeId").Value;

                    obj.AddressTypeName = GetStringValue(dataReader, "AddressTypeName");
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.StatusName = GetStringValue(dataReader, "StatusName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }

        public async Task<SimpleResponse> GetAgencyTypeMaster()
        {
            SimpleResponse response = new SimpleResponse();
            List<AgencyMasterListResponse> objMaster = new List<AgencyMasterListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[MDM].AgencyMasterList");

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    AgencyMasterListResponse obj = new AgencyMasterListResponse();

                    obj.AgencyId = GetInt32Value(dataReader, "AgencyId").Value;

                    obj.AgencyName = GetStringValue(dataReader, "AgencyName");
                    obj.AgencyCode = GetStringValue(dataReader, "AgencyCode");
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.StatusName = GetStringValue(dataReader, "StatusName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }

        public async Task<SimpleResponse> GetBankList()
        {
            SimpleResponse response = new SimpleResponse();
            List<BankListResponse> objMaster = new List<BankListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[MDM].BankList");

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    BankListResponse obj = new BankListResponse();

                    obj.BankID = GetInt32Value(dataReader, "BankID").Value;

                    obj.BankName = GetStringValue(dataReader, "BankName");
                   
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.StatusName = GetStringValue(dataReader, "StatusName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }

        public async Task<SimpleResponse> GetAllstate()
        {
            SimpleResponse response = new SimpleResponse();
            List<StateListResponse> objMaster = new List<StateListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[MDM].usp_GetState");

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    StateListResponse obj = new StateListResponse();
                    obj.StateID = GetInt32Value(dataReader, "StateID").Value;
                    obj.StateCode = GetStringValue(dataReader, "StateCode");
                    obj.StateName = GetStringValue(dataReader, "StateName");
                   
                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }
        public async Task<SimpleResponse> GetallLedegrType()
        {
            SimpleResponse response = new SimpleResponse();
            List<LedgerTypeListResponse> objMaster = new List<LedgerTypeListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[MDM].ListLedgerTypeMaster");

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    LedgerTypeListResponse obj = new LedgerTypeListResponse();
                    obj.LedgerTypeId = GetInt32Value(dataReader, "LedgerTypeId").Value;
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.StatusName = GetStringValue(dataReader, "StatusName");
                    obj.LedgerTypeName = GetStringValue(dataReader, "LedgerTypeName");
                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }

        public async Task<SimpleResponse> GetallPlanMaster()
        {
            SimpleResponse response = new SimpleResponse();
            List<PlanMasterListResponse> objMaster = new List<PlanMasterListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[MDM].ListPlanMaster");

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    PlanMasterListResponse obj = new PlanMasterListResponse();
                    obj.PlanID = GetInt32Value(dataReader, "PlanID").Value;
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.StatusName = GetStringValue(dataReader, "StatusName");
                    obj.PlanName = GetStringValue(dataReader, "PlanName");
                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }
        public async Task<SimpleResponse> GetAllDistrict(int? StateId)
        {
            SimpleResponse response = new SimpleResponse();
            List<DistrictListResponse> objMaster = new List<DistrictListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[MDM].usp_GetDistrict");
            _database.AddInParameter(dbCommand, "@StateID", StateId);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    DistrictListResponse obj = new DistrictListResponse();
                    obj.StateID = GetInt32Value(dataReader, "StateID").Value;
                    obj.DistrictID = GetInt32Value(dataReader, "DistrictID").Value;
                    obj.DistrictCode = GetStringValue(dataReader, "DistrictCode");
                    obj.DistrictName = GetStringValue(dataReader, "DistrictName");
                    obj.StateName = GetStringValue(dataReader, "StateName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }

        public async Task<SimpleResponse> GetAllKycTypeMasterList(int? CompanyTypeId,int? UserTypeID)
        {
            SimpleResponse response = new SimpleResponse();
            List<KycTypeMasterListResponse> objMaster = new List<KycTypeMasterListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[MDM].KycTypeMasterList");
            _database.AddInParameter(dbCommand, "@CompanyTypeId", CompanyTypeId);
            _database.AddInParameter(dbCommand, "@UserTypeID", UserTypeID);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    KycTypeMasterListResponse obj = new KycTypeMasterListResponse();
                    obj.KycTypeID = GetInt32Value(dataReader, "KycTypeID").Value;
                    obj.CompnayTypeId = GetInt32Value(dataReader, "CompnayTypeId").Value;
                    obj.UserTypeId = GetInt32Value(dataReader, "UserTypeId").Value;
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.StatusName = GetStringValue(dataReader, "StatusName");
                    obj.UserTypeName = GetStringValue(dataReader, "UserTypeName");
                    obj.KycTypeName = GetStringValue(dataReader, "KycTypeName");
                    obj.CompanyTypeName = GetStringValue(dataReader, "CompanyTypeName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }

        public async Task<SimpleResponse> GetUserTypeList()
        {
            SimpleResponse response = new SimpleResponse();
            List<UserTypeListResponse> objMaster = new List<UserTypeListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[MDM].UserTypeList");

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    UserTypeListResponse obj = new UserTypeListResponse();

                    obj.UserTypeId = GetInt32Value(dataReader, "UserTypeId").Value;
                    obj.UserTypeName = GetStringValue(dataReader, "UserTypeName");
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.StatusName = GetStringValue(dataReader, "StatusName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }

        public async Task<SimpleResponse> GetDataByPincode(string Pincode)
        {
            SimpleResponse response = new SimpleResponse();
            List<PincodeDataResponse> objMaster = new List<PincodeDataResponse>();

            var dbCommand = _database.GetStoredProcCommand("[MDM].GetDataByPincode");
            _database.AddInParameter(dbCommand, "@Pincode", Pincode);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    PincodeDataResponse obj = new PincodeDataResponse();
                    obj.StateID = GetInt32Value(dataReader, "StateID").Value;
                    obj.DistrictID = GetInt32Value(dataReader, "DistrictID").Value;
                    obj.PincodeDataId = GetInt32Value(dataReader, "PincodeDataId").Value;
                    obj.AreaName = GetStringValue(dataReader, "AreaName");
                    obj.DistrictName = GetStringValue(dataReader, "DistrictName");
                    obj.StateName = GetStringValue(dataReader, "StateName");
                    obj.Pincode = GetStringValue(dataReader, "Pincode");
                    obj.SubDistrictName = GetStringValue(dataReader, "SubDistrictName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }

        public async Task<SimpleResponse> GetAllServiceType(int? AgencyId)
        {
            SimpleResponse response = new SimpleResponse();
            List<serviceTypeListResponse> objMaster = new List<serviceTypeListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[MDM].usp_GetDistrict");
            _database.AddInParameter(dbCommand, "@AgencyId", AgencyId);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    serviceTypeListResponse obj = new serviceTypeListResponse();
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.AgencyId = GetInt32Value(dataReader, "AgencyId").Value;
                    obj.ServiceTypeId = GetInt32Value(dataReader, "ServiceTypeId").Value;
                    obj.ServiceTypeName = GetStringValue(dataReader, "ServiceTypeName");
                    obj.AgencyName = GetStringValue(dataReader, "AgencyName");
                    obj.ServiceTypeName = GetStringValue(dataReader, "ServiceTypeName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }

        public async Task<SimpleResponse> GetAllPaymentChanel()
        {
            SimpleResponse response = new SimpleResponse();
            List<ListPaymentChanelResponse> objMaster = new List<ListPaymentChanelResponse>();

            var dbCommand = _database.GetStoredProcCommand("[MDM].usp_GetDistrict");
            
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    ListPaymentChanelResponse obj = new ListPaymentChanelResponse();
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.PaymentChanelID = GetInt32Value(dataReader, "PaymentChanelID").Value;
                    obj.PaymentChanelName = GetStringValue(dataReader, "PaymentChanelName");
                  
                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }

        public async Task<SimpleResponse> GetAllPaymentModes(int? PaymentChanelId)
        {
            SimpleResponse response = new SimpleResponse();
            List<ListPaymentModeResponse> objMaster = new List<ListPaymentModeResponse>();

            var dbCommand = _database.GetStoredProcCommand("[MDM].ListPaymentModeMaster");
            _database.AddInParameter(dbCommand, "@PaymentChanelId", PaymentChanelId);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    ListPaymentModeResponse obj = new ListPaymentModeResponse();
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.PaymentChanelID = GetInt32Value(dataReader, "PaymentChanelID").Value;
                    obj.PaymentModeID = GetInt32Value(dataReader, "PaymentModeID").Value;
                    obj.PaymentChanelName = GetStringValue(dataReader, "PaymentChanelName");
                    obj.PaymentModeName = GetStringValue(dataReader, "PaymentModeName");
                    obj.StatusName = GetStringValue(dataReader, "StatusName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }
    }
}
