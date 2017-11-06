using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MGF.Domain
{
    [Serializable()]
    public abstract class DomainBase : IProcessDirty
    {
        // Keep track of whether object is new, deleted or dirty
        private bool isObjectNew = true;
        private bool isObjectDirty = true;
        private bool isObjectDeleted;

        #region IProcessDirty Members

        [Browsable(false)]
        public bool IsNew
        {
            get { return isObjectNew; }
            set { isObjectNew = value; } // Only used during deserialization - must be public
        }

        [Browsable(false)]
        public bool IsDirty
        {
            get { return isObjectDirty; }
            set { isObjectDirty = value; }// Only used during deserialization - must be public
        }

        [Browsable(false)]
        public bool IsDeleted
        {
            get { return isObjectDeleted; }
            set { isObjectDeleted = value; }// Only used during deserialization - must be public
        }

        #endregion

        [NonSerialized()]
        private PropertyChangedEventHandler _nonSerializableHandlers;
        private PropertyChangedEventHandler _serializableHandlers;

        [
            Browsable(false),
            XmlIgnore()
        ]
        public virtual bool IsSavable
        {
            // Usually some Validation goes on here. Might add in a later video, but not right now.
            get { return isObjectDirty; }
        }

        // Pattern from CSLA.Net - a Domain Driven Design Pattern based on BindableBase - cslanet.com
        // Necessary to make serialization work properly and more importantly safely.
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if(value.Method.IsPublic &&
                    (value.Method.DeclaringType.IsSerializable || value.Method.IsStatic))
                {
                    _serializableHandlers = (PropertyChangedEventHandler)Delegate.Combine(_serializableHandlers, value);
                }
                else
                {
                    _nonSerializableHandlers = (PropertyChangedEventHandler)Delegate.Combine(_nonSerializableHandlers, value);
                }
            }
            remove
            {
                if(value.Method.IsPublic &&
                    (value.Method.DeclaringType.IsSerializable || value.Method.IsStatic))
                {
                    _serializableHandlers = (PropertyChangedEventHandler)Delegate.Remove(_serializableHandlers, value);
                }
                else
                {
                    _nonSerializableHandlers = (PropertyChangedEventHandler)Delegate.Remove(_nonSerializableHandlers, value);
                }
            }
        }

        // Automatically called by MarkDirty. Refreshes all properties (useful in applications that need to refresh data.)
        protected virtual void OnUnknownPropertyChanged()
        {
            OnPropertyChanged(string.Empty);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if(_nonSerializableHandlers != null)
            {
                _nonSerializableHandlers.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            if(_serializableHandlers != null)
            {
                _serializableHandlers.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Used by the constructor to denote a brand new object that is not stored in the database and ensures it isn't being marked for deletion.
        protected virtual void MarkNew()
        {
            isObjectNew = true;
            isObjectDeleted = false;
            MarkDirty();
        }

        // Used by Fetch to denote an object that already exists and has been pulled from the database
        protected virtual void MarkOld()
        {
            isObjectNew = false;
            MarkClean();
        }

        protected void MarkDeleted()
        {
            isObjectDeleted = true;
            MarkDirty();
        }

        // Call any time data changes to denote that the object needs to be saved.
        protected void MarkDirty()
        {
            MarkDirty(false);
        }

        protected void MarkDirty(bool suppressEvent)
        {
            isObjectDirty = true;
            if(!suppressEvent)
            {
                // Force properties to refresh - only useful for web pages and windows forms.
                OnUnknownPropertyChanged();
            }
        }

        protected void PropertyHasChanged()
        {
            PropertyHasChanged(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name.Substring(4));
        }

        protected virtual void PropertyHasChanged(string propertyName)
        {
            MarkDirty(true);
            OnPropertyChanged(propertyName);
        }

        protected void MarkClean()
        {
            isObjectDirty = false;
        }

        public virtual void Delete()
        {
            this.MarkDeleted();
        }
    }
}
