
namespace Portal.Model.Interfaces
{
    public interface IBusinessPlanEntity
    {
        int? BusinessPlanID { get; set; }
        int? SortOrder { get; set; }
        string Name { get; set; }
    }
}
