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
            cost = new Cost(newCost);
            //alert
            var connected = oldGroups.Select(group => group.CentralStation).ToList(); //alert wydajność - zbiory haszujące
            
            var notConnected = newGroups.Select(group => group.CentralStation).ToList();

            var (allStations, otherNewCost, edges) =
                JoinNearestNeighbors.Run(cost, instance, notConnected, connected);
            cost = new Cost(otherNewCost);

            
            return allStations.Concat<Station>(instance.PrivateStations).ToList();
            //alert 

            //alert
           // if (!noInitialMobileStations) return allStations.Concat<Station>(joiningStations).Concat<Station>(instance.PrivateStations).ToList();

/*            joiningStations.ForEach(joining => { if (!instance.MapObjects.Contains(joining)) instance.MapObjects.Add(joining); });
            allStations.ForEach(station => { if (!instance.MapObjects.Contains(station)) instance.MapObjects.Add(station); });*/
            //instance = new Instance(instance.Stations, instance.Units, instance.StationCounts);
            //alert ta instance nie ma przeliczonych relacji!!!
            //var ret = new SimpleOptimize().Run(instance, edges);

            
            //return ret.Concat<Station>(allStations.Where(station => station.IsPrivate && !instance.MapObjects.Contains(station))).ToList();
            //return allStations.Concat<Station>(joiningStations).ToList();
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
