﻿using System;
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
        public virtual bool IsInRange(MapObject other)
        {
            return GetDistanceFrom(other) < other.Range;
        }

        //public static bool AreInRange(MapObject o1, MapObject o2)
        //{
        //    return o1.IsInRange(o2) && o2.IsInRange(o1);
        //}

        public static bool AreInRange(Station s1, Station s2)
        {
            return s1.IsInRange(s2) && s2.IsInRange(s1);
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
            return CenterOfGravity(mapObjects); //alert alert wielki alert
            //return SmallestEnclosingCircleAdapter.GetCenter(mapObjects);
            //if (mapObjects.Count == 0) return null; //alert podstępny null
            //if (mapObjects.Count == 1) return mapObjects[0].Position;

            //var furthestPair = new Tuple<MapObject, MapObject>(mapObjects.First(), mapObjects.Last());
            //var maxDistance = 0.0;
            //mapObjects.ForEach(first => {
            //    mapObjects.ForEach(second => {
            //        if (first == second) return;
            //        var distance = Math.Max(maxDistance, first.GetDistanceFrom(second));
            //        if (distance > maxDistance)
            //        {
            //            maxDistance = distance;
            //            furthestPair = new Tuple<MapObject, MapObject>(first, second);
            //        }
            //}); });

            //return CenterOfGravity(new List<MapObject>() { furthestPair.Item1, furthestPair.Item2 });
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
            //TODO C# geolocation GetDistanceFrom
            //alert do weryfikacji - zarówno algorytm jak i przekopiowany kod!!!
            ////wzięte żywcem z js, trzeba zweryfikować
            //https://stackoverflow.com/questions/27928/calculate-distance-between-two-latitude-longitude-points-haversine-formula

            /*double p = 0.017453292519943295;    // Math.PI / 
            double a = 0.5 - Math.Cos((second.Lat - first.Lat) * p) / 2 +
                    Math.Cos(first.Lat * p) * Math.Cos(second.Lat * p) *
                    (1 - Math.Cos((second.Lng - first.Lng) * p)) / 2;

            return 12742 * Math.Asin(Math.Sqrt(a)); // 2 * R; R = 6371 */
            var lat1 = first.Lat;
            var lon1 = first.Lng;
            var lat2 = second.Lat;
            var lon2 = second.Lng;

            lon1 = ToRadians(lon1);
            lon2 = ToRadians(lon2);
            lat1 = ToRadians(lat1);
            lat2 = ToRadians(lat2);

            double dlon = lon2 - lon1;//alert critical nie mój kod!
            double dlat = lat2 - lat1;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Pow(Math.Sin(dlon / 2), 2);
            double c = 2 * Math.Asin(Math.Sqrt(a));
            double r = 6371;

            return (c * r);
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

        public static MapObject GetNextFromTowards(Station first, Station second, double maxAvailableRange, double tolerance = 0.1)
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
