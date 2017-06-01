namespace NLib.UI
{
    public interface IViewController<TViewModel> where TViewModel : ViewModel
    {
        TViewModel ViewModel { get; set; }
    }
}
