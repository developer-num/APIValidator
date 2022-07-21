using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Validator.Cliente.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1.0")]
    [ApiController]
    public class ValidatorController : ControllerBase
    {
        // GET: api/<ValidatorController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValidatorController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValidatorController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValidatorController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValidatorController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
