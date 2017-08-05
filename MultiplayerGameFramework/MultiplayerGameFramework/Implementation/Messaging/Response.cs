using MultiplayerGameFramework.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerGameFramework.Implementation.Messaging
{
    public class Response : IMessage
    {
        private readonly byte _code;
        private readonly int? _subCode;
        private readonly Dictionary<byte, object> _parameters;

        private readonly string _debugMessage;
        private readonly short _returnCode;
        public Response(byte code, int? subCode, Dictionary<byte,object> parameters)
        {
            _code = code;
            _subCode = subCode;
            _parameters = parameters;
        }

        public Response(byte code, int? subCode, Dictionary<byte,object> parameters, string debugMessage, short returnCode)
            : this(code, subCode, parameters)
        {
            _debugMessage = debugMessage;
            _returnCode = returnCode;
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
                return MessageType.Response;
            }
        }

        public string DebugMessage
        {
            get
            {
                return _debugMessage;
            }
        }

        public short ReturnCode
        {
            get
            {
                return _returnCode;
            }
        }
    }
}
