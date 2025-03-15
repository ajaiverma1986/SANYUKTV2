using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.CMS
{
    public class UserResponse
    {
        public string UserCode { get; set; }
        public string UserName { get; set; }

        public long? UserId { get; set; }
        public string LastLoginIpAddress { get; set; }
        public string Password { get; set; }
        public int? UserType { get; set; }
        public long? ParentId { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public string EmailId { get; set; }
        public string Mobile { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Pancard { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress1 { get; set; }
        public string CompanyAddress2 { get; set; }
        public string CompanyAddress3 { get; set; }
        public string CompanyCity { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public long? KioskBankId { get; set; }
        public string IfscCode { get; set; }
        public int CompanyDistrictId { get; set; }
        public string CompanyDistrict { get; set; }
        public int CompanyStateId { get; set; }
        public string CompanyState { get; set; }
        public int CompanyRegionId { get; set; }
        public string CompanyRegion { get; set; }
        public string CompanyPincode { get; set; }
        public string CompanyTelephone { get; set; }
        public string ResidenceAddress1 { get; set; }
        public string ResidenceAddress2 { get; set; }
        public string ResidenceAddress3 { get; set; }
        public string ResidenceCity { get; set; }
        public string Spouse { get; set; }
        public string AadharCard { get; set; }
        public int ResidenceDistrictId { get; set; }
        public string ResidenceDistrict { get; set; }
        public int ResidenceStateId { get; set; }
        public string ResidenceState { get; set; }
        public int ResidenceRegionId { get; set; }
        public string ResidenceRegion { get; set; }
        public string ResidencePincode { get; set; }
        public string ResidenceTelephone { get; set; }
        public decimal AvailableLimit { get; set; }
        public int MarginTypeId { get; set; }
        public string MarginTypeName { get; set; }
        public DateTime AccordDate { get; set; }
        public string AccountType { get; set; }

    }
}
