using System;

namespace Portal.Infrastructure.Helpers
{
    public static class Required
    {
        public static void NotNull<T>( T input, string name )
        {
            if ( input == null )
            {
                throw new ArgumentNullException( name );
            }
        }

        public static void NotNullOrEmpty<T>( T input, T emptyValue, string name )
        {
            NotNull<T>( input, name );

            if ( input.Equals( emptyValue ) )
            {
                throw new ArgumentException( string.Format( "Argument value, \"{0}\", is invalid.", emptyValue ), name );
            }
        }

        public static void NotEmpty( string value, string name )
        {
            if ( string.IsNullOrWhiteSpace( value ) )
            {
                throw new ArgumentNullException( name );
            }
        }

        public static void NotEquals(object value, object referenceValue, string name)
        {
            if (value == referenceValue)
            {
                throw new ArgumentNullException(name);
            }
        }

    }
}