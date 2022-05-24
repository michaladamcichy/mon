using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class Position
    {
        public double lat { get; set; } = 0.0;
        public double lng { get; set; } = 0.0;

        public Position() { }
        public Position(double lat, double lng)
        {
            this.lat = lat; this.lng = lng;
        }

        public override bool Equals(object obj)
        {
            return lat == ((Position) obj).lat && lng == ((Position) obj).lng;
        }
    }
    public class MapObject : IDistancable
    {
        public Position Position { get; set; } = new Position();

        public List<MapObject> Senders { get; set; } = new List<MapObject>();
        public List<MapObject> Receivers { get; set; } = new List<MapObject>();

        public MapObject() { }
        public MapObject(Position position)
        {
            this.Position = position;
        }

        public override bool Equals(Object other)
        {
            return Position == ((MapObject) other).Position;
        }
        public object Clone()
        {
            return new MapObject(Position); //alert dla dziedziczących też by raczej trzeba przeciążyć
        }

        public List<MapObject> GetNearestMapObjects(List<MapObject> mapObjects)
        {
            var toSort = mapObjects.FindAll(item => item != this);
            toSort.Sort((item1, item2) => Distance(this, item1).CompareTo(Distance(this, item2)));
            return toSort;
        }

        public List<Station> GetNearestStations(List<MapObject> mapObjects)
        {
            var stations = mapObjects.FindAll(item => item != this && item is Station).Cast<Station>().ToList();
            mapObjects.Sort((item1, item2) => Distance(this, item1).CompareTo(Distance(this, item2)));
            return stations;
        }
        public List<Unit> GetNearestUnits(List<MapObject> mapObjects)
        {
            var units = mapObjects.FindAll(item => item != this && item is Unit).Cast<Unit>().ToList();
            mapObjects.Sort((item1, item2) => Distance(this, item1).CompareTo(Distance(this, item2)));
            return units;
        }

        public static double Distance(IDistancable first, IDistancable second)
        {
            var f = first is Group ? ((Group)first).GetNearest(second) : first;
            var s = second is Group ? ((Group)second).GetNearest(first) : second;

            return Distance(f, s);
        }

        public static double Distance(Position first, Position second)
        {
            //alert do weryfikacji - zarówno algorytm jak i przekopiowany kod!!!
            ////wzięte żywcem z js, trzeba zweryfikować
            //https://stackoverflow.com/questions/27928/calculate-distance-between-two-latitude-longitude-points-haversine-formula

            double p = 0.017453292519943295;    // Math.PI / 
            double a = 0.5 - Math.Cos((second.lat - first.lat) * p) / 2 +
                    Math.Cos(first.lat * p) * Math.Cos(second.lat * p) *
                    (1 - Math.Cos((second.lng - first.lng) * p)) / 2;

            return 12742 * Math.Asin(Math.Sqrt(a)); // 2 * R; R = 6371 
        }

        public static double Distance(MapObject first, MapObject second)
        {
            return Distance(first.Position, second.Position);
        }
        public static double MinCoveringCircleRadius(List<MapObject> mapObjects) //alert not tested
        {
            double maxDistance = 0f;

            foreach (var first in mapObjects)
            {
                foreach (var second in mapObjects)
                {
                    maxDistance = Math.Max(maxDistance, Distance(first, second));
                }
            }

            return maxDistance / 2.0;
        }

        public static Position Center(List<MapObject> mapObjects) //alert czy można uśredniać lat i lng??
        {
            if(mapObjects.Count == 0)
            {
                return new Position();
            }

            var center = new Position();

            foreach(var mapObject in mapObjects) 
            {
                center.lat += mapObject.Position.lat;
                center.lng += mapObject.Position.lng;
            }

            center.lat /= mapObjects.Count;
            center.lng /= mapObjects.Count;

            return center;
        }

        public static void Attach(Station station, Unit unit)
        {
            station.AttachTo(unit);
            unit.Attach(station);
        }

        public void AddSender(MapObject sender)
        {
            Senders.Add(sender);
        }

        public void AddReceiver(MapObject receiver)
        {
            Receivers.Add(receiver);
        }

        public virtual bool IsInRange(IRangable other) //alert dont know what im doing - vritual
        {
            return GetDistance(other) <= other.Range + Station.RangeTolerance;
        }

        public double GetDistance(IDistancable other)
        {
            var o = other is Group ? ((Group)other).GetNearest(this) : other;
            return Distance(this, o);
        }
    }

    public class StationJSON
    {
        public Position position { get; set; } = new Position();
        public double range { get; set; } = 0.0;

        public StationJSON(Position position, double range)
        {
            this.position = position;
            this.range = range;
        }

        public StationJSON() { }
    }
    public class Station : MapObject, IRangable
    {
        public static double RangeTolerance { get; } = 1.0; //alert alert! nie koniecznie to co można pomyśleć
        static int _id = 0;
        public int id { get; } = ++_id;
        public double Range { get; set; }
       
        public Station(double range)
        {
            this.Range = range;
        }
        public Station(Position position, double range) : base(position)
        {
            this.Range = range;
        }

        public Station(StationJSON stationJSON) : base(stationJSON.position)
        {
            this.Range = stationJSON.range;
        }

        public StationJSON GetJSON()
        {
            return new StationJSON(Position, Range);
        }

        public void AttachTo(Unit unit)
        {
            Position = unit.Position;
            Senders.RemoveAll(item => item is Unit);
            Senders.Add(unit);
        }

        public bool IsAttached()
        {
            return Senders.Any(item => item is Unit);
        }

        public bool IsInRange(IRangable other)
        {
            if(other is Station)
            {
                var station = (Station)other;
                return Distance(this, station) <= station.Range + RangeTolerance;
            }

            if(other is Group)
            {
                var group = (Group)other;
                group.Any(item => Distance(this, item) < item.Range + RangeTolerance);
            }

            throw new Exception();
        }
    }

    public class UnitJSON //alert todo dać dziedziczenie może będzie działać
    {
        public Position position {  get; set; } = new Position();

        public UnitJSON()
        {

        }

        public UnitJSON(Position position)
        {
            this.position = position;
        }
    }

    public class Unit : MapObject
    {
        public Unit(UnitJSON unitJSON) : base(unitJSON.position)
        {

        }
        public Unit() { }

        public void Attach(Station station)
        {
            Receivers.Clear();
            Receivers.Add(station);
        }

        public bool HasAttachement()
        {
            return Receivers.Count > 0;
        }

        public override bool IsInRange(IRangable other)
        {
            return GetDistance(other) <= Station.RangeTolerance;
        }
    }

    public enum StationType
    {
        A = 0,
        B = 1,
        C = 2,
        Count = 3
    }
}
