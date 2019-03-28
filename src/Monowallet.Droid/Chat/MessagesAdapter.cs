using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Android.Support.V7.Widget;
using Android.Views;

namespace Monowallet.Droid.Chat
{
    public class MessagesAdapter : RecyclerView.Adapter
    {
        public RecyclerView RecyclerView { get; }
        public ObservableCollection<string> Messages { get; }

        public override int ItemCount => Messages.Count;

        public MessagesAdapter(RecyclerView recyclerView, ObservableCollection<string> messages)
        {
            RecyclerView = recyclerView;
            Messages = messages;
            Messages.CollectionChanged += OnMessagesCollectionChanged;
        }

        private void OnMessagesCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    NotifyItemRangeInserted(args.NewStartingIndex, args.NewItems.Count);
                    RecyclerView.ScrollToPosition(Messages.Count - 1);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    NotifyItemRangeRemoved(args.OldStartingIndex, args.OldItems.Count);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    NotifyDataSetChanged();
                    break;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as MessageViewHolder;
            viewHolder.TextView.Text = Messages[position];
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.message_cell, parent, false);

            return new MessageViewHolder(view);
        }
    }
}

