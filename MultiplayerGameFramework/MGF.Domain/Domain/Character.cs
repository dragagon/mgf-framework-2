using MGF.Mappers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                stats = IsNew || 0 == id
                    ? new List<Stat>()
                    : CharacterMapper.LoadStats(this);
            }
        }

        // Add to inventory
        // Ensure can craft
        // etc etc
        #endregion
    }
}
