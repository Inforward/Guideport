using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Model
{
    public class AuditLog
    {
        public int AuditLogID { get; set; }
        public byte AuditTypeID { get; set; }
        public DateTime AuditDate { get; set; }
        public string TableName { get; set; }
        public int? TableIDValue { get; set; }
        public string RelatedKeyName { get; set; }
        public int? RelatedKeyValue { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public int UserID { get; set; }

        [NotMapped]
        public bool ChangesExist{ get { return OldData != NewData; } }
    }

    public enum AuditTypes : byte
    {
        Insert = 1,
        Update = 2,
        Delete = 3
    }
}
