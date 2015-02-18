using System.Collections.ObjectModel;
using Cirrious.MvvmCross.ViewModels;

namespace SampleApp.Core.ViewModels
{
    public class FirstViewModel 
        : MvxViewModel
    {
        private string _hello = "Hello MvvmCross";
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
    }
}
