using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    //https://localhost:5000/api/values
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;

        // public DataContext Context => _context; // 1 statement to do it all

        public ValuesController(DataContext context)
        {
         _context = context;   
        }
 

        // GET api/values
           [AllowAnonymous]
        [HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        public async Task<IActionResult> GetValues()
        {
            //throw new Exception("Test Exception");
            
            //return new string[] { "value1", "value5" };

            var values = await _context.Values.ToListAsync();
            return Ok(values);
        } 
        // GET api/values/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        public async Task<IActionResult> GetValue(int id)
        {
            var value = await _context.Values.FirstOrDefaultAsync(m=>m.Id == id);
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
