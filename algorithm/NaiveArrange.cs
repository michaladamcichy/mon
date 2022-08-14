using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class NaiveArrange : IArrangeAlgorithm
    {
        public List<Station> Run(Instance instance)
        {
            var cost = new Cost(instance);

            foreach (var unit in instance.Units)
            {
                for(var i = 0; i < 2; i++)
                {
                    var range = cost.GetMin();
                    if (range == null) return instance.Stations.ToList();
                    var station = new Station(unit.Position, range.Value);
                    if(i == 1) //alert niezrozumiałe
                    {
                        unit.Attach(station);
                    }
                    instance.MapObjects.Add(station);
                }
            }

            var centralStations = instance.Stations.Where(station => !station.IsAttached()).ToList();
            var connected = new HashSet<Station>();
            var additionalStations = new List<Station>();

            if(centralStations.Count == 0) return instance.Stations;

            connected.Add(centralStations.First());
            var lastConnected = centralStations.First();
            while(centralStations.Any(station => !connected.Contains(station)))
            {
                var nearestNotConnected = lastConnected.GetOneNearest(connected.ToList());
                /*Tuple<Station, Station> nearest = null;
                double nearestDistance = 0.0;
                foreach(var notConnected in centralStations.Where(station => !connected.Contains(station)))
                {
                    foreach(var _connected in connected)
                    {
                        if(nearest == null || notConnected.GetDistanceFrom(_connected) < nearestDistance)
                        {
                            nearest = new Tuple<Station, Station>(_connected, notConnected);
                            nearestDistance = notConnected.GetDistanceFrom(_connected);
                        }
                    }
                }
                if (nearest == null) break;
                */
                //var (newCost, moreAdditionalStations) = RecursiveArrangeBetween.Run(cost, nearest.Item1, nearest.Item2);
                var (newCost, moreAdditionalStations) = RecursiveArrangeBetween.Run(cost, lastConnected, nearestNotConnected);
                cost = new Cost(newCost);
                connected.Add(nearestNotConnected);
                additionalStations.AddRange(moreAdditionalStations);
                //moreAdditionalStations.ForEach(station => { connected.Add(station); });
            }

            return instance.Stations.Concat<Station>(additionalStations).ToList();
        }
    }

}
