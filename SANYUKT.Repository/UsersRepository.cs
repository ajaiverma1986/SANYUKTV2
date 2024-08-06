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
            _database.AddInParameter(dbCommand, "@UserMasterId", serviceUser.UserID);
        
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                if (dataReader.Read())
                {
                   
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
        public async Task<long> UpdateUserLogo(UploadLogoRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].usp_UserOrgLogo");
            _database.AddInParameter(dbCommand, "@Userid", request.UserId);
            _database.AddInParameter(dbCommand, "@Logourl", request.Logourl);
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
        public async Task<List<OriginatorListAccountResponse>> GetallOriginatorsAccount(ISANYUKTServiceUser serviceUser)
        {
           List< OriginatorListAccountResponse> response = new List<OriginatorListAccountResponse>();

            var dbCommand = _database.GetStoredProcCommand("[USR].GetallOriginatorsAccounts");

            _database.AddInParameter(dbCommand, "@UserId", serviceUser.UserID);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                if (dataReader.Read())
                {
                    OriginatorListAccountResponse row = new OriginatorListAccountResponse();

                    row.Status = GetInt32Value(dataReader, "Status").Value;
                    row.BankId = GetInt32Value(dataReader, "BankId").Value;
                    row.UserId = GetInt32Value(dataReader, "UserId").Value;
                    row.OriginatorAccountID = GetInt32Value(dataReader, "OriginatorAccountID").Value;
                    row.CreatedOn = GetDateValue(dataReader, "BenBranchCode");
                    row.UpdatedOn = GetDateValue(dataReader, "UpdatedOn");
                    row.StatusName = GetStringValue(dataReader, "StatusName");
                    row.AccountName = GetStringValue(dataReader, "AccountName");
                    row.BankName = GetStringValue(dataReader, "BankName");
                    row.AccountNo = GetStringValue(dataReader, "AccountNo");
                    row.BranchAddress = GetStringValue(dataReader, "BranchAddress");
                    row.CreatedBy = GetStringValue(dataReader, "CreatedBy");
                    row.UpdatedBy = GetStringValue(dataReader, "UpdatedBy");
                    row.Fullname = GetStringValue(dataReader, "Fullname");
                    row.Usercode  = GetStringValue(dataReader, "Usercode");
                    row.Ifsccode = GetStringValue(dataReader, "Ifsccode");
                    response.Add(row);
                }
            }
            return response;
        }

        public async Task<long> AddUserAddress(CreateUserDetailAddressRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].Add_UserDetailAddress");
            _database.AddInParameter(dbCommand, "@UserID", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@AddressTypeId", request.AddressTypeId);
            _database.AddInParameter(dbCommand, "@Pincode", request.Pincode);
            _database.AddInParameter(dbCommand, "@Address1", request.Address1);
            _database.AddInParameter(dbCommand, "@@Address2", request.Address2);
            _database.AddInParameter(dbCommand, "@@Address3", request.Address3);
            _database.AddInParameter(dbCommand, "@CreatedBy", serviceUser.UserMasterID);
            _database.AddInParameter(dbCommand, "@PincodeDataId", request.PincodeDataId);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }

        public async Task<List<UserAddressListResponse>> GetAllUserAddress(ISANYUKTServiceUser serviceUser)
        {
            List<UserAddressListResponse> response = new List<UserAddressListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[USR].GetallUserAddresses");

            _database.AddInParameter(dbCommand, "@UserId", serviceUser.UserID);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                if (dataReader.Read())
                {
                    UserAddressListResponse row = new UserAddressListResponse();

                    row.Status = GetInt32Value(dataReader, "Status").Value;
                    row.AddressTypeId = GetInt32Value(dataReader, "AddressTypeId").Value;
                    row.UserID = GetInt32Value(dataReader, "UserID").Value;
                    row.UserAddressID = GetInt32Value(dataReader, "UserAddressID").Value;
                    row.CreatedOn = GetDateValue(dataReader, "BenBranchCode");
                    row.UpdatedOn = GetDateValue(dataReader, "UpdatedOn");
                    row.StatusName = GetStringValue(dataReader, "StatusName");
                    row.Address1 = GetStringValue(dataReader, "Address1");
                    row.Address2 = GetStringValue(dataReader, "Address2");
                    row.Address3 = GetStringValue(dataReader, "Address3");
                    row.AddressTypeName = GetStringValue(dataReader, "AddressTypeName");
                    row.CreatedBy = GetStringValue(dataReader, "CreatedBy");
                    row.UpdatedBy = GetStringValue(dataReader, "UpdatedBy");
                    row.AreaName = GetStringValue(dataReader, "AreaName");
                    row.PincodeDataId = GetInt32Value(dataReader, "PincodeDataId").Value;
                    row.DistrictName = GetStringValue(dataReader, "DistrictName");
                    row.StateName = GetStringValue(dataReader, "StateName");
                    row.SubDistrictName = GetStringValue(dataReader, "SubDistrictName");
                    row.Pincode = GetStringValue(dataReader, "Pincode");
                    response.Add(row);
                }
            }
            return response;
        }

        public async Task<long> AddUserDeatilKYC(CreateUserDetailKyc request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].Add_UserDetailKyc");
            _database.AddInParameter(dbCommand, "@UserID", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@KycID", request.KycID);
            _database.AddInParameter(dbCommand, "@DocumentNo", request.DocumentNo);
            _database.AddInParameter(dbCommand, "@FileUrl", request.FileUrl);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<List<UserKYYCResponse>> GetAllUserKyc(ISANYUKTServiceUser serviceUser)
        {
            List<UserKYYCResponse> response = new List<UserKYYCResponse>();

            var dbCommand = _database.GetStoredProcCommand("[USR].GetallUserDeatilsKyc");

            _database.AddInParameter(dbCommand, "@UserId", serviceUser.UserID);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                if (dataReader.Read())
                {
                    UserKYYCResponse row = new UserKYYCResponse();

                    row.Status = GetInt32Value(dataReader, "Status").Value;
                    row.KycID = GetInt32Value(dataReader, "KycID").Value;
                    row.UserId = GetInt32Value(dataReader, "UserId").Value;
                    row.UserKYCID = GetInt32Value(dataReader, "UserKYCID").Value;
                    row.CreatedOn = GetDateValue(dataReader, "CreatedOn");
                    row.UpdatedOn = GetDateValue(dataReader, "UpdatedOn");
                    row.StatusName = GetStringValue(dataReader, "StatusName");
                    row.DocumentNo = GetStringValue(dataReader, "DocumentNo");
                    row.FileUrl = GetStringValue(dataReader, "FileUrl");
                    row.CreatedBy = GetStringValue(dataReader, "CreatedBy");
                    row.UpdatedBy = GetStringValue(dataReader, "UpdatedBy");
                    
                    response.Add(row);
                }
            }
            return response;
        }
    }
}
