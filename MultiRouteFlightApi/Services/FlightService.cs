using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiRouteFlightApi.Helper;
using MultiRouteFlightApi.Model;

namespace MultiRouteFlightApi.Services
{
    public class FlightService : IFlightService
    {
        private readonly FlightByRoutes _flightByRoutes;

        public FlightService(FlightByRoutes flightByRoutes)
        {
            _flightByRoutes = flightByRoutes;
        }

        
        public async Task<MessageHelperModel> GetFlightByRoutesResponsesAsync()
        {
            MessageHelperModel message = new MessageHelperModel();
            try
            {
                var flightResponses = GenerateFlightRoutes();
                message.FlightResponses = flightResponses;
                message.statusCode = 200;
                message.Message = "Flight responses generated successfully.";
            }
            catch (Exception ex)
            {
                message.FlightResponses = null;
                message.statusCode = 500;
                message.Message = "An error occurred while generating flight responses.";
            }
            return message;
        }

        private List<FlightResponseModel> GenerateFlightRoutes()
        {
            var flightResponses = new List<FlightResponseModel>();
            GenerateFlightRoutesRecursive(new List<FlightModel>(), _flightByRoutes.DirectionWiseFlight, flightResponses);
            return flightResponses;
        }

        private void GenerateFlightRoutesRecursive(List<FlightModel> currentRoute, List<List<FlightModel>> remainingRoutes, List<FlightResponseModel> flightResponses)
        {
            if (remainingRoutes.Count == 0)
            {
                flightResponses.Add(ConvertToFlightResponse(currentRoute));
                return;
            }

            foreach (var flight in remainingRoutes.First())
            {
                var newRoute = new List<FlightModel>(currentRoute);
                newRoute.Add(flight);
                GenerateFlightRoutesRecursive(newRoute, remainingRoutes.Skip(1).ToList(), flightResponses);
            }
        }

        private FlightResponseModel ConvertToFlightResponse(List<FlightModel> route)
        {
            decimal totalPrice = 0;
            for(int i = 0; i < route.Count; i++)
            {
                totalPrice += route[i].TotalPrice;
            }
            var totalBasePrice = route.Sum(f => f.BasePrice);
            var totalTaxPrice = route.Sum(f => f.TaxPrice);

            var flightDetails = route.Select(f => new FlightDetails
            {
                Id = f.Id,
                BasePrice = f.BasePrice,
                TaxPrice = f.TaxPrice,
                TotalPrice = f.TotalPrice,
                Origin = f.Origin,
                Destination = f.Destination
            }).ToList();

            var taxBreakdowns = route.SelectMany(f => f.TaxBreakDowns).Distinct().ToList();

            return new FlightResponseModel
            {
                TotalPrice = totalPrice,
                TotalBasePrice = totalBasePrice,
                TotalTaxPrice = totalTaxPrice,
                Flights = flightDetails,
                TaxBreakDowns = taxBreakdowns
            };
        }
    }
}
