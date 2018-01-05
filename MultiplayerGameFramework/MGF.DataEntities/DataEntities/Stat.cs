using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGF.DataEntities
{
    public class Stat
    {
        public int StatId { get; set; }
        public String Name { get; set; }

        public int CharacterId { get; set; }
        public Character Character { get; set; }

        public int Value { get; set; }
    }
}
