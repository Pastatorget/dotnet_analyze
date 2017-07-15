using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_analyze.Resources
{
    public class DiagnosticsHelper
    {
        public static String FormatString(string msg)
        {
            var now = DateTime.Now;
            return $"{now} - {msg}";
        }

        public void WriteToConsole(string msg)
        {
            var now = DateTime.Now;
            var tmp = $"{now} - {msg}";

            Console.WriteLine(tmp);
        }

        /// <summary>
        /// Will print out both in trace and console
        /// </summary>
        /// <param name="msg"></param>
        public void WriteMessage(string msg)
        {
            var now = DateTime.Now;
            var tmp = $"{now} - {msg}";

            Console.WriteLine(tmp);
            Trace.WriteLine(tmp);
        }
    }
}
