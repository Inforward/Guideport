
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Portal.Model.Report
{
    public class Category
    {
        public int CategoryID { get; set; }
        public int? ParentCategoryID { get; set; }
        public string Name { get; set; }
        public Category ParentCategory { get; set; }

        [IgnoreDataMember]
        public ICollection<View> Views { get; set; }

        [IgnoreDataMember]
        public ICollection<Category> SubCategories { get; set; }
    }
}