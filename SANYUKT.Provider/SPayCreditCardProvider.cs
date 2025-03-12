using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.Paysprint;
using SANYUKT.Provider.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SANYUKT.Provider
{
    public class SPayCreditCardProvider:BaseProvider
    {
        private readonly SysMgrProvider _sysprd = null;
        private readonly PaySprintIntegratorProvider _pro = null;
        public SPayCreditCardProvider() {
            _sysprd = new SysMgrProvider();
            _pro = new PaySprintIntegratorProvider();
        }
        public async Task<SpBaseResponse> GenerateOTP(CCGenerateOTPView request, ISANYUKTServiceUser serviceUser)
        {
            CCGenerateOTPView request1 = new CCGenerateOTPView();
            SpBaseResponse resp = new SpBaseResponse();
            request1.mobile = request.mobile;
            request1.refid = request.refid;
            request1.network = request.network;
            request1.remarks = request.remarks;
            request1.amount = request.amount;
            request1.card_number = request.card_number;
            request1.name = request.name;
            request1.payee_name = request.payee_name;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 27);
            return resp;
        }
        public async Task<SpBaseResponse> Billpayment(CCPaybillRequestView request, ISANYUKTServiceUser serviceUser)
        {
            CCPaybillRequest request1 = new CCPaybillRequest();
            SpBaseResponse resp = new SpBaseResponse();
            request1.mobile = request.mobile;
            request1.refid = request.refid;
            request1.network = request.network;
            request1.remarks = request.remarks;
            request1.amount = request.amount;
            request1.card_number = request.card_number;
            request1.name = request.name;
            request1.payee_name = request.payee_name;
            request1.otp = request.otp;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 28);
            return resp;
        }
        public async Task<SpBaseResponse> GetCCPaymentStatus(CCPaybillStatusRequestView request, ISANYUKTServiceUser serviceUser)
        {
            CCPaybillStatusRequest request1 = new CCPaybillStatusRequest();
            SpBaseResponse resp = new SpBaseResponse();
            request1.refid = request.refid;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 29);
            return resp;
        }
        public async Task<SpBaseResponse> GetCCRefundOTP(CCRefundOTPRequestView request, ISANYUKTServiceUser serviceUser)
        {
            CCRefundRequestOTP request1 = new CCRefundRequestOTP();
            SpBaseResponse resp = new SpBaseResponse();
            request1.refid = request.refid;
            request1.ackno = request.ackno;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 30);
            return resp;
        }
        public async Task<SpBaseResponse> GetCCRefund(CCRefundRequestView request, ISANYUKTServiceUser serviceUser)
        {
            CCRefundRequest request1 = new CCRefundRequest();
            SpBaseResponse resp = new SpBaseResponse();
            request1.refid = request.refid;
            request1.ackno = request.ackno;
            request1.otp = request.otp;
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 31);
            return resp;
        }
    }
}
