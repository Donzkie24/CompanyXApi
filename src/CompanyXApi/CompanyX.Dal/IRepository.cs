using System.Collections.Generic;
using CompanyX.Domain;

namespace CompanyX.Dal
{
    /// <summary>
    /// Repository pattern for CRUD operation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : DomainObject
    {
        /// <summary>
        /// get all rows
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(int id);

        /// <summary>
        /// Save and return id of the element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        int Save(T element);
    }
}
