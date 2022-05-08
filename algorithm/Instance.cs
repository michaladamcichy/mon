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
        }

        List<MapObject> prepareMapObjects(List<Station> stations, List<Unit> units)
        {
            var mapObjects = stations.Cast<MapObject>().Concat(units.Cast<MapObject>()).ToList();

            foreach (MapObject first in mapObjects)
            {
                foreach (MapObject second in mapObjects)
                {
                    if (first == second || second is not Station) continue;

                    if (MapObject.Distance(first, second) <= ((Station)second).range)
                    {
                        first.hosts.Add(second);
                        second.clients.Add(first);
                    }
                }
            }

            return mapObjects;
        } 
    }
}
