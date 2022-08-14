using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class SimpleArrange : IArrangeAlgorithm //critical alert
    {
        public List<Station> Run(Instance instance)
        {
            var cost = new Cost(instance);
            instance.MapObjects.RemoveAll(item => item is Station && !((Station) item).IsStationary);

            var (groups, newCost) = (new SimpleCreateGroups(instance)).Run(cost);
            cost = new Cost(newCost);
            if (instance.Units.Any(unit => !unit.HasAttachement())) return Group.Flatten(groups).ToList(); //alert data flow po kryjomu modyfikuje instnace.Stations
            
            var coreStations = groups.FindAll(group => group.CentralStation != null).Select(group => group.CentralStation).Concat<Station>(instance.StationaryStations).ToList();

            var (additionalStations, otherNewCost) = JoinNearestNeighbors.Run(cost, instance, coreStations);
            cost = new Cost(otherNewCost);
            //var additionalStations = new List<Station>();
            return Group.Flatten(groups).Concat<Station>(additionalStations).ToList().ToList();
        }


        //alert scenariusze łączęnia grup:
        //zbuduje drogę miedzy stacjami
        //powiększ którąś ze stacji!
        //pytanie: czy lepiej powiększać czy lepiej dostawiać
        //a może może lepiej dążyć do równych rozmiarów stacji sąsiadujących?
        //może to jest klucz do dobrego planaowania?
    }

}
