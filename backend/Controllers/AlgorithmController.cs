using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using algorithm;

namespace backend.Controllers
{
    public class A
    {
        public string x { get; set; } = "hello";
        public ArrayList arrayList { get; set; } = new ArrayList { "a", "b", "c" };
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AlgorithmController : ControllerBase
    {
        // GET: api/<ValuesController>
        ///[EnableCors("_myAllowSpecificOrigins")]
        [HttpGet]
        public A Get()
        {
            //return new string[] { "value1", "value2" };
            //return new string[] { "value1", "value2" };
            //return new ArrayList { "a", "b", "c" };
            System.Diagnostics.Debug.WriteLine("HELLO");
           
            var a = new A();
            return a;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        [Route("isConnected")]
        public bool Post(InstanceJSON instanceJSON) {
            var instance = new Instance(instanceJSON);
            return Algorithm.IsConnected(instance);
        }

        [HttpPost]
        [Route("naiveArrangeAlgorithm")]
        public List<StationJSON> naiveArrangeAlgorithm(InstanceJSON instanceJSON)
        {
            var instance = new Instance(instanceJSON);
            var ret = Algorithm.NaiveArrange(instance).Select(item => item.GetJSON()).ToList();
            return ret;
        }

        [HttpPost]
        [Route("simpleArrangeAlgorithm")]
        public List<StationJSON> simpleArrangeAlgorithm(InstanceJSON instanceJSON)
        {
            var instance = new Instance(instanceJSON);
            var ret = Algorithm.SimpleArrange(instance).Select(item => item.GetJSON()).ToList();
            return ret;
        }

        [HttpPost]
        [Route("greedySalesmanAlgorithm")]
        public List<StationJSON> greedySalesmanAlgorithm(InstanceJSON instanceJSON)
        {
            var instance = new Instance(instanceJSON);
            var ret = Algorithm.GreedySalesman(instance.Stations.Cast<IDistancable>().ToList()).Cast<Station>().Select(item => item.GetJSON()).ToList();
            return ret;
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
