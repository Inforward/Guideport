using Newtonsoft.Json;
using Portal.Model;
using RefactorThis.GraphDiff;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Portal.Data.Sql.EntityFramework
{
    public abstract class EntityRepository<TContext> : IEntityRepository where TContext : DbContext, new()
    {
        private readonly TContext _context;
        private bool disposed;

        private AuditLogFactory _auditLogFactory;
        private readonly List<AuditLog> _auditList = new List<AuditLog>();
        private readonly List<DbEntityEntry> _auditEntityList = new List<DbEntityEntry>();

        public TContext Context
        {
            get { return _context; }
        }

        protected EntityRepository()
        {
            if (_context != null) return;

            _context = new TContext();
            _auditLogFactory = new AuditLogFactory(_context);
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public IQueryable<TEntity> FindBy<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return _context.Set<TEntity>().Where(predicate);
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public EntityState GetEntityState<TEntity>(TEntity entity) where TEntity : class
        {
            return _context.Entry(entity).State;
        }

        public int Save()
        {
            int retVal;

            try
            {
                _auditList.Clear();
                _auditEntityList.Clear();

                var entityList = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Deleted || e.State == EntityState.Modified);

                foreach (var entity in entityList.Where(e => e.Entity is Auditable))
                {
                    var auditLog = _auditLogFactory.GetAuditLog(entity);

                    if (auditLog.ChangesExist)
                    {
                        _auditList.Add(auditLog);
                        _auditEntityList.Add(entity);
                    }
                }

                retVal = _context.SaveChanges();

                if (_auditList.Count > 0)
                {
                    for (var i = 0; i < _auditList.Count; i++)
                    {
                        var auditLog = _auditList[i];

                        if (auditLog.AuditTypeID == (byte) AuditTypes.Insert)
                            auditLog.TableIDValue = _auditLogFactory.GetKeyValue(_auditEntityList[i]);

                        Add(auditLog);
                    }

                    _context.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException("Entity Validation Failed - errors follow:\n" + sb.ToString(), ex);
            }

            return retVal;
        }

        public TEntity SaveGraph<TEntity>(TEntity entity) where TEntity : class, new()
        {
            var savedEntity = Context.UpdateGraph(entity);

            Save();

            return savedEntity;
        }

        protected void Audit<TEntity>(AuditTypes auditType, int? tableIdValue, int auditUserId, dynamic oldData, dynamic newData, Func<int> save)
            where TEntity : class, new()
        {
            Audit<TEntity>(null, auditType, tableIdValue, null, auditUserId, oldData, newData, save);
        }

        protected void Audit<TEntity>(string associationPropertyPath, AuditTypes auditType, int? tableIdValue, int? relatedKeyValue, int auditUserId, 
            dynamic oldData, dynamic newData, Func<int> save)
            where TEntity : class, new()
        {
            var auditLog = new AuditLog
            {
                AuditTypeID = (byte)auditType,
                AuditDate = DateTime.Now,
                UserID = auditUserId,
                TableName = _auditLogFactory.GetTableName(new TEntity(), associationPropertyPath),
                TableIDValue = tableIdValue,
                RelatedKeyName = relatedKeyValue != null ? _auditLogFactory.GetPrimaryKeyName(new TEntity()) : null,
                RelatedKeyValue = relatedKeyValue,
                OldData = oldData != null ? JsonConvert.SerializeObject(oldData, Formatting.Indented) : null,
                NewData = newData != null ? JsonConvert.SerializeObject(newData, Formatting.Indented) : null
            };

            var retVal = save();

            // Don't log if no records were updated or audit log indicates no changes exist
            if (retVal <= 0 || !auditLog.ChangesExist) return;

            // Assume save returns new identity when tableIdValue <= 0
            if (tableIdValue <= 0)
                auditLog.TableIDValue = retVal;

            Add(auditLog);
            _context.SaveChanges();
        }

        public DbContextTransaction BeginTransaction()
        {
            return Context.Database.BeginTransaction();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            disposed = true;
        }
    }
}