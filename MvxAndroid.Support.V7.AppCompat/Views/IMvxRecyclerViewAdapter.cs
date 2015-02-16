using System.Collections;
using Cirrious.MvvmCross.Binding.Attributes;

namespace MvxAndroid.Support.V7.AppCompat.Views
{
    public interface IMvxRecyclerViewAdapter
    {
        [MvxSetToNullAfterBinding]
        IEnumerable ItemsSource { get; set; }

        int ItemTemplateId { get; set; }

        object GetRawItem(int position);
        int GetPosition(object value);
    }
}