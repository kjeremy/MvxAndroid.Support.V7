using System;
using System.Windows.Input;
using Android.Support.V7.Widget;
using Android.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;

namespace MvxAndroid.Support.V7.Views
{
    public class MvxRecyclerViewViewHolder : RecyclerView.ViewHolder, IMvxRecyclerViewViewHolder
    {
        private object _cachedDataContext;

        public MvxRecyclerViewViewHolder(View itemView, IMvxAndroidBindingContext bindingContext)
            : base(itemView)
        {
            _bindingContext = bindingContext;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ClearAllBindings();
                _cachedDataContext = null;
            }

            base.Dispose(disposing);
        }

        protected IMvxAndroidBindingContext AndroidBindingContext
        {
            get { return this._bindingContext; }
        }

        public object DataContext
        {
            get { return _bindingContext.DataContext; }
            set { _bindingContext.DataContext = value; }
        }

        public void OnAttachedToWindow()
        {
            if (_cachedDataContext != null && DataContext == null)
                DataContext = _cachedDataContext;
        }

        public void OnDetachedFromWindow()
        {
            _cachedDataContext = DataContext;
            DataContext = null;
        }

        private readonly IMvxAndroidBindingContext _bindingContext;
        public IMvxBindingContext BindingContext
        {
            get { return _bindingContext; }
            set { throw new NotImplementedException("BindingContext is readonly in the list item"); }
        }

        private ICommand _click;
        public ICommand Click
        {
            get { return this._click; }
            set { this._click = value; if (this._click != null) this.EnsureClickOverloaded(); }
        }

        private bool _clickOverloaded = false;
        private void EnsureClickOverloaded()
        {
            if (this._clickOverloaded)
                return;
            this._clickOverloaded = true;
            this.ItemView.Click += (sender, args) => ExecuteCommandOnItem(this.Click);
        }

        private ICommand _longClick;
        public ICommand LongClick
        {
            get { return this._longClick; }
            set { this._longClick = value; if (this._longClick != null) this.EnsureLongClickOverloaded(); }
        }

        private bool _longClickOverloaded = false;
        private void EnsureLongClickOverloaded()
        {
            if (this._longClickOverloaded)
                return;
            this._longClickOverloaded = true;
            this.ItemView.LongClick += (sender, args) => ExecuteCommandOnItem(this.LongClick);
        }

        protected virtual void ExecuteCommandOnItem(ICommand command)
        {
            if (command == null)
                return;

            var item = DataContext;
            if (item == null)
                return;

            if (!command.CanExecute(item))
                return;

            command.Execute(item);
        }
    }
}