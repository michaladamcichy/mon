using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class PriorityArrange
    {
        public List<Station> Run(Instance initialInstance)
        {
            initialInstance.RemoveRelations();

            Action<List<MapObject>> removeRelations = (List<MapObject> stations) => { stations.ForEach(station => { station.Receivers.Clear(); station.Senders.Clear(); }); };

            var priorities = new HashSet<int>();
            initialInstance.Units.ForEach(unit => priorities.Add(unit.Priority));

            var sortedPriorities = priorities.ToList();
            sortedPriorities.Sort();
            sortedPriorities.Reverse();

            var stations = new List<Station>();
            var units = new List<Unit>();

            foreach (var priority in priorities) //alert tutaj będę polegał wyłącznie na dobudowywaniu
            {
                /*removeRelations(stations.Cast<MapObject>().ToList());
                removeRelations(units.Cast<MapObject>().ToList());*/
                units.AddRange(initialInstance.Units.FindAll(unit => unit.Priority == priority).ToList());

                var instance = new Instance(stations, units, initialInstance.StationCounts);
                stations = new ArrangeWithExisting().Run(instance); //alert nie obsługuję nigdzie niedomyślnych rangów
                

            }

            return stations;
        }
    }
}
