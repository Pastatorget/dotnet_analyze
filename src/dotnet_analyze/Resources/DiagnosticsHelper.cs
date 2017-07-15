using System;
using System.Collections.Generic;
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
            System.Diagnostics.Debug.WriteLine(tmp);
        }
    }
}
