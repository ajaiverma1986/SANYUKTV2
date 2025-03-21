using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.Paysprint
{
    public class PaySprintCreateCustomerRequest
    {
        public string FirstName {  get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string AadharNo { get; set; }
        public string FinoKYCId { get; set; }
    }
    public class PaySprintCreateCustomerResponse
    {
        public long CustomerID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string AadharNo { get; set; }
        public string FinoKYCId { get; set; }
    }
}
