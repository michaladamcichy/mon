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
        public double Lat { get; set; } = 0.0;
        public double Lng { get; set; } = 0.0;

        public Position() { }
        public Position(double lat, double lng)
        {
            this.Lat = lat; this.Lng = lng;
        }

        public override bool Equals(object obj)
        {
            return Lat == ((Position) obj).Lat && Lng == ((Position) obj).Lng;
        }
    }
    public class MapObject
    {
        public virtual Position Position { get; set; } = new Position();
        public virtual double Range { get; set; } = 0;

        public List<MapObject> Senders { get; set; } = new List<MapObject>();
        public List<MapObject> Receivers { get; set; } = new List<MapObject>();

        public MapObject() {}
        public MapObject(Position position)
        {
            this.Position = position;
        }
        public virtual double GetDistanceFrom(MapObject other)
        {
            return Distance(this.Position, other.Position);
        }
        public List<MapObject> GetNearest(List<MapObject> mapObjects)
        {
            var THIS = this; //alert alert!
            var toSort = mapObjects.FindAll(item => item != THIS);
            toSort.Sort((item1, item2) => this.GetDistanceFrom(item1).CompareTo(this.GetDistanceFrom(item2)));
            return toSort;
        }

        public List<Station> GetNearest(List<Station> mapObjects)
        {
            return GetNearest(mapObjects.Cast<MapObject>().ToList()).Cast<Station>().ToList();
        }


        public void AddSender(MapObject sender)
        {
            Senders.Add(sender);
        }

        public void AddReceiver(MapObject receiver)
        {
            Receivers.Add(receiver);
        }

        public virtual bool IsInRange(MapObject other)
        {
            return GetDistanceFrom(other) < other.Range;
        }

        //
        public static double MinCoveringCircleRadius(List<MapObject> mapObjects) //alert not tested, not optimalised!!!
        {
            double maxDistance = 0.0;
            mapObjects.ForEach(first => { mapObjects.ForEach(second => { maxDistance = Math.Max(maxDistance, first.GetDistanceFrom(second));});});
            return maxDistance / 2;
        }

        public static double MinCoveringCircleRadius(List<Station> stations)
        {
            return MinCoveringCircleRadius(stations.Cast<MapObject>().ToList());
        }

        public static Position CenterOfGravity(List<Station> stations)
        {
            return CenterOfGravity(stations.Cast<MapObject>().ToList());
        }

        public static Position CenterOfGravity(List<MapObject> mapObjects) //alert czy można uśredniać lat i lng??
            //TODO - lepszy algorytm ze stackoverflow
        {
            if (mapObjects.Count == 0)
            {
                return new Position();
            }

            var center = new Position();

            foreach (var mapObject in mapObjects)
            {
                center.Lat += mapObject.Position.Lat;
                center.Lng += mapObject.Position.Lng;
            }

            center.Lat /= mapObjects.Count;
            center.Lng /= mapObjects.Count;

            return center;
        }

        public static Position MinCoveringCircleCenter(List<MapObject> mapObjects)
        {
            if (mapObjects.Count == 0) return null; //alert podstępny null
            if (mapObjects.Count == 1) return mapObjects[0].Position;

            var furthestPair = new Tuple<MapObject, MapObject>(mapObjects.First(), mapObjects.Last());
            var maxDistance = 0.0;
            mapObjects.ForEach(first => {
                mapObjects.ForEach(second => {
                    if (first == second) return;
                    var distance = Math.Max(maxDistance, first.GetDistanceFrom(second));
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        furthestPair = new Tuple<MapObject, MapObject>(first, second);
                    }
            }); });

            return CenterOfGravity(new List<MapObject>() { furthestPair.Item1, furthestPair.Item2 });
        }

        public static Position MinCoveringCircleCenter(List<Station> stations)
        {
            return MinCoveringCircleCenter(stations.Cast<MapObject>().ToList());
        }

        public static Station GetFurthestFrom(List<Station> stations, Position point)
        {
            return stations.Aggregate((first, second) => MapObject.Distance(first.Position, point) > MapObject.Distance(second.Position, point) ? first : second);
        }

        public static double Distance(Position first, Position second)
        {
            //TODO C# geolocation GetDistanceFrom
            //alert do weryfikacji - zarówno algorytm jak i przekopiowany kod!!!
            ////wzięte żywcem z js, trzeba zweryfikować
            //https://stackoverflow.com/questions/27928/calculate-distance-between-two-latitude-longitude-points-haversine-formula

            double p = 0.017453292519943295;    // Math.PI / 
            double a = 0.5 - Math.Cos((second.Lat - first.Lat) * p) / 2 +
                    Math.Cos(first.Lat * p) * Math.Cos(second.Lat * p) *
                    (1 - Math.Cos((second.Lng - first.Lng) * p)) / 2;

            return 12742 * Math.Asin(Math.Sqrt(a)); // 2 * R; R = 6371 
        }

        public static double? MinCoveringRange(Double[] ranges, List<MapObject> mapObjects) //alert move to mapobject
        {
            double minCoveringRadius = MinCoveringCircleRadius(mapObjects);

            foreach (var range in ranges)
            {
                if (minCoveringRadius <= range)
                {
                    return range;
                }
            }

            return null;
        }

        public static double? MinCoveringRange(Double[] ranges, List<Station> stations)
        {
            return MinCoveringRange(ranges, stations.Cast<MapObject>().ToList());
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
