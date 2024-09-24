using SANYUKT.Database;
using SANYUKT.Datamodel.Entities;
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
    public class ReportRepository:BaseRepository
    {
        public readonly ISANYUKTDatabase _database = null;
        public ReportRepository() { 
            _database = new SANYUKTDatabase();
        }
        public async Task<TransactionSummaryByUserResponse> GetTransactionSummaryByUserId(int userId,ISANYUKTServiceUser serviceUser)
        {
            TransactionSummaryByUserResponse response = new TransactionSummaryByUserResponse();

            var dbCommand = _database.GetStoredProcCommand("[TXN].GetTransactionSummaryByUserId");
            long? Useridnew = 0;

            if (serviceUser.UserTypeId == 3)
            {
                Useridnew = serviceUser.UserID;
            }
            else
            {
                Useridnew = userId;
            }

            _database.AddInParameter(dbCommand, "@UserId", Useridnew);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                if (dataReader.Read())
                {
                    response.TxnCount = GetInt32Value(dataReader, "TxnCount").Value;
                    response.ServiceName = GetStringValue(dataReader, "ServiceName");
                    response.RepType = GetStringValue(dataReader, "RepType");
                    response.TotalAmount = GetDecimalValue(dataReader, "TotalAmount") ?? 0;
                }
            }
            return response;
        }
        public async Task<SimpleResponse> GetDayBookByUserId(GetDayBookRequest Request, ISANYUKTServiceUser serviceUser)
        {
            List< GetDayBookResponse> response = new List<GetDayBookResponse>();
            SimpleResponse response1 = new SimpleResponse();

            var dbCommand = _database.GetStoredProcCommand("[TXN].GetTransactionSummaryByUserId");
            long? Useridnew = 0;

            if (serviceUser.UserTypeId == 3)
            {
                Useridnew = serviceUser.UserID;
            }
            else
            {
                Useridnew = Request.UserID;
            }

            _database.AddInParameter(dbCommand, "@UserID", Useridnew);
            _database.AddInParameter(dbCommand, "@FromDate", Request.FromDate);
            _database.AddInParameter(dbCommand, "@ToDate", Request.ToDate);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    GetDayBookResponse row = new GetDayBookResponse();
                    row.txnTotalcount = GetInt32Value(dataReader, "txnTotalcount").Value;
                    row.ServiceName = GetStringValue(dataReader, "ServiceName");
                    row.txnSuccescount = GetInt32Value(dataReader, "txnSuccescount").Value;
                    row.txntotalAmt = GetDecimalValue(dataReader, "txntotalAmt") ?? 0;
                    row.txnPendingcount = GetInt32Value(dataReader, "txnPendingcount").Value;
                    row.OrganisationName = GetStringValue(dataReader, "OrganisationName");
                    row.PartnerId = GetInt32Value(dataReader, "PartnerId").Value;
                    row.txnPendingAmt = GetDecimalValue(dataReader, "txnPendingAmt") ?? 0;
                    row.txnFailurecount = GetInt32Value(dataReader, "txnFailurecount").Value;
                    row.txnFailureAmt = GetDecimalValue(dataReader, "txnFailureAmt") ?? 0;
                    row.Surcharge = GetDecimalValue(dataReader, "Surcharge").Value;
                    row.Commission = GetDecimalValue(dataReader, "Commission") ?? 0;
                    response.Add(row);
                }
              
            }
            response1.Result = response;
            return response1;
        }
    }
}
