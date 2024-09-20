using SANYUKT.Database;
using SANYUKT.Datamodel.Entities;
using SANYUKT.Datamodel.Entities.Users;
using SANYUKT.Datamodel.Interfaces;
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
    }
}
