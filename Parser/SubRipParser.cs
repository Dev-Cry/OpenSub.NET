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
            var lines = input.Split(Environment.NewLine, StringSplitOptions.None);
            var currentSubRip = default(Model.SubRip);
            var index = 0;

            while (index < lines.Length)
            {
                var line = lines[index];

                if (int.TryParse(line, out var number))
                {
                    currentSubRip = new Model.SubRip
                    {
                        Index = number
                    };
                    index++;

                    var timeLine = lines[index];
                    var times = timeLine.Split(new[] { " --> " }, StringSplitOptions.None);
                    currentSubRip.StartTime = TimeSpan.Parse(times[0].Trim());
                    currentSubRip.EndTime = TimeSpan.Parse(times[1].Trim());
                    index++;

                    while (index < lines.Length && !string.IsNullOrWhiteSpace(lines[index]))
                    {
                        currentSubRip.Text += lines[index] + Environment.NewLine;
                        index++;
                    }

                    result.Add(currentSubRip);
                }
                else
                {
                    index++;
                }
            }

            return result;
        }
    }
}
