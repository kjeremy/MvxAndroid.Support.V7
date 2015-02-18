using Cirrious.MvvmCross.Binding.BindingContext;

namespace MvxAndroid.Support.V7.AppCompat.Views
{
    public interface IMvxRecyclerViewViewHolder : IMvxBindingContextOwner
    {
        object DataContext { get; set; }

        void OnAttachedToWindow();
        void OnDetachedFromWindow();
    }
}