using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEGANOMIX.Services
{
    public interface IMethodService
    {
        string EncodeToString();

        byte[] EncodeToByte();

        string DecodeToString();

        byte[] DecoteToByte();
    }
}
