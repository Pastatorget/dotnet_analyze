using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_analyze.Resources
{
    public class LogMessage
    {
        public int StatusCode { get; set; }
        public string RequestMethod { get; set; }
        public string RequestUri { get; set; }
        public long ElapsedTime { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $"{this.StatusCode} {RequestMethod} {RequestUri} {ElapsedTime} {Message}";
        }

        public string ToJson()
        {
            var msg = new
            {
                statusCode = this.StatusCode,
                requestMethod = this.RequestMethod,
                requestUri = this.RequestUri,
                elapsedTime = this.ElapsedTime,
                message = this.Message
            };

            return JsonConvert.SerializeObject(msg);
        }
    }
}
