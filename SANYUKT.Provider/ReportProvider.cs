using SANYUKT.Datamodel.Entities;
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
    public class ReportProvider:BaseProvider
    {
        public readonly ReportRepository repository = null;
        public ReportProvider() { 
            repository=new ReportRepository();
        }

        public async Task<SimpleResponse> GetTransactionSummaryByUserId(int userId, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response=new SimpleResponse ();
            response.Result=await repository.GetTransactionSummaryByUserId(userId, serviceUser);
            return response;
        }
        public async Task<SimpleResponse> GetDayBookByUserId(GetDayBookRequest Request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            response.Result = await repository.GetDayBookByUserId(Request, serviceUser);
            return response;
        }
    }
}
