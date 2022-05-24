using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class SimpleArrange : IArrangeAlgorithm
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
            //List<Group> grouped = CreateGroups(instance);
            //List<Station> joined = JoinGroups(instance.StationRanges, grouped);

            //return joined;
            return new List<Station>();
        }

        List<Station> ArrangeBetween(double[] ranges, Station first, Station second)
        {
            var stations = new List<Station>();

            var commonRange = ranges.Max();
            first.Range = commonRange;
            second.Range = commonRange;

            var distanceToCover = first.GetDistance(second) - commonRange * 2;

            var stationsCount = (int) Math.Floor(distanceToCover / ranges.Max()) - 1; //alert!
            var stepLength = ranges.Max();
            var stepLat = second.Position.lat - first.Position.lat;
            var stepLng = second.Position.lng - first.Position.lng;

            for (var i = 1; i < stationsCount; i++)
            {
                var station = new Station(new Position(first.Position.lat + stepLat * i, second.Position.lng + stepLng * i), ranges.Max());
                stations.Add(station);
            }

            if(!second.IsInRange(stations.Last()))
            {
                List<Station> additionalStations = ArrangeBetween(ranges, stations.Last(), second);
                stations.AddRange(additionalStations);
            }

            return stations;
        }

        //alert zrobić wersję dla IRangable
        List<Station> ArrangeBetween(IEnumerable<Station> ranges, IRangable first, IRangable second)
        {
            var f = first is Group ? ((Group) first).GetNearest(second) : first;
            var s = second is Group ? ((Group)second).GetNearest(first) : second;

            return ArrangeBetween(ranges, f, s);
            //if(first is Station && second is Station)
            //{
            //    return ArrangeBetween(ranges, (Station)first, (Station)second);
            //}

            //if(first is Group && second is Group)
            //{
            //    return ArrangeBetween(ranges, ((Group)first).GetNearest(second), ((Group)second).GetNearest(first));
            //}
        }

        List<Station> Join(double[] ranges, IDistancable first, IDistancable second)
        {
            var f = first is Group ? ((Group) first).GetNearest(second) : first;
            var s = second is Group? ((Group)second).GetNearest(first) : second;

            return Join(ranges, f, s);
        }

        List<Station> Join(double[] ranges, Station first, Station second) //alert męczące to przekazywanie atrybutów instancji//chociaz może to będzie przydatne przy ograniczeniach??
        {
            if(first.IsInRange(second) && second.IsInRange(first)) //alert założenie symetrii! czy tak ma zostać? niekoniecznie
            {
                return new List<Station>();
            }

            var firstStartRange = first.Range;
            var secondStartRange = second.Range;

            var currentRange = Math.Max(first.Range, second.Range); //alert mogą być lepsze rozwiązania z mniejszymi, chyba
            first.Range = currentRange;
            second.Range = currentRange;

            foreach(var range in ranges)
            {
                if (range < currentRange) continue;
                currentRange = range;

                if(second.IsInRange(first) && first.IsInRange(second)) //alert tu mamy symetrię
                {
                    return new List<Station>();
                }
            }

            first.Range = firstStartRange;
            second.Range = secondStartRange;

            var stationsBetween = ArrangeBetween(ranges, first, second);


            return stationsBetween;
        }

        List<Station> JoinGroups(double[] ranges, List<Group> groups)
        {
            var scheduled = Algorithm.GreedySalesman(groups.Cast<IDistancable>().ToList());

            for(var i = 1; i < scheduled.Count; i++)
            {
                Join(ranges, scheduled[i], scheduled[i - 1]);
            }


            return Group.Flatten(groups);
        }
        List<Group> CreateGroups(Instance instance)
        {
            instance.MapObjects.RemoveAll(item => item is Station); //alert!!

            var connected = new Dictionary<Station, bool>(); //alert - jak zrobić, żeby były tylko stacje związane z jednostkami?

            foreach (var unit in instance.Units)
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

            foreach (var first in instance.Stations)
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

                    if (minCoveringRangeAfterAdding == null)
                    {
                        if (alternativeMinCoveringRange != null)
                        {
                            continue;
                        }
                    }
                    group.Add(furthestFromCenter, connected);
                    removed.Remove(furthestFromCenter);
                    group.Remove(second, connected);
                }

                foreach (var removedStation in removed)
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
                if (range == null)
                {
                    throw new Exception(); //alert
                }

                foreach (var stationRange in instance.StationRanges)
                {
                    if (group.Any(item => item.Range >= stationRange)) //alert
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

            foreach (var station in instance.Stations)
            {
                if (connected[station] == false)
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

}
