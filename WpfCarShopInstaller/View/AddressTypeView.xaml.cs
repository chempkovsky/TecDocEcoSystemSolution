using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Unity;
using WpfCarShopInstaller.ViewModel;

namespace WpfCarShopInstaller.View
{
    /// <summary>
    /// Логика взаимодействия для AddressTypeView.xaml
    /// </summary>
    public partial class AddressTypeView : UserControl
    {

        public AddressTypeView()
        {
            InitializeComponent();
        }
/*
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            // Не загружайте свои данные во время разработки.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Загрузите свои данные здесь и присвойте результат CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
        }
*/
        [Dependency]
        public AddressTypeViewModel ViewModel
        {
            get
            {
                return this.DataContext as AddressTypeViewModel;
            }
            set
            {
                this.DataContext = value; 
            }
        }

        private DataGridColumn currentSortColumn;
        private ListSortDirection currentSortDirection;


        private void ItemsDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (ViewModel == null) return;
            e.Handled = ViewModel.IsPagingDisplay;
            string sortField = e.Column.SortMemberPath;
            ListSortDirection direction = (e.Column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;
            bool sortAscending = direction == ListSortDirection.Ascending;

            ViewModel.DoSort(sortField, sortAscending);

            currentSortColumn.SortDirection = null;
            e.Column.SortDirection = direction;
            currentSortColumn = e.Column;
            currentSortDirection = direction;
        }

        /// <summary>
        /// Sets the sort direction for the current sorted column since the sort direction
        /// is lost when the DataGrid's ItemsSource property is updated.
        /// </summary>
        /// <param name="sender">The parts data grid.</param>
        /// <param name="e">Ignored.</param>        
        private void ItemsDataGrid_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (currentSortColumn != null)
            {
                currentSortColumn.SortDirection = currentSortDirection;
            }
        }

        private void ItemsDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;

            // The current sorted column must be specified in XAML.
            // <DataGridTextColumn x:Name="addressTypeIdColumn" Binding="{Binding AddressTypeId}" Width="SizeToHeader" SortDirection="Ascending">
            //
            currentSortColumn = dataGrid.Columns.Where(c => c.SortDirection.HasValue).Single();
            currentSortDirection = currentSortColumn.SortDirection.Value;
        }
    }
}

// Content="{Binding LabelDisplayNames[AddressTypeId]}" 
// Header="{Binding RelativeSource={RelativeSource FindAncestor, AncestorType={x:Type UserControl}},Path=DataContext.AddressTypeId}"