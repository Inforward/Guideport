using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Model
{
    public class GroupResponse
    {
        public int TotalRecordCount { get; set; }
        public IEnumerable<Group> Groups { get; set; }
    }
}
