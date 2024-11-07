using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEGANOMIX.Services
{
    public interface IMethodService
    {
        string Encode();

        string Decode();    
    }
}
