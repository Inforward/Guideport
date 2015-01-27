using System;
using System.Linq;
using System.Data.Entity;

namespace Portal.Data
{
    public interface IEntityRepository : IDisposable
    {
        IQueryable<T> GetAll<T>() where T : class;
        IQueryable<T> FindBy<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class;
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        int Save();
        TEntity SaveGraph<TEntity>(TEntity entity) where TEntity : class, new();
        DbContextTransaction BeginTransaction();
    }
}
