using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class InstanceJSON
    {
        public double[] stationRanges { get; set; } = new double[0];
        public int[] stationCounts { get; set; } = new int[0];

        public List<StationJSON> stations { get; set; } = new List<StationJSON>();
        public List<UnitJSON> units { get; set;  } = new List<UnitJSON>();

        public InstanceJSON(double[] stationRanges, int[] stationCounts, List<StationJSON> stations, List<UnitJSON> units)
        {
            this.stationRanges = stationRanges;
            this.stationCounts = stationCounts;
            this.stations = stations;
            this.units = units;
        }
    }
    public class Instance
    {
        public double[] StationRanges { get; set; } = new double[0];
        public int[] StationCounts { get; set; } = new int[0];
        public List<int> Priorities { get { return getPriorities(); } }

        public List<MapObject> MapObjects { get; set; } = new List<MapObject>(); //alert! setter

        public List<Station> Stations { get { return MapObjects.FindAll(item => item is Station).Cast<Station>().ToList(); } }

        public List<Unit> Units { get { return MapObjects.FindAll(item => item is Unit).Cast<Unit>().ToList(); } }

        public Instance(InstanceJSON instanceJSON)
        {
            this.StationRanges = instanceJSON.stationRanges;
            this.StationCounts = instanceJSON.stationCounts;
            var stations = instanceJSON.stations.Select(item => new Station(item)).ToList();
            var units = instanceJSON.units.Select(item => new Unit(item)).ToList();
            this.MapObjects = prepareMapObjects(stations, units);

            int id = 0; //alert brzydko
            this.Stations.ForEach(station => { station.id = ++id; });
            Station._id = id;
        }

        public Instance(List<Station> stations, List<Unit> units)
        {
            var ranges = new List<double>();
            stations.ForEach(item => ranges.Add(item.Range));

            this.StationRanges = ranges.ToArray();
            this.StationCounts = new int[3] { 1000, 1000, 1000 }; // alert!

            this.MapObjects = prepareMapObjects(stations, units);
        }

        public Instance(List<Station> stations) : this(stations, new List<Unit>()) {}

        List<MapObject> prepareMapObjects(List<Station> stations, List<Unit> units)
        {
            var mapObjects = stations.Cast<MapObject>().Concat(units.Cast<MapObject>()).ToList();

            foreach(var first in mapObjects)
            {
                foreach(var second in stations)
                {
                    if (first == second) continue;

                    if(first.IsInRange(second))
                    {
                        first.AddSender(second);
                        second.AddReceiver(first);
                    }
                }
            }

            foreach(var station in stations)
            {
                foreach(var unit in units)
                {
                    if(!unit.HasAttachement() && !station.IsAttached() && station.Position.Equals(unit.Position)) //alert to powinno byc property
                    {
                        unit.Attach(station);
                    }
                }
            }

            return mapObjects;
        }

        public bool CanAcquireStation(double range, int howMany = 1) //alert stations czy station??
        {
            var index = StationRanges.ToList().FindIndex(item => item == range);
            if (index == -1) return false;
            return StationCounts[index] >= howMany;
        }

        public Station? AquireStation(List<Station> stations)
        {
            double minCoveringRadius = MapObject.MinCoveringCircleRadius(stations);

            for(int i =0; i < StationRanges.Count(); i++)
            {
                if(StationRanges[i] <= minCoveringRadius && StationCounts[i] > 0)
                {
                    StationCounts[i]--;
                    return new Station(StationRanges[i]);
                }
            }

            return null;
        }

        public Station? AquireMinStation() //alert walnąć jednolinijkowca
        {
            for (int i = 0; i < StationRanges.Count(); i++)
            {
                if (StationCounts[i] > 0)
                {
                    StationCounts[i]--;
                    return new Station(StationRanges[i]);
                }
            }

            return null;
        }

        public Station? AquireMaxStation()
        {
            for (int i = StationRanges.Count() - 1; i >= 0; i--)
            {
                if (StationCounts[i] > 0)
                {
                    StationCounts[i]--;
                    return new Station(StationRanges[i]);
                }
            }

            return null;
        }

        public void GiveBackStation(Station station)
        {
            for(int i=0; i < StationRanges.Count(); i++)
            {
                if(StationRanges[i] == station.Range)
                {
                    StationCounts[i]++;
                }
            }
        }

        List<int> getPriorities()
        {
            var priorities = Units.Select(unit => unit.Priority).ToHashSet().ToList();
            priorities.Sort();
            return priorities;
        }
    }
}
