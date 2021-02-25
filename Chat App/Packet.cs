using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_App
{
    [Serializable]
    public struct Packet
    {
        public string tag;
        public string message;
        public ConsoleColor txtColor;
    }
}
