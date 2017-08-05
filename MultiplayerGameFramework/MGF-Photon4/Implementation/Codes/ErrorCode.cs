using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGF_Photon.Implementation.Codes
{
    internal enum ErrorCode : short
    {
        OperationDenied = -3,
        OperationInvalid = -2,
        InternalServerError = -1,

        OK = 0
    }
}
