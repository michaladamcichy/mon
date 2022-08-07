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

            foreach(var lonely in lonelyStations)
            {
                var group = lonely.GetOneNearest(groups);
                //alert teoretycznie nie najbliższa grupa może być najlepsza - ale chrzanić to
                var minRange = cost.QueryMin(1, lonely.GetDistanceFrom(group.CentralStation));

                if (!lonely.IsInRange(group.CentralStation))
                {
                    if (minRange == null || !cost.ChangeRange(new List<Station>() { group.CentralStation, lonely }, minRange.Value)) continue; //alert logika
                }
                if (!group.CentralStation.IsInRange(lonely))
                {
                    if (minRange == null || !cost.ChangeRange(lonely, minRange.Value)) continue;
                }
                assigned.Add(lonely);
                group.Add(lonely);
            }

            /*foreach(var group in groups)
            {
                var nearestLonelyStations = group.CentralStation.GetNearest(lonelyStations.FindAll(station => !assigned.Contains(station)));

                foreach(var nearest in nearestLonelyStations)
                {
                    if (assigned.Contains(nearest)) continue;
                    var coreNearestDistance = group.CentralStation.GetDistanceFrom(nearest);
                    var minRange = cost.QueryMin(1, coreNearestDistance);

                    if(!nearest.IsInRange(group.CentralStation))
                    {
                        if (minRange == null || !cost.ChangeRange(new List<Station>() { group.CentralStation, nearest }, minRange.Value)) continue; //alert logika
                        assigned.Add(nearest);
                        group.Add(nearest);
                        continue;
                    }
                    if(!group.CentralStation.IsInRange(nearest))
                    {
                        if (minRange == null || !cost.ChangeRange(nearest, minRange.Value)) continue;
                        assigned.Add(nearest);
                        group.Add(nearest);
                        continue;
                    }
                    assigned.Add(nearest);
                    group.Add(nearest);
                }
            }*/

            return (cost, groups, lonelyStations.FindAll(station => !assigned.Contains(station)));
        }
    }
}
