using System;

namespace NLib.UI.Bindings
{
    public interface IValueConverter<TContextValue, TViewValue>
    {
        TViewValue Convert(TContextValue contextValue);

        TContextValue ConvertBack(TViewValue viewValue);
    }
}
