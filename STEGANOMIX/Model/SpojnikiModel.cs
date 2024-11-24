using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEGANOMIX.Model
{
    public class SpojnikiModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public int Code { get; set; }
    }

    public class SpojnikiDictionary : Dictionary<string, SpojnikiModel>
    {
        public void Add(string key, string value, int code)
        {
            SpojnikiModel val = new SpojnikiModel()
            {
                Key = key,
                Value = value,
                Code = code
            };
            this.Add(key, val);
        }

    }
}
