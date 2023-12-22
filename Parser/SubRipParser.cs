using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSub.NET.Parser
{
    public static class SubRipParser
    {
        public static List<Model.SubRip> Parse(string input)
        {
            var result = new List<Model.SubRip>();
            var lines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var index = 0;
            while (index < lines.Length)
            {
                var line = lines[index];
                if (int.TryParse(line, out var number))
                {
                    index++;
                    var timeLine = lines[index];
                    var times = timeLine.Split(new[] { "-->" }, StringSplitOptions.None);
                    var startTime = TimeSpan.Parse(times[0].Trim());
                    var endTime = TimeSpan.Parse(times[1].Trim());
                    index++;
                    var text = new StringBuilder();
                    while (index < lines.Length && !string.IsNullOrWhiteSpace(lines[index]))
                    {
                        text.AppendLine(lines[index]);
                        index++;
                    }
                    result.Add(new Model.SubRip
                    {
                        Index = number,
                        StartTime = startTime,
                        EndTime = endTime,
                        Text = text.ToString().Trim()
                    });
                }
                index++;
            }
            return result;
        }
    }
}
