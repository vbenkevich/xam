using System;
using Foundation;
using UIKit;

namespace NLib.iOS.TableView
{
    public class DataSource : UITableViewDataSource
    {
        public DataSource()
        {
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            throw new NotImplementedException();
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            throw new NotImplementedException();
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return base.NumberOfSections(tableView);
        }
    }

    public interface ICellFactory
    {
        bool TryGetCell(object cellData, UITableView tableView, NSIndexPath indexPath, out UITableViewCell cell);
    }

    public interface ITableSource
    {
        int SectionsCount { get; }

        int RowsCount(int sectionIndex);

        object GetItem(int sectionIndex, int itemIndex);
    }
}
