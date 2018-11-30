using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApiSelfHost1.WebApi
{
    public class ValuesController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "你是谁", "value2" };
        }

        // GET api/values/5 
        public string Get(int id)
        {
            return "啊";
        }

        // POST api/values 
        public string Post([FromBody]string value)
        {
            return "啊Post";
        }

        // PUT api/values/5 
        public string Put(int id, [FromBody]string value)
        {
            return "啊Put";
        }

        // DELETE api/values/5 
        public string Delete(int id)
        {
            return "啊Delete";
        }
    }
}
