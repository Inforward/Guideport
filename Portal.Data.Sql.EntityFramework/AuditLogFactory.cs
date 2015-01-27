using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Portal.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;

namespace Portal.Data.Sql.EntityFramework
{
   public class AuditLogFactory
    {
        private readonly DbContext _context;
        private static readonly string[] IgnorePropertyNameList =  { "ModifyDate", "ModifyDateUtc" };

        public AuditLogFactory(DbContext context)
        {
            _context = context;
        }

        public AuditLog GetAuditLog(DbEntityEntry entry)
        {
            var auditLog = new AuditLog
            {
                TableName = GetTableName(entry.Entity),
                TableIDValue = GetKeyValue(entry),
                AuditDate = DateTime.Now
            };

            var relatedKeyInfo = GetRelatedKeyInfo(entry);
            if (relatedKeyInfo != null)
            {
                auditLog.RelatedKeyName = relatedKeyInfo.Value.Key;
                auditLog.RelatedKeyValue = relatedKeyInfo.Value.Value;
            }

            switch (entry.State)
            {
                case EntityState.Added:
                {
                    auditLog.NewData = GetAddedProperties(entry);
                    auditLog.AuditTypeID = (byte)AuditTypes.Insert;
                    auditLog.UserID = entry.Entity is Auditable ? (entry.Entity as Auditable).CreateUserID : 0;
                }
                    break;
                case EntityState.Deleted:
                {
                    auditLog.OldData = GetDeletedProperties(entry);
                    auditLog.AuditTypeID = (byte) AuditTypes.Delete;
                    auditLog.UserID = entry.Entity is Auditable ? (entry.Entity as Auditable).ModifyUserID : 0;
                }
                    break;
                case EntityState.Modified:
                {
                    string oldValues, newValues;
                    SetModifiedProperties(entry, out oldValues, out newValues);
                    auditLog.OldData = oldValues;
                    auditLog.NewData = newValues;
                    auditLog.AuditTypeID = (byte) AuditTypes.Update;
                    auditLog.UserID = entry.Entity is Auditable ? (entry.Entity as Auditable).ModifyUserID : 0;
                }
                    break;
            }
 
            return auditLog;
        }

        public int GetKeyValue(DbEntityEntry entry)
        {
            var id = 0;
            var objectStateEntry = ((IObjectContextAdapter)_context).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);

            if (objectStateEntry.EntityKey.EntityKeyValues != null)
                id = Convert.ToInt32(objectStateEntry.EntityKey.EntityKeyValues[0].Value);

            return id;
        }

        public KeyValuePair<string, int>? GetRelatedKeyInfo(DbEntityEntry entry)
        {
            var values = entry.State == EntityState.Deleted ? entry.OriginalValues : entry.CurrentValues;
            var relatedKeyName = entry.Entity.GetType().GetProperties()
                                    .Where(p => p.GetCustomAttributes(typeof(ForeignKeyAttribute), false).Length > 0)
                                    .Select(p => p.Name).FirstOrDefault();


            if (!string.IsNullOrEmpty(relatedKeyName) && values[relatedKeyName] is int)
                return new KeyValuePair<string, int>(relatedKeyName, values.GetValue<int>(relatedKeyName));

            return null;
        }

        public string GetPrimaryKeyName<TEntity>(TEntity dbEntity)
        {
            var objectContext = ((IObjectContextAdapter)_context).ObjectContext;
            var objectSet = CreateObjectSet(objectContext, (dynamic)dbEntity);
            
            return objectSet.EntitySet.ElementType.KeyMembers[0].ToString();
        }

        public string GetTableName<TEntity>(TEntity dbEntity, string associationPropertyPath = null) where TEntity : class
        {
            var objectContext = ((IObjectContextAdapter)_context).ObjectContext;
            var objectSet = CreateObjectSet(objectContext, (dynamic) dbEntity);

            if (!string.IsNullOrEmpty(associationPropertyPath))
                objectSet = objectSet.Include(associationPropertyPath);

            var sql = objectSet.ToTraceString();
            var regex = new Regex(@"FROM\s+(?<table>.+)\s+AS");
            var matches = regex.Matches(sql);
            var match = matches[matches.Count - 1];
            var tableName = match.Groups["table"].Value.Replace("[", "").Replace("]", "");                

            return tableName ?? dbEntity.GetType().Name;
        }

        private static ObjectSet<T> CreateObjectSet<T>(ObjectContext context, T entity) where T : class
        {
        return context.CreateObjectSet<T>();
        }

        private static string GetAddedProperties(DbEntityEntry entry)
        {
            var newData = entry.CurrentValues.PropertyNames.ToDictionary(propertyName => propertyName, propertyName => entry.CurrentValues[propertyName]);

            return JsonConvert.SerializeObject(newData, Formatting.Indented);
        }
 
        private static string GetDeletedProperties(DbEntityEntry entry)
        {
            var dbValues = entry.GetDatabaseValues();
            var oldData = dbValues.PropertyNames.ToDictionary(propertyName => propertyName, propertyName => dbValues[propertyName]);

            return JsonConvert.SerializeObject(oldData, Formatting.Indented);
        }
 
        private static void SetModifiedProperties(DbEntityEntry entry, out string oldDataJson, out string newDataJson)
        {
            var oldData = new Dictionary<string, object>();
            var newData = new Dictionary<string, object>();

            var dbValues = entry.GetDatabaseValues();

            foreach (var propertyName in entry.OriginalValues.PropertyNames.Where(p => ! IgnorePropertyNameList.Contains(p)))
            {
                var oldValue = dbValues[propertyName];
                var newValue = entry.CurrentValues[propertyName];

                if (oldValue != null && newValue != null && !Equals(oldValue, newValue))
                {

                    oldData.Add(propertyName, oldValue);
                    newData.Add(propertyName, newValue);
                }
            }

            oldDataJson = JsonConvert.SerializeObject(oldData, Formatting.Indented);
            newDataJson = JsonConvert.SerializeObject(newData, Formatting.Indented);
        }
    }
}
