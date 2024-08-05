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
        public async Task<long> CreateNewUserRequest(CreateUserWithlogoRequest request,string Filename, ISANYUKTServiceUser serviceUser)
        {
            long outputresponse = 0;
            CreateUserRequest request1 =new CreateUserRequest();
            request1.LogoUrl=Filename.ToString();
            request1.FirstName=request.FirstName;
            request1.LastName=request.LastName;
            request1.MiddleName=request.MiddleName;
            request1.UserTypeId =request.UserTypeId;
            request1.EmailId=request.EmailId;
            request1.MobileNo=request.MobileNo;
            
            outputresponse = await _repository.CreateNewUserRequest(request1, serviceUser);

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
        public async Task<long> AddUserAddress(CreateUserDetailAddressRequest request, ISANYUKTServiceUser serviceUser)
        {
            long outputresponse = 0;

            outputresponse = await _repository.AddUserAddress(request, serviceUser);

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
    }
}
