using Microsoft.AspNetCore.Http;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.Library;
using SANYUKT.Datamodel.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

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
        public long? UserId { get; set; }
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
        public long? UserId { get; set; }
    }
    public class UploadOrgLogo1
    {
        public IFormFile iform { get; set; }
        public long UserId { get; set; }
    }

    public class CreateOriginatorAccountRequest
    {
        public int BankId { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string Ifsccode { get; set; }
        public string BranchAddress { get; set; }
      
    }
    public class ApproveRejectOriAccountRequest
    {
        public long RequestId { get; set; }
        public int Status { get; set; }
        public string RemarksReason { get; set; }
    
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
        public string Filename { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int Status { get; set; }


    }
    public class OriginatorListAccountRequest:ListRequest
    {
        public int Status { get; set; }
      

    }
    public class OriginatorListAccountforadminRequest : ListRequest
    {
        public int Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
    public class CreateUserDetailAddressRequest
    {
        public int AddressTypeId { get; set; }
        public int userId { get; set; }
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
        public string KycTypeName { get; set; }
        public string FullName { get; set; }
        public string FileUrl { get; set; }
        public string StatusName { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int Status { get; set; }


    }
    public class CreateNewPartnerRequest
    {
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string OrganisationName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
    public class ApplicationListResponse
    {
        public int? ApplicationID { get; set; }
        public long? OrganizationID { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationDescription { get; set; }
        public string ApplicationToken { get; set; }
        public string CreatedBy { get; set; }
        public string OrganisationName { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public DateTime? CreatedOn { get; set; }
        
    }
    public class CreateNewUserRequest
    {
        public int applicationID { get; set; }
        public int UserTypeId { get; set; }
        public string Password { get; set; }
        public string AccessID { get; set; }
    }
    public class UserrListResponse
    {
        public long? UserMasterID { get; set; }
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string OrganisationName { get; set; }
        public string UserType { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }

    }
    public class UploadUserKYCRequest
    {
        public string DocumentNo { get; set; }
        public string fileurl { get; set; }
        public int? KycID { get; set; }
    }
    public class UploadUserKYCFileRequest
    {
        public int? KycID { get; set; }
        public string DocumentNo { get; set;}
    }
    public class UserrKYCListResponse
    {
        public long? UserKYCID { get; set; }
        public int? KycID { get; set; }
        public string KycTypeName { get; set; }
        public string DocumentNo { get; set; }
        public string FileUrl { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }

    }
    public class UserKycdownloadListResponse
    {
        public long? UserKYCID { get; set; }
        public int? KycID { get; set; }
        public string KycTypeName { get; set; }
        public string DocumentNo { get; set; }
        public string FileUrl { get; set; }

        public string ContentType { get; set; }
        public Byte[] FileBytes { get; set; }

        public string Base64String { get; set; }

        public string MediaContentType { get; set; }
        public string MediaExtension { get; set; }

    }
    public class PayinAccountRegistrationChequeRequest
    {
        public string Filename { get; set; }
        public long? AccountId { get; set; }
    }
    public class UserAccountsChecueFileResponse
    {
        public long OriginatorAccountID { get; set; }
        public string FileUrl { get; set; }
        public Byte[] FileBytes { get; set; }
        public string Base64String { get; set; }
        public string MediaExtension { get; set; }

    }
    public class ApplicationParentMenuResponse
    {
        public long MenuID { get; set; }
        public string Title { get; set; }
        public string Tooltip { get; set; }
        public string Description { get; set; }
        public string RoutePath { get; set; }
        public int DisplayOrder { get; set; }
        public string Target { get; set; }
        public List<ApplicationMenuResponse> submenu { get; set; }


    }
    public class ApplicationMenuResponse
    {
        public long MenuID { get; set; }
        public long? ParentID { get; set; }
        public string Title { get; set; }
        public string Tooltip { get; set; }
        public string Description { get; set; }
        public string RoutePath { get; set; }
        public int DisplayOrder { get; set; }
        public string Target { get; set; }


    }

    public class PartnerDeatilsResponse
    {
        public long UserId { get; set; }
        public string Usercode { get; set; }
        public string OrganisationName { get; set; }
        public string ContactPersonName { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public decimal? AvailableLimit { get; set; }
       
    }
    public class UserConfigResponse
    {
        public long ConfigurationId { get; set; }
        public long UserId { get; set; }
        public decimal MinTxn { get; set; }
        public decimal MaxTxn { get; set; }
        public int ChargeTypeOn { get; set; }
        public string ChargeDeductionType { get; set; }
        public int PlanId { get; set; }

    }
    public class ListOrganisationDetailRequest : ListRequest
    {
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public long? UserId { get; set; } 

    }
    public class ListOrganisationResponse
    {
        public long UserId { get; set; }
        public string Usercode { get; set; }
        public string OrganisationName { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string StatusName { get; set; }
        public int Status { get; set; }

    }
    public class ApproveRejectUserDocumentRequest
    {
        public long UserKYCID { get; set; }
        public int Status { get; set; }
        public string RejectedReason { get; set; }
    }
    public class UserConfigrationResponse
    {
        public long ConfigurationId { get; set; }
        public long UserId { get; set; }
        public decimal MinTxn { get; set; }
        public decimal MaxTxn { get; set; }
        public int ChargeTypeOn { get; set; }
        public string ChargeDeductionType { get; set; }
        public decimal MaxPayinamount { get; set; }
        public int MaxNoofcountPayin { get; set; }
        public int SameAmountPayinAllowed { get; set; }
        public int PlanId { get; set; }
        public string PlanName { get; set; }

    }
    public class UserConfigrationRequest
    {
        public long UserId { get; set; }
        public decimal MinTxn { get; set; }
        public decimal MaxTxn { get; set; }
        public int ChargeTypeOn { get; set; }
        public decimal MaxPayinamount { get; set; }
        public int MaxNoofcountPayin { get; set; }
        public int SameAmountPayinAllowed { get; set; }
        public int PlanId { get; set; }

    }
    public class ActivateAPIUserRequest
    {
        public long UserId { get; set; }
        public int Status { get; set; }
        public string Reason { get; set; }
     
    }
    public class ActivateAPIUserMasterRequest
    {
        public long UserMasterId { get; set; }
        public int Status { get; set; }
        public string Reason { get; set; }

    }
    public class ListUserMasterRequest : ListRequest
    {
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public long? UserMasterId { get; set; }

    }
    public class ListUserMasterResponse
    {
        
        public long UserMasterID { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypename { get; set; }
        public string UserName { get; set; }

        public string OrganisationName { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
       // public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string StatusName { get; set; }
        public int Status { get; set; }

    }
    public class ListUserAddressRequest : ListRequest
    {
        public long UserId { get; set; }

    }
    public class ChangePasswordRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }

    }
    public class AddIPAddressRequest
    {
        public long ApplicationId { get; set; }
        public string IPAddress { get; set; }

    }
    public class GetIPAddressResponse
    {
        public long IPAddressId { get; set; }
        public long ApplicationId { get; set; }
        public string OrganisationName { get; set; }
        public string ApplicationName { get; set; }
        public string StatusName { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string IPAddress { get;set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int Status { get; set; }


    }
    public class ApproveRejectIPAddressRequest
    {
        public long IpAddressId { get; set; }
        public int Status { get; set; }
      
    }
    public class IPAddressListDetail:ListRequest
    {
        public long UserId { get; set; }
        public long applicationID { get; set; }
        public int Status { get; set; }

    }
}
