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

            foreach(var unit in instance.Units)
            {
                for(var i = 0; i < 2; i++)
                {
                    var range = cost.GetMin();
                    if (range == null) return instance.Stations;
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

            while(centralStations.Any(station => !connected.Contains(station)))
            {
                var notConnected = centralStations.Where(station => !connected.Contains(station)).First();
                var nearestConnected = notConnected.GetOneNearest(connected.ToList());
                
                var (newCost, moreAdditionalStations) = ArrangeBetween.Run(cost, nearestConnected, notConnected);
                cost = new Cost(newCost);
                connected.Add(notConnected);
                additionalStations.AddRange(moreAdditionalStations);
                moreAdditionalStations.ForEach(station => { connected.Add(station); });
            }

            return instance.Stations.Concat<Station>(additionalStations).ToList();
        }
    }

}
