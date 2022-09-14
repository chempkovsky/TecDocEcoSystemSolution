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
    /// Логика взаимодействия для SimpleTypesLoadUnloadView.xaml
    /// </summary>
    public partial class SimpleTypesLoadUnloadView : UserControl
    {
        public SimpleTypesLoadUnloadView()
        {
            InitializeComponent();
        }
        [Dependency]
        public SimpleTypesLoadUnloadViewModel ViewModel
        {
            get
            {
                return this.DataContext as SimpleTypesLoadUnloadViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }


    }
}
