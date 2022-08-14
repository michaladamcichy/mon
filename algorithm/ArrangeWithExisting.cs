using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class ArrangeWithExisting
    {
        public (Cost, List<Group>, List<Group>) AssignToGroups(Cost initialCost, List<Station> lonelyStations, List<Station> coreStations, int[] counts)
        {
            var cost = new Cost(initialCost);
            var (newCost, groups, lonelyLeft) = (new ExpandGroups()).Run(cost, lonelyStations, coreStations);
            cost = new Cost(newCost);
            var (newGroups, otherNewCost) = (new SimpleCreateGroups(new Instance(lonelyLeft, counts))).Run(cost); //alert czy to zadziała?
            cost = new Cost(otherNewCost);

            return (cost, groups, newGroups);
        }

        public List<Station> Run(Instance instance)
        {
            /*instance.UpdateCounts();*/ //alert
            Cost cost = new Cost(instance);
            
            var lonelyUnits = instance.Units.FindAll(unit => !unit.HasAttachement()).ToList(); //alert zmieniłem tutaj
            var additionalStations = new List<Station>();

            var lonelyStations = new List<Station>();
            foreach(var unit in lonelyUnits)
            {
                var range = cost.GetMin();
                if (range == null) return lonelyStations.Concat<Station>(instance.Stations).ToList(); //alert
                var station = new Station(unit.Position, range.Value);
                lonelyStations.Add(station);
                unit.Attach(station);
            }

            var coreStations = instance.Stations.FindAll(station => station.IsCore).ToList(); //alert a dlaczego nie zrobiłem is not attached?
            var (newCost, oldGroups, newGroups) = AssignToGroups(cost, lonelyStations, coreStations, instance.StationCounts);
            cost = new Cost(newCost);

            var newStations = Group.Flatten(oldGroups.Concat<Group>(newGroups).ToList());
            var allStations = new List<Station>(newStations);
            instance.Stations.ForEach(station => { if (!newStations.Contains(station)) allStations.Add(station); });

            var connected = new HashSet<Station>();
            oldGroups.Where(group => !group.CentralStation.IsStationary).ToList().ForEach(group => connected.Add(group.CentralStation));
            var (joiningStations, otherNewCost) =
                JoinNearestNeighbors.Run(cost, instance, allStations.FindAll(station => !station.IsAttached()), connected);
            cost = new Cost(otherNewCost);
             
            return allStations.Concat<Station>(joiningStations).ToList();
            /*var (groups, newCost) = (new SimpleCreateGroups(instance)).Run(cost);
            cost = new Cost(newCost);
            if (instance.Units.Any(unit => !unit.HasAttachement())) return Group.Flatten(groups).Concat<Station>(instance.StationaryStations).ToList(); //alert data flow po kryjomu modyfikuje instnace.Stations

            var coreStations = groups.FindAll(group => group.CoreStation != null).Select(group => group.CoreStation).ToList();

            var (additionalStations, otherNewCost) = JoinNearestNeighbors.Run(cost, coreStations, instance.StationaryStations, instance.UnitsConnectedToStationaryStations);*//*
            cost = new Cost(otherNewCost);
            //var additionalStations = new List<Station>();
            return Group.Flatten(groups).Concat<Station>(additionalStations).ToList().Concat<Station>(instance.StationaryStations).ToList();*/
        }

    }
}
