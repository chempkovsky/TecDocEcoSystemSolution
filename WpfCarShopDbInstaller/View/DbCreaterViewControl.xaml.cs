using System.Windows.Controls;
using Unity;
using WpfCarShopDbInstaller.ViewModel;

namespace WpfCarShopDbInstaller.View
{
    /// <summary>
    /// Логика взаимодействия для DbCreaterViewControl.xaml
    /// </summary>
    public partial class DbCreaterViewControl : UserControl
    {
        public DbCreaterViewControl()
        {
            InitializeComponent();
        }

        [Dependency]
        public DbCreaterViewModel ViewModel
        {
            get
            {
                return this.DataContext as DbCreaterViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

    }
}
