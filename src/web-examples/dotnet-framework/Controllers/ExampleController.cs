using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace dotnet_framework.Controllers
{
    public class ExampleController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Fetch([FromUri] int sleepTime)
        {


            return Request.CreateResponse(HttpStatusCode.OK, $"Your response took {sleepTime} seconds to complete");
        }
    }
}
