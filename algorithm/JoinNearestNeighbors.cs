using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class JoinNearestNeighbors
    {
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

            var centralStations = notConnected.Where(item => !(item.IsStationary && !item.Neighbors.Any(neighbor => neighbor.IsPrivate)));
     /*       var centralStations = notConnected.ToList();
            centralStations.RemoveAll(item => )*/

            //while (all.Any(station => stationToSet[all.First()] != stationToSet[station])) //alert!
            //while (notConnected.Any(station => stationToSet[connected.Count == 0 ? notConnected.First() : connected.First() ] != stationToSet[station])) //alert!
            while (centralStations.Any(station => stationToSet[connected.Count == 0 ? centralStations.First() : connected.First()] != stationToSet[station]))
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
                if (nearest == null)
                    break;

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
