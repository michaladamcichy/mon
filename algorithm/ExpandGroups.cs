using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class ExpandGroups
    {
        public (Cost, List<Group>, List<Station>) Run(Cost initialCost, List<Station> lonelyStations, List<Station> coreStations, bool freshStart = false)
        {
            var cost = new Cost(initialCost);
            var assigned = new HashSet<Station>();

            var groups = coreStations.Select(coreStation => new Group(new List<Station>() { coreStation })).ToList();
            if(groups.Count == 0 || !cost.CanGetAny()) return (cost, groups, lonelyStations.FindAll(station => !assigned.Contains(station)));
            
            foreach (var lonely in lonelyStations)
            {
                var group = lonely.GetOneNearest(groups);
                //alert teoretycznie nie najbliższa grupa może być najlepsza - ale chrzanić to
                var minRange = cost.QueryMin(1, lonely.GetDistanceFrom(group.CentralStation)); //alert!!! TOLERANCJA!!! ALERT!
                if (minRange == null) continue;

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

            return (cost, groups, lonelyStations.FindAll(station => !assigned.Contains(station)));
        }
    }
}
