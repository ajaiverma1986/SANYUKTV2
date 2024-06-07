using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.RblPayoutResponse
{
    public class BalAmt
    {
        public string amountValue { get; set; }
        public CurrencyCode currencyCode { get; set; }
    }

    public class Body
    {
        public BalAmt BalAmt { get; set; }
    }

    public class CurrencyCode
    {
    }

    public class GetAccountBalanceRes
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
        public string Status { get; set; }
        public string Error_Cde { get; set; }
        public string Error_Desc { get; set; }
    }

    public class BalanceResponse
    {
        public GetAccountBalanceRes getAccountBalanceRes { get; set; }
    }

    public class Signature1
    {
        public string Signature { get; set; }
    }
}
