using SANYUKT.Database;
using SANYUKT.Datamodel.Entities.RblPayout;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.Masters;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SANYUKT.Repository
{
    public class ConfigRepository:BaseRepository
    {
        public readonly ISANYUKTDatabase _database = null;
        public ConfigRepository() {
            _database=new SANYUKTDatabase();
        }
        public async Task<SimpleResponse> GetAllCalculationTypeMaster()
        {
            SimpleResponse response = new SimpleResponse();
            List<CalculationMasterResponse> objMaster = new List<CalculationMasterResponse>();

            var dbCommand = _database.GetStoredProcCommand("[CONFG].ListCalculationTypeMaster");
            
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    CalculationMasterResponse obj = new CalculationMasterResponse();

                    obj.CalculationTypeId = GetInt32Value(dataReader, "CalculationTypeId").Value;
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.CalculationTypeName = GetStringValue(dataReader, "CalculationTypeName");
                    obj.StatusName = GetStringValue(dataReader, "StatusName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }
        public async Task<SimpleResponse> GetAllChargeDeductionType()
        {
            SimpleResponse response = new SimpleResponse();
            List<ChargedeductionTypeListResponse> objMaster = new List<ChargedeductionTypeListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[CONFG].ListChargeDeductionType");

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    ChargedeductionTypeListResponse obj = new ChargedeductionTypeListResponse();

                    obj.ChargeDeductionId = GetInt32Value(dataReader, "ChargeDeductionId").Value;
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.ChargeDeductionType = GetStringValue(dataReader, "ChargeDeductionType");
                    obj.StatusName = GetStringValue(dataReader, "StatusName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }
        public async Task<SimpleResponse> GetallPlanList(int? PlanID)
        {
            SimpleResponse response = new SimpleResponse();
            List<PlanMasterListDataResponse> objMaster = new List<PlanMasterListDataResponse>();

            var dbCommand = _database.GetStoredProcCommand("[CONFG].ListPlanMaster");
            _database.AddInParameter(dbCommand, "@PlanID", PlanID);
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    PlanMasterListDataResponse obj = new PlanMasterListDataResponse();

                    obj.PlanId = GetInt32Value(dataReader, "PlanId").Value;
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.PlanCode = GetStringValue(dataReader, "PlanCode");
                    obj.PlanName = GetStringValue(dataReader, "PlanName");
                    obj.StatusName = GetStringValue(dataReader, "StatusName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }
        public async Task<SimpleResponse> GetallSlabType()
        {
            SimpleResponse response = new SimpleResponse();
            List<SlabTypeListResponse> objMaster = new List<SlabTypeListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[CONFG].ListSlabTypeMAster");
            
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    SlabTypeListResponse obj = new SlabTypeListResponse();

                    obj.SlabTypId = GetInt32Value(dataReader, "SlabTypId").Value;
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.SlabTypeName = GetStringValue(dataReader, "SlabTypeName");
                  
                    obj.StatusName = GetStringValue(dataReader, "StatusName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }
        public async Task<SimpleResponse> GetallCommissionDistribution(CommissionDistributionRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            List<CommissionDistributionResponse> objMaster = new List<CommissionDistributionResponse>();

            var dbCommand = _database.GetStoredProcCommand("[CONFG].Usp_listCommissionDistribution");
            _database.AddInParameter(dbCommand, "@AgencyId", request.AgencyId);
            _database.AddInParameter(dbCommand, "@ServiceId", request.ServiceId);
            _database.AddInParameter(dbCommand, "@PlanId", request.PlanId);
            _database.AddInParameter(dbCommand, "@CalculationTypeId", request.CalculationTypeId);
            _database.AddInParameter(dbCommand, "@amount", request.amount);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    CommissionDistributionResponse obj = new CommissionDistributionResponse();
                    obj.MarginConfigrationID = GetInt32Value(dataReader, "MarginConfigrationID").Value;
                    obj.PlanId = GetInt32Value(dataReader, "PlanId").Value;
                    obj.AgencyId = GetInt32Value(dataReader, "AgencyId").Value;
                    obj.ServiceId = GetInt32Value(dataReader, "ServiceId").Value;
                    obj.CalculationTypeId = GetInt32Value(dataReader, "CalculationTypeId").Value;
                    obj.FromAmount = GetDecimalValue(dataReader, "FromAmount").Value;
                    obj.Toamount = GetDecimalValue(dataReader, "Toamount").Value;
                    obj.CalculationValue = GetDecimalValue(dataReader, "CalculationValue").Value;
                    obj.AgencyName = GetStringValue(dataReader, "AgencyName");
                    obj.ServiceName = GetStringValue(dataReader, "ServiceName");
                    obj.CalculationTypeName = GetStringValue(dataReader, "CalculationTypeName");
                    obj.PlanName = GetStringValue(dataReader, "PlanName");
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.StatusName = GetStringValue(dataReader, "StatusName");
                  
                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }

        public async Task<SimpleResponse> GetAllTopupCharge(TopupChargeRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            List<TopupChargeResponse> objMaster = new List<TopupChargeResponse>();

            var dbCommand = _database.GetStoredProcCommand("[CONFG].ListTopupCharge");
            _database.AddInParameter(dbCommand, "@TopupChargeId", request.TopupChargeId);
            _database.AddInParameter(dbCommand, "@SlabTypeId", request.SlabTypeId);
            _database.AddInParameter(dbCommand, "@CalculationTypeId", request.CalculationTypeId);
            _database.AddInParameter(dbCommand, "@Amount", request.Amount);
           

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    TopupChargeResponse obj = new TopupChargeResponse();
                    obj.TopupChargeId = GetInt32Value(dataReader, "TopupChargeId").Value;
                    obj.SlabTypeId = GetInt32Value(dataReader, "SlabTypeId").Value;
                    obj.CalculationTypeId = GetInt32Value(dataReader, "CalculationTypeId").Value;
                    obj.FromAmount = GetDecimalValue(dataReader, "FromAmount").Value;
                    obj.Toamount = GetDecimalValue(dataReader, "Toamount").Value;
                    obj.CalculationValue = GetDecimalValue(dataReader, "CalculationValue").Value;
                    obj.CalculationTypeName = GetStringValue(dataReader, "CalculationTypeName");
                    obj.SlabTypeName = GetStringValue(dataReader, "SlabTypeName");
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.StatusName = GetStringValue(dataReader, "StatusName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }

        public async Task<SimpleResponse> GetallTransactionSlab(TransactionslabRequest request)
        {
            SimpleResponse response = new SimpleResponse();
            List<TransactionslabResponse> objMaster = new List<TransactionslabResponse>();

            var dbCommand = _database.GetStoredProcCommand("[CONFG].ListTransactionslab");
            _database.AddInParameter(dbCommand, "@SlabId", request.SlabId);
            _database.AddInParameter(dbCommand, "@SlabType", request.SlabType);
            _database.AddInParameter(dbCommand, "@CalculationType", request.CalculationType);
            _database.AddInParameter(dbCommand, "@AgencyID", request.AgencyID);
            _database.AddInParameter(dbCommand, "@ServiceID", request.ServiceID);
            _database.AddInParameter(dbCommand, "@Amount", request.Amount);


            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    TransactionslabResponse obj = new TransactionslabResponse();
                    obj.SlabId = GetInt32Value(dataReader, "SlabId").Value;
                    obj.SlabType = GetInt32Value(dataReader, "SlabType").Value;
                    obj.CalculationType = GetInt32Value(dataReader, "CalculationType").Value;
                    obj.AgencyID = GetInt32Value(dataReader, "AgencyID").Value;
                    obj.ServiceID = GetInt32Value(dataReader, "ServiceID").Value;
                    obj.FromAmount = GetDecimalValue(dataReader, "FromAmount").Value;
                    obj.Toamount = GetDecimalValue(dataReader, "Toamount").Value;
                    obj.CalculationValue = GetDecimalValue(dataReader, "CalculationValue").Value;
                    obj.CalculationTypeName = GetStringValue(dataReader, "CalculationTypeName");
                    obj.SlabTypeName = GetStringValue(dataReader, "SlabTypeName");
                    obj.ServiceName = GetStringValue(dataReader, "ServiceName");
                    obj.AgencyName = GetStringValue(dataReader, "AgencyName");
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.StatusName = GetStringValue(dataReader, "StatusName");

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }

        public async Task<SimpleResponse> GetAllPaymentAccounts(int? Bankid)
        {
            SimpleResponse response = new SimpleResponse();
            List<PaymentAccountsListResponse> objMaster = new List<PaymentAccountsListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[CONFG].ListPaymentAccountMaster");
            _database.AddInParameter(dbCommand, "@BankID", Bankid);
          
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    PaymentAccountsListResponse obj = new PaymentAccountsListResponse();
                    obj.PaymentAccountID = GetInt32Value(dataReader, "PaymentAccountID").Value;
                    obj.AccountName = GetStringValue(dataReader, "AccountName");
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.BankID = GetInt32Value(dataReader, "BankID").Value;
                    obj.StatusName = GetStringValue(dataReader, "StatusName");
                    obj.CreatedBy = GetStringValue(dataReader, "CreatedBy");
                    obj.UpdatedBy = GetStringValue(dataReader, "UpdatedBy");
                    obj.AccountNo = GetStringValue(dataReader, "AccountNo");
                    obj.BankName = GetStringValue(dataReader, "BankName");
                    obj.BranchName = GetStringValue(dataReader, "BranchName");
                    obj.Branchcode = GetStringValue(dataReader, "Branchcode");
                    obj.Ifsccode = GetStringValue(dataReader, "Ifsccode");
                    obj.BranchAddress = GetStringValue(dataReader, "BranchAddress");
                    obj.Micrcode = GetStringValue(dataReader, "Micrcode");
                    obj.CreatedOn = GetDateValue(dataReader, "CreatedOn").Value;
                   

                    objMaster.Add(obj);
                }
                response.Result = objMaster;
                return response;
            }

        }

        public async Task<long> AddPaymentAccounts(AddPaymentAccountMasterRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[CONFG].AddPaymentAccountMaster");
            _database.AddInParameter(dbCommand, "@BankID", request.BankID);
            _database.AddInParameter(dbCommand, "@AccountName", request.AccountName);
            _database.AddInParameter(dbCommand, "@AccountNo", request.AccountNo);
            _database.AddInParameter(dbCommand, "@Ifsccode", request.Ifsccode);
            _database.AddInParameter(dbCommand, "@BranchName", request.BranchName);
            _database.AddInParameter(dbCommand, "@CreatedBy", serviceUser.UserMasterID);
            _database.AddInParameter(dbCommand, "@Branchcode", request.Branchcode);
            _database.AddInParameter(dbCommand, "@Micrcode", request.Micrcode);
            _database.AddInParameter(dbCommand, "@BranchAddress", request.BranchAddress);
            _database.AddOutParameter(dbCommand, "@Out_ID", 100);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }

        public async Task<long> changesPaymentAccountsStatus(ChangePaymentAccStatusRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[CONFG].ChangePaymentAccountsStatus");
            _database.AddInParameter(dbCommand, "@PaymentAccountID", request.PaymentAccountID);
            _database.AddInParameter(dbCommand, "@Status", request.Status);
            _database.AddOutParameter(dbCommand, "@Out_ID", 100);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<long> CreateNewApplication(CreateapplicationRequest request,string AppToken, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[AAC].CreateApp");
            _database.AddInParameter(dbCommand, "@OrganizationID", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@ApplicationToken", AppToken);
            _database.AddInParameter(dbCommand, "@ApplicationName", request.ApplicationName);
            _database.AddInParameter(dbCommand, "@ApplicationDescription", request.ApplicationDescription);
            _database.AddInParameter(dbCommand, "@Createdby", serviceUser.UserMasterID);
            _database.AddOutParameter(dbCommand, "@Out_ID", 100);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
    }
}
