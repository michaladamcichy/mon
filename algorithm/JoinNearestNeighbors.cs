using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class JoinNearestNeighbors
    {
      /*  public static (List<Station>, Cost) RunX(Cost initialCost, List<Station> coreStations, List<Station> stationaryStations,
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
        }*/
        public static (List<Station>, Cost, DoubleDictionary<Station, List<Station>>) Run(Cost initialCost, Instance instance, List<Station> notConnected, List<Station> connected)
        {
            var cost = new Cost(initialCost);
            if(connected == null) connected = new List<Station>();

            var additionalStations = new List<Station>();
            var stationToSet = new Dictionary<Station, HashSet<Station>>();
            var connectedAsSet = connected.ToHashSet();
            connected.ForEach(station => stationToSet[station] = connectedAsSet);
            notConnected.ForEach(station => stationToSet[station] = new HashSet<Station> { station });
            var edges = new DoubleDictionary<Station, List<Station>>();
            var all = connected.Concat<Station>(notConnected).ToList();

            if (notConnected.Count == 0 || !cost.CanGetAny()) return (all, cost, new DoubleDictionary<Station, List<Station>>());

            while (notConnected.Any(station => stationToSet[connected.Count == 0 ? notConnected.First() : connected.First()] != stationToSet[station]))
            {
                Tuple<Station, Station> nearest = null;
                var nearestDistance = 0.0;

                foreach (var first in all) //alert zrobić kurde jednolinijkowca!
                {
                    foreach (var second in all) //alert pozwalam na dołączanie do infrastruktury
                    {
                        if (first == second) continue;
                        
                        if(!stationToSet.ContainsKey(first)) stationToSet[first] = new HashSet<Station> { first };
                        if(!stationToSet.ContainsKey(second)) stationToSet[second] = new HashSet<Station> { second };
                        var set1 = stationToSet[first];
                        var set2 = stationToSet[second];

                        if (set1 == set2) continue;
                        var distance = first.GetDistanceFrom(second);
                        if (nearest == null || distance < nearestDistance)
                        {
                            nearest = new Tuple<Station, Station>(first, second);
                            nearestDistance = distance; //alert optymalizacja
                        }
                    }
                }
                if (nearest == null) break;

                var (f, s) = nearest;
                var s1 = stationToSet[f];
                var s2 = stationToSet[s];
                var mergedSet = s1.Concat(s2).ToHashSet();
                mergedSet.ToList().ForEach(station => { stationToSet[station] = mergedSet; });
                
                var (newCost, moreAdditionalStations) = RecursiveArrangeBetween.Run(cost, f, s);
                cost = new Cost(newCost);
                additionalStations.AddRange(moreAdditionalStations);
                all.AddRange(moreAdditionalStations);
                edges[f, s] = new List<Station> { f }.Concat<Station>(moreAdditionalStations).Concat<Station>(new List<Station>{ s }).ToList();

                if (!cost.CanGetAny()) break;
            }

            return (all, cost, edges);
        }
    }
}
