using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerGameFramework.Interfaces.Client
{
    public interface IConnectionCollection<TPeer>
    {
        void Connect(TPeer peer);
        void Disconnect(TPeer peer);
        void Clear();
        List<T> GetPeers<T>() where T : class, TPeer;
    }
}
