using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using DataInsertionScriptGenerator.Config;
using DataInsertionScriptGenerator.Models;

namespace DataInsertionScriptGenerator.Helpers
{
    public static class Util
    {
        public static List<Script> GetScripts()
        {
            var scripts = new List<Script>();

            var config = (ScriptGeneratorSection)ConfigurationManager.GetSection("scriptGenerator");

            foreach (ScriptElement s in config.Scripts)
            {
                var script = new Script
                {
                    Name = s.Name, 
                    DefaultSchema = s.Filters.DefaultSchema, 
                    DefaultDatabase = s.Filters.DefaultDatabase, 
                    TablePrefix = s.Filters.TablePrefix,
                    GenerateCreateTable = s.GenerateCreateTable
                };

                foreach (TableElement t in s.Filters.ExcludeTables)
                {
                    if(!string.IsNullOrEmpty(t.Name.Trim()))
                        script.ExcludeTables.Add(new Table
                        {
                            Database = !string.IsNullOrEmpty(t.Database) ? t.Database : s.Filters.DefaultDatabase,
                            Schema = !string.IsNullOrEmpty(t.Schema) ? t.Schema : s.Filters.DefaultSchema, 
                            Name = t.Name
                        });
                }

                foreach (TableElement t in s.Filters.IncludeTables)
                {
                    if (!string.IsNullOrEmpty(t.Name.Trim()))
                        script.IncludeTables.Add(new Table
                        {
                            Database = !string.IsNullOrEmpty(t.Database) ? t.Database : s.Filters.DefaultDatabase,
                            Schema = !string.IsNullOrEmpty(t.Schema) ? t.Schema : s.Filters.DefaultSchema, 
                            Name = t.Name
                        });
                }

                scripts.Add(script);
            }

            return scripts;
        }

        public static string BytesToString(this long byteCount)
        {
            var suffixes = new[]{ "B", "KB", "MB", "GB", "TB", "PB", "EB" };

            if (byteCount == 0)
                return "0" + suffixes[0];

            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);

            return (Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture) + " " + suffixes[place];
        }
    }
}
