using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class _
    {
        static bool enabled = false;
        public static void Print(List<Station> stations, string message = "")
        {
            if (!enabled) return;
            //Debug.Write("\n" + message + ": ");
            //Debug.Write("Count = " + stations.Count.ToString() + "  | [");
            stations.ForEach(station => Debug.Write(station.Id.ToString() + ", "));
            //Debug.Write("]\n");
        }
    }

    class Kruskal
    {
        public List<Tuple<Station, Station>> Run(List<Station> vertices, DoubleDictionary<Station, List<Station>> edges)
        {
            var selectedEdges = new List<Tuple<Station, Station>>();
            var stationToSet = new Dictionary<Station, HashSet<Station>>();
            var sortedEdges = edges.GetDistinctKeys();
            sortedEdges.Sort((item1, item2) => Cost.IsCheaperThan(edges[item1], edges[item2]) ? -1 : 1);
            
            foreach(var (station1, station2) in sortedEdges)
            {
                if (stationToSet.Values.ToList().Distinct().ToList().Count == 1 && vertices.All(item => stationToSet.Values.ToList().Distinct().First().Contains(item))) break;
                //alert za długi ŚWINQ
                if(!stationToSet.ContainsKey(station1)) stationToSet[station1] = new HashSet<Station>() { station1 };
                if (!stationToSet.ContainsKey(station2)) stationToSet[station2] = new HashSet<Station>() { station2 };
                var set1 = stationToSet[station1];
                var set2 = stationToSet[station2];

                if (set1 == set2) continue;

                selectedEdges.Add(new Tuple<Station, Station>(station1, station2));
                var mergedSet = set1.Concat(set2).ToHashSet();
                mergedSet.ToList().ForEach(station => { stationToSet[station] = mergedSet; });
            }

            return selectedEdges;
        }
    }
    public class DoubleDictionary<X, Y> : Dictionary<Tuple<X, X>, Y> where X : class where Y : class
    {
        public Y this[X key1, X key2]
        {
            get => this[new Tuple<X, X>(key1, key2)];
            set { this[new Tuple<X, X>(key1, key2)] = value; this[new Tuple<X, X>(key2, key1)] = value; }
        }

        public List<Tuple<X,X>> GetDistinctKeys()
        {
            var distinctKeys = new List<Tuple<X, X>>();

            Tuple<X, X> lastKey = null;
            foreach(var key in Keys)
            {
                if (lastKey != null && this[lastKey] == this[key] ) continue; //alert

                distinctKeys.Add(key);

                lastKey = key;
            }

            return distinctKeys;
        }
    }
    public class Spider
    {
        HashSet<Station> CentralStations;

        public Spider(HashSet<Station> centralStations)
        {
            CentralStations = centralStations;
        }

        bool IsCentral(Station station )
        {
            return CentralStations.Contains(station);
        }

        public void LittleSpider(Station station, HashSet<List<Station>> paths, List<Station> _path, bool firstTime = true)
        {
            var path = new List<Station>(_path);
            paths.Add(path);
            path.Add(station);

            if (IsCentral(station) && firstTime == false)
            {
                return;
            }

            foreach (var neighbor in station.Neighbors)
            {
                //if(visited.Contains(neighbor)) continue;
                if (neighbor.IsPrivate || path.Contains(neighbor)) continue; //alert!
                LittleSpider(neighbor, paths, path, false);
            }
            paths.Remove(path); //alert! wydajność!
        }

      
        //DoubleDictionary<Station, List<Station>> edges = new ;

        bool Validate(Station station, List<Station> path)
        {
            if (path.Count < 2) return false;
            if(!IsCentral(path.First()) || !IsCentral(path.Last())) return false;
            Debug.Assert(station == path.First()); //alert
            if (path.Any(item => item != path.First() && item != path.Last() && IsCentral(item))) return false; //alert to niekonieczne, tylko dla sprawdzenia czy działą
            return true;
        }

        void AddEdges(Station first, DoubleDictionary<Station, List<Station>> edges, HashSet<List<Station>> _paths)
        {
            //Debug.WriteLine("station " + first.Id.ToString());
            var paths = _paths.ToList();
            paths.ForEach(path => _.Print(path, "Before validation..."));
            paths = paths.Where(path => Validate(first, path)).ToList();
            paths.ForEach(path => _.Print(path, "After validation"));

            var neighbors = paths.Select(path => path.Last()).Distinct().ToList();

            var neighborToPaths = new Dictionary<Station, List<List<Station>>>();
            paths.ForEach(path => {
                var neighbor = path.Last();
                if (!neighborToPaths.ContainsKey(neighbor)) neighborToPaths[neighbor] = new List<List<Station>>();
                neighborToPaths[neighbor].Add(path);
            });

            neighbors.ForEach(neighbor =>
            {
                var minPath = Cost.GetCheapest(neighborToPaths[neighbor]);
                edges[minPath.First(), neighbor] = minPath;
            });
        }

        public DoubleDictionary<Station, List<Station>> Run(List<Station> centralStations)
        {
            //Debug.WriteLine("Spider running");
            var edges = new DoubleDictionary<Station, List<Station>>();
            foreach (var centralStation in centralStations)
            {
                var paths = new HashSet<List<Station>>();
                LittleSpider(centralStation, paths, new List<Station>());
                AddEdges(centralStation, edges, paths);
            }
            return edges;

            //new PowerfulOptimizer().Run(instance);
            //return new DoubleDictionary<Station, List<Station>>();
        }
    }

    public class SimpleOptimize
    {
        /*List<Station> GetEdge(Station privateA, Station privateB)
        {

        }*/

        public List<Station> Run(Instance instance, DoubleDictionary<Station, List<Station>> edges = null)
        {
            instance.RemoveRelations();
            instance.CreateRelations(0.1);
            /*var stations = instance.Stations.*//*Where(station => !station.IsAttached()).*//* //alert!
                Concat<Station>(instance.StationaryStations).ToList();
            return new Dijkstra().Run(instance, stations, stations.Where(item => item.IsAttached()).First(), stations.Where(item => item.IsAttached()).Last());*/
            //return new List<Station>();
            //Debug.WriteLine("Simple optimize");

            var groups = new RecoverGroups().Run(instance);
            var centralStations = groups.Select(group => group.CentralStation).ToList();

            if (edges == null) edges = new Spider(centralStations.ToHashSet()).Run(centralStations);
            var necessaryEdges = new Kruskal().Run(centralStations, edges);
            var necessaryStations = new HashSet<Station>();

            foreach(var necessaryEdge in necessaryEdges)
            {
                edges[necessaryEdge].ForEach(station => necessaryStations.Add(station)); 
            }

            /*var centralStations = new RecoverGroups().Run(instance).Select(group => group.CentralStation).ToList();*/
            centralStations.ForEach(centralStation => necessaryStations.Add(centralStation));
            instance.PrivateStations.ForEach(privateStation => necessaryStations.Add(privateStation));

            instance.Stations = instance.Stations.Where(station => necessaryStations.Contains(station)).ToList();
            foreach (var station in instance.Stations)
            {
                if(!necessaryStations.Contains(station) && !station.IsStationary)   
                {
                    instance.Stations.Remove(station);
                }
            }

            return instance.Stations; //alert
        }
    }
}
