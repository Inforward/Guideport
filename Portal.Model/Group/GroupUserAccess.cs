
namespace Portal.Model
{
    public partial class GroupUserAccess
    {
        public int GroupID { get; set; }
        public int UserID { get; set; }
        public bool IsReadOnly { get; set; }
        public User User { get; set; }
        public virtual Group Group { get; set; }
    }
}
