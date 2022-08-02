using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class SimpleHierarchicalTree
    {
        public SimpleHierarchicalTree()
        {

        }

        public List<Station> EveryoneWithEveryone(Cost initialCost, List<Station> level, List<Station> otherLevel) //todo obsłużyć cost
        {
            Cost cost = new Cost(initialCost);
            var connected = new HashSet<Tuple<Station, Station>>();
            var additionalStations = new List<Station>();
            foreach(var first in level)
            {
                foreach(var second in otherLevel)
                {
                    if (connected.Contains(new Tuple<Station, Station>(first, second)) || connected.Contains(new Tuple<Station, Station>(second, first)))
                        continue;
                    //alert nieprofesjonalnie

                    var (newCost, stations) = ArrangeBetween.Run(cost, first, second);
                    additionalStations.AddRange(stations);

                    connected.Add(new Tuple<Station, Station>(first, second));
                }
            }
            return additionalStations;
        }

        public List<Station> Run(Instance instance) //alert koszty
        {
            Cost cost = new Cost(instance);
            var additionalStations = new List<Station>();
            //alert skopiowany kod!!!
            //if nie rozbudowujemy tylko zaczynamy od zera //alert!
            instance.MapObjects.RemoveAll(item => item is Station); //alert!!
            Station._id = 0;//alert

            foreach (var unit in instance.Units)
            {
                var minRange = cost.GetMin();
                if (minRange == null) return additionalStations; //alert todo
                var station = new Station(minRange.Value);
                instance.MapObjects.Add(station);
                unit.Attach(station);
            }

            additionalStations.AddRange(instance.Stations); //alert brzydko

            var firstLevel = instance.Stations.FindAll(station => station.GetUnit().Priority == 1).ToList();
            var secondLevel = instance.Stations.FindAll(station => station.GetUnit().Priority == 2).ToList();
            var thirdLevel = instance.Stations.FindAll(station => station.GetUnit().Priority == 3).ToList();
            var fourthLevel = instance.Stations.FindAll(station => station.GetUnit().Priority == 4).ToList();
            
            var fifthLevel = instance.Stations.FindAll(station => station.GetUnit().Priority == 5).ToList();


            additionalStations.AddRange(EveryoneWithEveryone(cost, fourthLevel, thirdLevel)); //alert todo koszt
            additionalStations.AddRange(EveryoneWithEveryone(cost, thirdLevel, secondLevel)); //alert todo koszt

            return additionalStations;
        }
    }
}
