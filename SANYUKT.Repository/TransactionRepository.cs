using SANYUKT.Database;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.DTO.Request;
using SANYUKT.Datamodel.Entities.Authorization;
using SANYUKT.Datamodel.Entities.RblPayout;
using SANYUKT.Datamodel.Entities.Transactions;
using SANYUKT.Datamodel.Entities.Users;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SANYUKT.Repository
{
    public class TransactionRepository:BaseRepository
    {
        private readonly ISANYUKTDatabase _database = null;
        public TransactionRepository() {
            _database = new SANYUKTDatabase();
        }
        public async Task<string> NewNonFinacialTransaction(BaseTransactionRequest request, ISANYUKTServiceUser serviceUser)
        {

            string outputstr = "";
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("usp_NewTransactionNonFinancial");
            _database.AddInParameter(dbCommand, "@agencyid", request.agencyid);
            _database.AddInParameter(dbCommand, "@serviceid", request.serviceid);
            _database.AddInParameter(dbCommand, "@partnerid", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@partnerretailorid", request.partnerretailorid);
            _database.AddInParameter(dbCommand, "@partnerreferenceno", request.partnerreferenceno);
            _database.AddInParameter(dbCommand, "@createdby", serviceUser.UserMasterID);
            _database.AddInParameter(dbCommand, "@txnplateform", request.TxnPlateForm);
            _database.AddInParameter(dbCommand, "@TxnType", request.TxnType);
            _database.AddOutParameter(dbCommand, "@Out_ID", 100);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputString(dbCommand);

            return outputstr;

        }
        public async Task<string> UpdateNonFinacialTransaction(UpdateNonfinacialRequest request, ISANYUKTServiceUser serviceUser)
        {

            string outputstr = "";
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[TXN].UspUpdateNonFinancialTxn");
            _database.AddInParameter(dbCommand, "@Txncode", request.Txncode);
            _database.AddInParameter(dbCommand, "@errorcode", request.errorcode);
            _database.AddInParameter(dbCommand, "@errorDescrtiopn", request.errorDescrtiopn);
            _database.AddOutParameter(dbCommand, "@Out_ID", 100);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputString(dbCommand);

            return outputstr;

        }
        public async Task<string> NewTransaction(NewTransactionRequest request, ISANYUKTServiceUser serviceUser)
        {

            string outputstr = "";
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[TXN].usp_NewTransaction");
            _database.AddInParameter(dbCommand, "@agencyid", request.agencyid);
            _database.AddInParameter(dbCommand, "@serviceid", request.serviceid);
            _database.AddInParameter(dbCommand, "@partnerid", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@partnertxnid", request.partnerreferenceno);
            _database.AddInParameter(dbCommand, "@partnerretailorid", request.partnerretailorid);
            _database.AddInParameter(dbCommand, "@description", request.description);
            _database.AddInParameter(dbCommand, "@createdby", serviceUser.UserMasterID);
            _database.AddInParameter(dbCommand, "@txnplateform", request.TxnPlateForm);
            _database.AddInParameter(dbCommand, "@txntype", request.TxnType);
            _database.AddInParameter(dbCommand, "@Amount", request.amount);
            _database.AddInParameter(dbCommand, "@Txnfee", request.txnFee);
            _database.AddInParameter(dbCommand, "@Margin", request.margincom);
            _database.AddOutParameter(dbCommand, "@Out_ID", 100);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputString(dbCommand);

            return outputstr;

        }
        public async Task<SevicechargeResponse> GetServiceChargeDetail(SevicechargeRequest request)
        {
            SevicechargeResponse response = null;
            var dbCommand = _database.GetStoredProcCommand("[CONFG].usp_getServiceChargediscount");
            _database.AddInParameter(dbCommand, "@AgencyID", request.AgencyId);
            _database.AddInParameter(dbCommand, "@ServiceID", request.ServiceId);
            _database.AddInParameter(dbCommand, "@amount", request.Amount);
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                dataReader.Read();
                if (dataReader.HasRows)
                {
                    response = new SevicechargeResponse();
                    response.CalculationType = GetInt32Value(dataReader, "CalculationType").Value;
                    response.SlabType = GetInt32Value(dataReader, "SlabType").Value;
                    response.CalculationValue = GetDecimalValue(dataReader, "CalculationValue").Value;
                    response.CalculationTypeName = GetStringValue(dataReader, "CalculationTypeName");
                   
                }
            }

            return response;
        }

        public async Task<SevicechargeResponse> GetServiceChargeDetailByPlan(SevicechargeByPlanRequest request)
        {
            SevicechargeResponse response = null;
            var dbCommand = _database.GetStoredProcCommand("[CONFG].usp_getServiceChargediscount");
            _database.AddInParameter(dbCommand, "@AgencyID", request.AgencyId);
            _database.AddInParameter(dbCommand, "@ServiceID", request.ServiceId);
            _database.AddInParameter(dbCommand, "@amount", request.Amount);
            _database.AddInParameter(dbCommand, "@PlanId", request.PlanId);
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                dataReader.Read();
                if (dataReader.HasRows)
                {
                    response = new SevicechargeResponse();
                    response.CalculationType = GetInt32Value(dataReader, "CalculationType").Value;
                    response.SlabType = GetInt32Value(dataReader, "SlabType").Value;
                    response.CalculationValue = GetDecimalValue(dataReader, "CalculationValue").Value;
                    response.CalculationTypeName = GetStringValue(dataReader, "CalculationTypeName");

                }
            }

            return response;
        }
        public async Task<string> NewTransactionUpdateStatus(UpdateTransactionStatusRequest request, ISANYUKTServiceUser serviceUser)
        {

            string outputstr = "";
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[TXN].usp_updatetransactionstatus");
            _database.AddInParameter(dbCommand, "@Transactioncode", request.Transactioncode);
            _database.AddInParameter(dbCommand, "@RefNo", request.RefNo);
            _database.AddInParameter(dbCommand, "@RelatedReference", request.RelatedReference);
            _database.AddInParameter(dbCommand, "@BankTxnDatetime", request.BankTxnDatetime);
            _database.AddInParameter(dbCommand, "@RefNo1", request.RefNo1);
            _database.AddInParameter(dbCommand, "@RefNo2", request.RefNo2);
            _database.AddInParameter(dbCommand, "@RefNo3", request.RefNo3);
            _database.AddInParameter(dbCommand, "@RefNo4", request.RefNo4);
            _database.AddInParameter(dbCommand, "@RefNo5", request.RefNo5);
            _database.AddInParameter(dbCommand, "@RefNo6", request.RefNo6);
            _database.AddInParameter(dbCommand, "@RefNo7", request.RefNo7);
            _database.AddInParameter(dbCommand, "@RefNo8", request.RefNo8);
            _database.AddInParameter(dbCommand, "@RefNo9", request.RefNo9);
            _database.AddInParameter(dbCommand, "@RefNo10", request.RefNo10);
            _database.AddInParameter(dbCommand, "@FailureReason", request.FailureReason);
            _database.AddInParameter(dbCommand, "@status", request.status);
            _database.AddInParameter(dbCommand, "@UpdatedBy", serviceUser.UserMasterID);
            _database.AddOutParameter(dbCommand, "@Out_ID", 100);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputString(dbCommand);

            return outputstr;

        }
        public async Task<List<TransactionDetailListResponse>> GetAllListTransactionDetail(TransactionDetailsRequest request, ISANYUKTServiceUser serviceUser)
        {
            List<TransactionDetailListResponse> response = new List<TransactionDetailListResponse> ();
            var dbCommand = _database.GetStoredProcCommand("[TXN].uspGetTransactionDetails");
            _database.AddInParameter(dbCommand, "@AgencyId", request.AgencyId);
            _database.AddInParameter(dbCommand, "@ServiceId", request.ServiceId);
            _database.AddInParameter(dbCommand, "@PartnerId", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@FromDate", request.FromDate);
            _database.AddInParameter(dbCommand, "@ToDate", request.ToDate );
            _database.AddInParameter(dbCommand, "@TxnType", request.TxnType);
            _database.AddInParameter(dbCommand, "@PartnerTransactionId", request.PartnerTransactionId);
            _database.AddInParameter(dbCommand, "@TransactionCode", request.TransactionCode);
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                dataReader.Read();
                if (dataReader.HasRows)
                {
                    TransactionDetailListResponse response2 = new TransactionDetailListResponse();
                    response2.TransactionId = GetInt64Value(dataReader, "TransactionId").Value;
                    response2.Transactioncode = GetStringValue(dataReader, "Transactioncode");
                    response2.PartnerId = GetInt64Value(dataReader, "PartnerId").Value;
                    response2.PartnerTxnId = GetStringValue(dataReader, "PartnerTxnId");
                    response2.ServiceId = GetInt32Value(dataReader, "ServiceId").Value;
                    response2.AgencyId = GetInt32Value(dataReader, "AgencyId").Value;
                    response2.PartnerRetailorId = GetStringValue(dataReader, "PartnerRetailorId");
                    response2.RefNo = GetStringValue(dataReader, "RefNo");
                    response2.RelatedReference = GetStringValue(dataReader, "RelatedReference");
                    response2.BankTxnDatetime = GetStringValue(dataReader, "BankTxnDatetime");
                    response2.Amount = GetDecimalValue(dataReader, "Amount") ?? 0;
                    response2.TxnFee = GetDecimalValue(dataReader, "TxnFee") ?? 0;
                    response2.RefNo1 = GetStringValue(dataReader, "RefNo1");
                    response2.RefNo2 = GetStringValue(dataReader, "RefNo2");
                    response2.RefNo3 = GetStringValue(dataReader, "RefNo3");
                    response2.RefNo4 = GetStringValue(dataReader, "RefNo4");
                    response2.RefNo5 = GetStringValue(dataReader, "RefNo5");
                    response2.RefNo6 = GetStringValue(dataReader, "RefNo6");
                    response2.RefNo7 = GetStringValue(dataReader, "RefNo7");
                    response2.RefNo8 = GetStringValue(dataReader, "RefNo8");
                    response2.RefNo9 = GetStringValue(dataReader, "RefNo9");
                    response2.RefNo10 = GetStringValue(dataReader, "RefNo10");
                    response2.FailureReason = GetStringValue(dataReader, "FailureReason");
                    response2.Status = GetInt32Value(dataReader, "Status").Value;
                    response2.PartnerName = GetStringValue(dataReader, "PartnerName");

                    response.Add(response2);
                }
               
            }

            return response;
        }

        public async Task<SimpleResponse> GetAllPayoutTxnList(TransactionDetailsPayoutRequest request, ISANYUKTServiceUser serviceUser)
        {
            List<TransactionDetailListPayoutResponse> response = new List<TransactionDetailListPayoutResponse>();
            SimpleResponse resp3=new SimpleResponse ();
            var dbCommand = _database.GetStoredProcCommand("[TXN].uspGetTransactionDetails");
            _database.AddInParameter(dbCommand, "@AgencyId", 1);
            _database.AddInParameter(dbCommand, "@ServiceId", 1);
            _database.AddInParameter(dbCommand, "@PartnerId", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@FromDate", request.FromDate);
            _database.AddInParameter(dbCommand, "@ToDate", request.ToDate);
            _database.AddInParameter(dbCommand, "@TxnType", request.TxnType);
            _database.AddInParameter(dbCommand, "@PartnerTransactionId", request.PartnerTransactionId);
            _database.AddInParameter(dbCommand, "@TransactionCode", request.TransactionCode);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    TransactionDetailListPayoutResponse response2 = new TransactionDetailListPayoutResponse();
                    response2.TransactionId = GetInt64Value(dataReader, "TransactionId").Value;
                    response2.Transactioncode = GetStringValue(dataReader, "Transactioncode");
                    response2.PartnerTxnId = GetStringValue(dataReader, "PartnerTxnId");
                    response2.RefNo = GetStringValue(dataReader, "RefNo");
                    response2.RelatedReference = GetStringValue(dataReader, "RelatedReference");
                    response2.BankTxnDatetime = GetStringValue(dataReader, "BankTxnDatetime");
                    response2.Amount = GetDecimalValue(dataReader, "Amount") ?? 0;
                    response2.FailureReason = GetStringValue(dataReader, "FailureReason");
                    response2.Status = GetInt32Value(dataReader, "Status").Value;
                    response2.PartnerName = GetStringValue(dataReader, "PartnerName");
                    response.Add(response2);
                }
            }
            resp3.Result = response;
            return resp3;
        }

        public async Task<SimpleResponse> AddNewPayinRequest(AddPaymentRequestRequest request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[TXN].AddNewPaymentRequest");
            _database.AddInParameter(dbCommand, "@UserId", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@PaymentChanelID", request.PaymentChanelID);
            _database.AddInParameter(dbCommand, "@PaymentModeId", request.PaymentModeId);
            _database.AddInParameter(dbCommand, "@Amount", request.Amount);
            _database.AddInParameter(dbCommand, "@Charge", request.Charge);
            _database.AddInParameter(dbCommand, "@DepositDate", request.DepositDate);
            _database.AddInParameter(dbCommand, "@RefNo1", request.RefNo1);
            _database.AddInParameter(dbCommand, "@RefNo2", request.RefNo2);
            _database.AddInParameter(dbCommand, "@Remarks", request.Remarks);
            _database.AddInParameter(dbCommand, "@CreatedBy", serviceUser.UserMasterID);
            _database.AddInParameter(dbCommand, "@OriginatorAccountId", request.OriginatorAccountId);
            _database.AddInParameter(dbCommand, "@BenficiaryAccountId", request.BenficiaryAccountId);
            _database.AddOutParameter(dbCommand, "@Out_ID", 100);

            await _database.ExecuteNonQueryAsync(dbCommand);

            response.Result = GetIDOutputLong(dbCommand);
           
            return response;

        }
        public async Task<SimpleResponse> ApproveRejectPayinRequest(ApproveRejectPayinRequest request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[TXN].ApproveRejectPayinRequest");
            _database.AddInParameter(dbCommand, "@Status", request.Status);
            _database.AddInParameter(dbCommand, "@RequestID", request.RequestID);
            _database.AddInParameter(dbCommand, "@RejectedReason", request.RejectedReason);
            _database.AddInParameter(dbCommand, "@UpdatedBy", serviceUser.UserMasterID);
            _database.AddOutParameter(dbCommand, "@Out_ID", 100);

            await _database.ExecuteNonQueryAsync(dbCommand);

            response.Result = GetIDOutputLong(dbCommand);

            return response;

        }
       
        public async Task<ListResponse> GetallPayinRequest(ListPayinRequestRequest request, ISANYUKTServiceUser serviceUser)
        {
            ListResponse response1 =new ListResponse();
            List< PayinRequestListResponse> response =new List<PayinRequestListResponse> ();
            var dbCommand = _database.GetStoredProcCommand("[TXN].usp_ListPayinRequest");
            _database.AddInParameter(dbCommand, "@PaymentChanelID", request.PaymentChanelID);
            _database.AddInParameter(dbCommand, "@PaymentModeId", request.PaymentModeId);
            _database.AddInParameter(dbCommand, "@Status", request.Status);
            _database.AddInParameter(dbCommand, "@UserId", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@FromDate", request.FromDate);
            _database.AddInParameter(dbCommand, "@ToDate", request.ToDate);
            _database.AddInParameter(dbCommand, "@PageNo", request.PageNo);
            _database.AddInParameter(dbCommand, "@PageSize", request.PageSize);
            _database.AddInParameter(dbCommand, "@OrderBy", request.OrderBy);
            _database.AddOutParameter(dbCommand, "@Out_TotalRec", 100);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
              while(dataReader.Read())
                {
                    
                        PayinRequestListResponse obj = new PayinRequestListResponse();
                        obj.RequestID = GetInt32Value(dataReader, "RequestID").Value;
                        obj.UserId = GetInt32Value(dataReader, "UserId").Value;
                        obj.PaymentChanelID = GetInt32Value(dataReader, "PaymentChanelID").Value;
                        obj.PaymentModeId = GetInt32Value(dataReader, "PaymentModeId").Value;
                        obj.OriginatorAccountId = GetInt32Value(dataReader, "OriginatorAccountId").Value;
                        obj.BenficiaryAccountId = GetInt32Value(dataReader, "BenficiaryAccountId").Value;
                        obj.Status = GetInt32Value(dataReader, "Status").Value;
                        obj.Amount = GetDecimalValue(dataReader, "Amount").Value;
                        obj.Charge = GetDecimalValue(dataReader, "Charge").Value;
                        obj.DepositDate = GetDateValue(dataReader, "DepositDate").Value;
                        obj.CreatedOn = GetDateValue(dataReader, "CreatedOn").Value;
                        obj.UpdatedOn = GetDateValue(dataReader, "UpdatedOn");
                        obj.StatusName = GetStringValue(dataReader, "StatusName");
                        obj.CreatedBy = GetStringValue(dataReader, "CreatedBy");
                        obj.UpdatedBy = GetStringValue(dataReader, "UpdatedBy");
                        obj.RefNo1 = GetStringValue(dataReader, "RefNo1");
                        obj.RefNo2 = GetStringValue(dataReader, "RefNo2");
                        obj.Remarks = GetStringValue(dataReader, "Remarks");
                        obj.RejectedReason = GetStringValue(dataReader, "RejectedReason");
                        obj.OriginatorBank = GetStringValue(dataReader, "OriginatorBank");
                        obj.OrgAccountName = GetStringValue(dataReader, "OrgAccountName");
                        obj.OrgAccountNo = GetStringValue(dataReader, "OrgAccountNo");
                        obj.OrgIfsccode = GetStringValue(dataReader, "OrgIfsccode");
                        obj.OrgBranchAddress = GetStringValue(dataReader, "OrgBranchAddress");
                        obj.BankName = GetStringValue(dataReader, "BankName");
                        obj.AccountName = GetStringValue(dataReader, "AccountName");
                        obj.AccountNo = GetStringValue(dataReader, "AccountNo");
                        obj.BranchName = GetStringValue(dataReader, "BranchName");
                        obj.Ifsccode = GetStringValue(dataReader, "Ifsccode");
                        obj.Branchcode = GetStringValue(dataReader, "Branchcode");
                        obj.BranchAddress = GetStringValue(dataReader, "BranchAddress");
                        obj.PaymentChanelName = GetStringValue(dataReader, "PaymentChanelName");
                        obj.PaymentModeName = GetStringValue(dataReader, "PaymentModeName");

                        response.Add(obj);
                    
                }

            }
            response1.SetPagingOutput(dbCommand);
            response1.CurrentPage = request.PageNo;
            response1.Result= response;
            return response1;
        }

        public async Task<List<PayinRequestReciptListResponse>> GetAllfilePayinFiles(long RequestId, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response1 = new SimpleResponse();
            List<PayinRequestReciptListResponse> response = new List<PayinRequestReciptListResponse>();
            var dbCommand = _database.GetStoredProcCommand("[TXN].usp_ListPayinRequestRecieptById");
            _database.AddInParameter(dbCommand, "@RequestID", RequestId);
            
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    PayinRequestReciptListResponse obj = new PayinRequestReciptListResponse();
                    obj.RequestID = GetInt32Value(dataReader, "RequestID").Value;
                    obj.RecieptFile = GetStringValue(dataReader, "RecieptFile");
                    response.Add(obj);
                }

            }
            
            return response;
        }
        public async Task<long> UpdatePayinRecieptFile(PayinRecieptRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[TXN].usp_UpdatePayinRequest");
            _database.AddInParameter(dbCommand, "@RequestID", request.RequestID);
            _database.AddInParameter(dbCommand, "@RecieptFile", request.RecieptFile);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<ListResponse> GetAllPayoutTransaction(TxnListRequest request, ISANYUKTServiceUser serviceUser)
        {
            ListResponse response1 = new ListResponse();
            List<AllTransactionListResponse> response = new List<AllTransactionListResponse>();
            var dbCommand = _database.GetStoredProcCommand("[TXN].uspGetPartnerTransactionDetails");
            _database.AddInParameter(dbCommand, "@TransactionCode", request.TransactionCode);
            _database.AddInParameter(dbCommand, "@PartnerId", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@FromDate", request.FromDate);
            _database.AddInParameter(dbCommand, "@ToDate", request.ToDate);
            _database.AddInParameter(dbCommand, "@TxnType", request.TxnType);
            _database.AddInParameter(dbCommand, "@PartnerTransactionId", request.PartnerTransactionId);
            _database.AddInParameter(dbCommand, "@Status", request.Status);
            _database.AddInParameter(dbCommand, "@PageNo", request.PageNo);
            _database.AddInParameter(dbCommand, "@PageSize", request.PageSize);
            _database.AddInParameter(dbCommand, "@OrderBy", request.OrderBy);
            _database.AddOutParameter(dbCommand, "@Out_TotalRec", 100);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {

                    AllTransactionListResponse obj = new AllTransactionListResponse();
                    obj.TransactionId = GetInt32Value(dataReader, "TransactionId").Value;
                    obj.PartnerId = GetInt32Value(dataReader, "PartnerId").Value;
                    obj.Amount = GetDecimalValue(dataReader, "Amount").Value;
                    obj.TxnFee = GetDecimalValue(dataReader, "TxnFee").Value;
                    obj.RelatedReference = GetStringValue(dataReader, "RelatedReference");
                    obj.Status = GetInt32Value(dataReader, "Status").Value;
                    obj.FailureReason = GetStringValue(dataReader, "FailureReason");
                    obj.PartnerName = GetStringValue(dataReader, "PartnerName");
                    obj.Transactioncode = GetStringValue(dataReader, "Transactioncode");
                    obj.PartnerTxnId = GetStringValue(dataReader, "PartnerTxnId");
                    obj.RefNo1 = GetStringValue(dataReader, "RefNo1");
                    obj.RefNo2 = GetStringValue(dataReader, "RefNo2");
                    obj.RefNo3 = GetStringValue(dataReader, "RefNo3");
                    obj.RefNo4 = GetStringValue(dataReader, "RefNo4");
                    obj.RefNo5 = GetStringValue(dataReader, "RefNo5");
                    obj.RefNo6 = GetStringValue(dataReader, "RefNo6");
                    obj.RefNo7 = GetStringValue(dataReader, "RefNo7");
                    obj.RefNo8 = GetStringValue(dataReader, "RefNo8");
                    obj.RefNo9 = GetStringValue(dataReader, "RefNo9");
                    obj.RefNo10 = GetStringValue(dataReader, "RefNo10");
                    obj.BankTxnDatetime = GetStringValue(dataReader, "BankTxnDatetime");
                    obj.RefNo = GetStringValue(dataReader, "RefNo");

                    response.Add(obj);

                }

            }
            response1.SetPagingOutput(dbCommand);
            response1.CurrentPage = request.PageNo;
            response1.Result = response;
            return response1;
        }

        public async Task<ListResponse> GetUSerStatement(UserStatementRequest request, ISANYUKTServiceUser serviceUser)
        {
            ListResponse response1 = new ListResponse();
            List<ListStatementResponse> response = new List<ListStatementResponse>();
            var dbCommand = _database.GetStoredProcCommand("[TXN].ListAccountStatement");
            _database.AddInParameter(dbCommand, "@FromDate", request.FromDate);
            _database.AddInParameter(dbCommand, "@ToDate", request.ToDate);
            _database.AddInParameter(dbCommand, "@UserId", serviceUser.UserID);
            _database.AddInParameter(dbCommand, "@PageNo", request.PageNo);
            _database.AddInParameter(dbCommand, "@PageSize", request.PageSize);
            _database.AddInParameter(dbCommand, "@OrderBy", request.OrderBy);
            _database.AddOutParameter(dbCommand, "@Out_TotalRec", 100);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {

                    ListStatementResponse obj = new ListStatementResponse();
                    obj.LedgerId = GetInt32Value(dataReader, "LedgerId").Value;
                    obj.LedgerDate = GetDateValue(dataReader, "LedgerDate");
                    obj.Amount = GetDecimalValue(dataReader, "Amount").Value;
                    obj.Limit = GetDecimalValue(dataReader, "Limit").Value;
                    obj.ReferenceId = GetStringValue(dataReader, "ReferenceId");
                    obj.LedgerTypeName = GetStringValue(dataReader, "LedgerTypeName");
                    obj.OrganisationName = GetStringValue(dataReader, "OrganisationName");
                    obj.Naration = GetStringValue(dataReader, "Naration");
                    obj.DbCr = GetStringValue(dataReader, "DbCr");
                  

                    response.Add(obj);

                }

            }
            response1.SetPagingOutput(dbCommand);
            response1.CurrentPage = request.PageNo;
            response1.Result = response;
            return response1;
        }

    }
}
