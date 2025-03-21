using SANYUKT.Database;
using SANYUKT.Datamodel.Entities.SysModel;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.Paysprint;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SANYUKT.Repository
{
    public class PaySprintRepository:BaseRepository
    {
        public readonly ISANYUKTDatabase _database = null;
        public PaySprintRepository()
        {
            _database = new SANYUKTDatabase();
        }
        public async Task<long> CreateNewCustomer(PaySprintCreateCustomerRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[CUST].CreateNewCustomer");
            _database.AddInParameter(dbCommand, "@FirstName", request.FirstName);
            _database.AddInParameter(dbCommand, "@MiddleName", request.MiddleName);
            _database.AddInParameter(dbCommand, "@LastName", request.LastName);
            _database.AddInParameter(dbCommand, "@MobileNo", request.MobileNo);
            _database.AddInParameter(dbCommand, "@AadharNo", request.AadharNo);
            _database.AddInParameter(dbCommand, "@FinoKYCId", request.FinoKYCId);
            _database.AddInParameter(dbCommand, "@CreatedBy", serviceUser.UserMasterID);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<PaySprintCreateCustomerResponse> GetAllCustomerDetail(string  MobileNo, ISANYUKTServiceUser serviceUser)
        {
            PaySprintCreateCustomerResponse response = new PaySprintCreateCustomerResponse();

            var dbCommand = _database.GetStoredProcCommand("[CUST].GetCustomerByMobileNo");

            _database.AddInParameter(dbCommand, "@MobileNo", MobileNo);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    response.CustomerID = GetInt32Value(dataReader, "CustomerID").Value;
                    response.AadharNo = GetStringValue(dataReader, "AadharNo");
                    response.FirstName = GetStringValue(dataReader, "FirstName");
                    response.MiddleName = GetStringValue(dataReader, "MiddleName");
                    response.LastName = GetStringValue(dataReader, "LastName");
                    response.MobileNo = GetStringValue(dataReader, "MobileNo");
                    response.FinoKYCId = GetStringValue(dataReader, "FinoKYCId");
                }
            }
            return response;
        }
    }
}
