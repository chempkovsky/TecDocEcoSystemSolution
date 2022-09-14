//extern alias wpftk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
//using WPFTLK = wpftk::System.Windows.Controls;
// WPFTLK.AutoCompleteBox
// WPFTLK.PopulatingEventArgs





namespace WpfCarShopInstaller.Utility
{


    //class AutoCompleteBehavior : Behavior<AutoCompleteBox>
    //{
    //    public ICommand FilterAutoCompleteCommand
    //    {
    //        get
    //        {
    //            return (ICommand)GetValue(FilterAutoCompleteCommandProperty);
    //        }
    //        set
    //        {
    //            SetValue(FilterAutoCompleteCommandProperty, value);
    //        }
    //    }
    //    public static readonly DependencyProperty FilterAutoCompleteCommandProperty = DependencyProperty.Register("FilterAutoCompleteCommand",
    //            typeof(ICommand),
    //            typeof(AutoCompleteBehavior),
    //            new PropertyMetadata(null));


    //    protected override void OnAttached()
    //    {
    //        base.OnAttached();

    //        // handle the populating event of the associated auto complete box
    //        AssociatedObject.Populating += DoPopulating;
    //        // this.AssociatedObject.PopulateComplete();
    //    }
    //    protected override void OnDetaching()
    //    {
    //        // detach the event handler
    //        AssociatedObject.Populating -= DoPopulating;

    //        base.OnDetaching();
    //    }

    //    private void DoPopulating(object sender, PopulatingEventArgs e)
    //    {
    //        // get the command
    //        ICommand filterCommand = FilterAutoCompleteCommand;

    //        if (filterCommand != null)
    //        {
    //            // create the parameters for the command
    //            var parameters = new FilterAutoCompleteParameters(AssociatedObject.PopulateComplete, e.Parameter);

    //            // execute command
    //            filterCommand.Execute(parameters);

    //            // cancel the population of the auto complete box
    //            e.Cancel = false;
                
    //        }
    //    }
    //}


}
