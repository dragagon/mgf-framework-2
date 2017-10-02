using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class PhotonEngine : MonoBehaviour, IPhotonPeerListener {

    // Unity Editor Parameters
    public string ServerAddress;
    public string ApplicationName;
    public byte SubCodeParameterCode; // Typically 0 - Check Server.config <subCodeParameterCode />
    public bool UseEncryption;

    public static PhotonEngine instance = null;
    public EngineState State { get; protected set; }
    public PhotonPeer Peer { get; protected set; }
    public int Ping { get; protected set; }
    public List<IMessageHandler> eventHandlerList { get; protected set; }
    public List<IMessageHandler> responseHandlerList { get; protected set; }


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            // Destroy this object because it is a duplicate and we don't want to override the already existing version
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        Initialize();
    }

    protected void Initialize()
    {
        State = EngineState.DisconnectedState;
        Application.runInBackground = true;
        GatherMessageHandlers();
        // Create a new peer, make it use UPD (for now)
        Peer = new PhotonPeer(this, ConnectionProtocol.Udp);
    }

    // Use this for initialization
    void Start () {
        // May want to remove this code and create a "Login" button that attempts to connect when clicked.
        Debug.Log(string.Format("Connecting to {0}", ServerAddress));
        ConnectToServer(ServerAddress, ApplicationName);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Ping = Peer.RoundTripTime;
        // Use the State to update (call Peer.Service()) so we get and send messages on each update.
        State.OnUpdate();
	}

    private void OnApplicationQuit()
    {
        Disconnect();
        instance = null;
    }

    public void Disconnect()
    {
        if(Peer != null && Peer.PeerState == PeerStateValue.Connected)
        {
            Peer.Disconnect();
        }
        State = EngineState.DisconnectedState;
    }

    public void ConnectToServer(string serverAddress, string applicationName)
    {
        if (State == EngineState.DisconnectedState)
        {
            Peer.Connect(serverAddress, applicationName);
            State = EngineState.WaitingToConnectState;
        }
    }

    public void GatherMessageHandlers()
    {
        // use LINQ to find and create all handlers and add them to the handler list
        var handlers = from t in Assembly.GetAssembly(GetType()).GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IMessageHandler))) select Activator.CreateInstance(t) as IMessageHandler;
        Debug.Log(string.Format("Found {0} handlers", handlers.Count()));
        eventHandlerList = handlers.Where(h => h.Type == MessageType.Async).ToList();
        responseHandlerList = handlers.Where(h => h.Type == MessageType.Response).ToList();
    }
    #region IPhotonPeerListener
    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log(string.Format("Debug Return: {0} - {1}", level, message));
    }

    public void OnEvent(EventData eventData)
    {
        // Convert to Event : IMessage
        var message = new Event(eventData.Code, (int?)eventData.Parameters[SubCodeParameterCode], eventData.Parameters);
        var handlers = eventHandlerList.Where(h => h.Code == message.Code && h.SubCode == message.SubCode);
        if(handlers == null || handlers.Count() == 0)
        {
            // Default Handler?
            Debug.Log(string.Format("Attempted to handle event code:{0} - subCode:{1}", message.Code, message.SubCode));
        }
        foreach(var handler in handlers)
        {
            handler.HandleMessage(message);
        }
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        // Convert to Response : IMessage
        var message = new Response(operationResponse.OperationCode, (int?)operationResponse.Parameters[SubCodeParameterCode], operationResponse.Parameters, operationResponse.DebugMessage, operationResponse.ReturnCode);
        var handlers = responseHandlerList.Where(h => h.Code == message.Code && h.SubCode == message.SubCode);
        if (handlers == null || handlers.Count() == 0)
        {
            // Default Handler?
            Debug.Log(string.Format("Attempted to handle response code:{0} - subCode:{1}", message.Code, message.SubCode));
        }
        foreach (var handler in handlers)
        {
            handler.HandleMessage(message);
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.Log(string.Format("Switching Status to {0}", statusCode.ToString()));
        switch(statusCode)
        {
            case StatusCode.Connect:
                if(UseEncryption)
                {
                    Peer.EstablishEncryption();
                    State = EngineState.ConnectingState;
                }
                else
                {
                    State = EngineState.ConnectedState;
                }
                break;
            case StatusCode.EncryptionEstablished:
                State = EngineState.ConnectedState;
                break;
            case StatusCode.Disconnect:
            case StatusCode.DisconnectByServer:
            case StatusCode.DisconnectByServerLogic:
            case StatusCode.DisconnectByServerUserLimit:
            case StatusCode.EncryptionFailedToEstablish:
            case StatusCode.Exception:
            case StatusCode.ExceptionOnConnect:
            case StatusCode.ExceptionOnReceive:
            case StatusCode.SecurityExceptionOnConnect:
            case StatusCode.TimeoutDisconnect:
                State = EngineState.DisconnectedState;
                break;
            default:
                State = EngineState.DisconnectedState;
                break;
        }
    }
    #endregion

    public void SendRequest(OperationRequest request)
    {
        State.SendRequest(request, true, 0, UseEncryption);
    }
}
