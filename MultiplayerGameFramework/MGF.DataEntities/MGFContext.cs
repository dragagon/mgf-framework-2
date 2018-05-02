using MGF.DataEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGF
{
#if MYSQL
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class MGFContext : DbContext
    {

        public MGFContext() : base("name=MySqlDbConnectionString")
        {
            Database.SetInitializer<MGFContext>(new DropCreateDatabaseIfModelChanges<MGFContext>());
        }
#else
    public class MGFContext : DbContext
    {

        public MGFContext() : base("name=MsSqlDbConnectionString")
        {
        }
#endif

        // Define Entities Here
        public DbSet<Character> Characters { get; set; }
        public DbSet<Stat> Stats { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
