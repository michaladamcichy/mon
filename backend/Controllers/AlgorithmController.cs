using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using algorithm;
using System.Diagnostics;

namespace backend.Controllers
{
    public class Result
    {
        public List<StationJSON> Stations { get; set; }
        public long Milliseconds { get; set; }

        public Result(List<StationJSON> stations, long milliseconds)
        {
            Stations = stations;
            Milliseconds = milliseconds;
        }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class AlgorithmController : ControllerBase
    {
        // POST api/<ValuesController>
        [HttpPost]
        [Route("isConnected")]
        public bool Post(InstanceJSON instanceJSON)
        {
            var instance = new Instance(instanceJSON);
            return Algorithm.IsConnected(instance); //alert
        }

        [HttpPost]
        [Route("naiveArrangeAlgorithm")]
        public Result naiveArrangeAlgorithm(InstanceJSON instanceJSON)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            instanceJSON.stations.RemoveAll(item => !item.isStationary);
            var instance = new Instance(instanceJSON);
            var ret = Algorithm.NaiveArrange(instance).Select(item => item.GetJSON()).ToList();

            stopwatch.Stop();
            return new Result(ret, stopwatch.ElapsedMilliseconds);// stopwatch.ElapsedMilliseconds);
        }

        [HttpPost]
        [Route("simpleArrangeAlgorithm")]
        public Result simpleArrangeAlgorithm(InstanceJSON instanceJSON)
        { //alert CRITICAL ALERT!
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            instanceJSON.stations.RemoveAll(item => !item.isStationary); //alert
            var instance = new Instance(instanceJSON);
            var ret = Algorithm.ArrangeWithExisting(instance).Select(item => item.GetJSON()).ToList();
            
            stopwatch.Stop();
            return new Result(ret, stopwatch.ElapsedMilliseconds);// stopwatch.ElapsedMilliseconds);
        }

        [HttpPost]
        [Route("priorityArrangeAlgorithm")]
        public Result priorityArrangeAlgorithm(InstanceJSON instanceJSON)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            instanceJSON.stations.RemoveAll(item => !item.isStationary); //alert
            var instance = new Instance(instanceJSON);
            ///var ret = Algorithm.PriorityArrange(instance).Select(item => item.GetJSON()).ToList();

            var units = new List<Unit>(instance.Units);

            instance.Units = units.Where(unit => unit.Priority >= 4).ToList();
            var ret = new ArrangeWithExisting().Run(instance);
            instance.Stations = ret;

            instance.Units = units.Where(unit => unit.Priority >= 3).ToList();
            ret = new ArrangeWithExisting().Run(instance);
            instance.Stations = ret;

            instance.Units = units.Where(unit => unit.Priority >= 2).ToList();
            ret = new ArrangeWithExisting().Run(instance);
            instance.Stations = ret;

            instance.Units = units.Where(unit => unit.Priority >= 1).ToList();
            ret = new ArrangeWithExisting().Run(instance);
            instance.Stations = ret;

            instance.Units = units.Where(unit => unit.Priority >= 0).ToList();
            ret = new ArrangeWithExisting().Run(instance);
            instance.Stations = ret;

            stopwatch.Stop();
            return new Result(ret.Select(item => item.GetJSON()).ToList(), stopwatch.ElapsedMilliseconds);
        }

        [HttpPost]
        [Route("arrangeWithExistingAlgorithm")]
        public Result arrangeWithExisting(InstanceJSON instanceJSON)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var instance = new Instance(instanceJSON);
            var ret = Algorithm.ArrangeWithExisting(instance).Select(item => item.GetJSON()).ToList();
            stopwatch.Stop();
            return new Result(ret, stopwatch.ElapsedMilliseconds);
        }

        [HttpPost]
        [Route("simpleOptimizeAlgorithm")]
        public Result simpleOptimizeAlgorithm(InstanceJSON instanceJSON)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var instance = new Instance(instanceJSON);
            var ret = Algorithm.SimpleOptimize(instance).Select(item => item.GetJSON()).ToList();
            stopwatch.Stop();
            return new Result(ret, stopwatch.ElapsedMilliseconds);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}