using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class BuildStationarySubtrees
    {
        public HashSet<Station> GetSubtree(Station station, HashSet<Station> visited = null)
        {
            visited = visited == null ? new HashSet<Station>() : visited;
            visited.Add(station);

            foreach(var neighbor in station.Neighbors.Where(neighbor => neighbor.IsStationary && !visited.Contains(neighbor)))
            {
                visited = visited.Union(GetSubtree(neighbor, visited)).ToHashSet();
            }

            return visited;
        }

        public Dictionary<Station, HashSet<Station>> Run(List<Station> stationaryStations)
        {
            var stationToSet = new Dictionary<Station, HashSet<Station>>();

            foreach(var station in stationaryStations)
            {
                if (stationToSet.ContainsKey(station)) continue;

                var set = GetSubtree(station);

                set.ToList().ForEach(station => { if (!stationToSet.ContainsKey(station)) stationToSet[station] = set; });
            }

            return stationToSet;
        }
    }
}
