namespace NLib.UI
{
    public interface IViewController
    {
        ViewModel ViewModel { get; }
    }

    public interface IViewController<TViewModel> : IViewController where TViewModel : ViewModel
    {
        new TViewModel ViewModel { get; set; }
    }
}
