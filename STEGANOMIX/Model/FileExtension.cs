using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEGANOMIX.Model
{
    public static class FileExtension
    {
        private static readonly char eof = '\uffff';
        private static readonly char[] newLine = Environment.NewLine.ToCharArray();
        private static readonly char newLineMarker = Environment.NewLine.Last();

        public static IEnumerable<string> EnumerateLines(this FileStream fs)
        {
            using(var sr = new StreamReader(fs))
            {
                char c;
                string line;
                var sb = new StringBuilder();

                while ((c = (char)sr.Read()) != eof)
                {
                    sb.Append(c);
                    if (c == newLineMarker &&
                        (line = sb.ToString()).EndsWith(Environment.NewLine))
                    {
                        yield return line;

                        sb.Clear();
                        sb.Append(Environment.NewLine);
                    }
                }

                if (sb.Length > 0)
                    yield return sb.ToString().Trim(newLine);
            }
        }
    }
}
