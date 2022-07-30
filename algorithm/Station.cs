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
        public int groupId { get; set; } = -1;

        public bool isStationary { get; set; } = false;
        public StationJSON(Position position, double range, int groupId = -1, bool isStationary = false)
        {
            this.position = position;
            this.range = range;
            this.groupId = groupId;
            this.isStationary = isStationary;
        }

        public StationJSON() { }
    }
    public class Station : MapObject
    {
        public static int _id = 0;
        public int groupId { get; private set; }
        public int id { get; set; } = ++_id; //alert
        public bool IsStationary { get; set; } = false;

        public Station(double range) : base(new Position())
        {
            this.Range = range;
        }
        public Station(Position position, double range, bool isStationary = false) : base(position)
        {
            this.Range = range;
            this.IsStationary = isStationary;
        }

        public Station(Station station) //alert nie przemyślane dobrze
        {
            this.Position = new Position(station.Position);
            this.Range = station.Range;
            this.id = station.id;
            this.Receivers = new List<MapObject>(station.Receivers);
            this.Senders = new List<MapObject>(station.Senders);
            this.groupId = station.groupId;
            this.IsStationary = station.IsStationary;
        }
        public void SetGroupId(int id)
        {
            groupId = id;
        }

        public Station(StationJSON stationJSON) : base(stationJSON.position)
        {
            this.Range = stationJSON.range;
            this.IsStationary = stationJSON.isStationary;
        }

        public StationJSON GetJSON()
        {
            return new StationJSON(Position, Range, groupId, IsStationary);
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

        public Unit GetUnit() //alert podstępny null
        {
            if (!IsAttached()) return null;
            var item = Senders.Find(item => item is Unit);
            return (Unit) item;
        }
    }
}
