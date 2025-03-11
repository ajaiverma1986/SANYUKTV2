using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Datamodel.Paysprint
{
    public class SpBaseResponse
    {
        public bool status { get; set; }
        public string response_code { get; set; }
        public string? message { get; set; }
        public object data { get; set; }    
    }
    public class FinoCustomerLimitResponse
    {
        public string limit { get; set; }
        public string mobile { get; set; }
    }
    public class FinoCustomerEkycResponse
    {
        public string ekyc_id { get; set; }
        public string stateresp { get; set; }
        public string mobile { get; set; }
    }
    public class FinoRegisterBenResponse
    {
        public string bene_id { get; set; }
        public string bankid { get; set; }
        public string bankname { get; set; }
        public string name { get; set; }
        public string accno { get; set; }
        public string ifsc  { get; set; }
        public string verified { get; set; }
        public string banktype { get; set; }
        public bool paytm { get; set; }
    }
   
    public class FinoTransactionResponse
    {
        public string ackno { get; set; }
        public string utr { get; set; }
        public string txn_status { get; set; }
        public string benename { get; set; }
        public string remarks { get; set; }
        public string customercharge { get; set; }
        public string gst { get; set; }
        public string tds { get; set; }
        public string  netcommission { get; set; }
        public string remitter { get; set; }
        public string account_number { get; set; }
        public string paysprint_share { get; set; }
        public string txn_amount { get; set; }
        public string balance { get; set; }
    }
    public class FinoTransactionStatusResponse
    {
        public string ackno { get; set; }
        public string utr { get; set; }
        public string amount { get; set; }
        public string referenceid { get; set; }
        public string account { get; set; }
        public string txn_status { get; set; }
        public string customercharge { get; set; }
        public string gst { get; set; }
        public string discount { get; set; }
        public string tds { get; set; }
        public string netcommission { get; set; }
        public string daterefunded  { get; set; }
        public string refundtxnid { get; set; }
    }
   
    public class FinoTransactionOTPResponse
    {
        public string stateresp { get; set; }
    }
    public class GetCustomerRequestView
    {
        public string Mobile { get; set; }
        public string TokenData { get; set; }
    }
    public class GetCustomerRequest
    {
        public string mobile { get; set; }
    }
    public class FinoEkycRequestView
    {
        public string Mobile { get; set; }
        public string TokenData { get; set; }
        public string AadharNo { get; set; }
        public string PidData { get; set; }
        public string AccessMode { get; set; }
        public int isIris { get; set; }
    }
    public class FinoEkycRequest
    {
        public string mobile { get; set; }
        public string aadhaar_number { get; set; }
        public string piddata { get; set; }
        public string accessmode { get; set; }
        public int is_iris { get; set; }
    }
    public class FinoRegCustomerRequestView
    {
        public string ekyc_id { get; set; }
        public string mobile { get; set; }
        public string otp { get; set; }
        public string stateresp { get; set; }
        public string TokenData { get; set; }
    }
    public class FinoRegCustomerRequest
    {
        public string ekyc_id { get; set; }
        public string mobile { get; set; }
        public string otp { get; set; }
        public string stateresp { get; set; }
    }
    public class AirtelRegCustomerRequestView
    {
        public string mobile { get; set; }
        public string otp { get; set; }
        public string stateresp { get; set; }
        public string data { get; set; }
        public string accessmode { get; set; }
        public int is_iris { get; set; }
        public string TokenData { get; set; }
    }
    public class AirtelRegCustomerRequest
    {
        public string mobile { get; set; }
        public string otp { get; set; }
        public string stateresp { get; set; }
        public string data { get; set; }
        public string accessmode { get; set; }
        public int is_iris { get; set; }
    }
    public class FinoRegBenRequestView
    {
        public string mobile { get; set; }
        public string benename { get; set; }
        public string bankid { get; set; }
        public string accno { get; set; }
        public string ifsccode { get; set; }
        public int verified { get; set; }
        public string TokenData { get; set; }
    }
    public class FinoRegBenRequest
    {
        public string mobile { get; set; }
        public string benename { get; set; }
        public string bankid { get; set; }
        public string accno { get; set; }
        public string ifsccode { get; set; }
        public int verified { get; set; }
        public string TokenData { get; set; }
    }
    public class FinoDeleteBenRequestView
    {
        public string mobile { get; set; }
        public string bene_id { get; set; }
        public string TokenData { get; set; }
    }
    public class FinoDeleteBenRequest
    {
        public string mobile { get; set; }
        public string bene_id { get; set; }
    }
    public class FinofetchBenRequestView
    {
        public string mobile { get; set; }
        public string beneid { get; set; }
        public string TokenData { get; set; }
    }
    public class FinofetchBenRequest
    {
        public string mobile { get; set; }
    }
    public class FinofetchBenbybenIDRequest
    {
        public string beneid { get; set; }
    }
    public class FinoTransactionRequestView
    {
        public string mobile { get; set; }
        public string accno { get; set; }
        public string bankid { get; set; }
        public string benename { get; set; }
        public string pincode { get; set; }
        public string address { get; set; }
        public string dob { get; set; }
        public string gst_state { get; set; }
        public string bene_id { get; set; }
        public string TokenData { get; set; }
    }
    
    public class FinoTransactionRequest
    {
        public string mobile { get; set; }
        public string accno { get; set; }
        public string bankid { get; set; }
        public string benename { get; set; }
        public string referenceid { get; set; }
        public string pincode { get; set; }
        public string address { get; set; }
        public string dob { get; set; }
        public string gst_state { get; set; }
        public string bene_id { get; set; }
    }
    public class FinoTransactionSendRequestView
    {
        public string mobile { get; set; }
        public string referenceid { get; set; }
        public string bene_id { get; set; }
        public string txntype { get; set; }
        public double amount { get; set; }
        public string TokenData { get; set; }
    }
    public class FinoTransactionSendRequest
    {
        public string mobile { get; set; }
        public string referenceid { get; set; }
        public string bene_id { get; set; }
        public string txntype { get; set; }
        public double amount { get; set; }
    }
    public class FinoTransactionFinalRequestView
    {
        public string mobile { get; set; }
        public string referenceid { get; set; }
        public string bene_id { get; set; }
        public string txntype { get; set; }
        public double amount { get; set; }
        public string otp { get; set; }
        public string stateresp { get; set; }
        public string TokenData { get; set; }
    }
    public class FinoTransactionFinalRequest
    {
        public string mobile { get; set; }
        public string referenceid { get; set; }
        public string bene_id { get; set; }
        public string txntype { get; set; }
        public double amount { get; set; }
        public string otp { get; set; }
        public string stateresp { get; set; }
    }
    public class FinoTransactionStatusRequestView
    {
        public string referenceid { get; set; }
        public string TokenData { get; set; }
    }
    public class FinoTransactionStatusRequest
    {
        public string referenceid { get; set; }

    }
    public class FinoRefundOtpRequestView
    {
        public string referenceid { get; set; }
        public string ackno { get; set; }
        public string TokenData { get; set; }
    }
    public class FinoRefundOtpRequest
    {
        public string referenceid { get; set; }
        public string ackno { get; set; }
    }
    public class FinoRefundRequestView
    {
        public string referenceid { get; set; }
        public string ackno { get; set; }
        public string otp { get; set; }
        public string TokenData { get; set; }
    }
    public class FinoRefundRequest
    {
        public string referenceid { get; set; }
        public string ackno { get; set; }
        public string otp { get; set; }
    }
    public class AirtelVerifyAadharRequestView
    {
        public string TokenData { get; set; }
        public string mobile { get; set; }
        public string aadhaar_no { get; set; }
       
    }
    public class AirtelVerifyAadharRequest
    {
        public string mobile { get; set; }
        public string aadhaar_no { get; set; }

    }
    public class CCGenerateOTPView
    {
        public string amount { get; set; }
        public string card_number { get; set; }
        public string mobile { get; set; }
        public string name { get; set; }
        public string network { get; set; }
        public string payee_name { get; set; }
        public string refid { get; set; }
        public string remarks { get; set; }
        public string TokenData { get; set; }
    }
    public class CCGenerateOTP
    {
        public string amount { get; set; }
        public string card_number { get; set; }
        public string mobile { get; set; }
        public string name { get; set; }
        public string network { get; set; }
        public string payee_name { get; set; }
        public string refid { get; set; }
        public string remarks { get; set; }
    }
}
