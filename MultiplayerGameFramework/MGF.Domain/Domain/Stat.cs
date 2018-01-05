using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGF.Domain
{
    public class Stat : DomainBase
    {
        #region Private Fields
        private int id;
        private string name;

        private int value;

        private int characterId;
        private Character character;

        private static Stat nullValue = new Stat();
        #endregion

        #region Properties

        public int Id
        {
            get { return id; }
            set
            {
                if (null == value)
                {
                    value = 0;
                }
                if (id != value)
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
                if (null == value)
                {
                    value = string.Empty;
                }
                if (name != value)
                {
                    name = value;
                    PropertyHasChanged(nameof(Name));
                }
            }
        }

        public int Value
        {
            get { return value; }
            set
            {
                if(null == value)
                {
                    value = 0;
                }
                if(this.value != value)
                {
                    this.value = value;
                    PropertyHasChanged(nameof(Value));
                }
            }
        }

        public int CharacterId
        {
            get { return characterId; }
            set
            {
                if(value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(CharacterId));
                }
                if(this.characterId != value)
                {
                    this.characterId = value;
                    PropertyHasChanged(nameof(CharacterId));
                }
            }
        }
        #endregion

        #region Constructors

        public Stat() : base() { }

        public Stat(int id, string name, int value)
        {
            this.id = id;
            this.name = name;
            this.value = value;
            base.MarkOld();
        }
        #endregion

        #region Methods

        // Business logic goes here
        public override bool Equals(object obj)
        {
            if (null == obj)
            {
                return false;
            }

            Stat other = obj as Stat;
            if (null == other)
            {
                return false;
            }

            // Get the hash code and make sure they are equal, but also any other important fields.
            return this.GetHashCode().Equals(other.GetHashCode());
            // return this.GetHashCode().Equals(other.GetHashCode()) && this.Value.Equals(other.Value);

        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, "{0}: {1} - {3} ({2})",
                this.GetType(), name, id, value);
        }

        public static Stat NullValue
        {
            get { return nullValue; }
        }
        #endregion

    }
}
