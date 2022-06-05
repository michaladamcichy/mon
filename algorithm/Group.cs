using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class Group : List<Station>, IDistancable
    {
        public Group() : base() { }

        public Group(List<Station> stations) : base(stations)
        {

        }
        public void Add(Station station, Dictionary<Station, bool> connected) //alert czy connected wciąż potrzebne?? raczej nie
        {
            Add(station);
            connected[station] = true;
        }

        public void Remove(Station station, Dictionary<Station, bool> connected)
        {
            Remove(station);
            connected[station] = false;
        }
        public List<MapObject> ToMapObjects()
        {
            return this.Cast<MapObject>().ToList();
        } 

        public static List<Station> Flatten(List<Group> groups)
        {
            var list = new List<Station>();
            
            foreach(Group group in groups)
            {
                foreach(var station in group)
                {
                    list.Add(station);
                }
            }
            list.Sort((item1, item2) => item1.id - item2.id); //alert opakować to, alert straciłem kolejność, czemu?

            return list;
        }

        public double GetDistance(IDistancable other)
        {
            if (other is MapObject)
            {
                var mapObject = (MapObject)other;
                var nearest = this.Min(item => MapObject.Distance(item, mapObject));
            }

            if (other is Group)
            {
                var otherGroup = (Group)other;

                return this.Min(first => otherGroup.Min(second => second.GetDistance(first)));
            }

            throw new Exception();
        }

        static double Distance(IDistancable first, IDistancable second)
        {
            return first.GetDistance(second);
        }

        public bool IsInRange(IRangable other)
        {
            return this.Any(item => item.IsInRange(other));
        }

        public Station GetNearest(IDistancable other)
        {
            //return null; //alert
            if(other is Group)
            {
                var min = this.First();
                var minValue = Distance(this.First(), ((Group)other).First());
                foreach(var first in this)
                {
                    foreach(var second in (Group)other)
                    {
                        if (first == second) continue;

                        if(Distance(first, second) < minValue)
                        {
                            min = first;
                            minValue = Distance(first, second);
                        }
                    }
                }

                return min;
            }

            return this.Aggregate((item1, item2) => item1.GetDistance(other) < item2.GetDistance(other) ? item1 : item2); //alert stabilność najmniejszych elementów
        }
    }
}
