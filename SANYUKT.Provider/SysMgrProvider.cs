using SANYUKT.Datamodel.Entities.SysModel;
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
    public class SysMgrProvider:BaseProvider
    {
    
        public readonly SysMgrRepository _repository=null;
        
        public SysMgrProvider()
        {
            _repository=new SysMgrRepository ();
        }
        public async Task<SimpleResponse> CreateNewRole(CreateRoleRequest request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();

            response.Result = await _repository.CreateNewRole(request, serviceUser);
            return response;
        }
        public async Task<SimpleResponse> GetAllRoles(GetRoleRequest request, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();

            response.Result = await _repository.GetAllRoles(request, serviceUser);
            return response;
        }
        public async Task<SimpleResponse> GenerateServiceSessionCode(int ServiceId, ISANYUKTServiceUser serviceUser)
        {
            SimpleResponse response = new SimpleResponse();

            response.Result = await _repository.GenerateServiceSessionCode(ServiceId, serviceUser);
            return response;
        }
      
    }
}
