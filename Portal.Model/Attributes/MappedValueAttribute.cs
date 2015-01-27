using System;

namespace Portal.Model
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class MappedValueAttribute : Attribute
    {
        public string Code { get; set; }
        public string SecondCode { get; set; }
    }

    public static class MappedExtensions
    {

        private static MappedValueAttribute GetMappedValueAttribute(Type enumType, string enumName)
        {
            var retVal = null as MappedValueAttribute;

            var field = enumType.GetField(enumName);

            var attributes = field.GetCustomAttributes(typeof(MappedValueAttribute), false) as MappedValueAttribute[];

            if (attributes != null && attributes.Length == 1)
            {
                retVal = attributes[0];
            }
            return retVal;
        }


        private static string MapTo(object e, Func<MappedValueAttribute, string> getValue)
        {
            if (e == null)
            {
                throw new InvalidOperationException();
            }

            var mappedValue = GetMappedValueAttribute(e.GetType(), e.ToString());

            if (mappedValue != null)
            {
                return getValue(mappedValue);
            }

            throw new InvalidOperationException();
        }


        private static ENUMVALUE MapToEnum<ENUMVALUE>(object mappedCode, Func<MappedValueAttribute, object, bool> isCodeMapped, ENUMVALUE defaultValue)
        {
            ENUMVALUE retVal = default(ENUMVALUE);
            bool foundEnum = false;

            var enumType = typeof(ENUMVALUE);
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException();
            }

            var enumFields = Enum.GetNames(enumType);

            foreach (var enumField in enumFields)
            {
                var mappedAttribute = GetMappedValueAttribute(enumType, enumField);
                if (mappedAttribute != null && isCodeMapped(mappedAttribute, mappedCode))
                {
                    retVal = (ENUMVALUE)Enum.Parse(enumType, enumField);
                    foundEnum = true;
                }
            }

            if (!foundEnum)
            {
                return defaultValue;
            }

            return retVal;
        }

        public static string GetMappedCode(this object e)
        {
            return MapTo(e, map => map.Code);
        }

        public static string GetMappedSecondCode(this object e)
        {
            return MapTo(e, map => map.SecondCode);
        }

        public static ENUMVALUE MapFromCodeTo<ENUMVALUE>(this object mappedCode, ENUMVALUE defaultValue)
        {
            return MapToEnum(mappedCode, (attr, val) => attr.Code.Equals(val.ToString(), StringComparison.InvariantCultureIgnoreCase), defaultValue);
        }

        public static ENUMVALUE MapFromSecondCodeTo<ENUMVALUE>(this object mappedSecondCode, ENUMVALUE defaultValue)
        {
            return MapToEnum(mappedSecondCode, (attr, val) => attr.SecondCode.Equals(val.ToString(), StringComparison.InvariantCultureIgnoreCase), defaultValue);
        }
    }
}
