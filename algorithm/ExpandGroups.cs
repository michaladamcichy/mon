using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class ExpandGroups
    {
        public (Cost, List<Group>, List<Station>) Run(Cost initialCost, List<Station> lonelyStations, List<Station> coreStations)
        {
            var cost = new Cost(initialCost);
            var assigned = new HashSet<Station>();

            var groups = coreStations.Select(coreStation => new Group(new List<Station>() { coreStation })).ToList();

            foreach(var group in groups)
            {
                var nearestLonelyStations = group.CoreStation.GetNearest(lonelyStations.FindAll(station => !assigned.Contains(station)));

                foreach(var nearest in nearestLonelyStations)
                {
                    if (assigned.Contains(nearest)) continue;
                    var coreNearestDistance = group.CoreStation.GetDistanceFrom(nearest);
                    var minRange = cost.QueryMin(1, coreNearestDistance);

                    if(!nearest.IsInRange(group.CoreStation))
                    {
                        if (minRange == null || !cost.ChangeRange(new List<Station>() { group.CoreStation, nearest }, minRange.Value)) continue; //alert logika
                        assigned.Add(nearest);
                        group.Add(nearest);
                        continue;
                    }
                    if(!group.CoreStation.IsInRange(nearest))
                    {
                        if (minRange == null || !cost.ChangeRange(nearest, minRange.Value)) continue;
                        assigned.Add(nearest);
                        group.Add(nearest);
                        continue;
                    }
                    assigned.Add(nearest);
                    group.Add(nearest);
                }
            }

            return (cost, groups, lonelyStations.FindAll(station => !assigned.Contains(station)));
        }
    }
}
