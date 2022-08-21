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

        (Group, Cost, double) AdjustGroup(Group _group, Cost initialCost)
        {
            var group = new Group(_group);
            Cost cost = new Cost(initialCost);

            while(group.Stations.Count > 1)
            {
                var minCoveringRange = cost.QueryMinCoveringRange(group.Stations);
                var minCoveringCircleCenter = MapObject.MinCoveringCircleCenter(group.Stations);
                if (minCoveringRange != null && cost.MakeGroup(group.Stations, new MapObject(minCoveringCircleCenter), minCoveringRange.Value))
                {
                    return (group, cost, minCoveringRange.Value);
                }

                var furthestFromCenter = group.GetFurthestFromCenter();
                group.Remove(furthestFromCenter);
            }

            return (null, cost, 0.0);
        }

        (Group?, Cost) CreateGroup(Station first, Cost initialCost, List<Group> groups)
        {
            var cost = new Cost(initialCost);
            var group = new Group(new List<Station> { first });
            var removedStations = new List<Station>();
            if (!cost.CanGetAny()) return (null, initialCost);

            var nearestStations = first.GetNearest(instance.Stations);
            if(nearestStations.Count == 0) return (new Group(new List<Station>() { first, new Station(first.Position, cost.GetMin().Value) }), cost);
            foreach (var station in nearestStations)
            {
                if (station == first || groups.Any(item => item.Contains(station))) continue;

                var minCoveringRangeAfterAdding = cost.QueryMinCoveringRange(group.Stations.Concat<Station>(new List<Station>() { station }).ToList());
                if (minCoveringRangeAfterAdding == null) continue; //alert co ze stacjami nie należacymi do grupy a istniejącymi?
                group.Add(station);

                //probujemy usunac i patrzymy czy poprawia sie skupienie
                if (group.Stations.Count > 2)
                {
                    var furthestFromCenter = MapObject.GetFurthestFrom(group.Stations, MapObject.MinCoveringCircleCenter(group.Stations)); //alert zamienić na metodę Group
                    if (furthestFromCenter == station) continue; //alert to było potrzebne, aby zachować właściwość pływania
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
            var minCoveringCircleCenter = MapObject.MinCoveringCircleCenter(group.Stations);
            if (minCoveringRange == null) throw new Exception();

            if (!cost.MakeGroup(group.Stations, new MapObject(minCoveringCircleCenter), minCoveringRange.Value))
            {
                var (adjustedGroup, otherNewCost, centralRange) = AdjustGroup(group, cost); //alert! critical alert!!!
                //Group adjustedGroup = null;
                //Cost otherNewCost = new Cost(cost);

                if (adjustedGroup != null)
                {
                    adjustedGroup.Add(new Station(MapObject.MinCoveringCircleCenter(adjustedGroup.Stations), centralRange));
                    return (adjustedGroup, otherNewCost);
                }

                var range = cost.GetMin().Value;
                return (new Group(new List<Station>() { first, new Station(first.Position, range)}), cost);
            }

            group.Add(new Station(minCoveringCircleCenter, minCoveringRange.Value));

            return (group, cost);
        }

        public (List<Group>, Cost) Run(Cost initialCost)    
        {
            Cost cost = new Cost(initialCost);
            //if nie rozbudowujemy tylko zaczynamy od zera //alert!
            //Station._id = 0;//alert
            var groups = new List<Group>();

           /* foreach (var unit in instance.Units) //alert tego już nie potrzeba!
            {
                var minRange = cost.GetMin();
                if(minRange == null) return (new List<Group>() { new Group(instance.Stations) }, cost) ;
                var station = new Station(minRange.Value);
                instance.Stations.Add(station);
                unit.Attach(station);
            }*/

            while(instance.Stations.Any(station => groups.All(group => !group.Contains(station)))) //alert może jakoś zgrabniej?
            {
                foreach (var station in instance.Stations)
                {
                    if (groups.Any(group => group.Contains(station))) continue;

                    Group lastGroup = null;
                    Dictionary<Station, double> groupSnapshot = null;
                    Group group = null;
                    Cost groupCost = null;
                    var reversedRanges = new List<double>(instance.Ranges.ToList());
                    reversedRanges.Reverse();

                    var rangesSnapshot = instance.SaveRangesSnapshot(); //alert!
                    foreach (var range in reversedRanges)
                    {
                        Instance.RestoreRangesSnapshot(rangesSnapshot);
                        var (_group, newCost) = CreateGroup(station, cost, groups);
                        
                        if(_group != null)
                        {
                            if(group == null /*|| Cost.CalculateCostPerStation(_group.Stations) < Cost.CalculateCostPerStation(group.Stations)*/) //alert dodałem =
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
                        Instance.RestoreRangesSnapshot(groupSnapshot);
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