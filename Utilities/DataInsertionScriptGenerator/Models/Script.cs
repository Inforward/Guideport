using System.Collections.Generic;
using System.Linq;

namespace DataInsertionScriptGenerator.Models
{
    public class Script
    {
        public Script()
        {
            ExcludeTables = new List<Table>();
            IncludeTables = new List<Table>();
        }

        public string Name { get; set; }
        public string DefaultSchema { get; set; }
        public string DefaultDatabase { get; set; }
        public string TablePrefix { get; set; }
        public List<Table> ExcludeTables { get; set; }
        public List<Table> IncludeTables { get; set; }
        public bool GenerateCreateTable { get; set; }

        public string Configure(string sqlTemplate, string database, string schema, string stagingDatabase)
        {
            var output = sqlTemplate.Replace("__DatabaseName__", database)
                                    .Replace("__SchemaName__", schema)
                                    .Replace("__TableNamePrefix__", TablePrefix)
                                    .Replace("__ExcludeTables__", string.Join(",", 
                                        ExcludeTables.Where(t => t.Database.Equals(database) && t.Schema.Equals(schema)).Select(t => t.Name)))
                                    .Replace("__IncludeTables__", string.Join(",", 
                                        IncludeTables.Where(t => t.Database.Equals(database) && t.Schema.Equals(schema)).Select(t => t.Name)))
                                    .Replace("__StagingDatabaseName__", stagingDatabase);

            return output;
        }

        public override string ToString()
        {
            return string.Format("Name: {0}\n\tGenerateCreateTable: {1}\n\tFilters\n\t\tDefaultSchema: {2}\n\t\tDefaultDatabase: {3}\n\tTablePrefix: {4}\n\tExcludeTables:\t{5}\n\tIncludeTables:\t{6}",
                Name, GenerateCreateTable, DefaultSchema, DefaultDatabase, TablePrefix, string.Join("\n\t\t\t", ExcludeTables.Select(t => t)), string.Join("\n\t\t\t", IncludeTables.Select(t => t)));
        }
    }
}
