using System;

namespace OpenSub.NET.Model
{
    public class SubRip
    {
        public int Index { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Text { get; set; } = string.Empty; // Initialize to empty string
    }
}
