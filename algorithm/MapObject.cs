using System;
using System.Collections.Generic;
using System.Linq;

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

        public Position(Position  position)
        {
            this.Lat = position.Lat; this.Lng = position.Lng;
        }

        public override bool Equals(object obj)
        {
            return Lat == ((Position) obj).Lat && Lng == ((Position) obj).Lng;
        }
    }
    public class MapObject
    {
        public virtual Position Position { get; set; } = new Position();
        public virtual double Range { get; set; } = 0.0;

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
            var toSort = mapObjects.FindAll(item => item != this);
            toSort.Sort((item1, item2) => this.GetDistanceFrom(item1).CompareTo(this.GetDistanceFrom(item2)));
            return toSort;
        }

        public Station GetOneNearest(List<Station> stations)
        {
            if (stations.Where(item => item != this).Count() == 0) return null;
            return stations.Where(item => item != this).Aggregate((currentMin, station) => station.GetDistanceFrom(this) < currentMin.GetDistanceFrom(this) ? station : currentMin);
        }
        public Group GetOneNearest(List<Group> groups)
        {
            if (groups.Where(item => item != this).Count() == 0) return null;
            return groups.Where(item => item != this).Aggregate((currentMin, group) => group.CentralStation.GetDistanceFrom(this) < currentMin.CentralStation.GetDistanceFrom(this) ? group : currentMin);
        }

        public List<Station> GetNearest(List<Station> mapObjects)
        {
            if(mapObjects.Where(item => item != this).Count()==0) return new List<Station>();
            return GetNearest(mapObjects.Cast<MapObject>().ToList()).Cast<Station>().ToList();
        }
        public virtual bool IsInRange(MapObject other, double tolerance = 0.0)
        {
            return GetDistanceFrom(other) < other.Range * (1.0 + tolerance);
        }

        //public static bool AreInRange(MapObject o1, MapObject o2)
        //{
        //    return o1.IsInRange(o2) && o2.IsInRange(o1);
        //}

        public static bool AreInRange(Station s1, Station s2, double tolerance = 0.0)
        {
            return s1.IsInRange(s2, tolerance) && s2.IsInRange(s1, tolerance);
        }

        //
        public static double MinCoveringCircleRadius(List<MapObject> mapObjects) //alert not tested, not optimalised!!!
        {
            //alert!
            var center = MinCoveringCircleCenter(mapObjects); //alert alert wielki alert
            return mapObjects.Select(item => item.GetDistanceFrom(new MapObject(center))).Max(); //alert alert alert
            //return SmallestEnclosingCircleAdapter.GetRange(mapObjects);
            //double maxDistance = 0.0;
            //mapObjects.ForEach(first => { mapObjects.ForEach(second => { maxDistance = Math.Max(maxDistance, first.GetDistanceFrom(second));});});
            //return maxDistance / 2;
        }

        public static double MinCoveringCircleRadius(List<Station> stations)
        {
            return MinCoveringCircleRadius(stations.Cast<MapObject>().ToList());
        }

        public static Position CenterOfGravity(List<Station> stations)
        {
            return CenterOfGravity(stations.Cast<MapObject>().ToList());
        }

        public static Position CenterOfGravity(List<MapObject> mapObjects)
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
            return CenterOfGravity(mapObjects);
        }

        public static Position MinCoveringCircleCenter(List<Station> stations)
        {
            return MinCoveringCircleCenter(stations.Cast<MapObject>().ToList());
        }

        public static Station GetFurthestFrom(List<Station> stations, Position point)
        {
            return stations.Aggregate((first, second) => MapObject.Distance(first.Position, point) > MapObject.Distance(second.Position, point) ? first : second);
        }

        static double ToRadians(double degrees)
        {
            return (Math.PI / 180) * degrees;
        }

        public static double Distance(Position first, Position second)
        {
            var lat1 = first.Lat;
            var lng1 = first.Lng;
            var lat2 = second.Lat;
            var lng2 = second.Lng;

            lng1 = ToRadians(lng1);
            lng2 = ToRadians(lng2);
            lat1 = ToRadians(lat1);
            lat2 = ToRadians(lat2);

            double dlng = lng2 - lng1;
            double dlat = lat2 - lat1;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dlng / 2), 2);
            double c = 2 * Math.Asin(Math.Sqrt(a));
            double r = 6371;

            return (c * r);
        }

        public static double? MinCoveringRange(Double[] ranges, List<MapObject> mapObjects)
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

        public static MapObject GetNextFromTowards(Station first, Station second, double maxAvailableRange, double tolerance = 0.0)
        {
            //alert smaller bigger nieaktualne!
            var smaller = first; //first.Range < second.Range ? first : second; //alert
            var bigger = second;//second.Range > first.Range ? second : first; //alert

            var distanceToCover = smaller.GetDistanceFrom(bigger);
            var direction = new Position((bigger.Position.Lat - smaller.Position.Lat) / distanceToCover, (bigger.Position.Lng - smaller.Position.Lng) / distanceToCover);

            var step = Math.Min(first.Range, maxAvailableRange);//Math.Min(smaller.Range, bigger.Range); //alert! niesprawdzone
            return new MapObject(new Position(smaller.Position.Lat + direction.Lat * step * (1.0 - tolerance / 2.0),
                smaller.Position.Lng + direction.Lng * step * (1.0 - tolerance / 2.0))); //alert! czy w simplearrange też tak robiłe? dokładnie tak?
        }
    }
}
