using MultiplayerGameFramework.Interfaces.Client;
using MultiplayerGameFramework.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerGameFramework.Implementation.Messaging
{
    public class ClientHandlerList : IHandlerList<IClientPeer>
    {
        private readonly List<IHandler<IClientPeer>> _requestCodeHandlerList;
        private readonly List<IHandler<IClientPeer>> _requestSubCodeHandlerList;

        public ClientHandlerList(IEnumerable<IHandler<IClientPeer>> handlers)
        {
            _requestCodeHandlerList = new List<IHandler<IClientPeer>>();
            _requestSubCodeHandlerList = new List<IHandler<IClientPeer>>();

            foreach(var handler in handlers)
            {
                RegisterHandler(handler);
            }
        }

        public bool RegisterHandler(IHandler<IClientPeer> handler)
        {
            var registered = false;

            if((handler.Type & MessageType.Request) == MessageType.Request)
            {
                if(handler.SubCode.HasValue)
                {
                    _requestSubCodeHandlerList.Add(handler);
                    registered = true;
                }
                else
                {
                    _requestCodeHandlerList.Add(handler);
                    registered = true;
                }
            }
            return registered;
        }

        public bool HandleMessage(IMessage message, IClientPeer peer)
        {
            bool handled = false;
            IEnumerable<IHandler<IClientPeer>> handlers;
            switch(message.Type)
            {
                case MessageType.Request:
                    // Get all matching code and subcode - Normal message handling
                    handlers = _requestSubCodeHandlerList.Where(h => h.Code == message.Code && h.SubCode == message.SubCode);
                    if (handlers == null || handlers.Count() == 0)
                    {
                        // If no normal message handling occurs - check if there is one that handles only by code - normal forward handlers
                        handlers = _requestCodeHandlerList.Where(h => h.Code == message.Code);
                    }
                    // If still no message handling occurs - default handler
                    if (handlers == null || handlers.Count() == 0)
                    {
                        // No default handler for incomming client requests. Usually output error message on server.
                        //_defaultRequestHandler.HandleMessage(message, peer);
                    }
                    // if the default handler was called, its because the handler list was null or empty (it should always return empty, null checks are just in case), 
                    // otherwise we call all matching handlers
                    foreach (var handler in handlers)
                    {
                        handler.HandleMessage(message, peer);
                        handled = true;
                    }
                    break;

            }
            return handled;
        }
    }
}
