using Portal.Data;
using Portal.Domain.Services;

namespace Portal.Services
{
    public class Group : GroupService
    {
        public Group(IGroupRepository groupRepository)
            : base(groupRepository)
        { }
    }
}
