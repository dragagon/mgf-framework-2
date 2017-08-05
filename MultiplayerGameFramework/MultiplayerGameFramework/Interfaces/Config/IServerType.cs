using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerGameFramework.Interfaces.Config
{
    public interface IServerType
    {
        string Name { get; }

        IServerType GetServerType(int serverType);
    }
}
