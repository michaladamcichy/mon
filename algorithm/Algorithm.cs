using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public interface IArrangeStationsAlgorithm
    {
        List<Station> Run(Instance instance);
    }

    public class SimpleArrangeStationsAlgorithm : IArrangeStationsAlgorithm
    {
        public List<Station> Run(Instance instance)
        {
            //var stations = new List<Station>();
            //stations.Add(instance.stations[0]); //alert czy nie muszę się odwoływać wyłącznie do mapObjects?

            var solution = new List<Station>();

            var connected = new Dictionary<Unit, bool>();

            foreach(var unit in instance.Units)
            {
                connected[unit] = false; 
            }

            while (connected.Any(item => item.Value == false))
            {
                foreach (var unit in instance.Units.FindAll(item => connected[item] == false))
                {
                    if (connected[unit] == true) continue;

                    double selectedRange = instance.StationRanges.Min(); //alert
                    var neareastUnits = unit.GetNearestMapObjects(instance.MapObjects).FindAll(item => item is Unit && connected[unit] == false).Cast<Unit>().ToList();
                    var selectedUnits = new List<Unit>();

                    selectedUnits.Add(unit);

                    foreach (var nearestUnit in neareastUnits)
                    {
                        if(connected[nearestUnit] == true) continue;

                        var temporarySelectedUnits = new List<Unit>(selectedUnits);
                        
                        temporarySelectedUnits.Add(nearestUnit);

                        var minCoveringRadius = MapObject.MinCoveringCircleRadius(temporarySelectedUnits.Cast<MapObject>().ToList());
                        if (minCoveringRadius > instance.StationRanges.Max())
                        {
                            continue;
                        }
                        
                        foreach(var range in instance.StationRanges)
                        {
                            if(minCoveringRadius <= range)
                            {
                                selectedUnits = temporarySelectedUnits;
                                selectedRange = range;
                                break;
                            }
                        }
                    }
                    var center = MapObject.Center(selectedUnits.Cast<MapObject>().ToList()); //alert zwraca 0,0 moze lepiej null?
                    foreach(var selectedUnit in selectedUnits)
                    {
                        connected[selectedUnit] = true;
                    }
                    solution.Add(new Station(center, selectedRange));
                }
            }

            
            return solution; //return instance.Stations;
        }
    }

    public class ConnectionCheckAlgorithm
    {
        public bool Run(Instance instance)
        {
            if (instance.MapObjects.Count == 0)
            {
                return true;
            }

            foreach (var unit in instance.MapObjects.FindAll(item => item is Unit))
            {
                var visited = new Dictionary<MapObject, bool>();

                instance.MapObjects.ForEach(mapObject => visited.Add(mapObject, false));

                DFS(unit, visited);

                bool allConnected = visited.All(item => item.Key is not Unit || item.Value == true);
                if (!allConnected)
                {
                    return false;
                }
            }

            return true;
        }
        public void DFS(MapObject start, Dictionary<MapObject, bool> visited)
        {
            visited[start] = true;

            foreach (var host in start.hosts)
            {
                if (visited[host]) continue;

                DFS(host, visited);
            }

            if (start is Station)
            {
                foreach (var client in start.clients.FindAll(item => item is Unit))
                {
                    if (visited[client]) continue;

                    DFS(client, visited);
                }
            }
        }
    }

    public class Algorithm
    {
        public static bool IsConnected(Instance instance)
        {
            return new ConnectionCheckAlgorithm().Run(instance);
        }

        public static List<Station> SimpleArrangeAlgorithm(Instance instance)
        {
            return new SimpleArrangeStationsAlgorithm().Run(instance);
        }
    }
}
