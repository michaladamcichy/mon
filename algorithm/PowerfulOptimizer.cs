using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class PowerfulSpider
    {
        HashSet<Station> CentralStations;

        bool IsCentral(Station station)
        {
            return CentralStations.Contains(station);
        }
        public PowerfulSpider(HashSet<Station> centralStations)
        {
            CentralStations = centralStations;
        }
        public void Spider(Station station, HashSet<List<Station>> paths, List<Station> path = null, bool optimize = false)
        {
            path = path == null ? new List<Station>() : new List<Station>(path);
            path.Add(station);
            paths.Add(path);

            var relevantNeigbors = station.Neighbors.Where(neighbor => neighbor != station && neighbor.IsCore && !path.Contains(neighbor)).ToList();

            if (relevantNeigbors.Count == 0 || (IsCentral(station) && path.Count > 1)) return;

            foreach (var neighbor in relevantNeigbors)
            {
                Spider(neighbor, paths, path);
            }

            //paths.Remove(path);
        }

        public HashSet<List<Station>> Run(Station centralStation)
        {
            var paths = new HashSet<List<Station>>();
            Spider(centralStation, paths);
            return paths;
        }
    }

    public class PowerfulOptimizer
    {
        public List<Station> Run(Instance instance)
        {
            instance.RemoveRelations();
            instance.CreateRelations(); //alert
            var groups = new RecoverGroups().Run(instance);
            var centralStations = groups.Select(group => group.CentralStation).ToList();
            var centralStationsAsSet = centralStations.ToHashSet();
            foreach (var centralStation in centralStations)
            {
                Debug.WriteLine("STATION ID = " + centralStation.Id.ToString());
                _.Print(centralStation.Neighbors.Where(neighbor => neighbor.IsCore).ToList());
                Debug.WriteLine("START");
                var paths = new PowerfulSpider(centralStationsAsSet).Run(centralStation);

                paths.ToList().ForEach(path => _.Print(path));

            }

            return new List<Station>(); //alert
        }
    }
}
