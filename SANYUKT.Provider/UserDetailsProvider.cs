using SANYUKT.Commonlib.Utility;
using SANYUKT.Datamodel.DTO.Request;
using SANYUKT.Datamodel.Entities.Authorization;
using SANYUKT.Datamodel.Entities.Transactions;
using SANYUKT.Datamodel.Entities.Users;
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
    public class UserDetailsProvider:BaseProvider
    {
        public readonly UsersRepository _repository = null;
        public UserDetailsProvider() { 
            _repository = new UsersRepository();
        }

        public async Task<bool> CheckAvailableBalance(decimal Amount,decimal txnFee, ISANYUKTServiceUser serviceUser)
        {
            UsersDetailsResponse response=new UsersDetailsResponse ();
           bool avail=false;
            response = await _repository.CheckAvailbleLimit(serviceUser);
            if (response==null)
            {
                avail = true;
            }
            if(response.AvailableLimit<=0)
            {
                avail = true;
            }
            else if(response.AvailableLimit<Amount)
            {
                avail = true;
            }
            else if (response.AvailableLimit<=response.ThresoldLimit)
            {
                avail = true;
            }
            else if (response.AvailableLimit<Amount+response.ThresoldLimit)
            {
                avail=true;
            }
            else
            {
                avail = false;
            }
            return avail;
        }
        public async Task<SimpleResponse> CheckBalalnce( ISANYUKTServiceUser serviceUser)
        {
            UsersDetailsResponse response = new UsersDetailsResponse();
            SimpleResponse response1=new SimpleResponse ();
          
            response1.Result = await _repository.CheckPartnerAvailbleLimit(serviceUser);
            return response1;
        }
        public async Task<long> AddNewBenficiary(AddBenficiaryRequest request, ISANYUKTServiceUser serviceUser)
        {
            long outputresponse = 0;
          
            outputresponse = await _repository.AddNewBenficiary(request,serviceUser);
            
            return outputresponse;
        }
        public async Task<List<BenficiaryResponse>> GetAllBenficiary(ListBenficaryRequest request, ISANYUKTServiceUser serviceUser)
        {
            return await _repository.GetAllBenficiary(request, serviceUser);
        }
        public async Task<List<ApplicationListResponse>> Getallapplication(ISANYUKTServiceUser serviceUser)
        {
            return await _repository.Getallapplication( serviceUser);
        }
        public async Task<BenficiaryResponse> GetBenficiaryDetailsByID(long BenFiciaryId)
        {
            return await _repository.GetBenficiaryDetailsByID(BenFiciaryId);
        }
        public async Task<long> ChangeBenficairyStatus(BenficaryChangeStatusRequest request, ISANYUKTServiceUser serviceUser)
        {
            long outputresponse = 0;

            outputresponse = await _repository.ChangeBenficairyStatus(request, serviceUser);

            return outputresponse;
        }
        public async Task<long> CreateNewUserRequest(CreateUserWithlogoRequest request, ISANYUKTServiceUser serviceUser)
        {
            long outputresponse = 0;
            CreateUserRequest request1 =new CreateUserRequest();
            request1.FirstName=request.FirstName;
            request1.LastName=request.LastName;
            request1.MiddleName=request.MiddleName;
            request1.UserTypeId =request.UserTypeId;
            request1.EmailId=request.EmailId;
            request1.MobileNo=request.MobileNo;
            
            outputresponse = await _repository.CreateNewUserRequest(request1, serviceUser);

            return outputresponse;
        }
        public async Task<long> UpdateUserOrgLogo(UploadOrgLogo request,string Filename, ISANYUKTServiceUser serviceUser)
        {
            long outputresponse = 0;
            UploadLogoRequest request1 = new UploadLogoRequest();
            request1.Logourl = Filename;
            request1.UserId = request.UserId;
          

            outputresponse = await _repository.UpdateUserLogo(request1, serviceUser);

            return outputresponse;
        }
        public async Task<long> AddOriginatorAccounts(CreateOriginatorAccountRequest request, ISANYUKTServiceUser serviceUser)
        {
            long outputresponse = 0;
            
            outputresponse = await _repository.AddOriginatorAccounts(request, serviceUser);

            return outputresponse;
        }
        public async Task<SimpleResponse> GetallOriginatorsAccount(ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();

            response.Result= await _repository.GetallOriginatorsAccount( serviceUser);
            return response;
        }
        public async Task<List<OriginatorListAccountResponse>> GetallOriginatorsAccountByID(long AccountID, ISANYUKTServiceUser serviceUser)
        {
            List<OriginatorListAccountResponse> response = new List<OriginatorListAccountResponse>();

            response = await _repository.GetallOriginatorsAccountByID(AccountID,serviceUser);
            return response;
        }
        public async Task<UserAccountsChecueFileResponse> DocumentViewOriginatorAcc_Search(long AccountID, ISANYUKTServiceUser serviceUser)
        {
            List<OriginatorListAccountResponse> response = new List<OriginatorListAccountResponse>();
            response = await GetallOriginatorsAccountByID(AccountID, serviceUser);
           

            FileManager fileManager = new FileManager();
            UserAccountsChecueFileResponse resp = new UserAccountsChecueFileResponse();
             if (response[0].Filename != null && response[0].Filename != "")
                {
                resp.OriginatorAccountID = response[0].OriginatorAccountID;
                resp.FileUrl = response[0].Filename;
                    resp.FileBytes = fileManager.ReadFileOther(response[0].Filename, "AccountCheque");
                    resp.Base64String = Convert.ToBase64String(resp.FileBytes);
                    resp.MediaExtension = System.IO.Path.GetExtension(response[0].Filename).ToLower();
                }
                


          

           return resp;
          
        }
        public async Task<SimpleResponse> ListAllOriginatorsAccounts(OriginatorListAccountRequest request,ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();

            response.Result = await _repository.ListAllOriginatorsAccounts(request,serviceUser);
            return response;
        }
        public async Task<long> AddUserAddress(CreateUserDetailAddressRequest request, ISANYUKTServiceUser serviceUser)
        {
            long outputresponse = 0;

            outputresponse = await _repository.AddUserAddress(request, serviceUser);

            return outputresponse;
        }
        public async Task<long> CreateOrgAPIPartner(CreateNewPartnerRequest request, ISANYUKTServiceUser serviceUser)
        {
            long outputresponse = 0;


            string pwd = BCrypt.Net.BCrypt.HashPassword(request.Password);

            //SyatemConfig obj = new SyatemConfig();
            //obj.SendEmail("Test", "Hello how are You", "ajaibit@gmail.com");

            outputresponse = await _repository.CreateOrgAPIPartner(request, pwd, serviceUser);
            //if (outputresponse>0)
            //{
            //    SyatemConfig obj =new SyatemConfig();
            //    obj.SendEmail("Test", "Hello how are You", "ajaibit@gmail.com");
            //}

            return outputresponse;
        }
        public async Task<long> CreateNewUser(CreateNewUserRequest request, ISANYUKTServiceUser serviceUser)
        {
            long outputresponse = 0;


            string pwd = BCrypt.Net.BCrypt.HashPassword(request.Password);

           // SyatemConfig obj = new SyatemConfig();
           // obj.SendEmail("Test", "Hello how are You", "ajaibit@gmail.com");
           // return 0;
            outputresponse = await _repository.CreateNewUser(request, pwd, serviceUser);
            //if (outputresponse>0)
            //{
            //    SyatemConfig obj =new SyatemConfig();
            //    obj.SendEmail("Test", "Hello how are You", "ajaibit@gmail.com");
            //}

            return outputresponse;
        }
        public async Task<SimpleResponse> GetAllUserAddress(ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();

            response.Result = await _repository.GetAllUserAddress(serviceUser);
            return response;
        }
        
        public async Task<long> AddUserDeatilKYC(CreateUserDetailKyc1 request, string Filename, ISANYUKTServiceUser serviceUser)
        {
            long outputresponse = 0;
            CreateUserDetailKyc request1 = new CreateUserDetailKyc();
            request1.FileUrl = Filename.ToString();
            request1.DocumentNo = request.DocumentNo;
            request1.KycID = request.KycID;
           

            outputresponse = await _repository.AddUserDeatilKYC(request1, serviceUser);

            return outputresponse;
        }
        public async Task<SimpleResponse> GetAllUserKyc(ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();

            response.Result = await _repository.GetAllUserKyc(serviceUser);
            return response;
        }
        public async Task<SimpleResponse> GetAllUserKycById(long KycId, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();

            response.Result = await _repository.GetAllUserKycById(KycId,serviceUser);
            return response;
        }
        public async Task<List<UserrListResponse>> GetallUserByOrg(ISANYUKTServiceUser serviceUser)
        {
            return await _repository.GetallUserByOrg(serviceUser);
        }
        public async Task<long> UploadUserKYC(UploadUserKYCFileRequest request, string Filename, ISANYUKTServiceUser serviceUser)
        {
            long outputresponse = 0;
            UploadUserKYCRequest request1 = new UploadUserKYCRequest();
            request1.fileurl = Filename;
            request1.KycID =request.KycID;
            request1.DocumentNo=request.DocumentNo;


            outputresponse = await _repository.UploadUserKYC(request1, serviceUser);

            return outputresponse;
        }
        public async Task<SimpleResponse> DocumentView_Search(long  KYCID, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();
            List<UserKYYCResponse> list = new  List<UserKYYCResponse>();
             list = await _repository.GetAllUserKycById(KYCID, serviceUser);
         
            FileManager fileManager = new FileManager();
            UserKycdownloadListResponse resp=new UserKycdownloadListResponse();
            

            foreach (UserKYYCResponse item in list)
                {
                if (item.FileUrl!=null && item.FileUrl!="")
                {
                    resp.UserKYCID = item.UserKYCID;
                    resp.DocumentNo = item.DocumentNo;
                    resp.KycID = item.KycID;
                    resp.ContentType = "image";
                    resp.MediaContentType = "png";
                    resp.FileBytes = fileManager.ReadFile(item.FileUrl, "PartnerDocument", serviceUser.UserID.ToString());
                    if(resp.FileBytes!=null && resp.FileBytes.Length>0)
                    {
                        resp.Base64String = "";
                    }
                    else
                    {
                        resp.Base64String = Convert.ToBase64String(resp.FileBytes);
                    }
                    
                    resp.MediaExtension = System.IO.Path.GetExtension(item.FileUrl).ToLower();
                    resp.FileUrl = item.FileUrl;

                }
                else
                {
                    response.SetError("File not Exists");
                }


            }
            
            response.Result = resp;
            return response;
        }
        public async Task<SimpleResponse> UpdateOriginatorChequeFile(PayinAccountRegistrationChequeRequest request, ISANYUKTServiceUser serviceUser)
        {
            long outputresponse = 0;
           SimpleResponse response = new SimpleResponse();

            response.Result = await _repository.UpdateOriginatorChequeFile(request, serviceUser);

            return response;
        }
    }
}
