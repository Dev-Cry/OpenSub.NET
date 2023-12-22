using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSub.NET.Model
{
    public class WebVTT
    {
        public int Index { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Text { get; set; }
        // Přidání vlastností pro metadata a další pokročilé funkce
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
        public string Position { get; set; } // např. "align:left size:50%"

    }
}
