using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class JoinNearestNeighbors
    {
        public static (Cost, List<Station>) Run(Cost initialCost, List<Station> coreStations)
        {
            if(coreStations.Count == 0 || !initialCost.CanGetAny()) return (initialCost, new List<Station>());
            Cost cost = new Cost(initialCost);

            var connected = new HashSet<Station>();
            var additionalStations = new List<Station>();
            connected.Add(coreStations.First());

            while (connected.Count < coreStations.Count)
            {
                Tuple<Station, Station> nearest = null;
                var nearestDistance = 0.0;

                var notConnected = coreStations.FindAll(item => !connected.Contains(item));
                foreach (var notConnectedStation in notConnected) //alert zrobić kurde jednolinijkowca!
                {
                    foreach (var connectedStation in connected.ToList().Concat(additionalStations)) //alert pozwalam na dołączanie do infrastruktury
                    {
                        if (nearest == null || notConnectedStation.GetDistanceFrom(connectedStation) < nearestDistance)
                        {
                            nearest = new Tuple<Station, Station>(connectedStation, notConnectedStation);
                            nearestDistance = notConnectedStation.GetDistanceFrom(connectedStation); //alert optymalizacja
                        }
                    }
                }
                connected.Add(nearest.Item2); //alert nieczytelne
                var (newCost, moreAdditionalStations) = ArrangeBetween.Run(cost, nearest.Item1, nearest.Item2); 
                cost = new Cost(newCost);
                additionalStations.AddRange(moreAdditionalStations);
                if (!cost.CanGetAny()) break;
            }

            return (cost, additionalStations);
        }
    }
}
