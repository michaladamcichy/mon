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

            //var stations = new List<Station>();
            var units = new List<Unit>(initialInstance.Units);
            //var instance = initialInstance;
                foreach (var priority in sortedPriorities) //alert tutaj będę polegał wyłącznie na dobudowywaniu
            {
                //units.AddRange(initialInstance.Units.FindAll(unit => unit.Priority == priority).ToList());

                //instance = new Instance(instance.Stations, units, initialInstance.Ranges, initialInstance.Counts); //alert
                //var newStations = new ArrangeWithExisting().Run(instance);
                //instance.Stations.AddRange(newStations.Where(station => !instance.Stations.Contains(station))); //alert nie obsługuję nigdzie niedomyślnych rangów


                initialInstance.Units = units.Where(unit => unit.Priority >= priority).ToList();
                initialInstance.CreateRelations();
                initialInstance.Stations = new ArrangeWithExisting().Run(initialInstance);

                
            }

            return initialInstance.Stations;
        }
    }
}
