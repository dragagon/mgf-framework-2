using MGF.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace MGF.Mappers
{
    public class CharacterMapper : MapperBase<Character>
    {
        protected override Character Delete(Character domainObject)
        {
            if(null == domainObject)
            {
                throw new ArgumentNullException(nameof(domainObject));
            }

            // Immediately call delete now and return the object
            DeleteNow(domainObject.Id);
            return domainObject;
        }

        protected override void DeleteNow(int id)
        {
            using (MGFContext entities = new MGFContext())
            {
                MGF.DataEntities.Character entity = new DataEntities.Character { Id = id };
                // Gets the character list and attaches the entity to the contain (makes this object exist in the list of objects).
                entities.Characters.Attach(entity);
                // Remove the character from the container
                entities.Characters.Remove(entity);
                entities.SaveChanges();
            }
        }

        // Gets a list of ALL Characters in the database
        protected override IList<Character> Fetch()
        {
            using (MGFContext entities = new MGFContext())
            {
                return entities.Characters
                    // Do not cache the entities in EF
                    .AsNoTracking()
                    // Order the entities by ID
                    .OrderBy(characterEntity => characterEntity.Id)
                    // Execute the query and return a list
                    .ToList()
                    // Using the list of entities, create new DomainBase Characters
                    .Select(characterEntity => new Character(
                        characterEntity.Id,
                        characterEntity.Name))
                    // return a List<Character> of characters
                    .ToList();
            }
        }

        protected override Character Fetch(int id)
        {
            Character characterObject = null;
            using (MGFContext entities = new MGFContext())
            {
                DataEntities.Character entity = entities.Characters
                    // Eagerly grab this entities linked object - Stats
                    //.Include(characterEntity => characterEntity.Stats)
                    .FirstOrDefault(characterEntity => characterEntity.Id == id);

                if(entity != null)
                {
                    // Load data and extra data such as linked objects or XML data etc
                    characterObject = new Character(entity.Id, entity.Name);
                }
            }
            return characterObject;
        }

        protected override Character Insert(Character domainObject)
        {
            using (MGFContext entities = new MGFContext())
            {
                DataEntities.Character entity = new DataEntities.Character();
                Map(domainObject, entity);
                entities.Characters.Add(entity);
                domainObject = SaveChanges(entities, entity);
            }
            return domainObject;
        }

        public IList<Character> LoadByUserId(int userId)
        {
            using (MGFContext entities = new MGFContext())
            {
                return entities.Characters
                    // Do not cache the entities in EF
                    .AsNoTracking()
                    // Order the entities by ID
                    .OrderBy(characterEntity => characterEntity.Id)
                    // Find all characters that match the provided userId
                    .Where(characterEntity => characterEntity.UserId == userId)
                    // Execute the query and return a list
                    .ToList()
                    // Using the list of entities, create new DomainBase Characters
                    .Select(characterEntity => new Character(
                        characterEntity.Id,
                        characterEntity.Name))
                    // return a List<Character> of characters
                    .ToList();
            }
        }

        // One way mapping of all data in the domain object to the entity for adding/updating
        protected override void Map(Character domainObject, object entity)
        {
            DataEntities.Character characterEntity = entity as DataEntities.Character;

            if(null == domainObject)
            {
                throw new ArgumentNullException(nameof(domainObject));
            }
            if(null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if(null == characterEntity)
            {
                throw new ArgumentOutOfRangeException(nameof(entity));
            }

            // Map all fields from the domain object to the entity except the ID if it isn't allowed to change (most IDs should NEVER be changed)
            //characterEntity.Id = domainObject.Id;
            characterEntity.Name = domainObject.Name;
            foreach(Stat stat in domainObject.Stats)
            {
                DataEntities.Stat statEntity = null;
                StatMapper mapper = new StatMapper();
                mapper.MapStat(stat, statEntity);
                characterEntity.Stats.Add(statEntity);
            }
        }

        protected override Character Update(Character domainObject)
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
                DataEntities.Character entity = entities.Characters
                    .Include(characterEntity => characterEntity.Stats)
                    .FirstOrDefault(characterEntity => characterEntity.Id == id);

                if(entity != null)
                {
                    Map(domainObject, entity);
                    domainObject = SaveChanges(entities, entity);
                }
            }
            return domainObject;
        }

        private Character SaveChanges(MGFContext entities, DataEntities.Character entity)
        {
            // Save everything in the context (unit of work means it should only be this entity and anything it contains)
            entities.SaveChanges();
            // reload what the database has based on the ID that we modified
            return Fetch(entity.Id);
        }

        public static IList<Stat> LoadStats(Character domainObject)
        {
            int id;
            List<Stat> stats;

            if(null == domainObject)
            {
                throw new ArgumentNullException(nameof(domainObject));
            }

            id = domainObject.Id;

            stats = new List<Stat>();
            using (MGFContext entities = new MGFContext())
            {
                var query = entities.Stats
                    .Where(statEntity => statEntity.CharacterId == id);
                foreach(DataEntities.Stat stat in query)
                {
                    stats.Add(new Stat(stat.StatId, stat.Name, stat.Value));
                }
            }

            return stats;
        }
    }
}
