using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WpfCarShopCommon.InteractionRequest
{
    public class InteractionDialogBase : UserControl
    {

        public InteractionDialogBase()
        {
        }

        public static readonly DependencyProperty MessageTemplateProperty =
            DependencyProperty.Register("MessageTemplate", typeof(DataTemplate), typeof(InteractionDialogBase), new PropertyMetadata(null));

        public DataTemplate MessageTemplate
        {
            get { return (DataTemplate)GetValue(MessageTemplateProperty); }
            set { SetValue(MessageTemplateProperty, value); }
        }

        public event EventHandler Closed;

        public void Close()
        {
            this.OnClose(EventArgs.Empty);
        }

        protected virtual void OnClose(EventArgs e)
        {
            var handler = this.Closed;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
