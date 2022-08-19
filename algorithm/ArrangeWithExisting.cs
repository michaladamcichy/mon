using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class ArrangeWithExisting
    {
        public (Cost, List<Group>, List<Group>) AssignToGroups(Instance instance, Cost initialCost, List<Station> lonelyStations, bool freshStart = false)
        {
            var cost = new Cost(initialCost);

            var (newCost, oldGroups, lonelyLeft) = (new ExpandGroups()).Run(cost, lonelyStations, instance.CoreStations, freshStart);
            cost = new Cost(newCost);
            var (newGroups, otherNewCost) = (new SimpleCreateGroups(new Instance(lonelyLeft, instance.Ranges, instance.Counts))).Run(cost); //alert czy to zadziała?
            cost = new Cost(otherNewCost);

            var stationaryGroups = oldGroups.Where(group => group.CentralStation.IsStationary);
            oldGroups = oldGroups.Where(group => group.CentralStation.IsMobile).ToList();
            newGroups.AddRange(stationaryGroups);
            return (cost, oldGroups, newGroups);
        }

        public List<Station> Run(Instance instance)
        {
            Cost cost = new Cost(instance);
            var freshStart = instance.MobileStations.Count == 0;

            var lonelyUnits = instance.Units.FindAll(unit => !unit.HasAttachement()).ToList();
            
            var lonelyStations = new List<Station>();
            foreach(var unit in lonelyUnits)
            {
                var range = cost.GetMin();
                if (range == null) return instance.Stations;
                var station = new Station(unit.Position, range.Value);
                lonelyStations.Add(station);
                instance.Stations.Add(station);
                unit.Attach(station);
            }

            var (newCost, oldGroups, newGroups) = AssignToGroups(instance, cost, lonelyStations, freshStart);
            //return Group.Flatten(oldGroups.Concat<Group>(newGroups).ToList());
            cost = new Cost(newCost);
            //alert
            var connected = oldGroups.Select(group => group.CentralStation).ToList(); //alert wydajność - zbiory haszujące
            
            var notConnected = newGroups.Select(group => group.CentralStation).ToList();

            instance.CreateRelations();

            var (allStations, otherNewCost, edges) =
                JoinNearestNeighbors.Run(cost, instance, notConnected, connected);
            cost = new Cost(otherNewCost);
            
            instance.PrivateStations.Where(station => !allStations.Contains(station)).ToList().ForEach(station => allStations.Add(station));
            return allStations;
        }

    }
}
