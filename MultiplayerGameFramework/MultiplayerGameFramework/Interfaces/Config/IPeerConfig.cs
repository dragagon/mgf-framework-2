using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerGameFramework.Interfaces.Config
{
    public interface IPeerConfig
    {
        T GetConfig<T>() where T : class;
        void AddConfig(object config);
    }
}
