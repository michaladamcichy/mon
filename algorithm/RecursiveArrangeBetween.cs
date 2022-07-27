using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class RecursiveArrangeBetween
    {
        public static (Cost, List<Station>) ArrangeBetween(Cost initialCost, Station first, Station last, double tolerance = 0.1)
        {
            Cost cost = new Cost(initialCost);
            var stations = new List<Station>();

            if(MapObject.AreInRange(first, last)) return (cost, stations);

            var joiningStation = cost.Join(first, last);
            if (joiningStation != null)
            {
                stations.Add(joiningStation);
                return (cost, stations);
            }

            var next = MapObject.GetNextFromTowards(first, last);
            if(!cost.CanGetAny()) return (cost, null);
            var station = new Station(next.Position, cost.GetMax().Value);

            stations.Add(station);
            var (newCost, newStations) = ArrangeBetween(cost, station, last, tolerance);
            if (newStations == null) return (cost, null);
            cost = new Cost(newCost);
            stations.AddRange(newStations);

            return (cost, stations);
        }
        public static (Cost, List<Station>) Run(Cost initialCost, Station first, Station last, double tolerance = 0.1)
        {
            var cost = new Cost(initialCost);

            Cost optimalCost = null;
            List<Station> optimalStations = null;
            foreach(var range in cost.Ranges)
            {
                var (newCost, stations) = ArrangeBetween(cost, first, last, tolerance);
                if(stations != null)
                {
                    if(optimalStations == null || optimalStations.Count < stations.Count) //alert docelowo tutaj porównanie kosztów 
                    {
                        optimalStations = stations;
                        optimalCost = newCost;
                    }
                }
                
                cost.AddForbiddenRange(range);
            }

            return (optimalCost == null ? initialCost : optimalCost, optimalStations == null ? new List<Station>() : optimalStations);
            
        }
    }
}
