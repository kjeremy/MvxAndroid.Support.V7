using System.Collections;
using System.Windows.Input;
using Cirrious.MvvmCross.Binding.Attributes;

namespace MvxAndroid.Support.V7.AppCompat.Views
{
    public interface IMvxRecyclerViewAdapter
    {
        [MvxSetToNullAfterBinding]
        IEnumerable ItemsSource { get; set; }

        int ItemTemplateId { get; set; }
        ICommand ItemClick { get; set; }
        ICommand ItemLongClick { get; set; }

        object GetRawItem(int position);
        int GetPosition(object value);
    }
}