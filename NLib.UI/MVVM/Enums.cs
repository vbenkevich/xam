using System;

namespace NLib.UI
{
    public enum UpdateOrder
    {
        BeforeAppear = 0,

        AfterAppear = 1,

        AfterReload = 2
    }

    public enum ViewState
    {
        Appearing = 0,

        Visible = 1,

        Hidden = 2,

        Unknown = 4,
    }
}
