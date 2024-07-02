﻿using SANYUKT.Datamodel.Entities;
using SANYUKT.Datamodel.Entities.RblPayout;
using SANYUKT.Datamodel.Entities.Transactions;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Provider.Shared;
using SANYUKT.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SANYUKT.Provider
{
    public class TransactionProvider:BaseProvider
    {
        public readonly TransactionRepository _repository=null;
        public TransactionProvider()
        {
            _repository = new TransactionRepository();  
        }
        public async Task<string> NewNonFinacialTransaction(BaseTransactionRequest request,ISANYUKTServiceUser serviceUser)
        {
            string TransactionCode = "";
            TransactionCode=await _repository.NewNonFinacialTransaction(request, serviceUser);
            return TransactionCode;
        }
        public async Task<string> UpdateNonFinacialTransaction(UpdateNonfinacialRequest request, ISANYUKTServiceUser serviceUser)
        {
            string TransactionCode = "";
            TransactionCode = await _repository.UpdateNonFinacialTransaction(request, serviceUser);
            return TransactionCode;
        }
        public async Task<TransactionResponse> NewTransaction(NewTransactionRequest request, ISANYUKTServiceUser serviceUser)
        {
            string TransactionCode = "";
            TransactionResponse response =new TransactionResponse();
            TransactionCode = await _repository.NewTransaction(request, serviceUser);
            if(TransactionCode !="")
            {
                response.Transactioncode = TransactionCode;
            }
            else
            {
                response.SetError(ErrorCodes.SERVER_ERROR);
            }
            return response;
        }
        public async Task<SevicechargeResponse> GetServiceChargeDetail(SevicechargeRequest request)
        {
            SevicechargeResponse response = new SevicechargeResponse();
            response = await _repository.GetServiceChargeDetail(request);
            return response;
        }
        public async Task<TransactionResponse> NewTransactionUpdateStatus(UpdateTransactionStatusRequest request, ISANYUKTServiceUser serviceUser)
        {
            string TransactionCode = "";
            TransactionResponse response = new TransactionResponse();
            TransactionCode = await _repository.NewTransactionUpdateStatus(request, serviceUser);
            if (TransactionCode != "")
            {
                response.Transactioncode = TransactionCode;
            }
            else
            {
                response.SetError(ErrorCodes.SERVER_ERROR);
            }
            return response;
        }
    }
}
