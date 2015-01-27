
namespace DataInsertionScriptGenerator.Models
{
    public class Table
    {
        public string Database { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            var key = Schema + '.' + Name;

            if (!string.IsNullOrEmpty(Database))
                key = Database + '.' + key;

            return key;
        }
    }
}
