using SANYUKT.Database;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.Entities.Application;
using SANYUKT.Datamodel.Entities.RblPayout;
using SANYUKT.Datamodel.Entities.Users;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SANYUKT.Repository
{
    public class UsersRepository:BaseRepository
    {
        public readonly ISANYUKTDatabase _database = null;
        public UsersRepository() { 
            _database = new SANYUKTDatabase();
        }
        public async Task<UsersDetailsResponse> CheckAvailbleLimit(ISANYUKTServiceUser serviceUser)
        {
            UsersDetailsResponse response = new UsersDetailsResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].CheckAvailableBalance");
            _database.AddInParameter(dbCommand, "@UserMasterId", serviceUser.UserMasterID);
        
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                if (dataReader.Read())
                {
                    response.UserMasterId = GetInt64Value(dataReader, "UserMasterId").Value;
                    response.UserId = GetInt64Value(dataReader, "UserId").Value;
                    response.Usercode = GetStringValue(dataReader, "Usercode");
                    response.ThresoldLimit = GetDecimalValue(dataReader, "ThresoldLimit");
                    response.AvailableLimit = GetDecimalValue(dataReader, "AvailableLimit");
                    response.UserTypeId = GetInt32Value(dataReader, "UserTypeId").Value;
                    
                }
            }
            return response;
        }
        public async Task<long> AddNewBenficiary(AddBenficiaryRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].AddBenficiary");
            _database.AddInParameter(dbCommand, "@partnerid", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@benficiaryname", request.BenbankName);
            _database.AddInParameter(dbCommand, "@benmobile", request.BenMobile);
            _database.AddInParameter(dbCommand, "@emailid", request.EmailId);
            _database.AddInParameter(dbCommand, "@benaddress", request.BenAddress);
            _database.AddInParameter(dbCommand, "@benbankname", request.BenbankName);
            _database.AddInParameter(dbCommand, "@benbankcode", request.BenBankcode);
            _database.AddInParameter(dbCommand, "@benbranchcode", request.BenBranchCode);
            _database.AddInParameter(dbCommand, "@benaccountno", request.BenAccountNo);
            _database.AddInParameter(dbCommand, "@benifsccode", request.BenIfsccode);
            _database.AddInParameter(dbCommand, "@CreatedBy", serviceUser.UserMasterID);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<List<BenficiaryResponse>> GetAllBenficiary(ListBenficaryRequest request,ISANYUKTServiceUser serviceUser)
        {
            List<BenficiaryResponse> response=new List<BenficiaryResponse> ();
          
            var dbCommand = _database.GetStoredProcCommand("[USR].GetAllBenficiary");
            _database.AddInParameter(dbCommand, "@PartnerId", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@MobileNo", request.MobileNo);
            _database.AddInParameter(dbCommand, "@BenFiciaryId", request.BenFiciaryId);
            _database.AddInParameter(dbCommand, "@AccountNo", request.AccountNo);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                if (dataReader.Read())
                {
                    BenficiaryResponse objp = new BenficiaryResponse();
                    objp.PartnerId = GetInt64Value(dataReader, "PartnerId").Value;
                    objp.BenBranchCode = GetStringValue(dataReader, "BenBranchCode");
                    objp.BenAddress = GetStringValue(dataReader, "BenAddress");
                    objp.BenBankcode = GetStringValue(dataReader, "BenBankcode");
                    objp.BenAccountNo = GetStringValue(dataReader, "BenAccountNo");
                    objp.BenbankName = GetStringValue(dataReader, "BenbankName");
                    objp.BenBankcode = GetStringValue(dataReader, "BenBankcode");
                    objp.BenIfsccode = GetStringValue(dataReader, "BenIfsccode");
                    objp.BenMobile = GetStringValue(dataReader, "BenMobile");
                    objp.EmailId = GetStringValue(dataReader, "EmailId");
                    objp.BenficiaryName = GetStringValue(dataReader, "BenficiaryName");
                    response.Add(objp);
                }
            }
            return response;
        }
        public async Task<BenficiaryResponse> GetBenficiaryDetailsByID(long BenFiciaryId)
        {
            BenficiaryResponse response = new BenficiaryResponse();

            var dbCommand = _database.GetStoredProcCommand("[USR].GetBenficiaryById");
           
            _database.AddInParameter(dbCommand, "@BenFiciaryId", BenFiciaryId);
            
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                if (dataReader.Read())
                {

                    response.PartnerId = GetInt64Value(dataReader, "PartnerId").Value;
                    response.BenBranchCode = GetStringValue(dataReader, "BenBranchCode");
                    response.BenAddress = GetStringValue(dataReader, "BenAddress");
                    response.BenBankcode = GetStringValue(dataReader, "BenBankcode");
                    response.BenAccountNo = GetStringValue(dataReader, "BenAccountNo");
                    response.BenbankName = GetStringValue(dataReader, "BenbankName");
                    response.BenBankcode = GetStringValue(dataReader, "BenBankcode");
                    response.BenIfsccode = GetStringValue(dataReader, "BenIfsccode");
                    response.BenMobile = GetStringValue(dataReader, "BenMobile");
                    response.EmailId = GetStringValue(dataReader, "EmailId");
                    response.BenficiaryName = GetStringValue(dataReader, "BenficiaryName");
                   
                }
            }
            return response;
        }
        public async Task<long> ChangeBenficairyStatus(BenficaryChangeStatusRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].ChangeBenficiaryStatus");
            _database.AddInParameter(dbCommand, "@PartnerId",serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@BenficiaryId", request.BenFiciaryId);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<long> CreateNewUserRequest(CreateUserRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].usp_UserOnboarding");
            _database.AddInParameter(dbCommand, "@UserTypeId", request.UserTypeId);
            _database.AddInParameter(dbCommand, "@MobileNo", request.MobileNo);
            _database.AddInParameter(dbCommand, "@EmailId", request.EmailId);
            _database.AddInParameter(dbCommand, "@FirstName", request.FirstName);
            _database.AddInParameter(dbCommand, "@MiddleName", request.MiddleName);
            _database.AddInParameter(dbCommand, "@LastName", request.LastName);
            _database.AddInParameter(dbCommand, "@CreatedBy", serviceUser.UserMasterID);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<long> AddOriginatorAccounts(CreateOriginatorAccountRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].CreateOriginatorAccountMaster");
            _database.AddInParameter(dbCommand, "@UserId", request.UserId);
            _database.AddInParameter(dbCommand, "@BankId", request.BankId);
            _database.AddInParameter(dbCommand, "@AccountName", request.AccountName);
            _database.AddInParameter(dbCommand, "@AccountNo", request.AccountNo);
            _database.AddInParameter(dbCommand, "@Ifsccode", request.Ifsccode);
            _database.AddInParameter(dbCommand, "@BranchAddress", request.BranchAddress);
            _database.AddInParameter(dbCommand, "@CreatedBy", serviceUser.UserMasterID);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
    }
}
