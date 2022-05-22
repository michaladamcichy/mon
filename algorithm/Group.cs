using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class Group : List<Station>
    {
        public void Add(Station station, Dictionary<Station, bool> connected)
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
            list.Sort((item1, item2) => item1.id - item2.id); //alert opakować to

            return list;
        }
    }
}
