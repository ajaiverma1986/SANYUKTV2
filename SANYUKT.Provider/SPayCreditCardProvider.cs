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
            resp = await _pro.GenericIntegrator(request1, request.TokenData, 14);
            return resp;
        }
    }
}
