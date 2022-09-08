using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class RecoverGroups
    {
        public List<Group> Run(Instance instance)
        {
            var groups = new List<Group>();

            var score = new Dictionary<Station, int>();
            foreach(var coreStation in instance.CoreStations)
            {
                score[coreStation] = coreStation.Neighbors.Where(neighbor => neighbor.IsPrivate).Count();
            }

            var assigned = new HashSet<Station>();
            foreach(var keyValue in score.OrderByDescending(keyValue => keyValue.Value))
            {
                var station = keyValue.Key;

                var group = new Group(new List<Station>{ station });
                station.Neighbors.Where(neighbor => neighbor.IsPrivate).ToList().ForEach(neighbor =>
                {
                    if (!assigned.Contains(neighbor))
                    {
                        group.Add(neighbor);
                    }
                });

                if(group.Stations.Count == 1)
                {
                    continue;
                }

                group.Stations.ForEach(station => assigned.Add(station));
                groups.Add(group);
            }

            return groups;
        }
    }
}
