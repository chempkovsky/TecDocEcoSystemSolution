using Prism.Mvvm;
using WpfCarShop.Properties;

namespace WpfCarShop.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title =  "MainWindow";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
