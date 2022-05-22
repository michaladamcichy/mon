using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        static void Add(List<Station> stations, Station station, Dictionary<Station, bool> connected)
        {
            stations.Add(station);
            connected[station] = true;
        }

        static void Remove(List<Station> stations, Station station, Dictionary<Station, bool> connected)
        {
            stations.Remove(station);
            connected[station] = false;
        }

        public List<Station> Run(Instance instance)
        {
            List<Group> grouped = CreateGroups(instance);
            List<Station> joined = JoinGroups(grouped);

            return joined;
        }

        //public Dictionary<Tuple<Group, Group>, double> CreateMatrix(List<Group> groups)
        //{
        //    var matrix = new Dictionary<Tuple<Group, Group>, double>;

        //    foreach(var first in groups)
        //    {
        //        foreach(var second in groups)
        //        {
        //            if(first == second)
        //            {
        //                //matrix[]
        //            }
        //        }
        //    }

        //    return matrix;
        //}

        //public List<Group> SalesmanRoute(List<Group> groups)
        //{
        //    var pairs = new List<Tuple<Group, Group>>();

        //    groups.ForEach(first => { groups.ForEach(second => { pairs.Add(new Tuple<Group, Group>(first, second)); });

        //    //Group

        //    return groups; //alert
        //}

        List<Station> JoinGroups(List<Group> groups)
        {
            return Group.Flatten(groups);
        }
        List<Group> CreateGroups(Instance instance)
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
            var groups = new List<Group>();
            
            foreach(var first in instance.Stations)
            {
                if (connected[first] == true) continue; //alert a może nie należy używać tych connected - może należy pozwolić, aby powstały grupy nakładające się na siebie i wtedy wybrać?

                var group = new Group();
                group.Add(first, connected);

                var nearestStations = first.GetNearestStations(instance.MapObjects);

                var removed = new List<Station>();
               
                foreach (var second in nearestStations)
                {
                    if (connected[second]) continue;

                    var minCoveringRangeBeforeAdding = MinCoveringRange(instance.StationRanges, group.ToMapObjects());
                    group.Add(second, connected);

                    var minCoveringRangeAfterAdding = MinCoveringRange(instance.StationRanges, group.ToMapObjects());

                    var center = MapObject.Center(group.ToMapObjects());
                    //var furthestFromCenter = temp.Max(item => MapObject.Distance(item.Position, center));
                    var furthestFromCenter = group.Aggregate((first, second) => MapObject.Distance(first.Position, center) > MapObject.Distance(second.Position, center) ? first : second);

                    group.Remove(furthestFromCenter, connected);
                    removed.Add(furthestFromCenter);
                    var alternativeMinCoveringRange = MinCoveringRange(instance.StationRanges, group.ToMapObjects());

                    //alert todo! być może opłaca się podejmować poniższą decyzję nie na podstawie minCoveringRange lecz minCoveringRadius??
                    //premiowanie skupienia nawet, jeśli chwilowo nie zmienia to sytuacji ze stacjami?

                    //var minCoveringRanges = new double?[3] {minCoveringRangeBeforeAdding, minCoveringRangeAfterAdding, alternativeMinCoveringRange};

                    //alert ważna decyzja < czy = //chcemy pływanie lub nie, może być ono korzystne
                    if (minCoveringRangeAfterAdding != null && alternativeMinCoveringRange != null && minCoveringRangeAfterAdding <= alternativeMinCoveringRange)
                    {
                        group.Add(furthestFromCenter, connected);
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
                    group.Add(furthestFromCenter, connected);
                    removed.Remove(furthestFromCenter);
                    group.Remove(second, connected);
                }

                foreach(var removedStation in removed)
                {
                    if (connected[removedStation]) continue; //alert chyba niepotrzebne

                    var minCoveringRangeBeforeAdding = MinCoveringRange(instance.StationRanges, group.ToMapObjects());
                    group.Add(removedStation, connected);

                    var minCoveringRangeAfterAdding = MinCoveringRange(instance.StationRanges, group.ToMapObjects());

                    if (minCoveringRangeAfterAdding == null)
                    {
                        group.Remove(removedStation, connected);
                        removedStation.Range = instance.StationRanges.Min();
                        continue;
                    }
                }

                var range = MinCoveringRange(instance.StationRanges, group.ToMapObjects());
                if(range == null)
                {
                    Assert.IsTrue(false); //alert
                }

                foreach(var stationRange in instance.StationRanges)
                {
                    if(group.Any(item => item.Range >= stationRange)) //alert
                    {
                        continue;
                    }

                    if (Algorithm.IsConnected(group.ToList()))
                    {
                        break;
                    }

                    group.ToList().ForEach(item => item.Range = stationRange);
                }

                if (Algorithm.IsConnected(group.ToList()) && group.All(item => item.Range <= range))
                {
                    groups.Add(group);
                    continue;
                }
                //alert to wymaga większego przemyślenia
                //np byla taka sytuacja, ze stacje mialy duzy zasięg  - i algorytm się kończył, a 
                //lepije by  było, jakby miały mały zasięg i wstawić małą stację międzyu nie
                //bo chodzi o to, że linijkka po ifie może zmniejszyć zasięg stacji przy jednostkach

                group.ToList().ForEach(item => item.Range = range.Value);

                //return temp.ToList();//alert
                
                group.Add(new Station(MapObject.Center(group.ToMapObjects()), range.Value));
                groups.Add(group);
            }

            foreach(var station in instance.Stations)
            {
                if(connected[station] == false)
                {
                    var group = new Group();
                    group.Add(station);
                    groups.Add(group);
                }
            }
                //alert ważne wydaje mi się, że na tym etapie opłaca się operować na stałych zasięgach (ale stałych w obrębie grup)
                //i potem łączyć grupy jak największymi stacjami

            return groups;
        }

        //alert scenariusze łączęnia grup:
        //zbuduje drogę miedzy stacjami
        //powiększ którąś ze stacji!
        //pytanie: czy lepiej powiększać czy lepiej dostawiać
        //a może może lepiej dążyć do równych rozmiarów stacji sąsiadujących?
        //może to jest klucz do dobrego planaowania?
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
