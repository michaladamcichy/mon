using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class SimpleJoinGroups
    {
        Instance instance;
        HashSet<Group> connected = new HashSet<Group>();
        public SimpleJoinGroups(Instance instance)
        {
            this.instance = instance;
        }

        List<Station> ArrangeBetween(double[] ranges, Station first, Station second)
        {
            var stations = new List<Station>();

            var commonRange = ranges.Max();
            first.Range = commonRange;
            second.Range = commonRange;

            var distanceToCover = first.GetDistanceFrom(second);

            var stationsCount = (int)Math.Ceiling(distanceToCover / ranges.Max()); //alert! alert może być niepoprawne
            var stepLat = (second.Position.Lat - first.Position.Lat) / stationsCount;
            var stepLng = (second.Position.Lng - first.Position.Lng) / stationsCount;

            for (var i = 1; i < stationsCount; i++)
            {
                var station = new Station(new Position(first.Position.Lat + stepLat * i, first.Position.Lng + stepLng * i), ranges.Max());
                stations.Add(station);
            }

            if (stations.Count == 0)
            {
                if (!second.IsInRange(first))
                {
                    stations.Add(new Station(MapObject.CenterOfGravity((new MapObject[] { first, second }).ToList()), ranges.Max())); //alert dorobić dostosowanie rozmiaru
                    return stations;
                }
            }

            /*if (stations.Count > 0 && !second.IsInRange(stations.Last()))
            {
                stations.Add(new Station(MapObject.Center((new MapObject[] { second, stations.Last() }).ToList()), ranges.Max())); //alert dostosować rozmiar
            }*/

            return stations;
        }

        List<Station> Join(double[] ranges, Group first, Group second)
        {
            var f = first.GetNearest(second);
            var s = second.GetNearest(first);

            return Join(ranges, f, s);
        }

        List<Station> Join(double[] ranges, Group first, Station second)
        {
            return Join(ranges, first.GetNearest(second), second);
        }

        List<Station> Join(double[] ranges, Station first, Group second)
        {
            return Join(ranges, first, second.GetNearest(second));
        }

        List<Station> Join(double[] ranges, Station first, Station second) //alert męczące to przekazywanie atrybutów instancji//chociaz może to będzie przydatne przy ograniczeniach??
        {
            if (first.IsInRange(second) && second.IsInRange(first)) //alert założenie symetrii! czy tak ma zostać? niekoniecznie
            {
                return new List<Station>();
            }

            var firstStartRange = first.Range;
            var secondStartRange = second.Range;

            var currentRange = Math.Max(first.Range, second.Range); //alert mogą być lepsze rozwiązania z mniejszymi, chyba
            first.Range = currentRange;
            second.Range = currentRange;

            foreach (var range in ranges)
            {
                if (range < currentRange) continue;
                currentRange = range;

                if (second.IsInRange(first) && first.IsInRange(second)) //alert tu mamy symetrię
                {
                    return new List<Station>();
                }
            }

            first.Range = firstStartRange;
            second.Range = secondStartRange;

            var stationsBetween = ArrangeBetween(ranges, first, second);


            return stationsBetween;
        }
        public List<Station> Run(List<Group> groups)
        {
            if (groups.Count <= 1)
            {
                return Group.Flatten(groups);
            }

            var connectedGroups = new HashSet<Group>(); //alert memebre already exists
            var additionalStations = new List<Station>();

            connectedGroups.Add(groups.First());

            
            while(connectedGroups.Count < groups.Count)
            {
                Tuple<Station, Group> nearest = null;
                var nearestDistance = 0.0;

                var notConnectedGroups = groups.FindAll(item => !connectedGroups.Contains(item));
                foreach (var notConnected in notConnectedGroups)
                {
                    foreach (var connectedStation in Group.Flatten(connectedGroups.ToList()).Concat(additionalStations)) //alert pozwalam na dołączanie do infrastruktury
                    {
                        if (nearest == null || notConnected.GetDistanceFrom(connectedStation) < nearestDistance) //alert duża wtopa Group.GetDistance(Station) może zwracać co innego niż Station.GetDistanceFrom(Group)
                        {
                            nearest = new Tuple<Station, Group>(connectedStation, notConnected);
                            nearestDistance = notConnected.GetDistanceFrom(connectedStation); //alert optymalizacja
                        }
                    }
                }
                connectedGroups.Add(nearest.Item2); //alert nieczytelne
                additionalStations.AddRange(Join(instance.StationRanges, nearest.Item1, nearest.Item2));
            }

            groups.Add(new Group(additionalStations));//alert! nioelogiczne
            return Group.Flatten(groups);
        }
    }
}
