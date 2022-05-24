using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class NaiveArrange : IArrangeAlgorithm
    {
        HashSet<Station> connected = new HashSet<Station>();
        
        public List<Station> Run(Instance instance)
        {
            return new List<Station>(); //alert
            foreach (var unit in instance.Units) //alert uwspólnić
            {
                var station = new Station(instance.StationRanges.Min());
             
                instance.MapObjects.Add(station);
                MapObject.Attach(station, unit);
            }

            while(connected.Count < instance.Stations.Count)
            {
                //alert
            }

            return new List<Station>(instance.Stations); //alert!
        }  
    }

}
