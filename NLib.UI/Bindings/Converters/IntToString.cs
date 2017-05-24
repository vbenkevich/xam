using System;

namespace NLib.UI.Bindings
{
    class IntToString : IValueConverter<int, string>
    {
        readonly bool throwException;

        public IntToString(bool throwException = false)
        {
            this.throwException = throwException;
        }

        public string Convert(int contextValue)
        {
            return contextValue.ToString();
        }

        public int ConvertBack(string viewValue)
        {
            if (throwException)
            {
                return int.Parse(viewValue);
            }
            else
            {
                int integerValue;
                return int.TryParse(viewValue, out integerValue) ? integerValue : 0;
            }
        }
    }
}
