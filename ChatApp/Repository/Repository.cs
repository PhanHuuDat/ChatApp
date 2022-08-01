using System.Linq.Expressions;

namespace ChatApp.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly List<T> _list;
        public Repository()
        {
            _list = new List<T>();
        }
        
        public void Add(T entity)
        {
            _list.Add(entity);
        }
        
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            IQueryable<T> query = _list.AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }         
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query;
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _list.AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
           
            return query.FirstOrDefault(defaultValue:null);
        }

        public void Remove(T entity)
        {
            _list.Remove(entity);
        }

        public void Replace(T oldEntity, T newEntity)
        {
            int index = _list.IndexOf(oldEntity);
            if (index != -1)
            {
                _list[index] = newEntity;
            }
        }

    }
}
