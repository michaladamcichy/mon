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
            initialInstance.RemoveRelations(); //ALERT! allstation czy stations w definicji tej metody
      
            var priorities = new HashSet<int>();
            initialInstance.Units.ForEach(unit => priorities.Add(unit.Priority));

            var sortedPriorities = priorities.ToList();
            sortedPriorities.Sort();
            sortedPriorities.Reverse();

            var stations = new List<Station>();
            var units = new List<Unit>();

            foreach (var priority in priorities) //alert tutaj będę polegał wyłącznie na dobudowywaniu
            {
                units.AddRange(initialInstance.Units.FindAll(unit => unit.Priority == priority).ToList());

                var instance = new Instance(stations.Concat<Station>(initialInstance.StationaryStations).ToList(), units, initialInstance.StationCounts); //alert
                stations = new ArrangeWithExisting().Run(instance); //alert nie obsługuję nigdzie niedomyślnych rangów
            }

            return stations;
        }
    }
}
