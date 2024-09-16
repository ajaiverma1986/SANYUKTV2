using SANYUKT.Datamodel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.Masters
{
    public class ServiceListResponse
    {
        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string ServiceAccountNo { get; set; }
        public string ServcieIfsccode { get; set; }
        public string ServiceAccName { get; set; }
        public string ServiceMobileNo { get; set; }

    }
    public class CompanyTypeMasterResponse
    {
        public int CompnayTypeId { get; set; }
        public string CompanyTypeName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
    }
    public class GenderResponse
    {
        public int GenderId { get; set; }
        public string GenderName { get; set; }
    }
    public class MaritalStatusResponse
    {
        public int MaritalStatusID { get; set; }
        public string MaritalStatusName { get; set; }
    }
    public class AddressTypeListResponse
    {
        public int AddressTypeId { get; set; }
        public string AddressTypeName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
    }
    public class AgencyMasterListResponse
    {
        public int AgencyId { get; set; }
        public string AgencyName { get; set; }
        public string AgencyCode { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
    }
    public class UserTypeListResponse
    {
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
    }
    public class BankListResponse
    {
        public int BankID { get; set; }
        public string BankName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
    }
    public class LedgerTypeListResponse
    {
        public int LedgerTypeId { get; set; }
        public string LedgerTypeName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
    }
    public class PlanMasterListResponse
    {
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
    }
    public class StateListResponse
    {
        public int StateID { get; set; }
        public string StateCode { get; set; }
       
        public string StateName { get; set; }
    }
    public class DistrictListResponse
    {
        public int DistrictID { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
    }
    public class KycTypeMasterListResponse
    {
        public int KycTypeID { get; set; }
        public int CompnayTypeId { get; set; }
        public int Status { get; set; }
        public int UserTypeId { get; set; }
        public string KycTypeName { get; set; }
        public string UserTypeName { get; set; }
        public string CompanyTypeName { get; set; }
        public string StatusName { get; set; }
    }
    public class PincodeDataResponse
    {
        public int PincodeDataId { get; set; }
        public int StateID { get; set; }
        public int DistrictID { get; set; }
        public string AreaName { get; set; }
        public string Pincode { get; set; }
        public string SubDistrictName { get; set; }
        public string DistrictName { get; set; }
        public string StateName { get; set; }
       
    }
    public class PincodeDataRequest:ListRequest
    {
        public string Pincode { get; set; }
     
    }
    public class DistrictListRequest : ListRequest
    {
        public int StateId { get; set; }

    }
    public class serviceTypeListResponse
    {
        public int ServiceTypeId { get; set; }
        public int AgencyId { get; set; }
        public int Status { get; set; }
        public string ServiceTypeName { get; set; }
        public string AgencyName { get; set; }
        public string StatusName { get; set; }
    }
    public class serviceListResponse
    {
        public int ServiceTypeId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string ServiceAccountNo { get; set; }
        public string ServcieIfsccode { get; set; }
        public string ServiceAccName { get; set; }
        public string ServiceMobileNo { get; set; }
        public string ServiceTypeName { get; set; }
    }
    public class ListPaymentChanelResponse
    {
        public int PaymentChanelID { get; set; }
        public int Status { get; set; }
        public string PaymentChanelName { get; set; }
        public string StatusName { get; set; }
    }
    public class ListPaymentModeResponse
    {
        public int PaymentModeID { get; set; }
        public int PaymentChanelID { get; set; }
        public int Status { get; set; }
        public string PaymentModeName { get; set; }
        public string PaymentChanelName { get; set; }
        public string StatusName { get; set; }
    }
}
