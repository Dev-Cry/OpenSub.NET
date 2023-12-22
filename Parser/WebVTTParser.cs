using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSub.NET.Parser
{
   
    public static class WebVTTParser
    {
        public static List<Model.WebVTT> Parse(string input)
        {
            var result = new List<Model.WebVTT>();
            var lines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var index = 0;
            while (index < lines.Length)
            {
                var line = lines[index];
                if (line.StartsWith("WEBVTT"))
                {
                    var webvtt = new Model.WebVTT();
                    index++;
                    while (index < lines.Length && !string.IsNullOrWhiteSpace(lines[index]))
                    {
                        var metadata = lines[index].Split(':');
                        var key = metadata[0].Trim();
                        var value = metadata[1].Trim();
                        switch (key)
                        {
                            case "X-TIMESTAMP-MAP":
                                var timestamp = value.Split(',');
                                webvtt.Metadata.Add("MPEGTS", timestamp[0].Trim());
                                webvtt.Metadata.Add("LOCAL", timestamp[1].Trim());
                                break;
                            default:
                                webvtt.Metadata.Add(key, value);
                                break;
                        }
                        index++;
                    }
                    index++;
                    while (index < lines.Length && !string.IsNullOrWhiteSpace(lines[index]))
                    {
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
                        webvtt.StartTime = startTime;
                        webvtt.EndTime = endTime;
                        webvtt.Text = text.ToString().Trim();
                        result.Add(webvtt);
                    }
                }
                index++;
            }
            return result;
        }
    }
}
