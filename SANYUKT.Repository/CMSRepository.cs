using SANYUKT.Database;
using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.Shared;
using SANYUKT.Repository.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SANYUKT.Repository
{
    public class CMSRepository : BaseRepository
    {
        //private readonly IFIADatabase _database = null;
        private readonly ISANYUKTDatabase _database = null;

        public CMSRepository()
        {
            //_database = new FIADatabase();
            _database = new SANYUKTDatabase();
        }
        public async Task<SimpleResponse> ListEntityuser(string Usercode, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            UserResponse objMaster = new UserResponse();

            var dbCommand = _database.GetStoredProcCommand("[usp_listentity]");
            _database.AddInParameter(dbCommand, "@usercode", Usercode);
            _database.AddInParameter(dbCommand, "@userid", null);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    objMaster.UserCode = GetStringValue(dataReader, "usercode");
                    objMaster.UserName = GetStringValue(dataReader, "UserName");
                    objMaster.CompanyDistrictId = GetInt32Value(dataReader, "companydistrictid").Value;
                    objMaster.CompanyStateId = GetInt32Value(dataReader, "companystateid").Value;
                    objMaster.UserType = GetInt32Value(dataReader, "UserType");
                    //objMaster.ParentId = GetInt64Value(dataReader, "ParentId");
                    //objMaster.ParentCode = GetStringValue(dataReader, "ParentCode");
                    //objMaster.ParentName = GetStringValue(dataReader, "ParentName");
                    objMaster.EmailId = GetStringValue(dataReader, "EmailId");
                    objMaster.Mobile = GetStringValue(dataReader, "Mobile");
                    objMaster.Status = GetStringValue(dataReader, "Status");
                    objMaster.Reason = GetStringValue(dataReader, "Reason");
                    objMaster.Prefix = GetStringValue(dataReader, "Prefix");
                    objMaster.FirstName = GetStringValue(dataReader, "FirstName");
                    objMaster.MiddleName = GetStringValue(dataReader, "MiddleName");
                    objMaster.LastName = GetStringValue(dataReader, "LastName");
                    objMaster.FatherName = GetStringValue(dataReader, "FatherName");
                    objMaster.MotherName = GetStringValue(dataReader, "MotherName");
                    objMaster.DateOfBirth = GetDateValue(dataReader, "DateOfBirth");
                    objMaster.Gender = GetStringValue(dataReader, "Gender");
                    objMaster.MaritalStatus = GetStringValue(dataReader, "maritialStatus");
                    objMaster.Pancard = GetStringValue(dataReader, "Pancard");
                    objMaster.CompanyName = GetStringValue(dataReader, "CompanyName");
                    objMaster.CompanyAddress1 = GetStringValue(dataReader, "CompanyAddress1");
                    objMaster.CompanyAddress2 = GetStringValue(dataReader, "CompanyAddress2");
                    objMaster.CompanyAddress3 = GetStringValue(dataReader, "CompanyAddress3");
                    objMaster.CompanyCity = GetStringValue(dataReader, "CompanyCity");
                    objMaster.AccountNumber = GetStringValue(dataReader, "AccountNumber");
                    objMaster.BankName = GetStringValue(dataReader, "BankName");
                    //objMaster.KioskBankId = GetInt64Value(dataReader, "KioskBankId");
                    objMaster.IfscCode = GetStringValue(dataReader, "IfscCode");
                    objMaster.CompanyDistrict = GetStringValue(dataReader, "CompanyDistrict");
                    objMaster.CompanyState = GetStringValue(dataReader, "CompanyState");
                    objMaster.CompanyRegion = GetStringValue(dataReader, "CompanyRegion");
                    objMaster.CompanyPincode = GetStringValue(dataReader, "CompanyPincode");
                    objMaster.CompanyTelephone = GetStringValue(dataReader, "CompanyTelephone");
                    objMaster.ResidenceAddress1 = GetStringValue(dataReader, "ResidenceAddress1");
                    objMaster.ResidenceAddress2 = GetStringValue(dataReader, "ResidenceAddress2");
                    objMaster.ResidenceAddress3 = GetStringValue(dataReader, "ResidenceAddress3");
                    objMaster.ResidenceCity = GetStringValue(dataReader, "ResidenceCity");
                    objMaster.Spouse = GetStringValue(dataReader, "Spouse");
                    objMaster.AadharCard = GetStringValue(dataReader, "AadharCard");
                    objMaster.ResidenceDistrict = GetStringValue(dataReader, "ResidenceDistrict");
                    objMaster.ResidenceState = GetStringValue(dataReader, "ResidenceState");
                    objMaster.ResidenceRegion = GetStringValue(dataReader, "ResidenceRegion");
                    objMaster.ResidencePincode = GetStringValue(dataReader, "ResidencePincode");
                    objMaster.ResidenceTelephone = GetStringValue(dataReader, "ResidenceTelephone");
                    objMaster.MarginTypeName = GetStringValue(dataReader, "MarginTypeName");
                    objMaster.AccountType = GetStringValue(dataReader, "AccountType");

                }
                response.Result = objMaster;
                return response;
            }
        }


        public async Task<SimpleResponse> CreateCMSAgent(AddCMSAgentRequest request, IFIAServiceUser serviceUser)
        {
            long outputstr;
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("USP_CreateCMSAgent");
            _database.AddInParameter(dbCommand, "@agentcode", request.agentcode);
            _database.AddInParameter(dbCommand, "@agentname ", request.agentname);
            _database.AddInParameter(dbCommand, "@status ", request.status);
            _database.AddOutParameter(dbCommand, "@Out_ID", 100);
            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputLong(dbCommand);
            if (outputstr == -1)
            {
                response.SetError("please try again");
            }
            else
            {
                response.Result = outputstr;
            }
            return response;

        }

        public async Task<SimpleResponse> UpdateCMSAgent(UpdateCMSAgentRequest request, IFIAServiceUser serviceUser)
        {
            string outputstr = "";
            SimpleResponse response = new SimpleResponse();
            var dbCommand = _database.GetStoredProcCommand("USP_UpdateCMSSAgent");
            _database.AddInParameter(dbCommand, "@agentcode", request.agentcode);
            _database.AddInParameter(dbCommand, "@status ", request.status);
            _database.AddInParameter(dbCommand, "@EKYCstatus ", request.ekycstatus);
            await _database.ExecuteNonQueryAsync(dbCommand);

            outputstr = GetIDOutputString(dbCommand);
            if (outputstr == "")
            {
                response.SetError("please try again");
            }
            else
            {
                response.Result = outputstr;
            }
            return response;


        }
        public async Task<string> GenerateSessioncode(string usercode, string RequestType, string ServiceID, string AgencyID)
        {
            string output = "";
            try
            {
                //Deduct Premium Amount
                var dbCommand = _database.GetStoredProcCommand("USP_GetServiceSessionRequestCode");
                _database.AddInParameter(dbCommand, "@usercode", usercode);
                _database.AddInParameter(dbCommand, "@RequestType", RequestType);
                _database.AddInParameter(dbCommand, "@ServiceID", ServiceID);
                _database.AddInParameter(dbCommand, "@AgencyID", AgencyID);
                _database.AddOutParameter(dbCommand, "@RequestCode", 100);

                await _database.ExecuteNonQueryAsync(dbCommand);
                output = GetIDOutputStringOther(dbCommand);


            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
        }
        public async Task<decimal> CheckAvailbleLimit(string Usercode)
        {
            SimpleResponse response = new SimpleResponse();
            decimal TotalLimit = 0;

            var dbCommand = _database.GetStoredProcCommand("usp_getavaillimitBycode");
            _database.AddInParameter(dbCommand, "@Usercode", Usercode);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {

                while (dataReader.Read())
                {
                    TotalLimit = GetDecimalValue(dataReader, "AvailLimit") ?? 0;
                }
            }
            return TotalLimit;
        }
        public async Task<decimal> CheckBondLimit(string Usercode)
        {
            SimpleResponse response = new SimpleResponse();
            decimal TotalLimit = 0;

            var dbCommand = _database.GetStoredProcCommand("usp_getBondLimitBycode");
            _database.AddInParameter(dbCommand, "@Usercode", Usercode);

            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {

                while (dataReader.Read())
                {
                    TotalLimit = GetDecimalValue(dataReader, "AvailLimit") ?? 0;
                }
            }
            return TotalLimit;
        }
        public async Task<long> GetTxnByInfo1(string info1)
        {
            SimpleResponse response = new SimpleResponse();
            long ServiceID = 0;
            var dbCommand = _database.GetStoredProcCommand("usp_gettxnSBIGByInfo1");
            _database.AddInParameter(dbCommand, "@info1", info1);
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    ServiceID = GetInt32Value(dataReader, "transactionid") ?? 0;
                }
            }
            return ServiceID;
        }
        public async Task<SimpleResponse> TransactionReciept(string TransactionID)
        {
            SimpleResponse response = new SimpleResponse();
            TransactionReciept obj = new TransactionReciept();
            string[] remarra = null;
            var dbCommand = _database.GetStoredProcCommand("usp_GetTransactionDetailByTransactionID");
            _database.AddInParameter(dbCommand, "@transactionid", TransactionID);
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    obj.createddate = GetDateValue(dataReader, "createddate").Value;
                    obj.Username = GetStringValue(dataReader, "Username");
                    obj.Usercode = GetStringValue(dataReader, "Usercode");
                    obj.txnamount = GetDecimalValue(dataReader, "txnamount").Value;
                    obj.status = GetStringValue(dataReader, "status");
                    obj.txndatetime = GetDateValue(dataReader, "txndatetime").Value;
                    obj.relatedreference = GetStringValue(dataReader, "relatedreference");
                    obj.reference = GetStringValue(dataReader, "reference");
                    obj.transactionid = GetStringValue(dataReader, "transactionid");
                    obj.BillerTransactionid = GetStringValue(dataReader, "BillerTransactionid");
                    if (!string.IsNullOrEmpty(GetStringValue(dataReader, "reason")))
                    {
                        if (GetStringValue(dataReader, "reason").ToString().Contains(","))
                        {
                            if (GetStringValue(dataReader, "reason").ToString() != "Transaction Initiated")
                            {
                                remarra = GetStringValue(dataReader, "reason").Split(",");
                                if (remarra.Length == 2)
                                {
                                    obj.CorporateName = remarra[0].ToString();
                                    obj.CorporateAgent = remarra[1].ToString();
                                }
                                else
                                {
                                    obj.CorporateName = remarra[0].ToString();
                                    obj.CorporateAgent = remarra[1].ToString();
                                    obj.CorporateType = remarra[2].ToString();
                                }


                            }
                        }


                    }

                }
            }
            response.Result = obj;
            return response;
        }
        public async Task<SimpleResponse> TransactionReport(TransactionReportRequest Request)
        {
            SimpleResponse response = new SimpleResponse();
            List<TransactionReciept> objm = new List<TransactionReciept>();
            var dbCommand = _database.GetStoredProcCommand("usp_listtransactionbyfilterByAgent");
            _database.AddInParameter(dbCommand, "@agencyid", 26);
            _database.AddInParameter(dbCommand, "@Usercode", Request.Usercode);
            _database.AddInParameter(dbCommand, "@FromDate", Request.FromDate);
            _database.AddInParameter(dbCommand, "@ToDate", Request.ToDate);
            _database.AddInParameter(dbCommand, "@transactionid", Request.transactionid);
            using (var dataReader = await _database.ExecuteReaderAsync(dbCommand))
            {
                while (dataReader.Read())
                {
                    TransactionReciept obj = new TransactionReciept();
                    obj.createddate = GetDateValue(dataReader, "createddate").Value;
                    obj.Username = GetStringValue(dataReader, "username");
                    obj.Usercode = GetStringValue(dataReader, "usercode");
                    obj.txnamount = GetDecimalValue(dataReader, "txnamount").Value;
                    obj.status = GetStringValue(dataReader, "status");
                    obj.txndatetime = GetDateValue(dataReader, "txndatetime").Value;
                    obj.relatedreference = GetStringValue(dataReader, "relatedreference");
                    obj.reference = GetStringValue(dataReader, "reference");
                    obj.transactionid = GetStringValue(dataReader, "transactionid");
                    obj.BillerTransactionid = GetStringValue(dataReader, "BillerTransactionid");
                    objm.Add(obj);
                }

            }
            response.Result = objm;
            return response;
        }
        public async Task<CMSDebitResponse> WalletDebit(WalletDebitRequest request)
        {
            CMSDebitResponse response = new CMSDebitResponse();

            if (string.IsNullOrEmpty(request.bcLoginId))
            {
                response.errorMessage = "BC Login ID is Required";
                return response;
            }

            if (string.IsNullOrEmpty(request.fpTransactionId))
            {
                response.errorMessage = "FP Transaction Id is Required";
                return response;
            }

            if (string.IsNullOrEmpty(request.transactionStatus))
            {
                response.errorMessage = "Transaction Status is Required";
                return response;
            }

            if (string.IsNullOrEmpty(request.typeOfTransaction))
            {
                response.errorMessage = "Transaction Type is Required";
                return response;
            }

            if (request.amount == 0)
            {
                response.errorMessage = "Please Provide the Transaction Amount";
                return response;
            }



            if (request.transactionStatus == "I")
            {
                decimal TotalLimit = await CheckAvailbleLimit(request.bcLoginId);
                decimal BondLimit = await CheckBondLimit(request.bcLoginId);

                if (TotalLimit <= 0)
                {
                    response.errorMessage = "Insufficient Limit";
                    return response;
                }
                if (TotalLimit < Convert.ToDecimal(request.amount) + BondLimit)
                {
                    response.errorMessage = "Insufficient Limit";
                    return response;
                }

                long istxnid = await GetTxnByInfo1(request.fpTransactionId);
                if (istxnid > 0)
                {
                    response.errorMessage = "Transaction already exists with same Transaction ID";
                    return response;
                }
            }

            try
            {
                //Deduct Premium Amount
                var dbCommand = _database.GetStoredProcCommand("usp_CMSWalletDebit");
                _database.AddInParameter(dbCommand, "@usercode", request.bcLoginId);
                _database.AddInParameter(dbCommand, "@Refno", request.fpTransactionId);
                _database.AddInParameter(dbCommand, "@txnamount", request.amount);
                _database.AddInParameter(dbCommand, "@transactiondate", request.transactionTimestamp);
                _database.AddInParameter(dbCommand, "@reason", request.remarks);
                _database.AddInParameter(dbCommand, "@BillerTransactionid", request.billerTransactionid);
                _database.AddInParameter(dbCommand, "@TxnStatus", request.transactionStatus);

                _database.AddOutParameter(dbCommand, "@Out_ID", 100);

                await _database.ExecuteNonQueryAsync(dbCommand);
                long output = GetIDOutputInt(dbCommand);

                if (output == 0)
                {
                    response.errorMessage = "Failed";
                    return response;
                }
                else
                {
                    response.merchantTransactionId = output.ToString();
                    response.status = "true";
                    response.errorMessage = "SUCCESS";
                }

            }
            catch (Exception ex)
            {
                response.errorMessage = ex.Message;

                return response;
            }
            return response;
        }

    }
}
