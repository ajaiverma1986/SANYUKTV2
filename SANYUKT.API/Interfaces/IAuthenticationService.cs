using SANYUKT.Datamodel.Interfaces;
using SANYUKT.Datamodel.Shared;
using System.Threading.Tasks;

namespace SANYUKT.API.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ErrorResponse> IsValidToken(ISANYUKTServiceUser FIAUser, bool IKnoeeAPIUser = true, bool slidingExpiration = true);
        Task<ErrorResponse> Validate(ISANYUKTServiceUser FIAUser, bool validateBothToken = true, bool slidingExpiration = true);
    }
}
