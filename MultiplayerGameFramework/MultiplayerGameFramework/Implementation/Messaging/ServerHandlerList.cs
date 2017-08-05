using MultiplayerGameFramework.Interfaces.Messaging;
using MultiplayerGameFramework.Interfaces.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerGameFramework.Implementation.Messaging
{
    public class ServerHandlerList : IHandlerList<IServerPeer>
    {
        private readonly IDefaultRequestHandler<IServerPeer> _defaultRequestHandler;
        private readonly IDefaultResponseHandler<IServerPeer> _defaultResponseHandler;
        private readonly IDefaultEventHandler<IServerPeer> _defaultEventHandler;

        private readonly Dictionary<int, IHandler<IServerPeer>> _requestCodeHandlerList;
        private readonly Dictionary<int, IHandler<IServerPeer>> _requestSubCodeHandlerList;

        private readonly Dictionary<int, IHandler<IServerPeer>> _responseCodeHandlerList;
        private readonly Dictionary<int, IHandler<IServerPeer>> _responseSubCodeHandlerList;

        private readonly Dictionary<int, IHandler<IServerPeer>> _eventCodeHandlerList;
        private readonly Dictionary<int, IHandler<IServerPeer>> _eventSubCodeHandlerList;

        public ServerHandlerList(IEnumerable<IHandler<IServerPeer>> handlers,
            IDefaultRequestHandler<IServerPeer> defaultRequestHandler,
            IDefaultResponseHandler<IServerPeer> defaultResponseHandler,
            IDefaultEventHandler<IServerPeer> defaultEventHandler)
        {
            _defaultRequestHandler = defaultRequestHandler;
            _defaultResponseHandler = defaultResponseHandler;
            _defaultEventHandler = defaultEventHandler;

            _requestCodeHandlerList = new Dictionary<int, IHandler<IServerPeer>>();
            _requestSubCodeHandlerList = new Dictionary<int, IHandler<IServerPeer>>();

            _responseCodeHandlerList = new Dictionary<int, IHandler<IServerPeer>>();
            _responseSubCodeHandlerList = new Dictionary<int, IHandler<IServerPeer>>();

            _eventCodeHandlerList = new Dictionary<int, IHandler<IServerPeer>>();
            _eventSubCodeHandlerList = new Dictionary<int, IHandler<IServerPeer>>();

            foreach(var handler in handlers)
            {
                RegisterHandler(handler);
            }
        }

        public bool RegisterHandler(IHandler<IServerPeer> handler)
        {
            var registered = false;

            if ((handler.Type & MessageType.Request) == MessageType.Request)
            {
                if (handler.SubCode.HasValue && !_requestSubCodeHandlerList.ContainsKey(handler.SubCode.Value))
                {
                    _requestSubCodeHandlerList.Add(handler.SubCode.Value, handler);
                    registered = true;
                }
                else if (!_requestCodeHandlerList.ContainsKey(handler.Code))
                {
                    _requestCodeHandlerList.Add(handler.Code, handler);
                    registered = true;
                }
            }

            // Response
            if ((handler.Type & MessageType.Response) == MessageType.Response)
            {
                if (handler.SubCode.HasValue && !_requestSubCodeHandlerList.ContainsKey(handler.SubCode.Value))
                {
                    _requestSubCodeHandlerList.Add(handler.SubCode.Value, handler);
                    registered = true;
                }
                else if (!_requestCodeHandlerList.ContainsKey(handler.Code))
                {
                    _requestCodeHandlerList.Add(handler.Code, handler);
                    registered = true;
                }
            }

            // Event
            if ((handler.Type & MessageType.Async) == MessageType.Async)
            {
                if (handler.SubCode.HasValue && !_requestSubCodeHandlerList.ContainsKey(handler.SubCode.Value))
                {
                    _requestSubCodeHandlerList.Add(handler.SubCode.Value, handler);
                    registered = true;
                }
                else if (!_requestCodeHandlerList.ContainsKey(handler.Code))
                {
                    _requestCodeHandlerList.Add(handler.Code, handler);
                    registered = true;
                }
            }
            return registered;
        }

        public bool HandleMessage(IMessage message, IServerPeer peer)
        {
            bool handled = false;
            switch (message.Type)
            {
                case MessageType.Request:
                    if (message.SubCode.HasValue && _requestSubCodeHandlerList.ContainsKey(message.SubCode.Value))
                    {
                        _requestSubCodeHandlerList[message.SubCode.Value].HandleMessage(message, peer);
                        handled = true;
                    }
                    else if (!message.SubCode.HasValue && !_requestCodeHandlerList.ContainsKey(message.Code))
                    {
                        _requestCodeHandlerList[message.Code].HandleMessage(message, peer);
                        handled = true;
                    }
                    else
                    {
                        _defaultRequestHandler.HandleMessage(message, peer);
                    }
                    break;

                // Response
                case MessageType.Response:
                    if (message.SubCode.HasValue && _responseSubCodeHandlerList.ContainsKey(message.SubCode.Value))
                    {
                        _responseSubCodeHandlerList[message.SubCode.Value].HandleMessage(message, peer);
                        handled = true;
                    }
                    else if (!message.SubCode.HasValue && !_responseCodeHandlerList.ContainsKey(message.Code))
                    {
                        _responseCodeHandlerList[message.Code].HandleMessage(message, peer);
                        handled = true;
                    }
                    else
                    {
                        _defaultResponseHandler.HandleMessage(message, peer);
                    }
                    break;

                // Event
                case MessageType.Async:
                    if (message.SubCode.HasValue && _eventSubCodeHandlerList.ContainsKey(message.SubCode.Value))
                    {
                        _eventSubCodeHandlerList[message.SubCode.Value].HandleMessage(message, peer);
                        handled = true;
                    }
                    else if (!message.SubCode.HasValue && !_eventCodeHandlerList.ContainsKey(message.Code))
                    {
                        _eventCodeHandlerList[message.Code].HandleMessage(message, peer);
                        handled = true;
                    }
                    else
                    {
                        _defaultEventHandler.HandleMessage(message, peer);
                    }
                    break;

            }
            return handled;
        }

    }
}
