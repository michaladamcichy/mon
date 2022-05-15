using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        double? MinCoveringRange(Double[] ranges, List<MapObject> mapObjects)
        {
            double minCoveringRadius = MapObject.MinCoveringCircleRadius(mapObjects);
            
            foreach (var range in ranges)
            {
                if (minCoveringRadius <= range)
                {
                    return range;
                }
            }

            return null;
        }

        static void Add(HashSet<Station> stations, Station station, Dictionary<Station, bool> connected)
        {
            stations.Add(station);
            connected[station] = true;
        }

        static void Remove(HashSet<Station> stations, Station station, Dictionary<Station, bool> connected)
        {
            stations.Remove(station);
            connected[station] = false;
        }

        public List<Station> Run(Instance instance)
        {
            instance.MapObjects.RemoveAll(item => item is Station); //alert!!

            var connected = new Dictionary<Station, bool>(); //alert - jak zrobić, żeby były tylko stacje związane z jednostkami?
            

            foreach(var unit in instance.Units)
            {
                var station = new Station(instance.StationRanges.Min()); //alert przydałaby się faza dobierania rozmiaru stacji
                //które są wyłączną własnością jednostek
                //może się okazać, że przy odpowiednim rozmiarze tych stacji gtupa już będzie miała zapewnioną komunikację
                //może zacząć od najmniejszych i próbować zwiększać i patrzeć czy coś zyskuję
                //albo od największych i próbować zmniejszać patrząc czy coś tracę
                instance.MapObjects.Add(station);
                MapObject.Attach(station, unit);
            }

            instance.Stations.ForEach(item => connected[item] = false);
            var solution = new List<Station>(instance.Stations);
            //var tempSolution = new List<Station>(solution);

            foreach(var first in instance.Stations)
            {
                if (connected[first] == true) continue; //alert a może nie należy używać tych connected - może należy pozwolić, aby powstały grupy nakładające się na siebie i wtedy wybrać?

                var temp = new HashSet<Station>();
                Add(temp, first, connected);

                var nearestStations = first.GetNearestStations(instance.MapObjects);

                var removed = new HashSet<Station>();
               
                foreach (var second in nearestStations)
                {
                    if (connected[second]) continue;

                    var minCoveringRangeBeforeAdding = MinCoveringRange(instance.StationRanges, temp.Cast<MapObject>().ToList());
                    Add(temp, second, connected);

                    var minCoveringRangeAfterAdding = MinCoveringRange(instance.StationRanges, temp.Cast<MapObject>().ToList());

                    var center = MapObject.Center(temp.Cast<MapObject>().ToList());
                    //var furthestFromCenter = temp.Max(item => MapObject.Distance(item.Position, center));
                    var furthestFromCenter = temp.Aggregate((first, second) => MapObject.Distance(first.Position, center) > MapObject.Distance(second.Position, center) ? first : second);

                    Remove(temp, furthestFromCenter, connected);
                    removed.Add(furthestFromCenter);
                    var alternativeMinCoveringRange = MinCoveringRange(instance.StationRanges, temp.Cast<MapObject>().ToList());

                    //alert todo! być może opłaca się podejmować poniższą decyzję nie na podstawie minCoveringRange lecz minCoveringRadius??
                    //premiowanie skupienia nawet, jeśli chwilowo nie zmienia to sytuacji ze stacjami?

                    //var minCoveringRanges = new double?[3] {minCoveringRangeBeforeAdding, minCoveringRangeAfterAdding, alternativeMinCoveringRange};

                    //alert ważna decyzja < czy = //chcemy pływanie lub nie, może być ono korzystne
                    if (minCoveringRangeAfterAdding != null && alternativeMinCoveringRange != null && minCoveringRangeAfterAdding <= alternativeMinCoveringRange)
                    {
                        Add(temp, furthestFromCenter, connected);
                        removed.Remove(furthestFromCenter);
                        continue;
                    }

                    if (minCoveringRangeAfterAdding != null && alternativeMinCoveringRange != null && alternativeMinCoveringRange < minCoveringRangeAfterAdding)
                    {
                        continue;
                    }

                    if(minCoveringRangeAfterAdding == null)
                    {
                        if(alternativeMinCoveringRange != null)
                        {
                            continue;
                        }
                    }
                    Add(temp, furthestFromCenter, connected);
                    removed.Remove(furthestFromCenter);
                    Remove(temp, second, connected);
                }

                foreach(var removedStation in removed)
                {
                    if (connected[removedStation]) continue; //alert chyba niepotrzebne

                    var minCoveringRangeBeforeAdding = MinCoveringRange(instance.StationRanges, temp.Cast<MapObject>().ToList());
                    Add(temp, removedStation, connected);

                    var minCoveringRangeAfterAdding = MinCoveringRange(instance.StationRanges, temp.Cast<MapObject>().ToList());

                    if (minCoveringRangeAfterAdding == null)
                    {
                        Remove(temp, removedStation, connected);
                        continue;
                    }
                }

                var range = MinCoveringRange(instance.StationRanges, temp.Cast<MapObject>().ToList());
                if(range == null)
                {
                    return new List<Station>();
                }

                foreach(var stationRange in instance.StationRanges)
                {
                    if(temp.ToList()[0].Range >= stationRange) //alert
                    {
                        continue;
                    }

                    if (Algorithm.IsConnected(temp.ToList()))
                    {
                        break;
                    }

                    temp.ToList().ForEach(item => item.Range = stationRange);
                }

                if (Algorithm.IsConnected(temp.ToList()) && temp.All(item => item.Range <= range))
                {
                    continue;
                }
                //alert to wymaga większego przemyślenia
                //np byla taka sytuacja, ze stacje mialy duzy zasięg  - i algorytm się kończył, a 
                //lepije by  było, jakby miały mały zasięg i wstawić małą stację międzyu nie
                //bo chodzi o to, że linijkka po ifie może zmniejszyć zasięg stacji przy jednostkach

                temp.ToList().ForEach(item => item.Range = range.Value);

                //return temp.ToList();//alert
                solution.Add(new Station(MapObject.Center(temp.Cast<MapObject>().ToList()), range.Value));
            }
                //alert ważne wydaje mi się, że na tym etapie opłaca się operować na stałych zasięgach (ale stałych w obrębie grup)
                //i potem łączyć grupy jak największymi stacjami


            return solution;
        }

        //alert scenariusze łączęnia grup:
        //zbuduje drogę miedzy stacjami
        //powiększ którąś ze stacji!
    }

    public class ConnectionCheckAlgorithm
    {
        public bool Run(Instance instance)
        {
            var units = instance.MapObjects.FindAll(item => item is Unit).Cast<Unit>().ToList();

            if (units.Count == 1 ) return true;
            
            if(units.Count == 0)
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
            
            foreach(var unit in instance.MapObjects.FindAll(item => item is Unit))
            {
                var visited = new Dictionary<MapObject, bool>();
                instance.MapObjects.ForEach(mapObject => visited.Add(mapObject, false));
                DFS(unit, visited);
                bool notAllConnected = visited.Any(item => item.Value == false);
                
                if(notAllConnected)
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

            foreach(var mapObject in start.Receivers)
            {
                if (visited[mapObject] == true) continue;

                DFS(mapObject, visited);
            }
        }
    }

    public class Algorithm
    {
        public static bool IsConnected(Instance instance)
        {
            return new ConnectionCheckAlgorithm().Run(instance);
        }
        public static bool IsConnected(List<Station> stations)
        {
            return new ConnectionCheckAlgorithm().Run(stations);
        }
        public static List<Station> SimpleArrangeAlgorithm(Instance instance)
        {
            return new SimpleArrangeStationsAlgorithm().Run(instance);
        }
    }
}
