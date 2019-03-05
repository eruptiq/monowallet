using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Foundation;
using Monowallet.iOS.Cells;
using UIKit;

namespace Monowallet.iOS.Tables
{
    public class MessagesTableSource : UITableViewSource
    {
        public UITableView TableView { get; }

        private ObservableCollection<string> Messages { get; }

        public MessagesTableSource(UITableView tableView, ObservableCollection<string> messages)
        {
            TableView = tableView;
            Messages = messages;

            Messages.CollectionChanged += OnMessagesCollectionChanged;
        }

        private void OnMessagesCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var newIndexPaths = CreateNSIndexPathArray(args.NewStartingIndex, args.NewItems.Count);
                    TableView.InsertRows(newIndexPaths, UITableViewRowAnimation.Automatic);
                    TableView.ScrollToRow(NSIndexPath.FromRowSection(Messages.Count - 1, 0), UITableViewScrollPosition.Bottom, true);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    var oldIndexPaths = CreateNSIndexPathArray(args.OldStartingIndex, args.OldItems.Count);
                    TableView.DeleteRows(oldIndexPaths, UITableViewRowAnimation.Automatic);
                    break;
            }
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Messages.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (MessageCell)tableView.DequeueReusableCell(MessageCell.Key);
            var msg = Messages[indexPath.Row];
            cell.Label.Text = msg;

            return cell;
        }

        private NSIndexPath[] CreateNSIndexPathArray(int startingPosition, int count)
        {
            var newIndexPaths = new NSIndexPath[count];
            for (var i = 0; i < count; i++)
            {
                newIndexPaths[i] = NSIndexPath.FromRowSection(i + startingPosition, 0);
            }

            return newIndexPaths;
        }
    }
}

