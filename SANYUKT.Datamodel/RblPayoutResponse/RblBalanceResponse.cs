using SANYUKT.Datamodel.RblPayoutRequest;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.RblPayoutResponse
{

    public class GetSinglePaymentStatusCorpRes
    {
        public Header Header { get; set; }
        public Signature Signature { get; set; }
    }

    public class Header
    {
        public string TranID { get; set; }
        public string Corp_ID { get; set; }
        public string Maker_ID { get; set; }
        public string Checker_ID { get; set; }
        public string Approver_ID { get; set; }
        public string Status { get; set; }
        public string Error_Cde { get; set; }
        public string Error_Desc { get; set; }
    }

    public class GetsinglePaymentReponse
    {
        public GetSinglePaymentStatusCorpRes get_Single_Payment_Status_Corp_Res { get; set; }

    }

    public class Signature
    {
        public string Signature1 { get; set; }
    }
    public class RblResponse
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public string ErrorCode { get; set; }
        public string ErorrDescription { get; set; }
        public string ChanelPartnerRefNo { get; set; }

    }

}
