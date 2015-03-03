using System.Collections.ObjectModel;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace SampleApp.Core.ViewModels
{
    public class FirstViewModel 
        : MvxViewModel
    {
        private ObservableCollection<string> _awesomeItems;

        public FirstViewModel()
        {
            this.AwesomeItems = new ObservableCollection<string>(new[] {"Candy", "Ninjas", "Robots", "Foxes", "Shiny Stuff", "Skis", "Cheese", "Money", "Barons"});
        }

        public ObservableCollection<string> AwesomeItems
        {
            get { return _awesomeItems; }
            set { SetProperty(ref _awesomeItems, value); }
        }

        #region Clicked AwesomeItem

        private string _clickedAwesomeItem;

        public string ClickedAwesomeItem
        {
            get { return _clickedAwesomeItem; }
            set { SetProperty(ref _clickedAwesomeItem, value); }
        }

        #endregion

        #region Click command

        private MvxCommand<string> _clickCommand;

        public ICommand ClickCommand
        {
            get
            {
                _clickCommand = _clickCommand ?? new MvxCommand<string>(DoClickCommand);
                return _clickCommand;
            }
        }

        private void DoClickCommand(string item)
        {
            ClickedAwesomeItem = item;
        }

        #endregion
    }
}
