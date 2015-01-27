using System.Collections.Generic;

namespace Portal.Model
{
    public class UserResponse
    {
        public int TotalRecordCount { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
