﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class Group : MapObject
    {
        public List<Station> Stations { get; set; } = new List<Station>();

        public override Position Position { get { return MapObject.Center(Stations); } set { } }  //alert empty set
        public override double Range { get { return 0.0; } set { throw new Exception(); } } //alert brzydko oraz 0.0
        public Group() { }

        public Group(List<Station> stations) : base(new Position())
        {
            Stations = new List<Station>(stations);
        }
        public void Add(Station station, Dictionary<Station, bool> connected) //alert czy connected wciąż potrzebne?? raczej nie
            //alert schizofrenia raz jest add bez conected, raz z connected
        {
            Stations.Add(station);
            connected[station] = true;
        }

        public void Remove(Station station, Dictionary<Station, bool> connected)
        {
            Stations.Remove(station);
            connected[station] = false;
        }

        public double GetDistance(MapObject other)
        {
            if (other is MapObject)
            {
                return Stations.Min(item => other.GetDistanceFrom(item));

            }

            if (other is Group)
            {
                var otherGroup = (Group)other;

                return Stations.Min(first => otherGroup.Stations.Min(second => second.GetDistanceFrom(first)));
            }

            throw new Exception();
        }

        public bool IsInRange(MapObject other)
        {
            return Stations.Any(item => item.IsInRange(other));
        }

        public Station GetNearest(MapObject other)
        {
            if (Stations.Count == 0) return null; //alert podstępny null
            
            if(other is not Group)
            {
                return Stations.Aggregate((item1, item2) => item1.GetDistanceFrom(other) < item2.GetDistanceFrom(other) ? item1 : item2); //alert stabilność najmniejszych elementów
            }

            var min = Stations.First();
            var minValue = Stations.First().GetDistanceFrom(((Group)other).Stations.First());
            
            foreach(var first in Stations)
            {
                foreach(var second in ((Group)other).Stations)
                {
                    if (first == second) continue;

                    if(first.GetDistanceFrom(second) < minValue)
                    {
                        min = first;
                        minValue = first.GetDistanceFrom(second);
                    }
                }
            }

            return min;
        }

        public static List<Station> Flatten(List<Group> groups)
        {
            var list = new List<Station>();

            foreach (Group group in groups)
            {
                foreach (var station in group.Stations)
                {
                    list.Add(station);
                }
            }
            list.Sort((item1, item2) => item1.id - item2.id); //alert opakować to, alert straciłem kolejność, czemu?

            return list;
        }
    }
}
