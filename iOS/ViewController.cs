using System;
using System.Collections.ObjectModel;
using UIKit;

namespace CellCounter.iOS
{
    public partial class ViewController : UITableViewController
    {
        Collection<Int32> numbers = new Collection<int>();
        Int32 counter = 0;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            IncreaseCounter();
        }

        public override nint NumberOfSections(UITableView tableView) => 1;

        public override nint RowsInSection(UITableView tableView, nint section) => numbers.Count;

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("BasicCell", indexPath);
            cell.TextLabel.Text = "This is cell number " + numbers[indexPath.Row];
            return cell;
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
            ShowAlert(indexPath.Row);
        }

        private void ShowAlert(Int32 index)
        {
            var alert = UIAlertController.Create(title: "Tap!", message: "You clicked cell number " + index, preferredStyle: UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("Dismiss", UIAlertActionStyle.Cancel, null));
            alert.AddAction(UIAlertAction.Create("Increase", UIAlertActionStyle.Default, (obj) => {
                IncreaseCounter();
                TableView.ReloadData();
            }));
            alert.AddAction(UIAlertAction.Create("Decrease", UIAlertActionStyle.Destructive, (obj) => {
                DecreaseCounter();
                TableView.ReloadData();
            }));
            PresentViewController(alert, true, null);
        }

        private void DecreaseCounter()
        {
            if (counter > 1)
            {
                --counter;
                numbers.Remove(counter);
                UpdateTitle();
            }
            else
            {
                var alert = UIAlertController.Create(title: "Ugh!", message: "You can't remove the last cell.", preferredStyle: UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Dismiss", UIAlertActionStyle.Cancel, null));
                PresentViewController(alert, true, null);
            }
        }

        private void IncreaseCounter()
        {
            numbers.Add(counter);
            ++counter;
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            Title = "You Have " + numbers.Count + " Cell";
            if (numbers.Count > 1)
                Title = Title + "s";
        }
    }
}
