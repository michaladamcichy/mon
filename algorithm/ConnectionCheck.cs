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

            foreach (var attachedStation in instance.PrivateStations)
            {
                var visited = new Dictionary<Station, bool>();
                instance.AllStations.ForEach(station => visited[station] = false);

                DFS(attachedStation, visited);
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

            foreach (var mapObject in start.Receivers.FindAll(mapObject => mapObject is Station).Cast<Station>().ToList())
            {
                if (visited[mapObject] == true) continue;

                DFS(mapObject, visited);
            }
        }
    }
}
