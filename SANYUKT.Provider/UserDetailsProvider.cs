using SANYUKT.Datamodel.Entities.Users;
using SANYUKT.Datamodel.Interfaces;
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
    }
}
