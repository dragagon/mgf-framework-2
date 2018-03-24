using MultiplayerGameFramework.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servers.Data.Client
{
    public class CharacterData : IClientData
    {
        public int UserId { get; set; }
    }
}
