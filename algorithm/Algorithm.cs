using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public interface IArrangeStationsAlgorithm
    {
        List<Station> Run(Instance instance);
    }

    public class SimpleArrangeStationsAlgorithm : IArrangeStationsAlgorithm
    {
        public List<Station> Run(Instance instance)
        {
            //var stations = new List<Station>();
            //stations.Add(instance.stations[0]); //alert czy nie muszę się odwoływać wyłącznie do mapObjects?
            return instance.Stations; //return instance.Stations;
        }
    }

    public class ConnectionCheckAlgorithm
    {
        public bool Run(Instance instance)
        {
            if (instance.MapObjects.Count == 0)
            {
                return true;
            }

            foreach (var unit in instance.MapObjects.FindAll(item => item is Unit))
            {
                var visited = new Dictionary<MapObject, bool>();

                instance.MapObjects.ForEach(mapObject => visited.Add(mapObject, false));

                DFS(unit, visited);

                bool allConnected = visited.All(item => item.Key is not Unit || item.Value == true);
                if (!allConnected)
                {
                    return false;
                }
            }

            return true;
        }
        public void DFS(MapObject start, Dictionary<MapObject, bool> visited)
        {
            visited[start] = true;

            foreach (var host in start.hosts)
            {
                if (visited[host]) continue;

                DFS(host, visited);
            }

            if (start is Station)
            {
                foreach (var client in start.clients.FindAll(item => item is Unit))
                {
                    if (visited[client]) continue;

                    DFS(client, visited);
                }
            }
        }
    }

    public class Algorithm
    {
        public static bool IsConnected(Instance instance)
        {
            return new ConnectionCheckAlgorithm().Run(instance);
        }

        public static List<Station> SimpleArrangeAlgorithm(Instance instance)
        {
            return new SimpleArrangeStationsAlgorithm().Run(instance);
        }
    }
}
