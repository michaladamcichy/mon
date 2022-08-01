using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{

    public class UnitJSON //alert todo dać dziedziczenie może będzie działać
    {
        public Position position { get; set; } = new Position();
        public int priority { get; set; } = 1;
        public int[] counts { get; set; } = {0,0,0};
        public UnitJSON()
        {

        }

        public UnitJSON(Position position, int priority, int[] counts)
        {
            this.position = position;
            this.priority = priority;
            this.counts = counts;
        }
    }

    public class Unit : MapObject
    {
        public int Priority = 1;
        public int[] Counts { get; set; } = {0, 0, 0};
        public Unit(UnitJSON unitJSON) : base(unitJSON.position)
        {
            this.Priority = unitJSON.priority;
            this.Counts = unitJSON.counts;
        }

        public void Attach(Station station)
        {
            Receivers.Clear();
            station.Senders.Add(this); //alert alert!
            station.Position = Position;
            Receivers.Add(station);
        }

        public bool HasAttachement()
        {
            return Receivers.Count > 0;
        }
    }
}
