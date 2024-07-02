using SANYUKT.Database;
using SANYUKT.Datamodel.Entities.Transactions;
using SANYUKT.Datamodel.Masters;
using SANYUKT.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SANYUKT.Repository
{
    public class MasterDataRepository:BaseRepository
    {
        public readonly ISANYUKTDatabase _database = null;
        public MasterDataRepository()
        {
            _database = new SANYUKTDatabase();
        }
        public async Task<ServiceListResponse> GetAllServcieList(int ServiceId)
        {
            ServiceListResponse response = null;
            var dbCommand = _database.GetStoredProcCommand("[MDM].GetAllServiceList");
            _database.AddInParameter(dbCommand, "@ServiceId", ServiceId);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                dataReader.Read();
                if (dataReader.HasRows)
                {
                    response = new ServiceListResponse();
                    response.ServiceId = GetInt32Value(dataReader, "ServiceId").Value;
                    response.ServiceTypeId = GetInt32Value(dataReader, "ServiceTypeId").Value;
                    response.ServiceCode = GetStringValue(dataReader, "ServiceCode");
                    response.ServiceName = GetStringValue(dataReader, "ServiceName");
                    response.ServcieIfsccode = GetStringValue(dataReader, "ServcieIfsccode");
                    response.ServiceAccountNo = GetStringValue(dataReader, "ServiceAccountNo");
                    response.ServiceAccName = GetStringValue(dataReader, "ServiceAccName");
                    response.ServiceMobileNo = GetStringValue(dataReader, "ServiceMobileNo");

                }
            }

            return response;
        }
    }
}
