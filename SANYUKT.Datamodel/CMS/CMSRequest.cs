using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.CMS
{
    public class AddCMSAgentRequest
    {
        public string agentcode { get; set; }
        public string agentname { get; set; }
        public int status { get; set; }
    }
    public class UpdateCMSAgentRequest
    {
        public string agentcode { get; set; }
        public string status { get; set; }
        public int ekycstatus { get; set; }
    }
    public class signonRequest
    {
        public string additionalParams { get; set; }
        public string latitude { get; set; }
        public int loginType { get; set; }
        public string longitude { get; set; }
        public string supermerchantId { get; set; }
        public string merchantId { get; set; }
        public string merchantPin { get; set; }
        public string mobileNumber { get; set; }
        public double amount { get; set; }
        public string superMerchantSkey { get; set; }
    }
    public class signonRequest1
    {
        public string additionalParams { get; set; }
        public string latitude { get; set; }
        public int loginType { get; set; }
        public string longitude { get; set; }
        public string merchantId { get; set; }
        public string merchantPin { get; set; }
        public string mobileNumber { get; set; }
        public double amount { get; set; }
    }
    public class WalletDebitRequest
    {
        public string transactionStatus { get; set; }
        public string fpTransactionId { get; set; }
        public string typeOfTransaction { get; set; }
        public string bcLoginId { get; set; }
        public string transactionTimestamp { get; set; }
        public string errorMessage { get; set; }
        public string remarks { get; set; }
        public double amount { get; set; }

        public string billerTransactionid { get; set; }
    }
    public class CMSDebitResponse
    {
        public CMSDebitResponse()
        {
            merchantTransactionId = "";
            status = "false";
            errorMessage = "";
        }
        public string merchantTransactionId { get; set; }
        public string status { get; set; }
        public string errorMessage { get; set; }
    }
    public class BioEKYCReq1
    {
        public string merchantLoginId { get; set; }
        public int primaryKeyId { get; set; }
        public string encodeFPTxnId { get; set; }
        public string requestRemarks { get; set; }
        public string deviceIMEI { get; set; }
        public CardnumberORUID cardnumberORUID { get; set; }
        public CaptureResponse captureResponse { get; set; }
    }
    public class MerchantAddress1
    {
        public string merchantAddress1 { get; set; }
        public string merchantAddress2 { get; set; }
        public int merchantState { get; set; }
        public string merchantCityName { get; set; }
        public string merchantDistrictName { get; set; }
        public string merchantPinCode { get; set; }
    }
    public class Merchant1
    {
        public string merchantLoginId { get; set; }
        public string merchantLoginPin { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        public string merchantPhoneNumber { get; set; }
        public MerchantAddress1 merchantAddress { get; set; }
        public string companyLegalName { get; set; }
        public string userType { get; set; }
        public int companyType { get; set; }
        public string emailId { get; set; }
        public string certificateOfIncorporationImage { get; set; }
        public MerchantKyc1 kyc { get; set; }
        public SettlementV2 settlementV1 { get; set; }
        public string tradeBusinessProof { get; set; }
        public string termsConditionCheck { get; set; }
        public string cancelledChequeImages { get; set; }
        public string physicalVerification { get; set; }
        public string videoKycWithLatLongData { get; set; }
        public MerchantKycAddressData1 merchantKycAddressData { get; set; }
    }
    public class MerchantKycAddressData1
    {
        public string shopAddress { get; set; }
        public string shopCity { get; set; }
        public string shopDistrict { get; set; }
        public int shopState { get; set; }
        public string shopPincode { get; set; }
        public string shopLatitude { get; set; }
        public string shopLongitude { get; set; }
        public string backgroundImageOfShop { get; set; }
    }
    public class SettlementV1
    {
        public string companyBankAccountNumber { get; set; }
        public string bankIfscCode { get; set; }
        public string companyBankName { get; set; }
        public string bankAccountName { get; set; }
    }
    public class SettlementV2
    {
        public string companyBankAccountNumber { get; set; }
        public string bankIfscCode { get; set; }
        public string companyBankName { get; set; }
        public string bankAccountName { get; set; }
    }
    public class MerchantAddress
    {
        public string merchantAddress1 { get; set; }
        public string merchantAddress2 { get; set; }
        public int merchantState { get; set; }
        public string merchantCityName { get; set; }
        public string merchantDistrictName { get; set; }
        public string merchantPinCode { get; set; }
    }
    public class MerchantKyc1
    {
        public string userPan { get; set; }
        public string aadhaarNumber { get; set; }
        public string gstinNumber { get; set; }
        public string companyOrShopPan { get; set; }
        public string merchantPanImage { get; set; }
        public string maskedAadharImage { get; set; }
        public string shopAndPanImage { get; set; }
    }
    public class MerchantKyc
    {
        public string userPan { get; set; }
        public string aadhaarNumber { get; set; }
        public string gstinNumber { get; set; }
        public string companyOrShopPan { get; set; }
        public string merchantPanImage { get; set; }
        public string maskedAadharImage { get; set; }
        public string shopAndPanImage { get; set; }
    }
   
    public class AddCMS
    {
        public string username { get; set; }
        public string password { get; set; }
        public string ipAddress { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public int supermerchantId { get; set; }
        public Merchant merchant { get; set; }
    }
    public class Merchant
    {
        public string merchantLoginId { get; set; }
        public string merchantLoginPin { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        public string merchantPhoneNumber { get; set; }
        public MerchantAddress merchantAddress { get; set; }
        public string companyLegalName { get; set; }
        public string userType { get; set; }
        public int companyType { get; set; }
        public string emailId { get; set; }
        public string certificateOfIncorporationImage { get; set; }
        public MerchantKyc kyc { get; set; }
        public SettlementV1 settlementV1 { get; set; }
        public string tradeBusinessProof { get; set; }
        public string termsConditionCheck { get; set; }
        public string cancelledChequeImages { get; set; }
        public string physicalVerification { get; set; }
        public string videoKycWithLatLongData { get; set; }
        public MerchantKycAddressData merchantKycAddressData { get; set; }
    }
    public class MerchantKycAddressData
    {
        public string shopAddress { get; set; }
        public string shopCity { get; set; }
        public string shopDistrict { get; set; }
        public int shopState { get; set; }
        public string shopPincode { get; set; }
        public string shopLatitude { get; set; }
        public string shopLongitude { get; set; }
        public string backgroundImageOfShop { get; set; }
    }
    public class AddCMS1
    {
        public string ipAddress { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public Merchant1 merchant { get; set; }
    }
    public class CaptureResponse
    {
        public string errCode { get; set; }
        public string errInfo { get; set; }
        public string fCount { get; set; }
        public string fType { get; set; }
        public string iCount { get; set; }
        public string iType { get; set; }
        public string pCount { get; set; }
        public string pType { get; set; }
        public string nmPoints { get; set; }
        public string qScore { get; set; }
        public string dpID { get; set; }
        public string rdsID { get; set; }
        public string rdsVer { get; set; }
        public string dc { get; set; }
        public string mi { get; set; }
        public string mc { get; set; }
        public string ci { get; set; }
        public string sessionKey { get; set; }
        public string hmac { get; set; }
        public string PidDatatype { get; set; }
        public string Piddata { get; set; }
    }
    public class CardnumberORUID
    {
        public string nationalBankIdentificationNumber { get; set; }
        public string indicatorforUID { get; set; }
        public string adhaarNumber { get; set; }
    }
    public class CashDropStatusCheck
    {
        public string merchantTransactionId { get; set; }
        public string hash { get; set; }
        public string bcLoginId { get; set; }
        public int superMerchantId { get; set; }
    }
    public class TransactionReciept
    {
        public string transactionid { get; set; }
        public DateTime txndatetime { get; set; }
        public DateTime createddate { get; set; }
        public string reference { get; set; }
        public string relatedreference { get; set; }
        public string Usercode { get; set; }
        public string Username { get; set; }
        public string status { get; set; }
        public decimal txnamount { get; set; }
        public string CorporateName { get; set; }
        public string CorporateAgent { get; set; }
        public string CorporateType { get; set; }
        public string BillerTransactionid { get; set; }
    }
    public class TransactionReportRequest
    {
        public string transactionid { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Usercode { get; set; }

    }
    public class Statuscheck
    {
        public int superMerchantId { get; set; }
        public string merchantLoginId { get; set; }
    }
    public class BioEKYCReq
    {
        public string superMerchantId { get; set; }
        public string merchantLoginId { get; set; }
        public int primaryKeyId { get; set; }
        public string encodeFPTxnId { get; set; }
        public string requestRemarks { get; set; }
        public CardnumberORUID cardnumberORUID { get; set; }
        public CaptureResponse captureResponse { get; set; }
    }
    public class validateOTPReq
    {
        public string superMerchantId { get; set; }
        public string merchantLoginId { get; set; }
        public string otp { get; set; }
        public int primaryKeyId { get; set; }
        public string encodeFPTxnId { get; set; }
    }
    public class CheckCashDropStatusRequest
    {
        public string AgentCode { get; set; }
        public string BillpayTxnId { get; set; }
    }
    public class validateOTPReq1
    {
        public string merchantLoginId { get; set; }
        public string otp { get; set; }
        public int primaryKeyId { get; set; }
        public string encodeFPTxnId { get; set; }
        public string deviceIMEI { get; set; }

    }
    public class resendOTPReq
    {
        public string superMerchantId { get; set; }
        public string merchantLoginId { get; set; }
        public int primaryKeyId { get; set; }
        public string encodeFPTxnId { get; set; }
    }
    public class resendOTPReq1
    {
        public string merchantLoginId { get; set; }
        public int primaryKeyId { get; set; }
        public string encodeFPTxnId { get; set; }
        public string deviceIMEI { get; set; }
    }
    public class MerchantOTPRequest
    {
        public string superMerchantId { get; set; }
        public string merchantLoginId { get; set; }
        public string transactionType { get; set; }
        public string mobileNumber { get; set; }
        public string aadharNumber { get; set; }
        public string panNumber { get; set; }
        public string matmSerialNumber { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }

    }
    public class MerchantOTPRequest1
    {
        public string merchantLoginId { get; set; }
        public string transactionType { get; set; }
        public string mobileNumber { get; set; }
        public string aadharNumber { get; set; }
        public string panNumber { get; set; }
        public string matmSerialNumber { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string deviceIMEI { get; set; }

    }
}
