using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class SimpleArrange : IArrangeAlgorithm
    {
        public List<Station> Run(Instance instance)
        {
            var initialCost = new Cost(instance);
            var groups = (new SimpleCreateGroups(instance)).Run();
            var coreStations = groups.Select(group => group.CoreStation).ToList();

            var (cost, additionalStations) = JoinNearestNeighbors.Run(initialCost, coreStations);

            groups.Add(new Group(additionalStations));

            return Group.Flatten(groups);
        }


        //alert scenariusze łączęnia grup:
        //zbuduje drogę miedzy stacjami
        //powiększ którąś ze stacji!
        //pytanie: czy lepiej powiększać czy lepiej dostawiać
        //a może może lepiej dążyć do równych rozmiarów stacji sąsiadujących?
        //może to jest klucz do dobrego planaowania?
    }

}
