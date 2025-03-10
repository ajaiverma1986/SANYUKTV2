using Microsoft.AspNetCore.Http;
using SANYUKT.Database;
using SANYUKT.Datamodel.Entities.SysModel;
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
    public class SysMgrRepository:BaseRepository
    {
        public readonly ISANYUKTDatabase _database=null;
        public SysMgrRepository() { 
            _database=new SANYUKTDatabase();
        }
        public async Task<long> CreateNewRole(CreateRoleRequest request, ISANYUKTServiceUser serviceUser)
        {

            long outputstr = 0;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("[AAC].CreateNewRole");
            _database.AddInParameter(dbCommand, "@RoleName", request.RoleName);
            _database.AddInParameter(dbCommand, "@RoleDescription", request.RoleDescription);
            _database.AddInParameter(dbCommand, "@CreatedBy", serviceUser.UserMasterID);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);

            return outputstr;

        }
        public async Task<List<GetRoleResponse>> GetAllRoles(GetRoleRequest request, ISANYUKTServiceUser serviceUser)
        {
            List<GetRoleResponse> response = new List<GetRoleResponse>();

            var dbCommand = _database.GetStoredProcCommand("[AAC].GetAllRoles");

            _database.AddInParameter(dbCommand, "@RoleName", request.RoleName);
            _database.AddInParameter(dbCommand, "@RoleID", request.RoleID);
            _database.AddInParameter(dbCommand, "@Status", request.Status);
           
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    GetRoleResponse row = new GetRoleResponse();

                    row.Status = GetInt32Value(dataReader, "Status").Value;
                    row.RoleID = GetInt32Value(dataReader, "RoleID").Value;
                    row.StatusName = GetStringValue(dataReader, "StatusName");
                    row.RoleDescription = GetStringValue(dataReader, "RoleDescription");
                    row.RoleName = GetStringValue(dataReader, "RoleName");
                   
                    response.Add(row);
                }
            }
            return response;
        }
        public async Task<string> GenerateServiceSessionCode(int ServiceId, ISANYUKTServiceUser serviceUser)
        {

            string outputstr = "";
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("usp_GenerateServiceSessionCode");
            _database.AddInParameter(dbCommand, "@serviceid", ServiceId);
            _database.AddInParameter(dbCommand, "@createdby", serviceUser.UserMasterID);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputString(dbCommand);
            return outputstr;

        }
        public async Task<string> GenerateServiceSessionID(int ServiceId, ISANYUKTServiceUser serviceUser)
        {

            string outputstr = "";
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("usp_GenerateServiceSessionID");
            _database.AddInParameter(dbCommand, "@serviceid", ServiceId);
            _database.AddInParameter(dbCommand, "@createdby", serviceUser.UserMasterID);
            _database.AddOutParameter(dbCommand, "@Out_ID", OUTPARAMETER_SIZE);

            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputString(dbCommand);
            return outputstr;

        }

        public async Task<OTPResponse> SendOTP(string mobileNumber, string OTP)
        {
            OTPResponse row = new OTPResponse();
            var dbCommand = _database.GetStoredProcCommand("usp_GenerateOTP");
            _database.AddInParameter(dbCommand, "@MobileNo", mobileNumber);
            _database.AddInParameter(dbCommand, "@OTP", OTP);
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                if (dataReader.Read())
                {

                    row.status = "Success";
                    row.response_message = "Otp Send Successfully";
                    row.OTPID = GetStringValue(dataReader, "OTPID");
                }
            }

            return row;
        }
        public async Task<SimpleResponse> ValidateOTP(string mobileNumber, string OTP)
        {
            SimpleResponse response = new SimpleResponse();
            int abc = 0;
            var dbCommand = _database.GetStoredProcCommand("usp_ValidateOTP");
            _database.AddInParameter(dbCommand, "@MobileNo", mobileNumber);
            _database.AddInParameter(dbCommand, "@OTP", OTP);
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {

                    abc = GetInt32Value(dataReader, "Result").Value;
                }
            }
            response.Result = abc;
            return response;
        }
    }
}
