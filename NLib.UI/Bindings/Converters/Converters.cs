using System;

namespace NLib.UI.Bindings
{
    public static class Converters
    {
        private static IValueConverter<int, string> intString;
        public static IValueConverter<int, string> IntString => intString ?? (intString = new IntToString());

        private static IValueConverter<int, string> intStringEx;
        public static IValueConverter<int, string> IntStringWithExeption => intStringEx ?? (intStringEx = new IntToString(true));
    }
}
