using STEGANOMIX.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace STEGANOMIX.Services
{
    public class LinkingWordsWithTemplateService : IMethodService
    {
        private FileStream FS { get; }

        private int Template { get; }

        private SpojnikiDictionary Spojniki { get; }

        private string Message { get; }

        private List<int> AsciiMessage { get; set; } = new List<int>();

        private List<string>? Znaczniki { get; }

        public LinkingWordsWithTemplateService(FileStream fs, int template, SpojnikiDictionary spojniki, string? message = null, List<string>? znaczniki = null)
        {
            this.FS = fs;
            Template = template;
            Spojniki = spojniki;
            Znaczniki = znaczniki;

            if (message != null)
            {
                Message = message;
                foreach (var s in message)
                {
                    var binary = Convert.ToString(s, 2);
                    if (binary.Length > 7)
                        continue;
                    foreach (var b in binary)
                    {
                        if (b == '1')
                            AsciiMessage.Add(1);
                        else
                            AsciiMessage.Add(0);
                    }
                }
            }
            var asciiText = string.Concat(AsciiMessage);
            Znaczniki = znaczniki;
        }

        public string EncodeToString()
        {
            string result = string.Empty;
            string file = "";

            var readedText = FS.EnumerateLines().ToArray();

            int replaceTemplate = Template;
            bool replaced = false;
            int counter = 0;
            for (int j=0;j<readedText.Length;j++)
            {
                var splittedFile = readedText[j].Split(' ');
                for (int i = 0; i < splittedFile.Length; i++)
                {
                    replaced = false;
                    var originalWord = splittedFile[i];
                    var newWord = splittedFile[i].Replace(".", string.Empty).Replace(",", string.Empty)
                        .Replace(";", string.Empty).Replace(":", string.Empty).Replace("'", string.Empty)
                        .Replace("\"", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty)
                        .Replace("?", string.Empty).Replace("!", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty);


                    if (Znaczniki != null && Znaczniki.Contains(newWord.ToLower()))
                        replaceTemplate = Template;
                    if (Spojniki.ContainsKey(newWord.ToLower()))
                    {
                        if (replaceTemplate == Template)
                        {
                            if (AsciiMessage[counter] == Spojniki[newWord.ToLower()].Code)
                            {
                                newWord = Spojniki[newWord.ToLower()].Value;
                                replaced = true;
                            }
                            counter++;
                            replaceTemplate = 1;
                        }
                        else
                            replaceTemplate++;
                    }

                    if (replaced)
                    {
                        var firstLetter = splittedFile[i][0];
                        if (char.IsUpper(firstLetter))
                        {
                            if (newWord.Length == 1)
                                newWord = newWord.ToUpper();
                            else
                                newWord = newWord[0].ToString().ToUpper() + newWord[1..];
                        }
                        splittedFile[i] = splittedFile[i].Replace(originalWord, newWord);
                    }

                    if (counter == AsciiMessage.Count)
                        break;
                }
                readedText[j] = string.Join(' ', splittedFile);

                if (counter == AsciiMessage.Count)
                    break;
            }
            for (int j=0;j<readedText.Length;j++)
            {
                if (readedText[j].StartsWith(Environment.NewLine))
                {
                    var letter = Environment.NewLine + Environment.NewLine;
                    if (readedText[j].StartsWith(letter))
                        readedText[j] = readedText[j].Replace(letter, Environment.NewLine);
                    else
                        readedText[j] = readedText[j].TrimStart();
                }
            }

            result = string.Concat(readedText);
            return result;
        }

        public byte[] EncodeToByte()
        {
            string result = string.Empty;
            string file = "";

            using (StreamReader sr = new StreamReader(FS))
            {
                while (!sr.EndOfStream)
                {
                    file += sr.ReadLine();
                }
            }


            return null;
        }

        public string DecodeToString()
        {
            string result = string.Empty;
            string file = "";

            var readedText = FS.EnumerateLines().ToArray();

            int replaceTemplate = Template;
            int counter = 0;
            List<int> resultList = new List<int>();
            for (int j=0;j<readedText.Length;j++)
            {
                var splittedFile = readedText[j].Split(' ');
                for (int i = 0; i < splittedFile.Length; i++)
                {
                    var originalWord = splittedFile[i];
                    var newWord = splittedFile[i].Replace(".", string.Empty).Replace(",", string.Empty)
                        .Replace(";", string.Empty).Replace(":", string.Empty).Replace("'", string.Empty)
                        .Replace("\"", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty)
                        .Replace("?", string.Empty).Replace("!", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty);


                    if (Znaczniki != null && Znaczniki.Contains(newWord.ToLower()))
                        replaceTemplate = Template;
                    if (Spojniki.ContainsKey(newWord.ToLower()))
                    {
                        if (replaceTemplate == Template)
                        {
                            int code = Spojniki[newWord.ToLower()].Code == 1 ? 0 : 1;
                            resultList.Add(code);
                            counter++;
                            replaceTemplate = 1;
                        }
                        else
                            replaceTemplate++;
                    }
                }
            }


            var splittedArray = resultList.Split(7);
            foreach(var splitted in splittedArray )
            {
                if(splitted.Count() == 7)
                {
                    var asciiBinary = string.Concat(splitted);
                    try
                    {
                        var asciiDecimal = Convert.ToInt32(asciiBinary, 2).ToString();
                        if(Int32.TryParse(asciiDecimal, out int code))
                        {
                            if (code >= 32 && code <= 126)
                            {
                                var letter = (char)code;
                                result += letter;
                            }
                            else
                                break;
                        }
                    }
                    catch(Exception e)
                    {
                        continue;
                    }
                }
            }

            return result;
        }

        public byte[] DecoteToByte()
        {
            return null;
        }
    }
}
