using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class ArrangeWithExisting
    {
        public (Cost, List<Station>) AssignToGroups(Cost initialCost, List<Station> lonelyStations, List<Station> coreStations)
        {
            //Alert!
            return (null, null);
            /*
            var cost = new Cost(initialCost);
            var assigned = new HashSet<Station>();
            if(coreStations.Count == 0)
            {
                return (cost, new List<Station>(lonelyStations));
            }

            foreach(var lonely in lonelyStations)
            {
                //alert zasoby
                var nearestCores = lonely.GetNearest(coreStations);
                foreach(var nearest in nearestCores)
                {
                    var minRange = lonely.GetDistanceFrom(nearest);
                    if (cost.QueryMax() != null && cost.QueryMax().Value < minRange) break; //alert return //alert tolerance w tej metodzie
                    if(cost.CanChangeRange(new List<Station>() { lonely, nearest }, cost.QueryMin(1, minRange))) //alert cholibka jak to zrobić optymalnie? :D
                    {
                        //
                    }
                    
                }
                
            }*/
        }

        public List<Station> Run(Instance instance)
        {
            //alert
            return null;

           /* Cost cost = new Cost(instance);

            var lonelyUnits = instance.Units.FindAll(unit => unit.Receivers.Count == 0).ToList();

            var additionalStations = new List<Station>();

            var lonelyStations = new List<Station>();
            foreach(var unit in lonelyUnits)
            {
                var range = cost.GetMin();
                if (range == null) return additionalStations;
                var station = new Station(unit.Position, range.Value);
                lonelyStations.Add(station);
                unit.Attach(station);
            }

            var coreStations = instance.Stations.FindAll(station => !station.IsAttached()).ToList();
            var (newCost, lonelyLeft) = AssignToGroups(cost, lonelyStations, coreStations);

            *//*var (groups, newCost) = (new SimpleCreateGroups(instance)).Run(cost);
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
