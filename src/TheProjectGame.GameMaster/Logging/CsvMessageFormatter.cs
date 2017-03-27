using System;
using System.IO;
using System.Linq;
using Serilog.Events;
using Serilog.Formatting;

namespace TheProjectGame.GameMaster.Logging
{
    internal class CsvMessageFormatter : ITextFormatter
    {
        public const string KEY = "GameEvent";

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
