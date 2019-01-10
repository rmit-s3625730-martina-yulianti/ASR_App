using System;
using System.Collections.Generic;
using System.Text;

namespace ASR_App
{
    class SlotException : Exception
    {
        
        public SlotException() { }

        public SlotException(string msg) : base(msg) { }

    }
}
