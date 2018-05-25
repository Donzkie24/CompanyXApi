using System.Collections.Generic;
using System.Linq;
using CompanyX.Base.Helpers;
using CompanyX.Domain;

namespace CompanyX.Dal
{
    // TODO, currently domain models saved using temp in memory list object, 
    //TODO, use EFCore with unitOfWork would manage relation between models
    /// <inheritdoc />
    public class RepositoryBase<T> : IRepository<T> where T : DomainObject
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected RepositoryBase()
        {
            Repository = new List<T>();
        }

        /// <summary>
        /// Temp list to hold the data
        /// </summary>
        protected List<T> Repository;

        /// <inheritdoc />
        public IEnumerable<T> GetAll()
        {
            return Repository;
        }

        /// <inheritdoc />
        public T Get(int id)
        {
            Guard.IsNotNull(id, () => id);

            return GetAll().SingleOrDefault(a => a.Id == id);
        }

        /// <inheritdoc />
        public int Save(T element)
        {
            Guard.IsNotNull(element, () => element);

            T existing = Get(element.Id);
            if (existing != null)
            {
                element.Id = existing.Id;
                Repository.Remove(existing);
            }
            else
            {
                //TODO, temp fix to increment id
                var highestId = Repository.Any() ? Repository.Select(x => x.Id).Max() : 1;
                element.Id = highestId + 1;
            }

            Repository.Add(element);

            return element.Id;
        }
    }
}
