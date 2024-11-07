using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEGANOMIX.Services
{
    public class LinkingWordsWithTemplateService : IMethodService
    {
        private FileStream FS { get; }

        public LinkingWordsWithTemplateService(FileStream fs)
        {
            this.FS = fs;
        }


        public string Decode()
        {
            return string.Empty;
        }

        public string Encode()
        {
            string result = string.Empty;
            string file = "";

            using(StreamReader sr = new StreamReader(FS))
            {
                while(!sr.EndOfStream)
                {
                    file += sr.ReadLine();
                }
            }


            return result;
        }
    }
}
