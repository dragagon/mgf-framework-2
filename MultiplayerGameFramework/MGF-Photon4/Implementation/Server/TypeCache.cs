using MultiplayerGameFramework.Implementation.Config;
using Photon.SocketServer.Rpc.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGF_Photon.Implementation.Server
{
    public class TypeCache
    {
        public CustomTypeCache GetCache()
        {
            var cache = new CustomTypeCache();
            cache.TryRegisterType(typeof(PeerInfo), 201, SerializePeerInfo, DeserializePeerInfo);
            return cache;
        }

        public static byte[] SerializePeerInfo(object obj)
        {
            PeerInfo info = obj as PeerInfo;
            IEnumerable<byte> retVal = BitConverter.GetBytes(info.MasterEndPoint.Port).
                Concat(BitConverter.GetBytes(info.ConnectRetryIntervalSeconds)).
                Concat(BitConverter.GetBytes(info.IsSiblingConnection)).
                Concat(BitConverter.GetBytes(info.MaxTries)).
                Concat(BitConverter.GetBytes(info.NumTries)).
                Concat(Encoding.ASCII.GetBytes(info.MasterEndPoint.Address.ToString() + "|" + info.ApplicationName));
            return retVal.ToArray();
        }

        public static object DeserializePeerInfo(byte[] obj)
        {
            int port = BitConverter.ToInt32(obj, 0);
            int retry = BitConverter.ToInt32(obj, 4);
            bool sibling = BitConverter.ToBoolean(obj, 8);
            int maxTries = BitConverter.ToInt32(obj, 9);
            int numTries = BitConverter.ToInt32(obj, 13);
            byte[] addy = new byte[obj.Length - 17];
            Array.Copy(obj, 17, addy, 0, addy.Length);
            string endString = Encoding.ASCII.GetString(addy);
            string[] strArray = endString.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            return new PeerInfo(strArray[0], port, retry, sibling, maxTries, strArray[1]) { NumTries = numTries };
        }
    }
}
