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
            var units = instance.MapObjects.FindAll(item => item is Unit).Cast<Unit>().ToList();

            if (units.Count == 1) return true;

            if (units.Count == 0)
            {
                foreach (var station in instance.MapObjects.FindAll(item => item is Station))
                {
                    var visited = new Dictionary<MapObject, bool>();
                    instance.MapObjects.ForEach(mapObject => visited.Add(mapObject, false));
                    DFS(station, visited);
                    bool notAllConnected = visited.Any(item => item.Value == false);

                    if (notAllConnected)
                    {
                        return false;
                    }
                }

                return true;
            }

            if (units.Any(item => !item.HasAttachement())) return false;

            foreach (var unit in instance.MapObjects.FindAll(item => item is Unit))
            {
                var visited = new Dictionary<MapObject, bool>();
                instance.MapObjects.ForEach(mapObject => visited.Add(mapObject, false));
                DFS(unit, visited);
                bool notAllConnected = visited.Any(item => item.Value == false);

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
            //if (stations.Count == 0)
            //{
            //    return true;
            //}

            //foreach (var station in stations)
            //{
            //    var visited = new Dictionary<MapObject, bool>();
            //    stations.ForEach(mapObject => visited.Add(mapObject, false));
            //    DFS(station, visited);
            //    bool allConnected = visited.All(item => item.Value == true);

            //    if (!allConnected)
            //    {
            //        return false;
            //    }
            //}

            //return true;
        }
        public void DFS(MapObject start, Dictionary<MapObject, bool> visited)
        {
            visited[start] = true;

            foreach (var mapObject in start.Receivers)
            {
                if (visited[mapObject] == true) continue;

                DFS(mapObject, visited);
            }
        }
    }
}
