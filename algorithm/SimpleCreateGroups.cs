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
            var cost = new Cost(initialCost);
            var group = new Group(new List<Station> { first });
            var removedStations = new List<Station>();
            if (!cost.CanGetAny()) return (group, cost);

            var nearestStations = first.GetNearest(instance.Stations);
            
            foreach (var station in nearestStations)
            {
                if (station == first || groups.Any(item => item.Contains(station))) continue; //alert nie powinienem przeglądać wszystkich stacji, tylko najbliższe
                                                                               //tylko jakim warunkiem to sprawdzać?

                var minCoveringRangeAfterAdding = cost.QueryMinCoveringRange(group.Stations.Concat<Station>(new List<Station>() { station }).ToList());
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
                var minCoveringRangeAfterAdding = cost.QueryMinCoveringRange(group.Stations.Concat<Station>(new List<Station>() { removedStation }).ToList());

                if (minCoveringRangeAfterAdding == null) continue;
                group.Add(removedStation);
            }

            var minCoveringRange = cost.QueryMinCoveringRange(group.Stations);

            if (minCoveringRange == null || !cost.CanGet(minCoveringRange.Value, group.Stations.Count + 1))
            {
                return (new Group(new List<Station>() { first }), cost);
            }

            //alert - powyższe można bardziej inteligentnie - np. spróbować zrobić grupę z mniejszymi rozmiarami stacji
            //lub spróbować zrobić grupę z dużą stacją z tyloma stacjami ile się da

            initialCost.Get(minCoveringRange.Value, group.Stations.Count + 1);
            group.Stations.ForEach(item => initialCost.ChangeRange(item, minCoveringRange.Value));
            


            foreach (var station in group.Stations)
            {
                if (cost.CanChangeRange(station, ))
            }
            group.Add(new Station(MapObject.MinCoveringCircleCenter(group.Stations), minCoveringRange.Value));

            return (group, cost);
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
    }
}
