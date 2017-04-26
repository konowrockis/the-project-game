using System;

namespace TheProjectGame.Contracts.Messages.Structures
{
    public interface ITimestamped
    {
        DateTime Timestamp { get; set; }
    }
}
