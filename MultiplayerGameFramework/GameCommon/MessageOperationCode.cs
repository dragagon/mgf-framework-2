using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCommon
{
    public enum MessageOperationCode : byte // byte
    {
        // Which server the message is going to or coming from.
        // Start with 1, because registration makes use of 0.
        Login = 1
    }
}
