using SANYUKT.Database;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.Entities.Activity;
using SANYUKT.Datamodel.Entities.RblPayout;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SANYUKT.Repository
{
    public class RblPayoutRepository:BaseRepository
    {
        private readonly ISANYUKTDatabase _database = null;
        public RblPayoutRepository()
        {
            _database = new SANYUKTDatabase();
        }
        public async Task<string> NewNonFinacialTransaction(NonFinancialTxnRequest request, ISANYUKTServiceUser serviceUser)
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
            _database.AddOutParameter(dbCommand, "@Out_ID", 100);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputString(dbCommand);
           
            return outputstr;

        }

    }
}
