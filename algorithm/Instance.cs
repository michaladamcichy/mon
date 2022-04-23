using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{

    public class Node
    {
        public MapObject mapObject;

        public Node(MapObject mapObject)
        {
            this.mapObject = mapObject;
        }
    }
    public class DistanceNode : Node
    {
        public MapObject mapObject;
        public List<DistanceNode> neighbors;

        public DistanceNode(MapObject mapObject) : base(mapObject)
        {
        }
    }
    public class ConnectivityNode : Node
    {
        public MapObject mapObject;
        public List<ConnectivityNode> hosts;
        public List<ConnectivityNode> clients;

        public ConnectivityNode(MapObject mapObject) : base(mapObject)
        {
        }
    }

    public class Instance
    {
        public double[] stationRanges { get; set; } = new double[0];
        public int[] stationCounts { get; set; } = new int[0];
        public List<Station> stations { get; set; } = new List<Station>();
        public List<Unit> units { get; set; } = new List<Unit>();

        List<MapObject> mapObjects = new List<MapObject>();

        public Instance(double[] stationRanges, int[] stationCounts, List<Station> stations, List<Unit> units)
        {
            this.stationRanges = stationRanges;
            this.stationCounts = stationCounts;

            this.mapObjects = stations.Cast<MapObject>().Concat(units.Cast<MapObject>()).ToList();
        }
 

        public bool IsConnected()
        {
            if (mapObjects.Count == 0)
            {
                return true;
            }

            foreach (var unit in mapObjects.FindAll(item => item is Unit))
            {
                var visited = new Dictionary<MapObject, bool>();

                mapObjects.ForEach(mapObject => visited.Add(mapObject, false));

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

        public List<ConnectivityNode> CalculateConnectivityGraph()
        {
            var nodes = new List<ConnectivityNode>();
            mapObjects.ForEach(mapObject => nodes.Add(new ConnectivityNode(mapObject)));

            foreach (ConnectivityNode first in nodes)
            {
                foreach (ConnectivityNode second in nodes)
                {
                    if (first == second || second.mapObject is not Station) continue;

                    if (MapObject.Distance(first.mapObject, second.mapObject) <= ((Station) second.mapObject).range)
                    {
                        first.hosts.Add(second);
                        second.clients.Add(first);
                    }
                }
            }

            return nodes;
        }

        public List<DistanceNode> CalculateDistanceGraph()
        {
            foreach (MapObject first in mapObjects)
            {
                foreach (MapObject second in mapObjects)
                {
                    if (first == second || second is not Station) continue;

                    if (MapObject.Distance(first, second) <= ((Station)second).range)
                    {
                        first.hosts.Add(second);
                        second.clients.Add(first);
                    }
                }
            }
        }
    }
}
