using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SANYUKT.Provider.Shared
{
    public class CommonProvider:BaseProvider
    {
        private readonly CommonRepository repository = null;
        public CommonProvider()
        {
            repository = new CommonRepository();
        }

        public async Task<SimpleResponse> TransactionHook(string apiname, string encryptedrequest, string encryptedresponse, string plainrequest, string plainresponse)
        {
            ApiRequestLog requestLog = new ApiRequestLog();
            requestLog.apiname = apiname;
            requestLog.encryptedrequest = encryptedrequest;
            requestLog.encryptedresponse = encryptedresponse;
            requestLog.plainrequest = plainrequest;
            requestLog.plainresponse = plainresponse;
            return await repository.APIRequestRecord_Log(requestLog);
        }
    }
}
