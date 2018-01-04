using System;
using System.Collections.Generic;
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
        #endregion

    }
}
