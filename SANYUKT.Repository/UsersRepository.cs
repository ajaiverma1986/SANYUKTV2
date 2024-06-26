using SANYUKT.Database;
using SANYUKT.Datamodel.Common;
using SANYUKT.Datamodel.Entities.Application;
using SANYUKT.Datamodel.Entities.Users;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SANYUKT.Repository
{
    public class UsersRepository:BaseRepository
    {
        public readonly ISANYUKTDatabase _database = null;
        public UsersRepository() { 
            _database = new SANYUKTDatabase();
        }
        public async Task<UsersDetailsResponse> CheckAvailbleLimit(ISANYUKTServiceUser serviceUser)
        {
            UsersDetailsResponse response = new UsersDetailsResponse();
            var dbCommand = _database.GetStoredProcCommand("[USR].CheckAvailableBalance");
            _database.AddInParameter(dbCommand, "@UserMasterId", serviceUser.UserMasterID);
        
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                if (dataReader.Read())
                {
                    response.UserMasterId = GetInt64Value(dataReader, "UserMasterId").Value;
                    response.UserId = GetInt64Value(dataReader, "UserId").Value;
                    response.Usercode = GetStringValue(dataReader, "Usercode");
                    response.ThresoldLimit = GetDecimalValue(dataReader, "ThresoldLimit");
                    response.AvailableLimit = GetDecimalValue(dataReader, "AvailableLimit");
                    response.UserTypeId = GetInt32Value(dataReader, "UserTypeId").Value;
                    
                }
            }
            return response;
        }
    }
}
