using MGF.Domain;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MGF.Mappers
{
    public abstract class MapperBase<T> where T : DomainBase
    {
        public XDocument ConvertToXML(string rootElementName, string data)
        {
            XElement rootElement = new XElement(rootElementName);

            if(!String.IsNullOrEmpty(data))
            {
                rootElement.Add(XDocument.Parse(data).Root.Nodes());
            }

            return new XDocument(rootElement);
        }

        public void Delete(int id)
        {
            this.DeleteNow(id);
        }

        public T Load(int id)
        {
            return Fetch(id);
        }

        public T Save(T obj)
        {
            if(null == obj) // Null == obj prevents object null exceptions trying to compare obj using its Equals operator.
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if(!obj.IsSavable)
            {
                // throw new InvalidOperationException("Operation cannot be completed on object in the current state");
            }

            if(obj.IsNew)
            {
                obj = this.Insert(obj);
            }
            else if(obj.IsDeleted)
            {
                obj = this.Delete(obj);
            }
            else if(obj.IsDirty)
            {
                obj = this.Update(obj);
            }

            return obj;
        }

        protected abstract T Delete(T domainObject);
        protected abstract void DeleteNow(int id);

        /// <summary>
        /// Gets all objects of this type and returns a list.
        /// </summary>
        /// <returns></returns>
        protected abstract IList<T> Fetch();

        protected abstract T Fetch(int id);
        protected abstract T Insert(T domainObject);

        /// <summary>
        /// Maps an entity over to an actual domain object. Provides separation from EF6 vs NHibernate classes
        /// </summary>
        /// <param name="domainObject"></param>
        /// <param name="entity"></param>
        protected abstract void Map(T domainObject, object entity);

        /// <summary>
        /// Updates the object to the actual database, until update is called, the entity might have unsaved changes, denoted by the IsDirty flag.
        /// </summary>
        /// <param name="domainObject"></param>
        protected abstract T Update(T domainObject);
    }
}
