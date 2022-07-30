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

            foreach (var attachedStation in instance.Stations.FindAll(station => station.IsAttached()))
            {
                var visited = new Dictionary<Station, bool>();
                instance.Stations.ForEach(station => visited[station] = false);
                instance.StationaryStations.ForEach(station => visited[station] = false); //alert wchodzę tutaj na bardzo grząski grunt //czy to w ogóle coś zmienia?

                DFS(attachedStation, visited);
                bool notAllConnected = visited.Any(item => ((Station)item.Key).IsAttached() && item.Value == false);

                if (notAllConnected)
                {
                    return false;
                }
            }

            return true;
        }

        public bool Run(List<Station> stations)
        {
            return Run(new Instance(stations));
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
