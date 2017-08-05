using MultiplayerGameFramework.Interfaces.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerGameFramework.Interfaces.Support
{
    public interface IPeerFactory
    {
        T CreatePeer<T>(IPeerConfig config) where T : class;
    }
}
