using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class Group : MapObject
    {
        public List<Station> Stations { get; set; } = new List<Station>();

        public override Position Position { get { return MapObject.MinCoveringCircleCenter(Stations); } set { } }  //alert empty set
        public override double Range { get { return 0.0; } set { throw new Exception(); } } //alert brzydko oraz 0.0
        
        public Station CentralStation { get {
                var core = Stations.FindAll(item => item.IsCore);
                if (core.Count != 1) return null; //alert podstępny null
                return core.First();
            } }
        public Group() { }
        
        public Group(Group group)
        {
            Stations = new List<Station>(group.Stations);
        }
        public Group(List<Station> stations) : base(new Position())
        {
            Stations = new List<Station>(stations);
        }
        public void Add(Station station) //alert czy connected wciąż potrzebne?? raczej nie
            //alert schizofrenia raz jest add bez conected, raz z connected
        {
            Stations.Add(station);
        }

        public void Remove(Station station)
        {
            Stations.Remove(station);
        }

        public bool Contains(Station station)
        {
            return Stations.Contains(station);
        }

        public override double GetDistanceFrom(MapObject other)
        {
            if (other is Group)
            {
                var otherGroup = (Group)other;

                return Stations.Min(first => otherGroup.Stations.Min(second => second.GetDistanceFrom(first)));
            }

            if (other is MapObject)
            {
                return Stations.Min(item => other.GetDistanceFrom(item));

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

        public Station GetFurthestFromCenter()
        {
            return MapObject.GetFurthestFrom(Stations, MapObject.CenterOfGravity(Stations));
        }

        public static List<Station> Flatten(List<Group> groups)
        {
            var list = new List<Station>();

            foreach (Group group in groups)
            {
                foreach (var station in group.Stations)
                {
                    station.SetGroupId(groups.IndexOf(group));
                    list.Add(station);
                }
            }
            list.Sort((item1, item2) => item1.Id - item2.Id); //alert opakować to, alert straciłem kolejność, czemu?

            return list;
        }
    }
}
