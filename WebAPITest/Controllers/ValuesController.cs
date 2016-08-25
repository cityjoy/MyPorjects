using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPITest.Filter;

namespace WebAPITest.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        /// <summary>
        /// 获取所有值
        /// </summary>
        /// <returns></returns>
        [APIAuthorize]
        public IEnumerable<string> Get()
        {

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        /// <summary>
        /// 获取id对应的值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}