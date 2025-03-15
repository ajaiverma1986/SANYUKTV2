using SANYUKT.Database;
using SANYUKT.Datamodel.CMS;
using SANYUKT.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SANYUKT.Repository
{
    public class AirtelCMSRepository:BaseRepository
    {
        private readonly ISANYUKTDatabase _database = null;
        public AirtelCMSRepository() {
            _database = new SANYUKTDatabase();
        }
        public async Task<AirtelCMSResponse> WalletDebit(AirtelCMSBalanceInqRequest request)
        {
            AirtelCMSResponse response = new AirtelCMSResponse();

            if (string.IsNullOrEmpty(request.eventType))
            {
                response.message = "Event is Required";
                response.status = 400;
                return response;
            }
            if (request.param.amount > 0)
            {
                response.message = "Amount is  Required";
                response.status = 400;
                return response;
            }

            if (string.IsNullOrEmpty(request.param.biller_id))
            {
                response.message = "biller_id is  Required";
                response.status = 400;
                return response;
            }

            if (string.IsNullOrEmpty(request.param.biller_name))
            {
                response.message = "biller_name is  Required";
                response.status = 400;
                return response;
            }

            if (request.param.mobile_no == "")
            {
                response.message = "biller_name is  Required";
                response.status = 400;
                return response;
            }
            if (request.param.datetime == "")
            {
                response.message = "datetime is  Required";
                response.status = 400;
                return response;
            }

            try
            {
                //Deduct Premium Amount
                var dbCommand = _database.GetStoredProcCommand("usp_CMSWalletDebit");
                //_database.AddInParameter(dbCommand, "@usercode", request.bcLoginId);
                //_database.AddInParameter(dbCommand, "@Refno", request.fpTransactionId);
                //_database.AddInParameter(dbCommand, "@txnamount", request.amount);
                //_database.AddInParameter(dbCommand, "@transactiondate", request.transactionTimestamp);
                //_database.AddInParameter(dbCommand, "@reason", request.remarks);
                //_database.AddInParameter(dbCommand, "@BillerTransactionid", request.billerTransactionid);
                //_database.AddInParameter(dbCommand, "@TxnStatus", request.transactionStatus);

                //_database.AddOutParameter(dbCommand, "@Out_ID", 100);

                await _database.ExecuteNonQueryAsync(dbCommand);
                long output = GetIDOutputInt(dbCommand);

                if (output == 0)
                {
                    response.message = "Failed";
                    response.status = 400;
                    return response;
                }
                else
                {
                    response.status = 200;
                    response.message = "Transaction completed successfully";
                }

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.status = 400;
                return response;
            }
            return response;
        }
    }
}
