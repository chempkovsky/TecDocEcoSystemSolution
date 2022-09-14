using Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfCarShopInstaller.ViewModel;

namespace WpfCarShopInstaller.View
{
    /// <summary>
    /// Логика взаимодействия для LoadCategoryTreeView.xaml
    /// </summary>
    public partial class LoadCategoryTreeView : UserControl
    {
        public LoadCategoryTreeView()
        {
            InitializeComponent();
        }

        [Dependency]
        public LoadCategoryTreeViewModel ViewModel
        {
            get
            {
                return this.DataContext as LoadCategoryTreeViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

    }
}
