using SANYUKT.Database;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SANYUKT.Repository.Shared
{
    public class CommonRepository:BaseRepository
    {
        private readonly ISANYUKTDatabase _database = null;
        public CommonRepository()
        {
            _database = new SANYUKTDatabase();
        }
        public async Task<SimpleResponse> APIRequestRecord_Log(ApiRequestLog request)
        {

            string outputstr = "";
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("usp_insertApiLog");
            _database.AddInParameter(dbCommand, "@apiname", request.apiname);
            _database.AddInParameter(dbCommand, "@plainrequest ", request.plainrequest);
            _database.AddInParameter(dbCommand, "@plainresponse", request.plainresponse);
            _database.AddInParameter(dbCommand, "@encryptedrequest", request.encryptedrequest);
            _database.AddInParameter(dbCommand, "@encryptedresponse", request.encryptedresponse);

            await _database.ExecuteNonQueryAsync(dbCommand);


            response.Result = outputstr;
            return response;

        }
    }
}
