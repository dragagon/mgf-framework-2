using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGF.DataEntities
{
    public class User
    {

        public int Id { get; set; }
        public String LoginName { get; set; }
        public String PasswordHash { get; set; }
        public String Salt { get; set; }


        public ICollection<Character> Characters { get; set; }
    }
}
