using MGF;
using MGF.DataEntities;

namespace DatabaseCreation
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MGFContext entities = new MGFContext())
            {
                User user = new User() { LoginName = "admin" };
                entities.Users.Add(user);
                entities.SaveChanges();
            }
        }
    }
}
