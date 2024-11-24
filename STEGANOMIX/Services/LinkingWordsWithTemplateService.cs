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

        private Dictionary<string,string> Spojniki { get; }

        private string Message { get; }

        private List<int> AsciiMessage { get; set; } = new List<int>();

        public LinkingWordsWithTemplateService(FileStream fs, int template, Dictionary<string, string> spojniki, string? message = null)
        {
            this.FS = fs;
            Template = template;
            Spojniki = spojniki;

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
        }

        public string EncodeToString()
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

            var splittedFile = file.Split(' ');
            int replaceTemplate = Template;
            bool replaced = false;
            int counter = 0;
            for(int i=0; i<splittedFile.Length; i++)
            {
                replaced = false;
                var originalWord = splittedFile[i];
                var newWord = splittedFile[i].Replace(".", string.Empty).Replace(",",string.Empty)
                    .Replace(";",string.Empty).Replace(":",string.Empty).Replace("'",string.Empty)
                    .Replace("\"", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty)
                    .Replace("?",string.Empty).Replace("!",string.Empty);
                

                if(Spojniki.ContainsKey(newWord.ToLower()))
                {
                    if (replaceTemplate == Template)
                    {
                        if (AsciiMessage[counter] == 1)
                        {
                            newWord = Spojniki[newWord.ToLower()];
                            replaced = true;
                        }
                        counter++;
                        replaceTemplate = 1;
                    }
                    else
                        replaceTemplate++;
                }
                else if(Spojniki.ContainsValue(newWord.ToLower()))
                {
                    if (replaceTemplate == Template)
                    {
                        if (AsciiMessage[counter] == 1)
                        {
                            var key = Spojniki.Where(x => x.Value == newWord.ToLower()).FirstOrDefault();
                            newWord = key.Key;
                            replaced = true;
                        }
                        counter++;
                        replaceTemplate = 1;
                    }
                    else
                        replaceTemplate++;
                }

                if(replaced)
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

            result = string.Join(' ', splittedFile);
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

            using (StreamReader sr = new StreamReader(FS))
            {
                while (!sr.EndOfStream)
                {
                    file += sr.ReadLine();
                }
            }

            var splittedFile = file.Split(' ');
            int replaceTemplate = Template;
            int counter = 0;
            List<int> resultList = new List<int>();
            for (int i = 0; i < splittedFile.Length; i++)
            {
                var originalWord = splittedFile[i];
                var newWord = splittedFile[i].Replace(".", string.Empty).Replace(",", string.Empty)
                    .Replace(";", string.Empty).Replace(":", string.Empty).Replace("'", string.Empty)
                    .Replace("\"", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty)
                    .Replace("?", string.Empty).Replace("!", string.Empty);


                if (Spojniki.ContainsKey(newWord.ToLower()))
                {
                    if (replaceTemplate == Template)
                    {
                        resultList.Add(1);
                    }
                    else
                        replaceTemplate++;
                }
                else if (Spojniki.ContainsValue(newWord.ToLower()))
                {
                    if (replaceTemplate == Template)
                    {
                        resultList.Add(0);
                        counter++;
                        replaceTemplate = 1;
                    }
                    else
                        replaceTemplate++;
                }
            }

            result = string.Join(' ', splittedFile);
            return result;
        }

        public byte[] DecoteToByte()
        {
            return null;
        }
    }
}
