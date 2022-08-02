using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public class ArrangeBetween
    {
        /*static Cost MakeBothStationsBigger(Cost initialCost, Station first, Station second) //alert powinno zwracać proponowane range'e
        {
            Cost cost = new Cost(initialCost);
            while (first.Range < second.Range && cost.CanMakeBigger(first))
            {
                cost.MakeBigger(first);
            }

            while (second.Range < first.Range && cost.CanMakeBigger(second))
            {
                cost.MakeBigger(second);
            }

            if (MapObject.AreInRange(first, second)) return cost;

            if (first.Range == second.Range)
            {
                var nextCommonRange = cost.QueryMakeBigger(first);
                while (nextCommonRange != null && cost.CanGet(nextCommonRange.Value, 2))
                {
                    cost.MakeBigger(first);
                    cost.MakeBigger(second);

                    if (MapObject.AreInRange(first, second))
                    {
                        break;
                    }

                    nextCommonRange = cost.QueryMakeBigger(first);
                }
            }

            return cost;
        }*/

        static Cost AdjustEnds(Station first, List<Station> middles, Station last, Cost initialCost, double tolerance)
        {
            Cost cost = new Cost(initialCost);
            cost.GiveBack(first.Range);
            cost.GiveBack(last.Range);

            if (middles.Count == 0)
            {
                var commonRange = first.GetDistanceFrom(last) * (1.0 - tolerance / 2.0); //alert
                first.Range = cost.GetMin(1, commonRange).Value;
                last.Range = cost.GetMin(1, commonRange).Value;
                
                return cost;
            }

            first.Range = cost.GetMin(1, middles.First().Range).Value;
            last.Range = cost.GetMin(1, middles.Last().Range).Value;

            return cost;
        }
        public static (Cost, List<Station>) Run(Cost initialCost, Station first, Station last, double tolerance= 0.1) //alert ograniczenie zasobów zaimplementowane tylko częściowo
        {
            Cost cost = new Cost(initialCost);
            var stations = new List<Station>();

            cost.ChangeRange(first, cost.QueryMax().Value);
            cost.ChangeRange(last, cost.QueryMax().Value);
            if (MapObject.AreInRange(first, last) || !cost.CanGetAny()) return (cost, new List<Station>());

            var distanceToCover = first.GetDistanceFrom(last);
            var distanceCovered = 0.0;

            var currentPosition = first.Position;
            var direction = new Position((last.Position.Lat - first.Position.Lat)/distanceToCover, (last.Position.Lng - first.Position.Lng)/distanceToCover);

            var lastUsedRange = 0.0;
            var previousStation = first;
            while (distanceCovered + lastUsedRange * (1 - tolerance) < distanceToCover)
            //alert sytuacje nieobsłużone:
            //1) first i second ostatnie można pomniejszyć
            //2) jeśli mamy tylko jedną dużą i dokładamy do niej małe, to równie dobrze można by kłaść tylko małe
            //3) ogólnie jest duzo przypadków, trudny problem
            //4) hmmm, dla wag 1, 1.5, 2.5 nie opłaca się mieszać rozmiarów, lecz należy brać x równych
            //5) dla jakichś innych wag może się opłacać mieszanie - jakby małe były drogie
            //6) cholera bardzo trudny problem
            {
                if (!cost.CanGetAny()) return (cost, stations); //alert dużo możliwości bezsensownego użycia
                var _range = cost.GetMax(1, (distanceToCover - distanceCovered + lastUsedRange * (1 - tolerance))); //to chyba nie działa
                if (_range == null) _range = cost.GetMax(); //alert functional
                if (_range == null) return (cost, stations);
                var range = _range.Value;

                var step = Math.Min(previousStation.Range, range);

                currentPosition = new Position(currentPosition.Lat + direction.Lat * step * (1 - tolerance),
                    currentPosition.Lng + direction.Lng * step * (1 - tolerance));
                distanceCovered += step * (1 - tolerance);
                var station = new Station(new Position(currentPosition), step);
                stations.Add(station);
                if (MapObject.AreInRange(station, last)) break;
                lastUsedRange = range;
                previousStation = station;
            }
            //alert chyba trzeba by wcześniej jeszcze pomniejszyć skrajne middlesy
            Cost newCost = AdjustEnds(first, stations, last, cost, tolerance);
            cost = new Cost(newCost);
            /*if (stations.Count > 0 && !second.IsInRange(stations.Last()))
            {
                stations.Add(new Station(MapObject.Center((new MapObject[] { second, stations.Last() }).ToList()), ranges.Max())); //alert dostosować rozmiar
            }*/

            return (cost, stations); //alert cost
        }
    }
}
