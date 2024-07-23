using SANYUKT.Database;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.DTO.Request;
using SANYUKT.Datamodel.Entities.Authorization;
using SANYUKT.Datamodel.Entities.RblPayout;
using SANYUKT.Datamodel.Entities.Transactions;
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
            _database.AddInParameter(dbCommand, "@partnerid", request.partnerid);
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
            _database.AddInParameter(dbCommand, "@partnerid", request.partnerid);
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
        public async Task<List<TransactionDetailListResponse>> GetAllListTransactionDetail(TransactionDetailsRequest request)
        {
            List<TransactionDetailListResponse> response = new List<TransactionDetailListResponse> ();
            var dbCommand = _database.GetStoredProcCommand("[TXN].uspGetTransactionDetails");
            _database.AddInParameter(dbCommand, "@AgencyId", request.AgencyId);
            _database.AddInParameter(dbCommand, "@ServiceId", request.ServiceId);
            _database.AddInParameter(dbCommand, "@PartnerId", request.PartnerId);
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

    }
}
