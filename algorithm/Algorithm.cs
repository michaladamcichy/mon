using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class Position
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class MapObject
    {
        public Position position { get; set; } = new Position();

        public List<MapObject> hosts { get; set; } = new List<MapObject>();
        public List<MapObject> clients { get; set; } = new List<MapObject>();

        //public static double Distance(MapObject first, MapObject second)
        //{
        //    double x = Math.Abs(first.position.lat - second.position.lat) * 110.574; //alert na odwrót //alert //alert!
        //    double y = (first.position.lng - second.position.lng) * Math.Cos((first.position.lat + second.position.lat) / 2.0 * Math.PI / 180.0) * 111.320f; //alert


        //    return Math.Sqrt(x * x + y * y);
        //}

        public static double Distance(MapObject first, MapObject second)
        {
            return Distance(first.position, second.position);
        }
        public static double Distance(Position first, Position second) 
        {
            //alert do weryfikacji - zarówno algorytm jak i przekopiowany kod!!!
            ////wzięte żywcem z js, trzeba zweryfikować
            //https://stackoverflow.com/questions/27928/calculate-distance-between-two-latitude-longitude-points-haversine-formula
            //
            double p = 0.017453292519943295;    // Math.PI / 
            double a = 0.5 - Math.Cos((second.lat - first.lat) * p) / 2 +
                    Math.Cos(first.lat * p) * Math.Cos(second.lat * p) *
                    (1 - Math.Cos((second.lng - first.lng) * p)) / 2;

            return 12742 * Math.Asin(Math.Sqrt(a)); // 2 * R; R = 6371 
        }
    }

    public class Station : MapObject
    {
        public double range { get; set; }
    }

    public class Unit : MapObject
    {
    }

    public enum StationType
    {
        A = 0,
        B = 1,
        C = 2,
        Count = 3
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

            prepareDataStructures();
        }

        void prepareDataStructures()
        {
            foreach(MapObject first in mapObjects)
            {
                foreach(MapObject second in mapObjects)
                {
                    if (first == second || second is not Station) continue;

                    if (MapObject.Distance(first, second) <= ((Station) second).range)
                    {
                        first.hosts.Add(second);
                        second.clients.Add(first);
                    }
                }
            }
        }

        public bool IsConnected()
        {
            if (mapObjects.Count == 0)
            {
                return true;
            }

            foreach(var unit in mapObjects.FindAll(item => item is Unit))
            {
                var visited = new Dictionary<MapObject, bool>();

                mapObjects.ForEach(mapObject => visited.Add(mapObject, false));

                DFS(unit, visited);

                bool allConnected = visited.All(item => item.Key is not Unit || item.Value == true);
                if(!allConnected)
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

            if(start is Station)
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

        //public DFS()
        //{

        //}
    }
}
