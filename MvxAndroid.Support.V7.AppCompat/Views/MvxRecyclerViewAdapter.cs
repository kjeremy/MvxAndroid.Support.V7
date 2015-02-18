using System;
using System.Collections;
using System.Collections.Specialized;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.WeakSubscription;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Binding.ExtensionMethods;

namespace MvxAndroid.Support.V7.AppCompat.Views
{
    public class MvxRecyclerViewAdapter : RecyclerView.Adapter, IMvxRecyclerViewAdapter
    {
        private readonly Context _context;
        private readonly IMvxAndroidBindingContext _bindingContext;
        private int _itemTemplateId;
        private IEnumerable _itemsSource;
        private IDisposable _subscription;

        public MvxRecyclerViewAdapter(Context context)
            : this(context, MvxAndroidBindingContextHelpers.Current())
        {}

        public MvxRecyclerViewAdapter(Context context, IMvxAndroidBindingContext bindingContext)
        {
            _context = context;
            _bindingContext = bindingContext;

            if (_bindingContext == null)
            {
                throw new MvxException(
                    "bindingContext is null during MvxAdapter creation - Adapter's should only be created when a specific binding context has been placed on the stack");
            }
        }

        protected Context Context
        {
            get { return _context; }
        }

        protected IMvxAndroidBindingContext BindingContext
        {
            get { return _bindingContext; }
        }

        public virtual int ItemTemplateId
        {
            get { return _itemTemplateId; }
            set
            {
                if (_itemTemplateId == value)
                    return;
                _itemTemplateId = value;

                if (_itemsSource != null)
                    this.NotifyDataSetChanged();
            }
        }

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set { SetItemsSource(value); }
        }

        protected virtual void SetItemsSource(IEnumerable value)
        {
            if (Object.ReferenceEquals(_itemsSource, value))
                return;

            if (_subscription != null)
            {
                _subscription.Dispose();
                _subscription = null;
            }

            _itemsSource = value;

            if (_itemsSource != null && !(_itemsSource is IList))
            {
                MvxBindingTrace.Trace(
                    MvxTraceLevel.Warning,
                    "Binding to IEnumerable rather than IList - this can be inefficient, especially for large lists");
            }

            var newObservable = _itemsSource as INotifyCollectionChanged;
            if (newObservable != null)
                _subscription = newObservable.WeakSubscribe(OnItemsSourceCollectionChanged);

            NotifyDataSetChanged();
        }

        protected virtual void  OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyDataSetChanged(e);
        }

        protected virtual void NotifyDataSetChanged(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems.Count == 1)
                        this.NotifyItemInserted(e.NewStartingIndex);
                    else
                        this.NotifyItemRangeInserted(e.NewStartingIndex, e.NewItems.Count);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems.Count == 1)
                        this.NotifyItemRemoved(e.OldStartingIndex);
                    else
                        this.NotifyItemRangeRemoved(e.OldStartingIndex, e.OldItems.Count);
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldItems.Count == 1)
                        this.NotifyItemMoved(e.OldStartingIndex, e.NewStartingIndex);
                    else
                        this.NotifyDataSetChanged();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    this.NotifyItemRangeChanged(e.OldStartingIndex, e.OldItems.Count);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.NotifyDataSetChanged();
                    break;
            }
        }

        public virtual new void NotifyDataSetChanged()
        {
            RealNotifyDataSetChanged();
        }

        protected virtual void RealNotifyDataSetChanged()
        {
            try
            {
                base.NotifyDataSetChanged();
            }
            catch (Exception exception)
            {
                Mvx.Warning("Exception masked during Adapter RealNotifyDataSetChanged {0}", exception.ToLongString());
            }
        }

        public override int ItemCount
        {
            get { return this._itemsSource.Count(); }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public virtual object GetRawItem(int position)
        {
            return this._itemsSource.ElementAt(position);
        }

        public virtual int GetPosition(object value)
        {
            return this._itemsSource.GetPosition(value);
        }

        public override void OnViewAttachedToWindow(Java.Lang.Object holder)
        {
            base.OnViewAttachedToWindow(holder);

            var viewHolder = (IMvxRecyclerViewViewHolder)holder;
            viewHolder.OnAttachedToWindow();
        }

        public override void OnViewDetachedFromWindow(Java.Lang.Object holder)
        {
            base.OnViewDetachedFromWindow(holder);

            var viewHolder = (IMvxRecyclerViewViewHolder)holder;
            viewHolder.OnDetachedFromWindow();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var bindingContext = CreateBindingContextForViewHolder();

            View view = InflateViewForHolder(parent, viewType, bindingContext);
            return new MvxRecyclerViewViewHolder(view, bindingContext);
        }

        protected virtual IMvxAndroidBindingContext CreateBindingContextForViewHolder()
        {
            return new MvxAndroidBindingContext(_context, _bindingContext.LayoutInflater);
        }

        protected virtual View InflateViewForHolder(ViewGroup parent, int viewType, IMvxAndroidBindingContext bindingContext)
        {
            return bindingContext.BindingInflate(this._itemTemplateId, parent, false);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var source = this.GetRawItem(position);

            var mvxViewHolder = (MvxRecyclerViewViewHolder)holder;
            mvxViewHolder.DataContext = source;
        }
    }
}