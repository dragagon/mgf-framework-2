using MultiplayerGameFramework.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerGameFramework.Implementation.Messaging
{
    public class Request : IMessage
    {
        private readonly byte _code;
        private readonly int? _subCode;
        private readonly Dictionary<byte, object> _parameters;

        public Request(byte code, int? subCode, Dictionary<byte,object> parameters)
        {
            _code = code;
            _subCode = subCode;
            _parameters = parameters;
        }
        public byte Code
        {
            get
            {
                return _code;
            }
        }

        public Dictionary<byte, object> Parameters
        {
            get
            {
                return _parameters;
            }
        }

        public int? SubCode
        {
            get
            {
                return _subCode;
            }
        }

        public MessageType Type
        {
            get
            {
                return MessageType.Request;
            }
        }
    }
}
