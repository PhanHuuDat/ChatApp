using System.Linq.Expressions;

namespace ChatApp.Repository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Remove(T entity);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
        T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null);
       
    }
}
