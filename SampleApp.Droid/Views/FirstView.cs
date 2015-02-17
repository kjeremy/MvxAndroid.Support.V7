using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Views;
using MvxAndroid.Support.V7.AppCompat.Views;
using SampleApp.Core.ViewModels;

namespace SampleApp.Views
{
    [Activity(Label = "View for FirstViewModel")]
    public class FirstView : MvxActivity<FirstViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FirstView);

            var recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.my_recycler_view);
            recyclerView.HasFixedSize = true;

            var layoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(layoutManager);
        }
    }
}