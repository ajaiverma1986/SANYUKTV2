﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.Entities.Users
{
    public class UsersDetailsResponse
    {
        public long UserId { get; set; }
        public int UserTypeId { get; set; }
        public string Usercode { get; set; }
        public decimal? AvailableLimit { get; set; }
        public decimal? ThresoldLimit { get; set; }
       
    }
    public class PartnerLimitResponse
    {
        public long PartnerID { get; set; }
        public string PartnerCode { get; set; }
        public decimal? Balance { get; set; }

    }
    public class AddBenficiaryRequest
    {
        public string BenficiaryName { get; set; }
        public string BenMobile { get; set; }
        public string EmailId { get; set; }
        public string BenAddress { get; set; }
        public string BenbankName { get; set; }
        public string BenBankcode { get; set; }
        public string  BenBranchCode { get; set; }
        public string BenAccountNo { get; set; }
        public string BenIfsccode { get; set; }
        
    }
    public class BenficiaryResponse
    {
        public long BenFiciaryId { get; set; }
        public long PartnerId { get; set; }
        public string BenficiaryName { get; set; }
        public string BenMobile { get; set; }
        public string EmailId { get; set; }
        public string BenAddress { get; set; }
        public string BenbankName { get; set; }
        public string BenBankcode { get; set; }
        public string BenBranchCode { get; set; }
        public string BenAccountNo { get; set; }
        public string BenIfsccode { get; set; }
        

    }
    public class ListBenficaryRequest
    {
        public long BenFiciaryId { get; set; }
        public string MobileNo { get; set; }
        public string AccountNo { get; set; }
        
    }
    public class BenficaryChangeStatusRequest
    {
        public long BenFiciaryId { get; set; }
      
    }
    public class CreateUserRequest
    {
        public int UserTypeId { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string LogoUrl { get; set; }

    }
    public class UploadLogoRequest
    {
        public string Logourl { get; set; }
        public long UserId { get; set; }
    }
    public class CreateUserWithlogoRequest
    {
        public int UserTypeId { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
    public class UploadOrgLogo
    {
        public Byte[] FileBytes { get; set; }
        public string FileName { get; set; }
        public long UserId { get; set; }
    }
    public class UploadOrgLogo1
    {
        public IFormFile iform { get; set; }
        public long UserId { get; set; }
    }

    public class CreateOriginatorAccountRequest
    {
        public long  UserId { get; set; }
        public int BankId { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string Ifsccode { get; set; }
        public string BranchAddress { get; set; }
      
    }
    public class OriginatorListAccountResponse
    {
        public long OriginatorAccountID { get; set; }
        public long UserId { get; set; }
        public int BankId { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string Ifsccode { get; set; }
        public string BranchAddress { get; set; }
        public string StatusName { get; set; }
        public string BankName { get; set; }
        public string Usercode { get; set; }
        public string Fullname { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int Status { get; set; }


    }
    public class CreateUserDetailAddressRequest
    {
        public int AddressTypeId { get; set; }
        public string Pincode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public long PincodeDataId { get; set; }
        
    }
    public class UserAddressListResponse
    {
        public long UserAddressID { get; set; }
        public long UserID { get; set; }
        public int AddressTypeId { get; set; }
        public long PincodeDataId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string AddressTypeName { get; set; }
        public string StatusName { get; set; }
        public string AreaName { get; set; }
        public string SubDistrictName { get; set; }
        public string DistrictName { get; set; }
        public string StateName { get; set; }
        public string CreatedBy { get; set; }
        public string Pincode { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int Status { get; set; }


    }

    public class CreateUserDetailKyc
    {
        public string DocumentNo { get; set; }
        public string FileUrl { get; set; }
        public int KycID { get; set; }

    }
    public class CreateUserDetailKyc1
    {
        public Byte[] FileBytes { get; set; }
        public string FileName { get; set; }
        public string DocumentNo { get; set; }
        public int KycID { get; set; }
    }
    public class UserKYYCResponse
    {
        public long UserKYCID { get; set; }
        public long UserId { get; set; }
        public int KycID { get; set; }
        public string DocumentNo { get; set; }
        public string FileUrl { get; set; }
        public string StatusName { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int Status { get; set; }


    }
}
