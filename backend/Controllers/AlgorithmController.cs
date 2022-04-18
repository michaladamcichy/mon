using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public void Post([FromBody] string value)
        {
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
