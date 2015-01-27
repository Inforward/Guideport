using System;

namespace Portal.Model
{
    [Serializable]
    public abstract class Auditable
    {
        public virtual int CreateUserID { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual DateTime CreateDateUtc { get; set; }
        public virtual int ModifyUserID { get; set; }
        public virtual DateTime ModifyDate { get; set; }
        public virtual DateTime ModifyDateUtc { get; set; }

        public void EnsureAuditFields(int userId = 0, DateTime? auditDate = null, DateTime? auditDateUtc = null)
        {
            auditDate = auditDate ?? DateTime.Now;
            auditDateUtc = auditDateUtc ?? DateTime.UtcNow;

            if (CreateUserID <= 0)
                CreateUserID = userId;

            if (CreateDate == DateTime.MinValue)
                CreateDate = auditDate.Value;

            if (CreateDateUtc == DateTime.MinValue)
                CreateDateUtc = auditDateUtc.Value;

            if (ModifyUserID <= 0)
                ModifyUserID = userId;

            if (ModifyDate == DateTime.MinValue)
                ModifyDate = auditDate.Value;

            if (ModifyDateUtc == DateTime.MinValue)
                ModifyDateUtc = auditDateUtc.Value;
        }

        public void SetAuditFields(int userId = 0, DateTime? auditDate = null, DateTime? auditDateUtc = null)
        {
            CreateUserID = ModifyUserID = userId;
            CreateDate = ModifyDate = auditDate ?? DateTime.Now;
            CreateDateUtc = ModifyDateUtc = auditDateUtc ?? DateTime.UtcNow;
        }

        public void UpdateAuditFields(int userId = 0, DateTime? auditDate = null, DateTime? auditDateUtc = null)
        {
            ModifyUserID = userId;
            ModifyDate = auditDate ?? DateTime.Now;
            ModifyDateUtc = auditDateUtc ?? DateTime.UtcNow;
        }

        protected Auditable()
        {
            SetAuditFields();
        }
    }
}
