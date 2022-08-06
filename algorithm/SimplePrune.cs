using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class SimplePrune
    {
        /*Cost remove(Cost initialCost, Station station, Instance instance)
        {
            var senders = station.Senders.Where(item => item is Station && ((Station)item).IsCore).Cast<Station>().ToList();
            var receivers = station.Receivers.Where(item => item is Station && ((Station)item).IsCore).Cast<Station>().ToList();

            if (senders.Count != 1 || receivers.Count != 1 || senders.First() != receivers.First()) 
                if(senders.Count != 0 || receivers.Count != 0) return new Cost(initialCost);

            var cost = new Cost(initialCost);
            foreach (var neighbor in senders.Concat<Station>(receivers))
            {   
                neighbor.Senders.Remove(station);
                neighbor.Receivers.Remove(station);
            }

            cost.GiveBack(station.Range);
            instance.MapObjects.Remove(station);

            if (senders.Count != 1 || receivers.Count != 1) return cost; 

            return remove(cost, senders.First(), instance);
        }*/
        
        Cost remove(Cost initialCost, Station station, Instance instance)
        {
            var receivers = station.Receivers.Where(item => item is Station && ((Station)item).IsCore).Cast<Station>().ToList();
            var senders = station.Senders.Where(item => item is Station && ((Station)item).IsCore).Cast<Station>().ToList();

            if (receivers.Count > 1) return new Cost(initialCost);

            var cost = new Cost(initialCost);
            foreach (var neighbor in senders.Concat<Station>(receivers))
            {   
                neighbor.Senders.Remove(station);
                neighbor.Receivers.Remove(station);
            }

            cost.GiveBack(station.Range);
            instance.MapObjects.Remove(station);

            if (receivers.Count == 0) return cost; 

            return remove(cost, receivers.First(), instance);
        }

        public Cost Run(Cost initialCost, Instance instance)
        {
            Cost cost = new Cost(initialCost);
            var coreStations = instance.Stations.FindAll(station => !station.IsAttached() && !station.IsStationary).ToList();
            var ends = coreStations.FindAll(station => station.Receivers.Count <= 1 && station.Senders.Count <= 1).ToList();

            foreach (var end in ends)
            {
                var newCost = remove(cost, end, instance);
                cost = new Cost(newCost);
            }

            return cost;
        }
    }
}
