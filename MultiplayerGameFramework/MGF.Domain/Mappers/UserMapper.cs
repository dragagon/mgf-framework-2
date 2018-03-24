using MGF.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace MGF.Mappers
{
    public class UserMapper : MapperBase<User>
    {
        protected override User Delete(User domainObject)
        {
            if(null == domainObject)
            {
                throw new ArgumentNullException(nameof(domainObject));
            }

            // Immediately call delete now and return the object
            DeleteNow(domainObject.Id);
            return domainObject;
        }

        public static User LoadByUserName(string loginName)
        {
            User userObject = null;
            using (MGFContext entities = new MGFContext())
            {
                DataEntities.User entity = entities.Users
                    // Eagerly grab this entities linked object - Stats
                    //.Include(characterEntity => characterEntity.Stats)
                    .FirstOrDefault(userEntity => userEntity.LoginName == loginName);

                if (entity != null)
                {
                    // Load data and extra data such as linked objects or XML data etc
                    userObject = new User(entity.Id, entity.LoginName, entity.PasswordHash, entity.Salt);
                }
            }
            return userObject;
        }

        protected override void DeleteNow(int id)
        {
            using (MGFContext entities = new MGFContext())
            {
                MGF.DataEntities.User entity = new DataEntities.User { Id = id };
                // Gets the character list and attaches the entity to the contain (makes this object exist in the list of objects).
                entities.Users.Attach(entity);
                // Remove the character from the container
                entities.Users.Remove(entity);
                entities.SaveChanges();
            }
        }

        // Gets a list of ALL Characters in the database
        protected override IList<User> Fetch()
        {
            using (MGFContext entities = new MGFContext())
            {
                return entities.Users
                    // Do not cache the entities in EF
                    .AsNoTracking()
                    // Order the entities by ID
                    .OrderBy(userEntity => userEntity.Id)
                    // Execute the query and return a list
                    .ToList()
                    // Using the list of entities, create new DomainBase Characters
                    .Select(userEntity => new User(
                        userEntity.Id,
                        userEntity.LoginName,
                        userEntity.PasswordHash,
                        userEntity.Salt))
                    // return a List<Character> of characters
                    .ToList();
            }
        }

        protected override User Fetch(int id)
        {
            User userObject = null;
            using (MGFContext entities = new MGFContext())
            {
                DataEntities.User entity = entities.Users
                    // Eagerly grab this entities linked object - Stats
                    //.Include(characterEntity => characterEntity.Stats)
                    .FirstOrDefault(userEntity => userEntity.Id == id);

                if(entity != null)
                {
                    // Load data and extra data such as linked objects or XML data etc
                    userObject = new User(entity.Id, entity.LoginName, entity.PasswordHash, entity.Salt);
                }
            }
            return userObject;
        }

        protected override User Insert(User domainObject)
        {
            using (MGFContext entities = new MGFContext())
            {
                DataEntities.User entity = new DataEntities.User();
                Map(domainObject, entity);
                entities.Users.Add(entity);
                domainObject = SaveChanges(entities, entity);
            }
            return domainObject;
        }

        // One way mapping of all data in the domain object to the entity for adding/updating
        protected override void Map(User domainObject, object entity)
        {
            DataEntities.User userEntity = entity as DataEntities.User;

            if(null == domainObject)
            {
                throw new ArgumentNullException(nameof(domainObject));
            }
            if(null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if(null == userEntity)
            {
                throw new ArgumentOutOfRangeException(nameof(entity));
            }

            // Map all fields from the domain object to the entity except the ID if it isn't allowed to change (most IDs should NEVER be changed)
            //characterEntity.Id = domainObject.Id;
            userEntity.LoginName = domainObject.LoginName;
            userEntity.PasswordHash = domainObject.PasswordHash;
            userEntity.Salt = domainObject.Salt;
        }

        protected override User Update(User domainObject)
        {
            // Pull out the id because we'll be using it in a lambda that might be deferred when calling and the thread may not have access to the domain object's context
            // (yay multithreading)
            int id;

            if(null == domainObject)
            {
                throw new ArgumentNullException(nameof(domainObject));
            }

            id = domainObject.Id;
            using (MGFContext entities = new MGFContext())
            {
                DataEntities.User entity = entities.Users
                    .Include(userEntity => userEntity.Characters)
                    .FirstOrDefault(userEntity => userEntity.Id == id);

                if(entity != null)
                {
                    Map(domainObject, entity);
                    domainObject = SaveChanges(entities, entity);
                }
            }
            return domainObject;
        }

        private User SaveChanges(MGFContext entities, DataEntities.User entity)
        {
            // Save everything in the context (unit of work means it should only be this entity and anything it contains)
            entities.SaveChanges();
            // reload what the database has based on the ID that we modified
            return Fetch(entity.Id);
        }

        public static IList<Character> LoadCharacters(User domainObject)
        {
            int id;
            List<Character> characters;

            if(null == domainObject)
            {
                throw new ArgumentNullException(nameof(domainObject));
            }

            id = domainObject.Id;

            characters = new List<Character>();
            using (MGFContext entities = new MGFContext())
            {
                var query = entities.Characters
                    .Where(characterEntity => characterEntity.UserId == id);
                foreach(DataEntities.Character character in query)
                {
                    characters.Add(new Character(character.Id, character.Name));
                }
            }

            return characters;
        }
    }
}
