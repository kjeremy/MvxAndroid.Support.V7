using Android.Support.V7.Widget;
using Android.Views;
using Cirrious.CrossCore.Core;

namespace MvxAndroid.Support.V7.AppCompat.Views
{
    public class MvxRecyclerViewViewHolder : RecyclerView.ViewHolder, IMvxDataConsumer
    {
        public MvxRecyclerViewViewHolder(View itemView)
            : base(itemView)
        {
        }


        public object DataContext { get; set; }
    }
}