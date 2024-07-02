using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.Entities.Users
{
    public class UsersDetailsResponse
    {
        public long UserId { get; set; }
        public long UserMasterId { get; set; }
        public int UserTypeId { get; set; }
        public string Usercode { get; set; }
        public decimal? AvailableLimit { get; set; }
        public decimal? ThresoldLimit { get; set; }
       
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
}
