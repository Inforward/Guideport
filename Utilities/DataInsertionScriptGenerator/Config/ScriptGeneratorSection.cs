using System;
using System.Configuration;

namespace DataInsertionScriptGenerator.Config
{
    public class ScriptGeneratorSection : ConfigurationSection
    {
        [ConfigurationProperty("scripts", Options = ConfigurationPropertyOptions.IsRequired)]
        public ScriptsCollection Scripts { get { return (ScriptsCollection) this["scripts"]; } }
    }

    [ConfigurationCollection(typeof(ScriptElement), AddItemName = "script")]
    public class ScriptsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ScriptElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return ((ScriptElement) element).Name;
        }
    }

    public class ScriptElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("generateCreateTable", IsRequired = false, DefaultValue = true)]
        public bool GenerateCreateTable
        {
            get { return (bool)this["generateCreateTable"]; }
            set { this["generateCreateTable"] = value; }
        }

        [ConfigurationProperty("filters", IsRequired = true)]
        public FiltersElement Filters
        {
            get { return (FiltersElement)this["filters"]; }
            set { this["filters"] = value; }
        }
    }

    public class FiltersElement : ConfigurationElement
    {
        [ConfigurationProperty("defaultSchema", IsRequired = false, DefaultValue = "dbo")]
        public string DefaultSchema
        {
            get { return (string)this["defaultSchema"]; }
            set { this["defaultSchema"] = value; }
        }

        [ConfigurationProperty("defaultDatabase", IsRequired = true)]
        public string DefaultDatabase
        {
            get { return (string)this["defaultDatabase"]; }
            set { this["defaultDatabase"] = value; }
        }

        [ConfigurationProperty("tablePrefix", IsRequired = false, DefaultValue = "")]
        public string TablePrefix
        {
            get { return (string)this["tablePrefix"]; }
            set { this["tablePrefix"] = value; }
        }

        [ConfigurationProperty("excludeTables", Options = ConfigurationPropertyOptions.None)]
        public TablesCollection ExcludeTables { get { return (TablesCollection)this["excludeTables"]; } }

        [ConfigurationProperty("includeTables", Options = ConfigurationPropertyOptions.None)]
        public TablesCollection IncludeTables { get { return (TablesCollection)this["includeTables"]; } }        
    }

    [ConfigurationCollection(typeof(TableElement), AddItemName = "table")]
    public class TablesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TableElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            var tableElement = ((TableElement) element);
            var key = tableElement.Schema + '.' + tableElement.Name;


            if (!string.IsNullOrEmpty(tableElement.Database))
                key = tableElement.Database + '.' + key;

            return key;
        }
    }

    public class TableElement : ConfigurationElement
    {
        [ConfigurationProperty("database", IsRequired = false)]
        public string Database
        {
            get { return (string)this["database"]; }
            set { this["databaseName"] = value; }
        }

        [ConfigurationProperty("schema", IsRequired = false)]
        public string Schema
        {
            get { return (string)this["schema"]; }
            set { this["schema"] = value; }
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
    }
}
