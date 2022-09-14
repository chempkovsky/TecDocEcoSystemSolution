using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

/*

<Application.Resources>
    <app:BooleanToVisibilityConverter 
        x:Key="BooleanToVisibilityConverter" 
        True="Collapsed" 
        False="Visible" />
</Application.Resources>
 
<Grid.Visibility>
    <Binding Path="IsYesNoButtonSetVisible" Converter="{StaticResource booleanToVisibilityConverter}" ConverterParameter="true"/>
</Grid.Visibility>  
  
 */


namespace WpfCarShopInstaller.Converters
{
    public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public BooleanToVisibilityConverter() :
            base(Visibility.Visible, Visibility.Collapsed) { }
    }
}
