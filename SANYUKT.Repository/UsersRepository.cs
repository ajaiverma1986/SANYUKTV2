using SANYUKT.Database;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.Entities.Application;
using SANYUKT.Datamodel.Entities.RblPayout;
using SANYUKT.Datamodel.Entities.Transactions;
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
    public class UsersRepository : BaseRepository
    {
        public readonly ISANYUKTDatabase _database = null;
        public UsersRepository()
        {
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
        public async Task<UserConfigResponse> GetUserConfig(ISANYUKTServiceUser serviceUser)
        {
            UserConfigResponse response = new UserConfigResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].GetAllUserConfigration");
            _database.AddInParameter(dbCommand, "@UserId", serviceUser.UserID);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                if (dataReader.Read())
                {

                    response.UserId = GetInt64Value(dataReader, "UserId").Value;
                    response.ChargeDeductionType = GetStringValue(dataReader, "ChargeDeductionType");
                    response.ConfigurationId = GetInt64Value(dataReader, "ConfigurationId").Value;
                    response.ChargeTypeOn = GetInt32Value(dataReader, "ChargeTypeOn").Value;
                    response.PlanId = GetInt32Value(dataReader, "PlanId").Value;
                    response.MaxTxn = GetDecimalValue(dataReader, "MaxTxn").Value;
                    response.MinTxn = GetDecimalValue(dataReader, "MinTxn").Value;

                }
            }
            return response;
        }
        public async Task<PartnerLimitResponse> CheckPartnerAvailbleLimit(ISANYUKTServiceUser serviceUser)
        {
            PartnerLimitResponse response = new PartnerLimitResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].PartnerCheckAvailableBalance");
            _database.AddInParameter(dbCommand, "@UserMasterId", serviceUser.UserID);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                if (dataReader.Read())
                {

                    response.PartnerID = GetInt64Value(dataReader, "PartnerID").Value;
                    response.PartnerCode = GetStringValue(dataReader, "PartnerCode");
                    response.Balance = GetDecimalValue(dataReader, "AvailableLimit");


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
        public async Task<List<BenficiaryResponse>> GetAllBenficiary(ListBenficaryRequest request, ISANYUKTServiceUser serviceUser)
        {
            List<BenficiaryResponse> response = new List<BenficiaryResponse>();

            var dbCommand = _database.GetStoredProcCommand("[USR].GetAllBenficiary");
            _database.AddInParameter(dbCommand, "@PartnerId", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@MobileNo", request.MobileNo);
            _database.AddInParameter(dbCommand, "@BenFiciaryId", request.BenFiciaryId);
            _database.AddInParameter(dbCommand, "@AccountNo", request.AccountNo);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    BenficiaryResponse objp = new BenficiaryResponse();
                    objp.PartnerId = GetInt64Value(dataReader, "PartnerId").Value;
                    objp.BenFiciaryId = GetInt64Value(dataReader, "BenFiciaryId").Value;
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
            _database.AddInParameter(dbCommand, "@PartnerId", serviceUser.UserID);
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
            _database.AddInParameter(dbCommand, "@UserId", serviceUser.UserID);
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
            List<OriginatorListAccountResponse> response = new List<OriginatorListAccountResponse>();

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
                    row.UpdatedOn = GetDateValue(dataReader, "UpdatedOn");
                    row.StatusName = GetStringValue(dataReader, "StatusName");
                    row.AccountName = GetStringValue(dataReader, "AccountName");
                    row.BankName = GetStringValue(dataReader, "BankName");
                    row.AccountNo = GetStringValue(dataReader, "AccountNo");
                    row.BranchAddress = GetStringValue(dataReader, "BranchAddress");
                    row.CreatedBy = GetStringValue(dataReader, "CreatedBy");
                    row.UpdatedBy = GetStringValue(dataReader, "UpdatedBy");
                    row.Fullname = GetStringValue(dataReader, "Fullname");
                    row.Usercode = GetStringValue(dataReader, "Usercode");
                    row.Filename = GetStringValue(dataReader, "Filename");
                    row.Ifsccode = GetStringValue(dataReader, "Ifsccode");
                    response.Add(row);
                }
            }
            return response;
        }
        public async Task<List<OriginatorListAccountResponse>> GetallOriginatorsAccountByID(long AccountID, ISANYUKTServiceUser serviceUser)
        {
            List<OriginatorListAccountResponse> response = new List<OriginatorListAccountResponse>();

            var dbCommand = _database.GetStoredProcCommand("[USR].GetallOriginatorsAccountsByID");

            _database.AddInParameter(dbCommand, "@RequestID", AccountID);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                if (dataReader.Read())
                {
                    OriginatorListAccountResponse row = new OriginatorListAccountResponse();

                    row.Status = GetInt32Value(dataReader, "Status").Value;
                    row.BankId = GetInt32Value(dataReader, "BankId").Value;
                    row.UserId = GetInt32Value(dataReader, "UserId").Value;
                    row.OriginatorAccountID = GetInt32Value(dataReader, "OriginatorAccountID").Value;
                    row.UpdatedOn = GetDateValue(dataReader, "UpdatedOn");
                    row.StatusName = GetStringValue(dataReader, "StatusName");
                    row.AccountName = GetStringValue(dataReader, "AccountName");
                    row.BankName = GetStringValue(dataReader, "BankName");
                    row.AccountNo = GetStringValue(dataReader, "AccountNo");
                    row.BranchAddress = GetStringValue(dataReader, "BranchAddress");
                    row.CreatedBy = GetStringValue(dataReader, "CreatedBy");
                    row.UpdatedBy = GetStringValue(dataReader, "UpdatedBy");
                    row.Fullname = GetStringValue(dataReader, "Fullname");
                    row.Usercode = GetStringValue(dataReader, "Usercode");
                    row.Filename = GetStringValue(dataReader, "Filename");
                    row.Ifsccode = GetStringValue(dataReader, "Ifsccode");
                    response.Add(row);
                }
            }
            return response;
        }
        public async Task<List<OriginatorListAccountResponse>> ListAllOriginatorsAccounts(OriginatorListAccountRequest request, ISANYUKTServiceUser serviceUser)
        {
            List<OriginatorListAccountResponse> response = new List<OriginatorListAccountResponse>();

            var dbCommand = _database.GetStoredProcCommand("[USR].ListallOriginatorsAccounts");

            _database.AddInParameter(dbCommand, "@UserId", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@Status", request.Status);
            _database.AddInParameter(dbCommand, "@PageNo", request.PageNo);
            _database.AddInParameter(dbCommand, "@PageSize", request.PageSize);
            _database.AddInParameter(dbCommand, "@OrderBy", request.OrderBy);
            _database.AddOutParameter(dbCommand, "@Out_TotalRec", 100);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    OriginatorListAccountResponse row = new OriginatorListAccountResponse();

                    row.Status = GetInt32Value(dataReader, "Status").Value;
                    row.BankId = GetInt32Value(dataReader, "BankId").Value;
                    row.UserId = GetInt32Value(dataReader, "UserId").Value;
                    row.OriginatorAccountID = GetInt32Value(dataReader, "OriginatorAccountID").Value;
                    row.UpdatedOn = GetDateValue(dataReader, "UpdatedOn");
                    row.StatusName = GetStringValue(dataReader, "StatusName");
                    row.AccountName = GetStringValue(dataReader, "AccountName");
                    row.BankName = GetStringValue(dataReader, "BankName");
                    row.AccountNo = GetStringValue(dataReader, "AccountNo");
                    row.BranchAddress = GetStringValue(dataReader, "BranchAddress");
                    row.CreatedBy = GetStringValue(dataReader, "CreatedBy");
                    row.UpdatedBy = GetStringValue(dataReader, "UpdatedBy");
                    row.Fullname = GetStringValue(dataReader, "Fullname");
                    row.Usercode = GetStringValue(dataReader, "Usercode");
                    row.Filename = GetStringValue(dataReader, "Filename");
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
            _database.AddInParameter(dbCommand, "@Address2", request.Address2);
            _database.AddInParameter(dbCommand, "@Address3", request.Address3);
            _database.AddInParameter(dbCommand, "@CreatedBy", serviceUser.UserMasterID);
            _database.AddInParameter(dbCommand, "@PincodeDataId", request.PincodeDataId);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<long> CreateOrgAPIPartner(CreateNewPartnerRequest request, string Password, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].CreateApiPartner");
            _database.AddInParameter(dbCommand, "@EmailId", request.EmailId);
            _database.AddInParameter(dbCommand, "@MobileNo", request.MobileNo);
            _database.AddInParameter(dbCommand, "@OrganisationName", request.OrganisationName);
            _database.AddInParameter(dbCommand, "@FirstName", request.FirstName);
            _database.AddInParameter(dbCommand, "@LastName", request.LastName);
            _database.AddInParameter(dbCommand, "@Password", Password);
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
        public async Task<List<ApplicationListResponse>> Getallapplication(ISANYUKTServiceUser serviceUser)
        {
            List<ApplicationListResponse> response = new List<ApplicationListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[AAC].ListAllApplication");
            _database.AddInParameter(dbCommand, "@OrganizationID", serviceUser.UserID);


            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    ApplicationListResponse objp = new ApplicationListResponse();
                    objp.ApplicationID = GetInt32Value(dataReader, "ApplicationID").Value;
                    objp.OrganizationID = GetInt64Value(dataReader, "OrganizationID").Value;
                    objp.ApplicationName = GetStringValue(dataReader, "ApplicationName");
                    objp.ApplicationDescription = GetStringValue(dataReader, "ApplicationDescription");
                    objp.ApplicationToken = GetStringValue(dataReader, "ApplicationToken");
                    objp.CreatedBy = GetStringValue(dataReader, "CreatedBy");
                    objp.EmailId = GetStringValue(dataReader, "EmailId");
                    objp.MobileNo = GetStringValue(dataReader, "MobileNo");
                    objp.OrganisationName = GetStringValue(dataReader, "OrganisationName");
                    objp.CreatedOn = GetDateValue(dataReader, "CreatedOn").Value;

                    response.Add(objp);
                }
            }
            return response;
        }
        public async Task<List<ApplicationListResponse>> GetallapplicationForAdmin(long UserID, ISANYUKTServiceUser serviceUser)
        {
            List<ApplicationListResponse> response = new List<ApplicationListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[AAC].ListAllApplication");
            _database.AddInParameter(dbCommand, "@OrganizationID", UserID);


            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    ApplicationListResponse objp = new ApplicationListResponse();
                    objp.ApplicationID = GetInt32Value(dataReader, "ApplicationID").Value;
                    objp.OrganizationID = GetInt64Value(dataReader, "OrganizationID").Value;
                    objp.ApplicationName = GetStringValue(dataReader, "ApplicationName");
                    objp.ApplicationDescription = GetStringValue(dataReader, "ApplicationDescription");
                    objp.ApplicationToken = GetStringValue(dataReader, "ApplicationToken");
                    objp.CreatedBy = GetStringValue(dataReader, "CreatedBy");
                    objp.EmailId = GetStringValue(dataReader, "EmailId");
                    objp.MobileNo = GetStringValue(dataReader, "MobileNo");
                    objp.OrganisationName = GetStringValue(dataReader, "OrganisationName");
                    objp.CreatedOn = GetDateValue(dataReader, "CreatedOn").Value;

                    response.Add(objp);
                }
            }
            return response;
        }
        public async Task<List<UserKYYCResponse>> GetAllUserKyc(ISANYUKTServiceUser serviceUser)
        {
            List<UserKYYCResponse> response = new List<UserKYYCResponse>();

            var dbCommand = _database.GetStoredProcCommand("[USR].GetallUserDeatilsKyc");

            _database.AddInParameter(dbCommand, "@UserId", serviceUser.UserID);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
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
                    row.KycTypeName = GetStringValue(dataReader, "KycTypeName");
                    row.FullName = GetStringValue(dataReader, "FullName");
                    row.FileUrl = GetStringValue(dataReader, "FileUrl");
                    row.CreatedBy = GetStringValue(dataReader, "CreatedBy");
                    row.UpdatedBy = GetStringValue(dataReader, "UpdatedBy");

                    response.Add(row);
                }
            }
            return response;
        }

        public async Task<List<UserKYYCResponse>> GetAllUserKycByUserId(long UserId, ISANYUKTServiceUser serviceUser)
        {
            List<UserKYYCResponse> response = new List<UserKYYCResponse>();

            var dbCommand = _database.GetStoredProcCommand("[USR].GetallUserDeatilsKyc");

            _database.AddInParameter(dbCommand, "@UserId", UserId);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
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
                    row.KycTypeName = GetStringValue(dataReader, "KycTypeName");
                    row.FullName = GetStringValue(dataReader, "FullName");
                    row.FileUrl = GetStringValue(dataReader, "FileUrl");
                    row.CreatedBy = GetStringValue(dataReader, "CreatedBy");
                    row.UpdatedBy = GetStringValue(dataReader, "UpdatedBy");

                    response.Add(row);
                }
            }
            return response;
        }

        public async Task<long> CreateNewUser(CreateNewUserRequest request, string Password, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].usp_CreateNewUser");
            _database.AddInParameter(dbCommand, "@applicationID", request.applicationID);
            _database.AddInParameter(dbCommand, "@OrganisationID", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@UserTypeId", request.UserTypeId);
            _database.AddInParameter(dbCommand, "@CreatedBy", serviceUser.UserMasterID);
            _database.AddInParameter(dbCommand, "@AccessID", request.AccessID);
            _database.AddInParameter(dbCommand, "@Password", Password);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<List<UserrListResponse>> GetallUserByOrg(ISANYUKTServiceUser serviceUser)
        {
            List<UserrListResponse> response = new List<UserrListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[USR].GetAllUserList");
            _database.AddInParameter(dbCommand, "@UserId", serviceUser.UserID);


            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    UserrListResponse objp = new UserrListResponse();
                    objp.UserMasterID = GetInt32Value(dataReader, "UserMasterID").Value;
                    objp.UserId = GetInt64Value(dataReader, "UserId").Value;
                    objp.UserName = GetStringValue(dataReader, "UserName");
                    objp.OrganisationName = GetStringValue(dataReader, "OrganisationName");
                    objp.UserType = GetStringValue(dataReader, "UserType");
                    objp.DisplayName = GetStringValue(dataReader, "DisplayName");
                    objp.EmailId = GetStringValue(dataReader, "EmailId");
                    objp.MobileNo = GetStringValue(dataReader, "MobileNo");
                    response.Add(objp);
                }
            }
            return response;
        }
        public async Task<long> UploadUserKYC(UploadUserKYCRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].UploadKycDocument");
            _database.AddInParameter(dbCommand, "@UserId", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@KycID", request.KycID);
            _database.AddInParameter(dbCommand, "@fileurl", request.fileurl);
            _database.AddInParameter(dbCommand, "@DocumentNo", request.DocumentNo);
            _database.AddInParameter(dbCommand, "@CreatedBy", serviceUser.UserMasterID);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<List<UserrKYCListResponse>> ListKYCByUser(ISANYUKTServiceUser serviceUser)
        {
            List<UserrKYCListResponse> response = new List<UserrKYCListResponse>();

            var dbCommand = _database.GetStoredProcCommand("[USR].GetallUserKYC");
            _database.AddInParameter(dbCommand, "@UserId", serviceUser.UserID);


            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    UserrKYCListResponse objp = new UserrKYCListResponse();
                    objp.UserKYCID = GetInt32Value(dataReader, "UserKYCID").Value;
                    objp.KycID = GetInt32Value(dataReader, "KycID").Value;
                    objp.CreatedBy = GetStringValue(dataReader, "CreatedBy");
                    objp.FileUrl = GetStringValue(dataReader, "FileUrl");
                    objp.DocumentNo = GetStringValue(dataReader, "DocumentNo");
                    objp.CreatedOn = GetDateValue(dataReader, "CreatedOn").Value;
                    objp.KycTypeName = GetStringValue(dataReader, "KycTypeName");

                    response.Add(objp);
                }
            }
            return response;
        }
        public async Task<List<UserKYYCResponse>> GetAllUserKycById(long KycId, ISANYUKTServiceUser serviceUser)
        {
            List<UserKYYCResponse> response = new List<UserKYYCResponse>();

            var dbCommand = _database.GetStoredProcCommand("[USR].GetallUserDeatilsKycById");

            //_database.AddInParameter(dbCommand, "@UserId", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@KycId", KycId);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
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
                    row.KycTypeName = GetStringValue(dataReader, "KycTypeName");
                    row.FullName = GetStringValue(dataReader, "FullName");
                    row.FileUrl = GetStringValue(dataReader, "FileUrl");
                    row.CreatedBy = GetStringValue(dataReader, "CreatedBy");
                    row.UpdatedBy = GetStringValue(dataReader, "UpdatedBy");

                    response.Add(row);
                }
            }
            return response;
        }


        public async Task<long> UpdateOriginatorChequeFile(PayinAccountRegistrationChequeRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].OriginatorChequeUpload");
            _database.AddInParameter(dbCommand, "@AccountId", request.AccountId);
            _database.AddInParameter(dbCommand, "@Filename", request.Filename);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<List<ApplicationParentMenuResponse>> GetAllMenu(ISANYUKTServiceUser serviceUser)
        {
            List<ApplicationParentMenuResponse> response = new List<ApplicationParentMenuResponse>();

            var dbCommand = _database.GetStoredProcCommand("[AAC].GetMenuDetail");

            _database.AddInParameter(dbCommand, "@UserMasterID", serviceUser.UserMasterID);
            _database.AddInParameter(dbCommand, "@ApplicationID", serviceUser.ApplicationID);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    ApplicationParentMenuResponse row = new ApplicationParentMenuResponse();

                    row.MenuID = GetInt32Value(dataReader, "MenuID").Value;
                    row.DisplayOrder = GetInt32Value(dataReader, "DisplayOrder").Value;

                    row.Target = GetStringValue(dataReader, "Target");
                    row.Tooltip = GetStringValue(dataReader, "Tooltip");
                    row.Description = GetStringValue(dataReader, "Description");
                    row.Title = GetStringValue(dataReader, "Title");
                    row.RoutePath = GetStringValue(dataReader, "RoutePath");

                    List<ApplicationMenuResponse> mm = new List<ApplicationMenuResponse>();

                    var dbCommand1 = _database.GetStoredProcCommand("[AAC].GetSubMenuDetail");

                    _database.AddInParameter(dbCommand1, "@UserMasterID", serviceUser.UserMasterID);
                    _database.AddInParameter(dbCommand1, "@ApplicationID", serviceUser.ApplicationID);
                    _database.AddInParameter(dbCommand1, "@ParentId", row.MenuID);
                    using (var dataReader1 = await _database.ExecuteReaderAsync(dbCommand1))
                    {
                        while (dataReader1.Read())
                        {
                            ApplicationMenuResponse xx = new ApplicationMenuResponse();
                            xx.MenuID = GetInt32Value(dataReader1, "MenuID").Value;
                            xx.ParentID = GetInt32Value(dataReader1, "ParentID").Value;
                            xx.DisplayOrder = GetInt32Value(dataReader1, "DisplayOrder").Value;

                            xx.Target = GetStringValue(dataReader1, "Target");
                            xx.Tooltip = GetStringValue(dataReader1, "Tooltip");
                            xx.Description = GetStringValue(dataReader1, "Description");
                            xx.Title = GetStringValue(dataReader1, "Title");
                            xx.RoutePath = GetStringValue(dataReader1, "RoutePath");

                            mm.Add(xx);
                        }
                    }
                    row.submenu = mm;

                    response.Add(row);
                }
            }
            return response;
        }

        public async Task<List<ApplicationMenuResponse>> GetAllSubMenu(int Menuid, ISANYUKTServiceUser serviceUser)
        {
            List<ApplicationMenuResponse> response = new List<ApplicationMenuResponse>();

            var dbCommand = _database.GetStoredProcCommand("[AAC].GetSubMenuDetail");

            _database.AddInParameter(dbCommand, "@UserMasterID", serviceUser.UserMasterID);
            _database.AddInParameter(dbCommand, "@ApplicationID", serviceUser.ApplicationID);
            _database.AddInParameter(dbCommand, "@ParentId", Menuid);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    ApplicationMenuResponse row = new ApplicationMenuResponse();

                    row.MenuID = GetInt32Value(dataReader, "MenuID").Value;
                    row.ParentID = GetInt32Value(dataReader, "ParentID").Value;
                    row.DisplayOrder = GetInt32Value(dataReader, "DisplayOrder").Value;

                    row.Target = GetStringValue(dataReader, "Target");
                    row.Tooltip = GetStringValue(dataReader, "Tooltip");
                    row.Description = GetStringValue(dataReader, "Description");
                    row.Title = GetStringValue(dataReader, "Title");
                    row.RoutePath = GetStringValue(dataReader, "RoutePath");

                    response.Add(row);
                }
            }
            return response;
        }

        public async Task<PartnerDeatilsResponse> GetAllUserDeatils(ISANYUKTServiceUser serviceUser)
        {
            PartnerDeatilsResponse response = new PartnerDeatilsResponse();

            var dbCommand = _database.GetStoredProcCommand("[USR].GetOrganisationDetails");

            _database.AddInParameter(dbCommand, "@UserId", serviceUser.UserID);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                if (dataReader.Read())
                {

                    response.UserId = GetInt32Value(dataReader, "UserId").Value;
                    response.Usercode = GetStringValue(dataReader, "Usercode");
                    response.ContactPersonName = GetStringValue(dataReader, "ContactPersonName");
                    response.AvailableLimit = GetDecimalValue(dataReader, "AvailableLimit") ?? 0;
                    response.OrganisationName = GetStringValue(dataReader, "OrganisationName");
                    response.EmailId = GetStringValue(dataReader, "EmailId");
                    response.MobileNo = GetStringValue(dataReader, "MobileNo");
                }
            }
            return response;
        }
        public async Task<List<OriginatorListAccountResponse>> ListAllOriginatorsAccountsforAdmin(OriginatorListAccountforadminRequest request, ISANYUKTServiceUser serviceUser)
        {
            List<OriginatorListAccountResponse> response = new List<OriginatorListAccountResponse>();

            var dbCommand = _database.GetStoredProcCommand("[USR].ListallOriginatorsAccountsForAdmin");

            _database.AddInParameter(dbCommand, "@FromDate", request.FromDate);
            _database.AddInParameter(dbCommand, "@ToDate", request.ToDate);
            _database.AddInParameter(dbCommand, "@Status", request.Status);
            _database.AddInParameter(dbCommand, "@PageNo", request.PageNo);
            _database.AddInParameter(dbCommand, "@PageSize", request.PageSize);
            _database.AddInParameter(dbCommand, "@OrderBy", request.OrderBy);
            _database.AddOutParameter(dbCommand, "@Out_TotalRec", 100);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    OriginatorListAccountResponse row = new OriginatorListAccountResponse();

                    row.Status = GetInt32Value(dataReader, "Status").Value;
                    row.BankId = GetInt32Value(dataReader, "BankId").Value;
                    row.UserId = GetInt32Value(dataReader, "UserId").Value;
                    row.OriginatorAccountID = GetInt32Value(dataReader, "OriginatorAccountID").Value;
                    row.UpdatedOn = GetDateValue(dataReader, "UpdatedOn");
                    row.StatusName = GetStringValue(dataReader, "StatusName");
                    row.AccountName = GetStringValue(dataReader, "AccountName");
                    row.BankName = GetStringValue(dataReader, "BankName");
                    row.AccountNo = GetStringValue(dataReader, "AccountNo");
                    row.BranchAddress = GetStringValue(dataReader, "BranchAddress");
                    row.CreatedBy = GetStringValue(dataReader, "CreatedBy");
                    row.UpdatedBy = GetStringValue(dataReader, "UpdatedBy");
                    row.Fullname = GetStringValue(dataReader, "Fullname");
                    row.Usercode = GetStringValue(dataReader, "Usercode");
                    row.Filename = GetStringValue(dataReader, "Filename");
                    row.Ifsccode = GetStringValue(dataReader, "Ifsccode");
                    response.Add(row);
                }
            }
            return response;
        }
        public async Task<long> ApproveRejectOriginatorAccounts(ApproveRejectOriAccountRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].ApproveRejectOriginatorAcc");
            _database.AddInParameter(dbCommand, "@RequestId", request.RequestId);
            _database.AddInParameter(dbCommand, "@Status", request.Status);
            _database.AddInParameter(dbCommand, "@RemarksReason", request.RemarksReason);
            _database.AddInParameter(dbCommand, "@UserMasterId", serviceUser.UserMasterID);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }

        public async Task<List<ListOrganisationResponse>> GetAllOrganisationDetails(ListOrganisationDetailRequest request, ISANYUKTServiceUser serviceUser)
        {
            List<ListOrganisationResponse> response = new List<ListOrganisationResponse>();

            var dbCommand = _database.GetStoredProcCommand("[USR].OrganisationList");

            _database.AddInParameter(dbCommand, "@MobileNo", request.MobileNo);
            _database.AddInParameter(dbCommand, "@EmailId", request.EmailId);
            _database.AddInParameter(dbCommand, "@UserId", request.UserId);
            _database.AddInParameter(dbCommand, "@PageNo", request.PageNo);
            _database.AddInParameter(dbCommand, "@PageSize", request.PageSize);
            _database.AddInParameter(dbCommand, "@OrderBy", request.OrderBy);
            _database.AddOutParameter(dbCommand, "@Out_TotalRec", 100);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    ListOrganisationResponse row = new ListOrganisationResponse();

                    row.UserId = GetInt32Value(dataReader, "UserId").Value;
                    row.UpdatedOn = GetDateValue(dataReader, "UpdatedOn");
                    row.ContactPerson = GetStringValue(dataReader, "ContactPerson");
                    row.Usercode = GetStringValue(dataReader, "Usercode");
                    row.UpdatedBy = GetStringValue(dataReader, "UpdatedBy");
                    row.EmailId = GetStringValue(dataReader, "EmailId");
                    row.MobileNo = GetStringValue(dataReader, "MobileNo");
                    row.OrganisationName = GetStringValue(dataReader, "OrganisationName");
                    row.StatusName = GetStringValue(dataReader, "StatusName");
                    row.Status = GetInt32Value(dataReader, "Status").Value;
                    response.Add(row);
                }
            }
            return response;
        }
        public async Task<long> ApproveRejectUserDocument(ApproveRejectUserDocumentRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].ApproveRejectDocument");
            _database.AddInParameter(dbCommand, "@UserKYCID", request.UserKYCID);
            _database.AddInParameter(dbCommand, "@Status", request.Status);
            _database.AddInParameter(dbCommand, "@RejectedReason", request.RejectedReason);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<List<UserConfigrationResponse>> GetAllUserConfigration(long UserId, ISANYUKTServiceUser serviceUser)
        {
            List<UserConfigrationResponse> response = new List<UserConfigrationResponse>();

            var dbCommand = _database.GetStoredProcCommand("[USR].GetUSerConfigurationByUserId");

            if (UserId == 0)
            {
                _database.AddInParameter(dbCommand, "@UserId", serviceUser.UserID);
            }
            else
            {
                _database.AddInParameter(dbCommand, "@UserId", UserId);
            }

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    UserConfigrationResponse row = new UserConfigrationResponse();

                    row.UserId = GetInt32Value(dataReader, "UserId").Value;
                    row.ConfigurationId = GetInt64Value(dataReader, "ConfigurationId").Value;
                    row.MinTxn = GetDecimalValue(dataReader, "MinTxn").Value;
                    row.MaxTxn = GetDecimalValue(dataReader, "MaxTxn").Value;
                    row.ChargeDeductionType = GetStringValue(dataReader, "ChargeDeductionType");
                    row.MaxPayinamount = GetDecimalValue(dataReader, "MaxPayinamount").Value;
                    row.ChargeTypeOn = GetInt32Value(dataReader, "ChargeTypeOn").Value;
                    row.PlanName = GetStringValue(dataReader, "PlanName");
                    row.SameAmountPayinAllowed = GetInt32Value(dataReader, "SameAmountPayinAllowed").Value;
                    row.PlanId = GetInt32Value(dataReader, "PlanId").Value;
                    row.MaxNoofcountPayin = GetInt32Value(dataReader, "MaxNoofcountPayin").Value;
                    response.Add(row);
                }
            }
            return response;
        }
        public async Task<long> UpDateUserConfigrationDetails(UserConfigrationRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            long? UserId = 0;

            if (serviceUser.UserTypeId == 2)
            {
                UserId = serviceUser.UserID;
            }
            else
            {
                UserId = request.UserId;
            }


            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].UpdateUserConfigration");
            _database.AddInParameter(dbCommand, "@userid", UserId);
            _database.AddInParameter(dbCommand, "@mintxn", request.MinTxn);
            _database.AddInParameter(dbCommand, "@maxtxn", request.MaxTxn);
            _database.AddInParameter(dbCommand, "@chargetypeon", request.ChargeTypeOn);
            _database.AddInParameter(dbCommand, "@planid", request.PlanId);
            _database.AddInParameter(dbCommand, "@maxpayinamount", request.MaxPayinamount);
            _database.AddInParameter(dbCommand, "@maxnoofcountpayin", request.MaxNoofcountPayin);
            _database.AddInParameter(dbCommand, "@sameamountpayinallowed", request.SameAmountPayinAllowed);
            _database.AddInParameter(dbCommand, "@updatedby", serviceUser.UserMasterID);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<long> ActivateDeactivateApiUser(ActivateAPIUserRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;

            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].ActivateDeactivateAPIUser");
            _database.AddInParameter(dbCommand, "@UserId", request.UserId);
            _database.AddInParameter(dbCommand, "@Status", request.Status);
            _database.AddInParameter(dbCommand, "@Reason", request.Reason);
            _database.AddInParameter(dbCommand, "@UpdatedBy", serviceUser.UserMasterID);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<long> ActivateDeactivateUserMaster(ActivateAPIUserMasterRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;

            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].ActivateDeactivateUserMaster");
            _database.AddInParameter(dbCommand, "@UserMasterID", request.UserMasterId);
            _database.AddInParameter(dbCommand, "@Status", request.Status);
            _database.AddInParameter(dbCommand, "@Reason", request.Reason);
            _database.AddInParameter(dbCommand, "@UpdatedBy", serviceUser.UserMasterID);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<ListResponse> GetAllUserMasterList(ListUserMasterRequest request, ISANYUKTServiceUser serviceUser)
        {
            ListResponse response1 = new ListResponse();
            List<ListUserMasterResponse> response = new List<ListUserMasterResponse>();

            var dbCommand = _database.GetStoredProcCommand("[USR].UserMasterList");

            _database.AddInParameter(dbCommand, "@MobileNo", request.MobileNo);
            _database.AddInParameter(dbCommand, "@EmailId", request.EmailId);
            _database.AddInParameter(dbCommand, "@UserMasterId", request.UserMasterId);
            _database.AddInParameter(dbCommand, "@PageNo", request.PageNo);
            _database.AddInParameter(dbCommand, "@PageSize", request.PageSize);
            _database.AddInParameter(dbCommand, "@OrderBy", request.OrderBy);
            _database.AddOutParameter(dbCommand, "@Out_TotalRec", 100);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    ListUserMasterResponse row = new ListUserMasterResponse();


                    row.UserTypeId = GetInt32Value(dataReader, "UserTypeId").Value;
                    row.UserMasterID = GetInt32Value(dataReader, "UserMasterID").Value;
                    // row.UpdatedOn = GetDateValue(dataReader, "UpdatedOn") ;
                    row.ContactPerson = GetStringValue(dataReader, "ContactPerson");
                    row.UserName = GetStringValue(dataReader, "UserName");
                    row.UpdatedBy = GetStringValue(dataReader, "UpdatedBy");
                    row.EmailId = GetStringValue(dataReader, "EmailId");
                    row.MobileNo = GetStringValue(dataReader, "MobileNo");
                    row.OrganisationName = GetStringValue(dataReader, "OrganisationName");
                    row.UserTypename = GetStringValue(dataReader, "UserTypename");
                    row.StatusName = GetStringValue(dataReader, "StatusName");
                    row.Status = GetInt32Value(dataReader, "Status").Value;
                    response.Add(row);
                }
            }
            response1.SetPagingOutput(dbCommand);
            response1.CurrentPage = request.PageNo;
            response1.Result = response;
            return response1;
        }

        public async Task<SimpleResponse> GetUserMasterDetailsforConfig(string UserName, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response1 = new SimpleResponse();
            ListUserMasterResponse response = new ListUserMasterResponse();

            var dbCommand = _database.GetStoredProcCommand("[USR].UserMasterListDetails");

            _database.AddInParameter(dbCommand, "@UserName", UserName);


            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                if (dataReader.Read())
                {



                    response.UserTypeId = GetInt32Value(dataReader, "UserTypeId").Value;
                    response.UserMasterID = GetInt32Value(dataReader, "UserMasterID").Value;
                    // row.UpdatedOn = GetDateValue(dataReader, "UpdatedOn") ;
                    response.ContactPerson = GetStringValue(dataReader, "ContactPerson");
                    response.UserName = GetStringValue(dataReader, "UserName");
                    response.UpdatedBy = GetStringValue(dataReader, "UpdatedBy");
                    response.EmailId = GetStringValue(dataReader, "EmailId");
                    response.MobileNo = GetStringValue(dataReader, "MobileNo");
                    response.OrganisationName = GetStringValue(dataReader, "OrganisationName");
                    response.UserTypename = GetStringValue(dataReader, "UserTypename");
                    response.StatusName = GetStringValue(dataReader, "StatusName");
                    response.Status = GetInt32Value(dataReader, "Status").Value;

                }
            }

            response1.Result = response;
            return response1;
        }
        public async Task<ListResponse> ListUserAddress(ListUserAddressRequest request, ISANYUKTServiceUser serviceUser)
        {
            ListResponse response1 = new ListResponse();
            List<UserAddressListResponse> response = new List<UserAddressListResponse>();
            long? newuserid = 0;

            var dbCommand = _database.GetStoredProcCommand("[USR].ListallUserAddresses");


            if (serviceUser.UserTypeId == 3)
            {
                newuserid = serviceUser.UserID;
            }
            else
            {
                newuserid = request.UserId;
            }

            _database.AddInParameter(dbCommand, "@UserId", newuserid);
            _database.AddInParameter(dbCommand, "@PageNo", request.PageNo);
            _database.AddInParameter(dbCommand, "@PageSize", request.PageSize);
            _database.AddInParameter(dbCommand, "@OrderBy", request.OrderBy);
            _database.AddOutParameter(dbCommand, "@Out_TotalRec", 100);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    UserAddressListResponse row = new UserAddressListResponse();

                    row.Status = GetInt32Value(dataReader, "Status").Value;
                    row.AddressTypeId = GetInt32Value(dataReader, "AddressTypeId").Value;
                    row.UserID = GetInt32Value(dataReader, "UserID").Value;
                    row.UserAddressID = GetInt32Value(dataReader, "UserAddressID").Value;
                    row.CreatedOn = GetDateValue(dataReader, "CreatedOn");
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
            response1.SetPagingOutput(dbCommand);
            response1.CurrentPage = request.PageNo;
            response1.Result = response;
            return response1;
        }
        public async Task<long> ChangePassword(ChangePasswordRequest request, string changepassword, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].ChangePassword");
            _database.AddInParameter(dbCommand, "@UserMasterID", serviceUser.UserMasterID);
            _database.AddInParameter(dbCommand, "@Password", changepassword);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<long> AddIPAddress(AddIPAddressRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[AAC].CreateIPAddress");
            _database.AddInParameter(dbCommand, "@ApplicationId", request.ApplicationId);
            _database.AddInParameter(dbCommand, "@IPAddress", request.IPAddress);
            _database.AddInParameter(dbCommand, "@CreatedBy", serviceUser.UserMasterID);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<SimpleResponse> GetallIPAddress(long UserId, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response1 = new SimpleResponse();
            List<GetIPAddressResponse> response = new List<GetIPAddressResponse>();

            long? newuserid = 0;
            if (serviceUser.UserTypeId == 3)
            {
                newuserid = serviceUser.UserID;
            }
            else
            {
                newuserid = UserId;
            }
            var dbCommand = _database.GetStoredProcCommand("[AAC].GetAllIPAddress");
            _database.AddInParameter(dbCommand, "@UserId", newuserid);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    GetIPAddressResponse row = new GetIPAddressResponse();

                    row.Status = GetInt32Value(dataReader, "Status").Value;
                    row.IPAddressId = GetInt32Value(dataReader, "IPAddressId").Value;
                    row.ApplicationId = GetInt32Value(dataReader, "ApplicationId").Value;
                    row.CreatedOn = GetDateValue(dataReader, "CreatedOn");
                    row.UpdatedOn = GetDateValue(dataReader, "UpdatedOn");
                    row.StatusName = GetStringValue(dataReader, "StatusName");
                    row.ApplicationName = GetStringValue(dataReader, "ApplicationName");
                    row.OrganisationName = GetStringValue(dataReader, "OrganisationName");
                    row.IPAddress = GetStringValue(dataReader, "IPAddress");
                    row.CreatedBy = GetStringValue(dataReader, "CreatedBy");
                    row.UpdatedBy = GetStringValue(dataReader, "UpdatedBy");

                    response.Add(row);
                }
            }
            response1.Result = response;
            return response1;
        }
        public async Task<long> ApproveRejectIP(ApproveRejectIPAddressRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[AAC].ApproveRejectIPAddress");
            _database.AddInParameter(dbCommand, "@IpAddressId",request.IpAddressId);
            _database.AddInParameter(dbCommand, "@Status", request.Status);
            _database.AddInParameter(dbCommand, "@UpdatedBy",serviceUser.UserMasterID);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<ListResponse> GetAllIPAddressforAdmin(IPAddressListDetail request, ISANYUKTServiceUser serviceUser)
        {
            ListResponse response1 = new ListResponse();
            List<GetIPAddressResponse> response = new List<GetIPAddressResponse>();

            var dbCommand = _database.GetStoredProcCommand("[AAC].GetAllIPAddressForAdmin");
            _database.AddInParameter(dbCommand, "@UserId", request.UserId);
            _database.AddInParameter(dbCommand, "@applicationID", request.applicationID);
            _database.AddInParameter(dbCommand, "@Status", request.Status);
            _database.AddInParameter(dbCommand, "@PageNo", request.PageNo);
            _database.AddInParameter(dbCommand, "@PageSize", request.PageSize);
            _database.AddInParameter(dbCommand, "@OrderBy", request.OrderBy);
            _database.AddOutParameter(dbCommand, "@Out_TotalRec", 100);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    GetIPAddressResponse row = new GetIPAddressResponse();

                    row.Status = GetInt32Value(dataReader, "Status").Value;
                    row.IPAddressId = GetInt32Value(dataReader, "IPAddressId").Value;
                    row.ApplicationId = GetInt32Value(dataReader, "ApplicationId").Value;
                    row.CreatedOn = GetDateValue(dataReader, "CreatedOn");
                    row.UpdatedOn = GetDateValue(dataReader, "UpdatedOn");
                    row.StatusName = GetStringValue(dataReader, "StatusName");
                    row.ApplicationName = GetStringValue(dataReader, "ApplicationName");
                    row.OrganisationName = GetStringValue(dataReader, "OrganisationName");
                    row.IPAddress = GetStringValue(dataReader, "IPAddress");
                    row.CreatedBy = GetStringValue(dataReader, "CreatedBy");
                    row.UpdatedBy = GetStringValue(dataReader, "UpdatedBy");

                    response.Add(row);
                }
            }
            response1.SetPagingOutput(dbCommand);
            response1.CurrentPage = request.PageNo;
            response1.Result = response;
            return response1;
        }
    }
}
