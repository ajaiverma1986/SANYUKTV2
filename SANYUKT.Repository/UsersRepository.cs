﻿using SANYUKT.Database;
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
            _database.AddInParameter(dbCommand, "@UserID", request.userId);
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
        public async Task<long> CreateOrgAPIPartner(CreateNewPartnerRequest request,string Password, ISANYUKTServiceUser serviceUser)
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
        public async Task<List<ApplicationListResponse>> Getallapplication( ISANYUKTServiceUser serviceUser)
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

            _database.AddInParameter(dbCommand, "@UserId", serviceUser.UserID);
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

    }
}
