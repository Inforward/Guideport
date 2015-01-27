using System.Collections.Generic;

namespace Portal.Model.Interfaces
{
    public interface IHierarchy<T>
    {
        T Parent { get; set; }
        List<T> Children { get; set; }
    }
}
