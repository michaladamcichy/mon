using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class StationJSON
    {
        public Position position { get; set; } = new Position();
        public double range { get; set; } = 0.0;

        public StationJSON(Position position, double range)
        {
            this.position = position;
            this.range = range;
        }

        public StationJSON() { }
    }
    public class Station : MapObject
    {
        static int _id = 0;
        public int id { get; } = ++_id;

        public Station(double range) : base(new Position())
        {
            this.Range = range;
        }
        public Station(Position position, double range) : base(position)
        {
            this.Range = range;
        }

        public Station(StationJSON stationJSON) : base(stationJSON.position)
        {
            this.Range = stationJSON.range;
        }

        public StationJSON GetJSON()
        {
            return new StationJSON(Position, Range);
        }

        public void AttachTo(Unit unit)
        {
            Position = unit.Position;
            Senders.RemoveAll(item => item is Unit);
            Senders.Add(unit);
        }

        public bool IsAttached()
        {
            return Senders.Any(item => item is Unit);
        }
    }
}
