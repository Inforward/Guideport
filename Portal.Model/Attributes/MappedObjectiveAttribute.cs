using System;

namespace Portal.Model.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MappedObjectiveAttribute : Attribute
    {
        public string ObjectiveName { get; set; }

        public MappedObjectiveAttribute(string objectiveName)
        {
            ObjectiveName = objectiveName;
        }
    }

    public static class MappedObjectiveExtensions
    {
        public static object GetMappedObjectiveProperty(this object e, string name)
        {
            object value = null;

            foreach (var property in e.GetType().GetProperties())
            {
                foreach (var attribute in property.GetCustomAttributes(false))
                {
                    var objectiveAttribute = attribute as MappedObjectiveAttribute;

                    if (objectiveAttribute != null && objectiveAttribute.ObjectiveName == name)
                    {
                        value = property.GetValue(e);
                    }
                }
            }

            return value;
        }

        public static T GetMappedObjectiveProperty<T>(this object e, string name)
        {
            var value = default(T);

            foreach (var property in e.GetType().GetProperties())
            {
                foreach (var attribute in property.GetCustomAttributes(false))
                {
                    var objectiveAttribute = attribute as MappedObjectiveAttribute;

                    if (objectiveAttribute != null && objectiveAttribute.ObjectiveName == name)
                    {
                        var obj = property.GetValue(e);

                        if (obj != null)
                        {
                            value = (T) obj;
                        }
                    }
                }
            }

            return value;
        }
    }
}
