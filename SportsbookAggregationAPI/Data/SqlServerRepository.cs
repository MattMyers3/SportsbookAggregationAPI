using System.Linq;

namespace SportsbookAggregationAPI.Data
{
    public class SqlServerRepository<T> : IRepository<T> where T : class
    {
        private readonly Context context;

        public SqlServerRepository(Context context)
        {
            this.context = context;
        }

        public IQueryable<T> Read()
        {
            return context.Set<T>();
        }

        public void Create(T entity)
        {
            context.Add(entity);
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            context.Remove(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            context.Update(entity);
            context.SaveChanges();
        }
    }
}