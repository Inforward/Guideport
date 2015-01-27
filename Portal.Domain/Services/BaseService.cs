using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Model;

namespace Portal.Domain.Services
{
    public abstract class BaseService
    {
        #region Protected Methods

        protected void AddAuditData(Auditable entity, int userId)
        {
            entity.CreateUserID = userId;
            entity.CreateDate = DateTime.Now;
            entity.CreateDateUtc = DateTime.UtcNow;

            UpdateAuditData(entity, userId);
        }

        protected void UpdateAuditData(Auditable entity, int userId)
        {
            entity.ModifyUserID = userId;
            entity.ModifyDate = DateTime.Now;
            entity.ModifyDateUtc = DateTime.UtcNow;
        }

        #endregion
    }
}
