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
        public int id { get; set; } = -1;
        public bool isStationary { get; set; } = false;
        public bool isCore { get; set; } = false;
        public StationJSON(Position position, double range, int groupId = -1, int id = -1, bool isStationary = false, bool isCore = false)
        {
            this.position = position;
            this.range = range;

            this.groupId = groupId;
            this.id = id;
            this.isStationary = isStationary;
            this.isCore = isCore;
        }

        public StationJSON() { }
    }
    public class Station : MapObject
    {
        public static int _id = 0;
        public List<Station> Neighbors { get; private set; } = new List<Station>();
        public int GroupId { get; private set; }
        public int Id { get; set; } = -1; //alert
        public bool IsMobile { get { return !IsStationary; } }
        public bool IsStationary { get; set; } = false;
        public bool IsCore { get { return !IsAttached(); } }

        public bool IsPrivate { get { return IsAttached(); } }

        public Unit Unit { get; private set; } = null;
        public Station(double range) : base(new Position())
        {
            this.Range = range;
        }
        public Station(Position position, double range, bool isStationary = false, bool isCore = false) : base(position)
        {
            this.Range = range;
            this.IsStationary = isStationary;
        }

        public Station(Station station) //alert nie przemyślane dobrze
        {
            this.Position = new Position(station.Position);
            this.Range = station.Range;
            this.Id = station.Id;
            this.Neighbors = new List<Station>(station.Neighbors);
            this.GroupId = station.GroupId;
            this.IsStationary = station.IsStationary;
        }
        public void SetGroupId(int id)
        {
            GroupId = id;
        }

        public Station(StationJSON stationJSON) : base(stationJSON.position)
        {
            this.Range = stationJSON.range;
            this.IsStationary = stationJSON.isStationary;
            this.Id = stationJSON.id;
        }

        public StationJSON GetJSON()
        {
            return new StationJSON(Position, Range, GroupId, Id, IsStationary, IsCore);
        }

        public void AddNeighbor(Station station)
        {
            if(!Neighbors.Contains(station)) Neighbors.Add(station);
        }

        public void RemoveNeighbor(Station station)
        {
            Neighbors.RemoveAll(item => item == station);
        }

        public void AttachTo(Unit unit)
        {
            Position = unit.Position; //alert pętla ! :)
            Unit = unit;
        }

        public bool IsAttached()
        {
            return Unit != null;
        }

        public Unit GetUnit() //alert podstępny null
        {
            return Unit; //alert
        }

        public void Detach()
        {
            Unit = null; //alert słabe są te metody
        }
    }
}
