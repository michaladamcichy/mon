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
        static bool enabled = true;
        public static void Print(List<Station> stations, string message = "")
        {
            if (!enabled) return;
            Debug.Write("\n" + message + ": ");
            Debug.Write("Count = " + stations.Count.ToString() + "  | [");
            stations.ForEach(station => Debug.Write(station.Id.ToString() + ", "));
            Debug.Write("]\n");
        }
    }
    /*public class PriorityQueue<X, Y>
    {
        Dictionary<Y, List<X>> queue = new Dictionary<Y, List<X>>();
        public PriorityQueue()
        {

        }

        public bool Contains(X item)
        {
            return queue.Any(keyValue => keyValue.Value.Contains(item));
        }

        public void Add(X item, Y priority)
        {
            if(!queue.ContainsKey(priority)) queue[priority] = new List<X>();
            queue[priority].Add(item); 
        }

        public int Count
        {
            get
            {
                return queue.Aggregate(0, (sum, item) => sum + item.Value.Count);
            }
        }

        public X GetMin()
        {
            var minPriority = queue.Keys.Min();
            var minValue = queue[minPriority].First();
            queue[minPriority].RemoveAt(0);
            if(queue[minPriority].Count == 0) queue.Remove(minPriority);
            return minValue;
        }
    }*/
    /*public class Dijkstra
    {
        //List<Station> queue = new List<Station>();
        Dictionary<Station, double> d = new Dictionary<Station, double>();
        Dictionary<Station, Station> previous = new Dictionary<Station, Station>();
        public List<Station> Run(Instance instance, List<Station> V, Station s, Station t)
        {
            Debug.WriteLine("s: " + s.id.ToString());
            Debug.WriteLine("t: " + t.id.ToString());
            //Debug.WriteLine("V = {");

            d[s] = 0;
            var queue = new PriorityQueue<Station, double>();
            queue.Add(s, d[s]);

            V.ForEach(v =>
            {
                if (v != s)
                {
                    d[v] = double.PositiveInfinity;
                    previous[v] = null;
                    queue.Add(v, d[v]);
                }
                //Debug.WriteLine("}\n");
            });


            while (queue.Count > 0)
            {
                var u = queue.GetMin();
                //alert wydajność 
                foreach(var v in u.Senders.Concat<MapObject>(u.Receivers).Where(item => item is Station && V.Contains(item)).Cast<Station>().Distinct().ToList()) //alert może wcześniej coś takeigo
                {
                    var alt = d[u] + u.GetDistanceFrom(v);
                    if(alt  < d[v] && !double.IsInfinity(d[u]))
                    {
                        d[v] = alt;
                        previous[v] = u;
                        //if(!queue.Contains(v)) queue.Add(v, alt);
                    }
                }
            }

            var solution = new List<Station>();
            var current = t;
            while(previous[current] != s)
            {
                Debug.WriteLine(previous[current].id);
                solution.Add(previous[current]);
                current = previous[current];
            }

            solution.Reverse();

            solution = solution.Prepend(s).ToList();
            solution.Add(t);
            return solution;
        } 
    }*/

    public class Kruskal
    {
        public List<Tuple<Station, Station>> Run(List<Station> vertices, DoubleDictionary<Station, List<Station>> edges)
        {
            var selectedEdges = new List<Tuple<Station, Station>>();
            var stationToSet = new Dictionary<Station, HashSet<Station>>();
            var sortedEdges = edges.GetDistinctKeys();
            sortedEdges.Sort((item1, item2) => Cost.IsCheaperThan(edges[item1], edges[item2]) ? -1 : 1);
            
            foreach(var (station1, station2) in sortedEdges)
            //while(stationToSet.Count != 1)
            {
                //var edgesLeft = sortedEdges.Where(edge => !selectedEdges.Contains(edge)).ToList();
                //if (edgesLeft.Count == 0) break; //alert do tego nie powinno dochodzić w grafie spójnym
                //var (station1, station2) = edgesLeft.Aggregate((currentMin, item) => Cost.IsCheaperThan(
                //    edges[item], edges[currentMin]) ? item : currentMin);
                
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
        public void LittleSpider(Instance instance, Station station, List<List<Station>> paths, List<Station> _path, bool firstTime = true)
        {
            var path = new List<Station>(_path);
            paths.Add(path);
            path.Add(station);

            if (station.IsPrivate && firstTime == false)
            {
                return;
            }

            foreach (var neighbor in station.Neighbors)
            {
                //if(visited.Contains(neighbor)) continue;
                if (path.Contains(neighbor)) continue; //alert!
                LittleSpider(instance, neighbor, paths, path, false);
            }
            paths.Remove(path); //alert! wydajność!
        }

      
        //DoubleDictionary<Station, List<Station>> edges = new ;

        bool Validate(Station station, List<Station> path)
        {
            if (path.Count <= 2) return false;
            if(!path.First().IsPrivate || !path.Last().IsPrivate) return false;
            Debug.Assert(station == path.First()); //alert
            if (path.Any(item => item != path.First() && item != path.Last() && item.IsPrivate)) return false; //alert to niekonieczne, tylko dla sprawdzenia czy działą
            return true;
        }

        void AddEdges(Station first, DoubleDictionary<Station, List<Station>> edges, List<List<Station>> paths)
        {
            Debug.WriteLine("station " + first.Id.ToString());
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

        public DoubleDictionary<Station, List<Station>> Run(Instance instance)
        {
            Debug.WriteLine("Spider running");
            var edges = new DoubleDictionary<Station, List<Station>>();
            foreach(var privateStation in instance.PrivateStations)
            {
                var paths = new List<List<Station>>();
                LittleSpider(instance, privateStation, paths, new List<Station>());
                AddEdges(privateStation, edges, paths);
            }
            return edges;
        }
    }

    public class SimpleOptimize
    {
        /*List<Station> GetEdge(Station privateA, Station privateB)
        {

        }*/

        public List<Station> Run(Instance instance)
        {
            /*var stations = instance.Stations.*//*Where(station => !station.IsAttached()).*//* //alert!
                Concat<Station>(instance.StationaryStations).ToList();
            return new Dijkstra().Run(instance, stations, stations.Where(item => item.IsAttached()).First(), stations.Where(item => item.IsAttached()).Last());*/
            //return new List<Station>();
            Debug.WriteLine("Simple optimize");

            var edges = new Spider().Run(instance);
            var necessaryEdges = new Kruskal().Run(instance.PrivateStations, edges);
            var necessaryStations = new HashSet<Station>();

            foreach(var necessaryEdge in necessaryEdges)
            {
                edges[necessaryEdge].ForEach(station => necessaryStations.Add(station)); 
            }

            foreach(var station in instance.Stations)
            {
                if(!necessaryStations.Contains(station))
                {
                    instance.MapObjects.Remove(station);
                }
            }

            return instance.AllStations; //alert
        }
    }
}
