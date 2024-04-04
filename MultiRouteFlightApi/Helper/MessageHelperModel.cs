using MultiRouteFlightApi.Model;

namespace MultiRouteFlightApi.Helper
{
    public class MessageHelperModel
    {
        public List<FlightResponseModel> FlightResponses { get; set; }
        public int statusCode { get; set; }
        public string Message { get; set; }
    }
}
