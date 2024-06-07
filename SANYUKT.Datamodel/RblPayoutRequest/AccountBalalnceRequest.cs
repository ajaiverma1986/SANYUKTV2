using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.RblPayoutRequest
{
    public class AccountBalalnceRequest
    {
        public GetAccountBalanceReq getAccountBalanceReq { get; set; }
    }
    public class Body
    {
        public string AcctId { get; set; }
    }

    public class GetAccountBalanceReq
    {
        public Header Header { get; set; }
        public Body Body { get; set; }
        public Signature1 Signature { get; set; }
    }

    public class Header
    {
        public string TranID { get; set; }
        public string Corp_ID { get; set; }
        public string Approver_ID { get; set; }
    }
    public class Signature1
    {
        public string Signature { get; set; }
    }

}
