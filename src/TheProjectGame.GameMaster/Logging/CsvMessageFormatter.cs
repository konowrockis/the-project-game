using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog.Events;
using Serilog.Formatting;

namespace TheProjectGame.GameMaster.Logging
{
    internal class CsvMessageFormatter : ITextFormatter
    {
        public const string KEY = "GameEvent";

        private readonly List<string> Columns = new List<string>()
        {
            nameof(GameEvent.Type),
            nameof(GameEvent.Timestamp),
            nameof(GameEvent.GameId),
            nameof(GameEvent.PlayerId),
            nameof(GameEvent.PlayerGuid),
            nameof(GameEvent.Color),
            nameof(GameEvent.Role)
        };

        public void Format(LogEvent logEvent, TextWriter output)
        {
            if (!logEvent.Properties.ContainsKey(KEY)) return;
            var message = logEvent.Properties[KEY] as StructureValue;

            var values = message
                .Properties
                .OrderBy(p => Columns.IndexOf(p.Name))
                .Select(p => p.Value.ToString());
            
            var str = string.Join(",", values).Replace("\"", "");

            output.WriteLine(str);
        }
    }
}
