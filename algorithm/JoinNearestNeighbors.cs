using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class JoinNearestNeighbors
    {
        /*public static (List<Station>, Cost) RunX(Cost initialCost, List<Station> coreStations, List<Station> stationaryStations, List<Unit> unitsConnectedToStationaryStations)
        {
            if (coreStations.Count == 0 || !initialCost.CanGetAny()) return (new List<Station>(), initialCost);
            Cost cost = new Cost(initialCost);

            var connected = new HashSet<Station>();
            var additionalStations = new List<Station>();

            var stations = new List<Station>(coreStations);

            if (unitsConnectedToStationaryStations.Count > 0)
            {
                stations.AddRange(stationaryStations);
            }
            connected.Add(coreStations.First());


            while (stations.Any(station => !connected.Contains(station)))
            {
                Tuple<Station, Station> nearest = null;
                var nearestDistance = 0.0;

                var notConnected = stations.FindAll(item => !connected.Contains(item));
                foreach (var notConnectedStation in notConnected) //alert zrobić kurde jednolinijkowca!
                {
                    if (notConnectedStation.IsStationary && unitsConnectedToStationaryStations.Count > 0 && stationaryStations.Any(station => connected.Contains(station)))
                    {
                        continue;
                    }

                    foreach (var connectedStation in connected.ToList().Concat(additionalStations)) //alert pozwalam na dołączanie do infrastruktury
                    {
                        if (nearest == null || notConnectedStation.GetDistanceFrom(connectedStation) < nearestDistance)
                        {
                            nearest = new Tuple<Station, Station>(connectedStation, notConnectedStation);
                            nearestDistance = notConnectedStation.GetDistanceFrom(connectedStation); //alert optymalizacja
                        }
                    }
                }
                if (coreStations.Any(station => !connected.Contains(station))) //alert COREstations, nie stations
                {
                    if (nearest == null) continue;

                    connected.Add(nearest.Item2); //alert nieczytelne
                    var (newCost, moreAdditionalStations) = RecursiveArrangeBetween.Run(cost, nearest.Item1, nearest.Item2);
                    cost = new Cost(newCost);
                    additionalStations.AddRange(moreAdditionalStations);
                }
                else
                {
                    break; //alert DKWIMD
                }

                if (!cost.CanGetAny()) break;
            }

            return (additionalStations, cost);
        }*/


        public static (List<Station>, Cost) Run(Cost initialCost, List<Station> coreStations, List<Station> stationaryStations,
            List<Unit> unitsConnectedToStationaryStations, HashSet<Station> _connected = null)
        {
            var cost = new Cost(initialCost);
            if (coreStations.Count == 0 || !cost.CanGetAny()) return (new List<Station>(), cost);

            var connected = _connected == null ? new HashSet<Station>() : _connected;
            var stations = new List<Station>();
            var additionalStations = new List<Station>();

            stations.AddRange(coreStations);
            stations.AddRange(stationaryStations);
            if(unitsConnectedToStationaryStations.Count > 0) //alert chyba jednak nie powinno tak być
                stationaryStations.ForEach(stationaryStation => connected.Add(stationaryStation));

            if (connected.Count == 0) connected.Add(coreStations.First());

            while (coreStations.Any(station => !connected.Contains(station)))
            {
                Tuple<Station, Station> nearest = null;
                var nearestDistance = 0.0;

                var notConnected = stations.FindAll(item => !connected.Contains(item));
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
                var (newCost, moreAdditionalStations) = RecursiveArrangeBetween.Run(cost, nearest.Item1, nearest.Item2);
                cost = new Cost(newCost);
                additionalStations.AddRange(moreAdditionalStations);

                if (!cost.CanGetAny()) break;
            }

            return (additionalStations, cost);
        }
    }
}
