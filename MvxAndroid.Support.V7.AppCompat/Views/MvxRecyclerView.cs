using System.Collections;
using System.Windows.Input;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace MvxAndroid.Support.V7.AppCompat.Views
{
    public class MvxRecyclerView : RecyclerView
    {
        public MvxRecyclerView(Context context, IAttributeSet attrs)
            : this(context, attrs, new MvxRecyclerViewAdapter())
        { }

        public MvxRecyclerView(Context context, IAttributeSet attrs, IMvxRecyclerViewAdapter adapter)
            : base(context, attrs)
        {
            if (adapter == null)
                return;

            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            adapter.ItemTemplateId = itemTemplateId;
            this.Adapter = adapter;
        }

        public new IMvxRecyclerViewAdapter Adapter
        {
            get { return this.GetAdapter(); }
            set { this.SetAdapter(value); }
        }

        public new IMvxRecyclerViewAdapter GetAdapter()
        {
            return base.GetAdapter() as IMvxRecyclerViewAdapter;
        }

        public virtual void SetAdapter(IMvxRecyclerViewAdapter adapter)
        {
            var existing = this.GetAdapter();
            if (existing == adapter)
                return;

            if (adapter != null && existing != null)
            {
                adapter.ItemsSource = existing.ItemsSource;
                adapter.ItemTemplateId = existing.ItemTemplateId;
            }
            base.SetAdapter((Adapter)adapter);
        }

        [MvxSetToNullAfterBinding]
        public IEnumerable ItemsSource
        {
            get { return this.Adapter.ItemsSource; }
            set { this.Adapter.ItemsSource = value; }
        }

        public int ItemTemplateId
        {
            get { return this.Adapter.ItemTemplateId; }
            set { this.Adapter.ItemTemplateId = value; }
        }

        protected virtual void ExecuteCommandOnItem(ICommand command, int position)
        {
            if (command == null)
                return;

            var item = this.Adapter.GetRawItem(position);
            if (item == null)
                return;

            if (!command.CanExecute(item))
                return;

            command.Execute(item);
        }
    }
}