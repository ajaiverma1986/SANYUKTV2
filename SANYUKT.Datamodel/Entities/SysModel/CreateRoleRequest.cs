using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.Entities.SysModel
{
    public class CreateRoleRequest
    {
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
    }
    public class GetRoleRequest
    {
        public GetRoleRequest() {
            Status = 1;
        }
        public string RoleName { get; set; }
        public int? RoleID { get; set; }
        public int? Status { get; set; }
    }
    public class GetRoleResponse
    {
        public int RoleID { get; set;}
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
    }
    public class DocValidatorPanRequest
    {
      public string client_ref_num { get; set; }
      public string Pan { get; set; }
    }
    public class DocValidatorAadharRequest
    {
        public string client_ref_num { get; set; }
        public string aadhaar { get; set; }
    }
    public class DocValidatorViewModelRequest
    {
        public string DocumentNo { get; set; }
        public int veritype { get; set; }
    }
    public class PanDocumentValidResponse
    {
        public string pan { get; set; }
        public string pan_type { get; set; }
        public string fullname { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string gender { get; set; }
        public string aadhaar_number { get; set; }
        public bool aadhaar_linked { get; set; }
        public string dob { get; set; }
        public string address { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
    }
    public class OTPRequest
    {
        public string mobileno { get; set; }
    }

    public class OTPResponse
    {
        public OTPResponse()
        {
            status = "Error";
            response_message = "Please Try Again";
            OTPID = "";
        }
        public string status { get; set; }
        public string response_message { get; set; }
        public string OTPID { get; set; }
    }
    public class OTPValidateRequest
    {
        public string mobileno { get; set; }
        public string otp { get; set; }
    }
}
