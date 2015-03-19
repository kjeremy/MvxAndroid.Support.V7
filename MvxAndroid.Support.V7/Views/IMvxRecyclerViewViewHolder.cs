using Cirrious.MvvmCross.Binding.BindingContext;

namespace MvxAndroid.Support.V7.Views
{
    public interface IMvxRecyclerViewViewHolder : IMvxBindingContextOwner
    {
        object DataContext { get; set; }

        void OnAttachedToWindow();
        void OnDetachedFromWindow();
    }
}