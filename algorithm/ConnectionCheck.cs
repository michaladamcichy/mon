using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class ConnectionCheck
    {
        public bool Run(Instance instance)
        {
            var units = instance.Units;

            if (units.Any(item => !item.HasAttachement())) return false;
            if (instance.PrivateStations.Any(station => station.Neighbors.Where(neighbor => neighbor.IsCore).Count() == 0)) return false;

            var neighbors = new HashSet<Station>();
            instance.PrivateStations.ForEach(station => station.Neighbors.ForEach(neighbor => { if (neighbor.IsCore) neighbors.Add(neighbor); }));

            foreach (var station in neighbors)
            {
                var visited = new Dictionary<Station, bool>();
                instance.Stations.ForEach(station => visited[station] = false);

                DFS(station, visited);
                bool notAllConnected = instance.PrivateStations.Any(station => !visited[station]);

                if (notAllConnected)
                {
                    return false;
                }
            }
            
            return true;
        }

        public bool Run(List<Station> stations, int[] counts)
        {
            return Run(new Instance(stations, counts));
        }
        public void DFS(Station start, Dictionary<Station, bool> visited)
        {
            visited[start] = true;

            foreach (var mapObject in start.Neighbors)
            {
                if (visited[mapObject] == true) continue;

                DFS(mapObject, visited);
            }
        }
    }
}
