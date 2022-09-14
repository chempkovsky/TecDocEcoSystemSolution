using System;
using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows;
using Prism.Interactivity.InteractionRequest;

namespace WpfCarShopCommon.InteractionRequest
{
    public class InteractionDialogAction : TriggerAction<Grid>
    {

        public static readonly DependencyProperty DialogProperty =
            DependencyProperty.Register("Dialog", typeof(InteractionDialogBase), typeof(InteractionDialogAction), new PropertyMetadata(null));

        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(InteractionDialogAction), new PropertyMetadata(null));

        public InteractionDialogBase Dialog
        {
            get { return (InteractionDialogBase)GetValue(DialogProperty); }
            set { SetValue(DialogProperty, value); }
        }

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        protected override void Invoke(Object parameter)
        {

            var args = parameter as InteractionRequestedEventArgs;
            if (args == null)
            {
                return;
            }

            InteractionDialogBase dialog = this.GetDialog(args.Context);
            var callback = args.Callback;

            EventHandler handler = null;

            handler = (s, e) =>
            {
                dialog.Closed -= handler;
                this.AssociatedObject.Children.Remove(dialog);
                callback();
            };

            dialog.Closed += handler;
        }

        public InteractionDialogAction()
            : base()
        {
        }

        InteractionDialogBase GetDialog(INotification notification)
        {
            var dialog = this.Dialog;
            dialog.DataContext = notification;
            dialog.MessageTemplate = this.ContentTemplate;

            dialog.SetValue(Grid.RowSpanProperty, this.AssociatedObject.RowDefinitions.Count == 0 ? 1 : this.AssociatedObject.RowDefinitions.Count);
            dialog.SetValue(Grid.ColumnSpanProperty, this.AssociatedObject.ColumnDefinitions.Count == 0 ? 1 : this.AssociatedObject.ColumnDefinitions.Count);
            this.AssociatedObject.Children.Add(dialog);
            return dialog;
        }
    }
}
