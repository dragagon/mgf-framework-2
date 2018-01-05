using MGF.Mappers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGF.Domain
{
    [Serializable]
    public class Character : DomainBase
    {
        #region Private Fields
        private int id;
        private string name;

        private List<Stat> stats;

        private static Character nullValue = new Character();
        #endregion

        #region Properties

        public int Id
        {
            get { return id; }
            set
            {
                if(null == value)
                {
                    value = 0;
                }
                if(id != value)
                {
                    id = value;
                    PropertyHasChanged(nameof(Id));
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if(null == value)
                {
                    value = string.Empty;
                }
                if(name != value)
                {
                    name = value;
                    PropertyHasChanged(nameof(Name));
                }
            }
        }

        public List<Stat> Stats
        {
            get
            {
                EnsureStatsListExists();
                return stats;
            }
        }
        #endregion

        #region Constructors
        public Character() : base() { }

        // Called when loading
        public Character(int id, string name)
        {
            this.id = id;
            this.name = name;
            base.MarkOld();
        }

        #endregion

        #region Methods

        // Business logic goes here

        protected void EnsureStatsListExists()
        {
            // protection from null object reference exceptions
            if(null == stats)
            {
                stats = (IsNew || 0 == id)
                    ? new List<Stat>()
                    : CharacterMapper.LoadStats(this).ToList();
            }
        }

        // Add to inventory
        // Ensure can craft
        // etc etc

        public override bool Equals(object obj)
        {
            if(null == obj)
            {
                return false;
            }

            Character other = obj as Character;
            if(null == other)
            {
                return false;
            }

            // Get the hash code and make sure they are equal, but also any other important fields.
            return this.GetHashCode().Equals(other.GetHashCode()) &&
                   this.Stats.SequenceEqual(other.Stats);
            // return this.GetHashCode().Equals(other.GetHashCode()) && this.Value.Equals(other.Value);

        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, "{0}: {1} ({2})",
                this.GetType(), name, id);
        }

        public static Character NullValue
        {
            get { return nullValue; }
        }
        #endregion
    }
}
