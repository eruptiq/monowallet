using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Monowallet.Droid.Chat
{
    public class MessageViewHolder : RecyclerView.ViewHolder
    {
        public MessageViewHolder(View view) : base(view)
        {
            TextView = (TextView)view.FindViewById(Resource.Id.message_cell_text_view);
        }

        public TextView TextView { get; }
    }
}

