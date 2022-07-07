using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class SimpleCreateGroups
    {
        Instance instance;

        public SimpleCreateGroups(Instance instance)
        {
            this.instance = instance;
        }

        public (Group, Cost) CreateGroup(Station first, Cost initialCost, List<Group> groups)
        {
            var group = new Group(new List<Station> { first });
            var removedStations = new List<Station>();
            if (!initialCost.CanGetAny()) return (group, initialCost);

            var nearestStations = first.GetNearest(instance.Stations);
            
            foreach (var station in nearestStations)
            {
                if (station == first || groups.Any(item => item.Contains(station))) continue; //alert nie powinienem przeglądać wszystkich stacji, tylko najbliższe
                                                                               //tylko jakim warunkiem to sprawdzać?

                var minCoveringRangeAfterAdding = initialCost.QueryMinCoveringRange(group.Stations.Concat<Station>(new List<Station>() { station }).ToList());
                if (minCoveringRangeAfterAdding == null) continue; //alert co ze stacjami nie należacymi do grupy a istniejącymi?
                group.Add(station);


                //probujemy usunac i patrzymy czy poprawia sie skupienie
                if (group.Stations.Count > 2)
                {
                    var furthestFromCenter = MapObject.GetFurthestFrom(group.Stations, MapObject.CenterOfGravity(group.Stations)); //alert
                    if (furthestFromCenter == station) continue; //alert to z jakiegoś powodu było potrzebne, aby zachować właściwość pływania
                    var minCoveringCircleRadiusBeforeRemoving = MapObject.MinCoveringCircleRadius(group.Stations);
                    group.Remove(furthestFromCenter);
                    var minCoveringCircleRadiusAfterRemoving = MapObject.MinCoveringCircleRadius(group.Stations);

                    if (minCoveringCircleRadiusAfterRemoving < minCoveringCircleRadiusBeforeRemoving)
                    {
                        removedStations.Add(furthestFromCenter);
                        continue;
                    }

                    group.Add(furthestFromCenter);
                }
            }

            foreach (var removedStation in removedStations)
            {
                var minCoveringRangeAfterAdding = initialCost.QueryMinCoveringRange(group.Stations.Concat<Station>(new List<Station>() { removedStation }).ToList());

                if (minCoveringRangeAfterAdding == null) continue;
                group.Add(removedStation);
            }

            var minCoveringRange = initialCost.QueryMinCoveringRange(group.Stations);

            if (minCoveringRange == null || !initialCost.CanGet(minCoveringRange.Value, group.Stations.Count + 1))
            {
                return (new Group(new List<Station>() { first }), initialCost);
            }
            initialCost.Get(minCoveringRange.Value, group.Stations.Count + 1);
            group.Stations.ForEach(item => initialCost.ChangeRange(item, minCoveringRange.Value));
            group.Add(new Station(MapObject.MinCoveringCircleCenter(group.Stations), minCoveringRange.Value));

            return (group, initialCost);
        }

        public List<Group> Run()
        {
            Cost initialCost = new Cost(instance);
            //if nie rozbudowujemy tylko zaczynamy od zera //alert!
            instance.MapObjects.RemoveAll(item => item is Station); //alert!!
            Station._id = 0;//alert
            var groups = new List<Group>();

            foreach (var unit in instance.Units)
            {
                var minRange = initialCost.GetMin();
                if(minRange == null) return new List<Group>() { new Group(instance.Stations) } ;
                var station = new Station(instance.StationRanges.Min());
                instance.MapObjects.Add(station);
                unit.Attach(station);
            }

            while(instance.Stations.Any(station => groups.All(group => !group.Contains(station)))) //alert może jakoś zgrabniej?
            {
                foreach (var station in instance.Stations)
                {
                    if (groups.Any(group => group.Contains(station))) continue;
                    var (group, cost) = CreateGroup(station, initialCost, groups);
                    groups.Add(group);
                }
            }

            return groups; //alert
        }

        /*public List<Group> RRun()
        {
            instance.MapObjects.RemoveAll(item => item is Station); //alert!!


            //alert
            //return new List<Group>();
            var connected = new Dictionary<Station, bool>(); //alert - jak zrobić, żeby były tylko stacje związane z jednostkami?

            foreach (var unit in instance.Units)
            {
                var station = new Station(instance.StationRanges.Min()); //alert przydałaby się faza dobierania rozmiaru stacji
                //które są wyłączną własnością jednostek
                //może się okazać, że przy odpowiednim rozmiarze tych stacji gtupa już będzie miała zapewnioną komunikację
                //może zacząć od najmniejszych i próbować zwiększać i patrzeć czy coś zyskuję
                //albo od największych i próbować zmniejszać patrząc czy coś tracę
                instance.MapObjects.Add(station);
                unit.Attach(station);
            }

            instance.Stations.ForEach(item => connected[item] = false);
            var groups = new List<Group>();

            foreach (var first in instance.Stations)
            {
                if (connected[first] == true) continue; //alert a może nie należy używać tych connected - może należy pozwolić, aby powstały grupy nakładające się na siebie i wtedy wybrać?

                var group = new Group();
                group.Add(first, connected);

                var nearestStations = first.GetNearest(instance.Stations);

                var removed = new List<Station>();

                foreach (var second in nearestStations)
                {
                    if (connected[second]) continue;

                    var minCoveringRangeBeforeAdding = MapObject.MinCoveringRange(instance.StationRanges, group.Stations);
                    group.Add(second, connected);

                    var minCoveringRangeAfterAdding = MapObject.MinCoveringRange(instance.StationRanges, group.Stations);

                    var center = MapObject.Center(group.Stations);
                    //var furthestFromCenter = temp.Max(item => MapObject.Distance(item.Position, center));
                    var furthestFromCenter = group.Stations.Aggregate((first, second) => MapObject.Distance(first.Position, center) > MapObject.Distance(second.Position, center) ? first : second);

                    group.Remove(furthestFromCenter, connected);
                    removed.Add(furthestFromCenter);
                    var alternativeMinCoveringRange = MapObject.MinCoveringRange(instance.StationRanges, group.Stations);

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

                    var minCoveringRangeBeforeAdding = MapObject.MinCoveringRange(instance.StationRanges, group.Stations);
                    group.Stations.Add(removedStation);
                    var minCoveringRangeAfterAdding = MapObject.MinCoveringRange(instance.StationRanges, group.Stations);

                    if (minCoveringRangeAfterAdding == null)
                    {
                        group.Remove(removedStation, connected);
                        removedStation.Range = instance.StationRanges.Min();
                        continue;
                    }
                }

                var range = MapObject.MinCoveringRange(instance.StationRanges, group.Stations);
                if (range == null)
                {
                    throw new Exception(); //alert
                }

                foreach (var stationRange in instance.StationRanges)
                {
                    if (group.Stations.Any(item => item.Range >= stationRange)) //alert
                    {
                        continue;
                    }

                    if (Algorithm.IsConnected(group.Stations))
                    {
                        break;
                    }

                    group.Stations.ForEach(item => item.Range = stationRange);
                }

                if (Algorithm.IsConnected(group.Stations) && group.Stations.All(item => item.Range <= range))
                {
                    groups.Add(group);
                    continue;
                }
                //alert to wymaga większego przemyślenia
                //np byla taka sytuacja, ze stacje mialy duzy zasięg  - i algorytm się kończył, a 
                //lepije by  było, jakby miały mały zasięg i wstawić małą stację międzyu nie
                //bo chodzi o to, że linijkka po ifie może zmniejszyć zasięg stacji przy jednostkach

                group.Stations.ForEach(item => item.Range = range.Value);

                //return temp.ToList();//alert

                group.Stations.Add(new Station(MapObject.Center(group.Stations), range.Value));
                groups.Add(group);
            }

            foreach (var station in instance.Stations)
            {
                if (connected[station] == false)
                {
                    var group = new Group();
                    group.Stations.Add(station);
                    groups.Add(group);
                }
            }
            //alert ważne wydaje mi się, że na tym etapie opłaca się operować na stałych zasięgach (ale stałych w obrębie grup)
            //i potem łączyć grupy jak największymi stacjami

            return groups;
        }*/
    }
}
