﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class Position
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
    public class MapObject : ICloneable
    {
        public Position position { get; set; } = new Position();

        public List<MapObject> hosts { get; set; } = new List<MapObject>();
        public List<MapObject> clients { get; set; } = new List<MapObject>();

        public MapObject(Position position)
        {
            this.position = position;
        }
        public object Clone()
        {
            return new MapObject(this.position);
        }

        public static double Distance(MapObject first, MapObject second)
        {
            return Distance(first.position, second.position);
        }
        public static double Distance(Position first, Position second)
        {
            //alert do weryfikacji - zarówno algorytm jak i przekopiowany kod!!!
            ////wzięte żywcem z js, trzeba zweryfikować
            //https://stackoverflow.com/questions/27928/calculate-distance-between-two-latitude-longitude-points-haversine-formula
            //
            double p = 0.017453292519943295;    // Math.PI / 
            double a = 0.5 - Math.Cos((second.lat - first.lat) * p) / 2 +
                    Math.Cos(first.lat * p) * Math.Cos(second.lat * p) *
                    (1 - Math.Cos((second.lng - first.lng) * p)) / 2;

            return 12742 * Math.Asin(Math.Sqrt(a)); // 2 * R; R = 6371 
        }

        public static double MinCoveringCircleRadius(List<MapObject> mapObjects)
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
    }

    public class Station : MapObject
    {
        public double range { get; set; }
    }

    public class Unit : MapObject
    {
    }

    public enum StationType
    {
        A = 0,
        B = 1,
        C = 2,
        Count = 3
    }
}
