using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;

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
            var station = new Station();
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
        public bool Post(Instance instance) {
            return instance.IsConnected();
            //return MapObject.Distance(instance.units[0], instance.units[1]);
            //System.Diagnostics.Debug.WriteLine(position.ToString());
            //Log.
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
