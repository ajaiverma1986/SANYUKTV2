using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.Interfaces
{
    public interface ISANUKTLoggedInUser
    {
        String Email { get; set; }
        String DisplayName { get; set; }
        String UserToken { get; set; }
        // String SessionID { get; set; } //for backward compatibility with v1 of mPay
        String ApiToken { get; set; }
        String IPAddress { get; set; }
        Int64 UserMasterID { get; set; }
        List<String> RolePermissions { get; set; }
        String UserName { get; set; }
        //OrganizationConfigurationResponse OrganizationConfiguration { get; set; }
    }
}
