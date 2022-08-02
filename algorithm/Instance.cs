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
        public double maxAffordableDistance { get; set; } = 0.0;

        public InstanceJSON(double[] stationRanges, int[] stationCounts, List<StationJSON> stations, List<UnitJSON> units, double maxAffordableDistance)
        {
            this.stationRanges = stationRanges;
            this.stationCounts = stationCounts;
            this.stations = stations;
            this.units = units;
            this.maxAffordableDistance = maxAffordableDistance;
        }
    }
    public class Instance
    {
        public double[] StationRanges { get; set; } = new double[0];
        public List<int> Priorities { get { return getPriorities(); } }

        public List<MapObject> MapObjects { get; set; } = new List<MapObject>(); //alert! setter

        public List<Station> Stations { get { return MapObjects.FindAll(item => item is Station).Cast<Station>().ToList(); }
            set { MapObjects.AddRange(value); } }

        public List<Station> StationaryStations { get; private set; } = new List<Station>();
        public List<Unit> UnitsConnectedToStationaryStations { get; private set; } = new List<Unit>();
        public List<Unit> Units { get { return MapObjects.FindAll(item => item is Unit).Cast<Unit>().ToList(); } }
        public List<Unit> Warehouses { get; private set; } = new List<Unit>();
        public double MaxAffordableDistance { get; private set; } = 0.0;

        public Instance(InstanceJSON instanceJSON)
        {
            this.StationRanges = instanceJSON.stationRanges;
            this.MaxAffordableDistance = instanceJSON.maxAffordableDistance == 0 ? double.MaxValue : instanceJSON.maxAffordableDistance;

            var stations = instanceJSON.stations.Select(item => new Station(item)).ToList();
            var units = instanceJSON.units.Select(item => new Unit(item)).ToList();

            this.MapObjects = prepareMapObjects(stations, units);
        }

        public Instance(List<Station> stations, List<Unit> units, double maxAffordableDistance)
        {
            var ranges = new List<double>();
            stations.ForEach(item => ranges.Add(item.Range));

            this.StationRanges = new double[] {20.0, 30.0, 50.0 }; //alert 
            this.MaxAffordableDistance = maxAffordableDistance;

            this.MapObjects = prepareMapObjects(stations, units);
        }

        public Instance(List<Station> stations, double maxAffordableDistance) : this(stations, new List<Unit>(), maxAffordableDistance) {}

        public Instance()
        {
            StationRanges = new double[] { 20.0, 30.0, 50.0 }; //alert
        }

        public List<MapObject> prepareMapObjects(List<Station> stations, List<Unit> units) //alert
        {
            //RemoveRelations(); //alert

            var mapObjects = stations.Cast<MapObject>().Concat(units.Cast<MapObject>()).ToList();

            foreach(var first in mapObjects) //alert nie uwzględniam tutaj stacjonarnych
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
                if (station.IsCore) continue;
                foreach(var unit in units)
                {
                    if(!unit.HasAttachement() && !station.IsAttached() && station.Position.Equals(unit.Position) && !station.IsStationary) //alert to powinno byc property
                    {
                        unit.Attach(station);
                    }
                }
            }

            StationaryStations = mapObjects.FindAll(item => item is Station).Cast<Station>().ToList().FindAll(item => item.IsStationary).ToList(); //alert brzydkie
            mapObjects.RemoveAll(item => item is Station && ((Station)item).IsStationary); //alert brzydkie
            var _units = mapObjects.FindAll(item => item is Unit).Cast<Unit>().ToList();
            UnitsConnectedToStationaryStations.AddRange(_units.FindAll(unit => StationaryStations.Any(station => unit.IsInRange(station))).ToList());
            mapObjects.RemoveAll(item => item is Unit && UnitsConnectedToStationaryStations.Contains(item));

            foreach(var stationaryStation in StationaryStations)
            {
                foreach(var otherStationaryStation in StationaryStations)
                {
                    if (stationaryStation == otherStationaryStation) continue;

                    if(!stationaryStation.Receivers.Contains(otherStationaryStation))
                    {
                        stationaryStation.AddReceiver(otherStationaryStation);
                    }
                }
            }

            //int id = 0; //alert brzydko
            //this.Stations.ForEach(station => { station.id = ++id; }); //alert to nie działa!
            //Station._id = id;

            Warehouses.AddRange(mapObjects.FindAll(item => item is Unit && ((Unit)item).Priority == 0).Cast<Unit>().ToList());

            return mapObjects;
        }

        public Dictionary<Station, double> SaveRangesSnapshot()
        {
            return SaveRangesSnapshot(Stations);
        }
        public static void RestoreRangesSnapshot(Dictionary<Station, double> snapshot)
        {
            snapshot.ToList().ForEach(keyValue => keyValue.Key.Range = keyValue.Value);
        }
        public static Dictionary<Station, double> SaveRangesSnapshot(List<Station> stations)
        {
            var snapshot = new Dictionary<Station, double>();
            stations.ForEach(station => snapshot[station] = station.Range);
            return snapshot;
        }

        List<int> getPriorities()
        {
            var priorities = Units.Select(unit => unit.Priority).ToHashSet().ToList();
            priorities.Sort();
            return priorities;
        }

        public void RemoveRelations()
        {
            MapObjects.ForEach(o => { o.Senders.Clear(); o.Receivers.Clear(); });
        }

    }
}
