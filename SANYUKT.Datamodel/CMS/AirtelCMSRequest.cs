using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.CMS
{
    public class AirtelCMSBalanceInqRequest
    {
        public string eventType { get; set; }
        public AirtelCMSRequestParamBalInq param { get; set; }
    }
    public class AirtelCMSRequestParamBalInq
    {
        public string refid { get; set; }
        public int amount { get; set; }
        public string biller_id { get; set; }
        public string biller_name { get; set; }
        public string mobile_no { get; set; }
        public string datetime { get; set; }
    }
    public class AirtelCMSRequestParamBalDebit
    {
        public string refid { get; set; }
        public int amount { get; set; }
        public string biller_id { get; set; }
        public string biller_name { get; set; }
        public string mobile_no { get; set; }
        public string datetime { get; set; }
        public double commission { get; set; }
    }
    public class AirtelCMSBalDebitRequest
    {
        public string eventType { get; set; }
        public AirtelCMSRequestParamBalDebit param { get; set; }
    }
    public class AirtelCMSRequestParamLowBalInq
    {
        public string refid { get; set; }
        public int amount { get; set; }
        public string biller_id { get; set; }
        public string biller_name { get; set; }
        public string mobile_no { get; set; }
        public string datetime { get; set; }
        public int Status { get; set; }
        public string errormsg { get; set; }
    }
    public class AirtelCMSLowBalInqRequest
    {
        public string eventType { get; set; }
        public AirtelCMSRequestParamLowBalInq param { get; set; }
    }
    public class AirtelCMSPostingRequest
    {
        public string eventType { get; set; }
        public AirtelCMSParamPostingRequest param { get; set; }
    }
    public class AirtelCMSParamPostingRequest
    {
        public string refid { get; set; }
        public string utr {  get; set; }    
        public string biller_id { get; set; }
        public string biller_name { get; set; }
        public string mobile_no { get; set; }
        public string ackno { get; set; }
        public string unique_id { get; set; }
        public int Status { get; set; }
        public string datetime { get; set; } 
    }
    public class AirtelCMSResponse
    {
        public int status { get; set; }
        public string message { get; set; }
      
    }
}
