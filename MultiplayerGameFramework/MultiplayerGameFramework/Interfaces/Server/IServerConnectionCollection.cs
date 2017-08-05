using MultiplayerGameFramework.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerGameFramework.Interfaces.Server
{
    public interface IServerConnectionCollection<TServerType, TPeer> : IConnectionCollection<TPeer>
    {
        Dictionary<TServerType, List<TPeer>> GetServers();
        List<T> GetServersByType<T>(TServerType type) where T : class, TPeer;
    }
}
