using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using Serilog.Formatting;

namespace TheProjectGame.Configuration
{
    public class Logger
    {
        public static void Initialize(bool verbose=false)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.LiterateConsole(restrictedToMinimumLevel: verbose? LogEventLevel.Verbose : LogEventLevel.Information)
            .WriteTo.RollingFile("log-{Date}.txt",restrictedToMinimumLevel:LogEventLevel.Verbose)
            .WriteTo.File(new CsvMessageFormatter(), "gm-{Date}.csv",restrictedToMinimumLevel : LogEventLevel.Verbose)
            .CreateLogger();
        }
    }

    class CsvMessageFormatter : ITextFormatter
    {
        private const string KEY = "gm-message";

        public void Format(LogEvent logEvent, TextWriter output)
        {
            if (!logEvent.Properties.ContainsKey(KEY)) return;
            var msg = logEvent.Properties[KEY] as StructureValue;
            var props = msg.Properties.ToList();
            props.Sort((a, b) => String.Compare(a.Name, b.Name, StringComparison.Ordinal));
            var values = props.Select(p => p.Value.ToString()).ToList();
            var str = string.Join(",", values).Replace("\"", "");
            output.WriteLine(str);
        }
    }
}
