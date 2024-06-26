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
}
