using SANYUKT.Datamodel.Entities;
using SANYUKT.Datamodel.Entities.RblPayout;
using SANYUKT.Datamodel.Entities.Transactions;
using SANYUKT.Datamodel.Entities.Users;
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
        public async Task<List<TransactionDetailListResponse>> GetAllListTransactionDetail(TransactionDetailsRequest request, ISANYUKTServiceUser serviceUser)
        {
            List<TransactionDetailListResponse> response = new List<TransactionDetailListResponse>();
            response = await _repository.GetAllListTransactionDetail(request, serviceUser);
            return response;
        }
        public async Task<TransactionDetailListResponse> GetTransactionDetail(TransactionDetailsRequest request, ISANYUKTServiceUser serviceUser)
        {
            TransactionDetailListResponse resp = new TransactionDetailListResponse();
            List<TransactionDetailListResponse> response = new List<TransactionDetailListResponse>();
            response = await _repository.GetAllListTransactionDetail(request, serviceUser);
            if(response.Count > 0)
            {
                resp = response[0];
            }
            return resp;
        }
        public async Task<SimpleResponse> GetPayoutTransactionList(TransactionDetailsPayoutRequest request, ISANYUKTServiceUser serviceUser)
        {
           SimpleResponse response = new SimpleResponse();
            response = await _repository.GetAllPayoutTxnList(request, serviceUser);
            return response;
        }
        public async Task<SimpleResponse> AddNewPayinRequest(AddPaymentRequestRequest request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse resp = new SimpleResponse();
            resp = await _repository.AddNewPayinRequest(request, serviceUser);
            return resp;
        }
        public async Task<SimpleResponse> ApproveRejectPayinRequest(ApproveRejectPayinRequest request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse resp = new SimpleResponse();
            resp = await _repository.ApproveRejectPayinRequest(request, serviceUser);
            return resp;
        }
        public async Task<ListResponse> GetallPayinRequest(ListPayinRequestRequest request, ISANYUKTServiceUser serviceUser)
        {
            ListResponse response = new ListResponse();
            
            response = await _repository.GetallPayinRequest(request, serviceUser);
            return response;
        }
        public async Task<SimpleResponse> UpdatePayinRecieptFile(PayinRecieptRequest request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse resp = new SimpleResponse();
            resp.Result = await _repository.UpdatePayinRecieptFile(request, serviceUser);
            return resp;
        }
        public async Task<SimpleResponse> DocumentViewPayinRequest_Search(long RequestID, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            List<PayinRequestReciptListResponse> list = new List<PayinRequestReciptListResponse>();
            list = await _repository.GetAllfilePayinFiles(RequestID, serviceUser) ;

            FileManager fileManager = new FileManager();
            PayinRequestReciptDownloadResponse resp = new PayinRequestReciptDownloadResponse();


            foreach (PayinRequestReciptListResponse item in list)
            {
                if (item.RecieptFile != null && item.RecieptFile != "")
                {
                    resp.RequestID = item.RequestID;
                    resp.RecieptFile = item.RecieptFile;
                    resp.FileBytes = fileManager.ReadFileOther(item.RecieptFile, "Wallet");
                    resp.Base64String = Convert.ToBase64String(resp.FileBytes);
                    resp.MediaExtension = System.IO.Path.GetExtension(item.RecieptFile).ToLower();
                }
                else
                {
                    response.SetError("File not Exists");
                }


            }

            response.Result = resp;
            return response;
        }
    }
}
