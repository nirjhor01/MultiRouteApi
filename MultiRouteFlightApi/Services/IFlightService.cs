using MultiRouteFlightApi.Helper;
using System.Threading.Tasks;

namespace MultiRouteFlightApi.Services
{
    public interface IFlightService
    {
        Task<MessageHelperModel> GetFlightByRoutesResponsesAsync();
    }
}
