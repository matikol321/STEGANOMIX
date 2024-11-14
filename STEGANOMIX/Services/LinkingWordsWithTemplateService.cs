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

            result = file;
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
            return string.Empty;
        }

        public byte[] DecoteToByte()
        {
            return null;
        }
    }
}
