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
    public class User : DomainBase
    {
        #region Private Fields
        private int id;
        private string loginName;
        private string passwordHash;
        private string salt;

        private List<Character> characters;

        private static User nullValue = new User();
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

        public string LoginName
        {
            get { return loginName; }
            set
            {
                if(null == value)
                {
                    value = string.Empty;
                }
                if(loginName != value)
                {
                    loginName = value;
                    PropertyHasChanged(nameof(LoginName));
                }
            }
        }
        public string PasswordHash
        {
            get { return passwordHash; }
            set
            {
                if (null == value)
                {
                    value = string.Empty;
                }
                if (passwordHash != value)
                {
                    passwordHash = value;
                    PropertyHasChanged(nameof(PasswordHash));
                }
            }
        }
        public string Salt
        {
            get { return salt; }
            set
            {
                if (null == value)
                {
                    value = string.Empty;
                }
                if (salt != value)
                {
                    salt = value;
                    PropertyHasChanged(nameof(Salt));
                }
            }
        }
        public List<Character> Characters
        {
            get
            {
                EnsureCharacterListExists();
                return characters;
            }
        }
        #endregion

        #region Constructors
        public User() : base() { }

        // Called when loading
        public User(int id, string loginName, string passwordHash, string salt)
        {
            this.id = id;
            this.loginName = loginName;
            this.passwordHash = passwordHash;
            this.salt = salt;
            base.MarkOld();
        }

        #endregion

        #region Methods

        // Business logic goes here

        protected void EnsureCharacterListExists()
        {
            // protection from null object reference exceptions
            if(null == characters)
            {
                characters = (IsNew || 0 == id)
                    ? new List<Character>()
                    : UserMapper.LoadCharacters(this).ToList();
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

            User other = obj as User;
            if(null == other)
            {
                return false;
            }

            // Get the hash code and make sure they are equal, but also any other important fields.
            return this.GetHashCode().Equals(other.GetHashCode()) &&
                   this.Characters.SequenceEqual(other.Characters);
            // return this.GetHashCode().Equals(other.GetHashCode()) && this.Value.Equals(other.Value);

        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, "{0}: {1} ({2})",
                this.GetType(), loginName, id);
        }

        public static User NullValue
        {
            get { return nullValue; }
        }
        #endregion
    }
}
