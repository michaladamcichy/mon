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

        (Group?, Cost) AdjustGroup(Group _group, Cost initialCost)
        {
            var group = new Group(_group);
            Cost cost = new Cost(initialCost);

            while(group.Stations.Count > 1)
            {
                var minCoveringRange = cost.QueryMinCoveringRange(group.Stations);
                if (minCoveringRange != null && cost.CanGet(minCoveringRange.Value, group.Stations.Count + 1))
                {
                    cost.Get(minCoveringRange.Value, group.Stations.Count);
                    return (group, cost);
                }

                var furthestFromCenter = group.GetFurthestFromCenter();
                group.Remove(furthestFromCenter);
            }

            return (null, cost);
        }

        (Group?, Cost) CreateGroup(Station first, Cost initialCost, List<Group> groups)
        {
            var cost = new Cost(initialCost);
            var group = new Group(new List<Station> { first });
            var removedStations = new List<Station>();
            if (!cost.CanGetAny()) return (null, initialCost);

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
                    var furthestFromCenter = MapObject.GetFurthestFrom(group.Stations, MapObject.CenterOfGravity(group.Stations)); //alert zamienić na metodę Group
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
            if (minCoveringRange == null) throw new Exception();


            var temporaryCost = new Cost(cost); //alert można fajniej
            temporaryCost.Get(minCoveringRange.Value);
            if (!temporaryCost.CanChangeRange(group.Stations, minCoveringRange.Value))
            {
                var (adjustedGroup, otherNewCost) = AdjustGroup(group, cost); //alert! critical alert!!!
                //Group adjustedGroup = null;
                //Cost otherNewCost = new Cost(cost);

                if (adjustedGroup != null)
                {
                    var adjustedMinCoveringRange = otherNewCost.GetMinCoveringRange(adjustedGroup.Stations);
                    adjustedGroup.Add(new Station(MapObject.MinCoveringCircleCenter(adjustedGroup.Stations), adjustedMinCoveringRange.Value));
                    return (adjustedGroup, otherNewCost);
                }

                var range = cost.GetMin().Value;
                return (new Group(new List<Station>() { first, new Station(first.Position, range)}), cost);
            }

            group.Stations.ForEach(item => cost.ChangeRange(item, minCoveringRange.Value));
            cost.Get(minCoveringRange.Value);
            group.Add(new Station(MapObject.MinCoveringCircleCenter(group.Stations), minCoveringRange.Value));

            return (group, cost);
        }

        public (List<Group>, Cost) Run(Cost initialCost)    
        {
            Cost cost = new Cost(initialCost);
            //if nie rozbudowujemy tylko zaczynamy od zera //alert!
            instance.MapObjects.RemoveAll(item => item is Station); //alert!!
            Station._id = 0;//alert
            var groups = new List<Group>();

            foreach (var unit in instance.Units)
            {
                var minRange = cost.GetMin();
                if(minRange == null) return (new List<Group>() { new Group(instance.Stations) }, cost) ;
                var station = new Station(minRange.Value);
                instance.MapObjects.Add(station);
                unit.Attach(station);
            }

            while(instance.Stations.Any(station => groups.All(group => !group.Contains(station)))) //alert może jakoś zgrabniej?
            {
                foreach (var station in instance.Stations)
                {
                    if (groups.Any(group => group.Contains(station))) continue;

                    Group lastGroup = null;
                    Dictionary<Station, double> groupSnapshot = null;
                    Group group = null;
                    Cost groupCost = null;
                    var reversedRanges = new List<double>(instance.StationRanges.ToList());
                    reversedRanges.Reverse();

                    var rangesSnapshot = instance.SaveRangesSnapshot(); //alert!
                    foreach (var range in reversedRanges)
                    {
                        instance.RestoreRangesSnapshot(rangesSnapshot);
                        var (_group, newCost) = CreateGroup(station, cost, groups);
                        
                        if(_group != null)
                        {
                            if(lastGroup == null || _group.Stations.Count > lastGroup.Stations.Count)
                            {
                                group = _group;
                                groupCost = newCost;
                                groupSnapshot = instance.SaveRangesSnapshot();
                            }

                            lastGroup = _group;
                        }
                        cost.AddForbiddenRange(range);
                    }
                    cost.RemoveAllForbiddenRanges();
                    
                    if (group == null)
                    {
                        group = new Group(new List<Station>() { station }); //alert nie przemyślane 
                    }
                    else
                    {
                        instance.RestoreRangesSnapshot(groupSnapshot);
                        groupCost.RemoveAllForbiddenRanges();
                        cost = new Cost(groupCost);
                    }
                    groups.Add(group);
                }
            }

            return (groups, cost); //alert
        }
    }
}
