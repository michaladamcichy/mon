using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class RecursiveArrangeBetween
    {

        public static (Cost, double?) AdjustLast(Cost initialCost, Station first, Station last, double tolerance = 0.1)
        {
            Cost cost = new Cost(initialCost);

            var lastCopy = new Station(last);

            while(cost.CanMakeBigger(lastCopy))
            {
                cost.MakeBigger(lastCopy);
                if (MapObject.AreInRange(first, last)) return (cost, lastCopy.Range);
            }
            return (cost, null);
        }
        public static (Cost, List<Station>) ArrangeBetween(Cost initialCost, Station first, Station last, double tolerance = 0.1)
        {
            Cost cost = new Cost(initialCost);
            var stations = new List<Station>();

            if(MapObject.AreInRange(first, last)) return (cost, stations);

            var (newCost, newRange) = AdjustLast(cost, first, last, tolerance);
            if (newRange != null)
            {
                cost = new Cost(newCost);
                cost.ChangeRange(last, newRange.Value);
                return (cost, stations);
            }

            var joiningStation = cost.Join(first, last, tolerance);
            if (joiningStation != null)
            {
                stations.Add(joiningStation);
                return (cost, stations);
            }

            var next = MapObject.GetNextFromTowards(first, last, tolerance);
            if(!cost.CanGetAny()) return (cost, null);
            var station = new Station(next.Position, cost.GetMax().Value);

            stations.Add(station);
            var (otherNewCost, newStations) = ArrangeBetween(cost, station, last, tolerance); //alert czy koszt jest zliczany dobrze w rekru
            if (newStations == null) return (otherNewCost, null);
            cost = new Cost(otherNewCost);
            stations.AddRange(newStations);

            return (cost, stations);
        }
        public static (Cost, List<Station>) Run(Cost initialCost, Station first, Station last, double tolerance = 0.1)
        {
            if (first.IsStationary && last.IsStationary) return (initialCost, new List<Station>()); //alert

            var cost = new Cost(initialCost);

            Cost optimalCost = null;
            List<Station> optimalStations = null;
            Dictionary<Station, double> optimalSnapshot = null;

            var snapshot = Instance.SaveRangesSnapshot(new List<Station> { first, last }); 
            foreach(var range in cost.Ranges) //alert trzeba sprawdzić czy tutaj rzeczywiście jednorodne dojdą do głosu
            {
                Instance.RestoreRangesSnapshot(snapshot);
                Cost loopCost = new Cost(cost);
                var queried = loopCost.QueryMax(2);
                if(queried != null && queried.Value > first.Range)
                {
                    loopCost.ChangeRange(first, queried.Value);
                }

                var (newCost, stations) = ArrangeBetween(loopCost, first, last, tolerance);
                if(stations != null)
                {
                    if(optimalStations == null /*|| (optimalStations.Count > stations.Count)*/) //alert docelowo tutaj porównanie kosztów 
                    {
                        optimalStations = stations;
                        optimalCost = newCost;
                        optimalSnapshot = Instance.SaveRangesSnapshot(optimalStations);
                    }
                }
                
                //cost.AddForbiddenRange(range); //alert to jeszcze nie działa jak powinno bo samo porównywanie liczby stacji jest do bani
            }
            if (optimalSnapshot != null) Instance.RestoreRangesSnapshot(optimalSnapshot);

            return (optimalCost == null ? initialCost : optimalCost, optimalStations == null ? new List<Station>() : optimalStations);
            
        }
    }
}
