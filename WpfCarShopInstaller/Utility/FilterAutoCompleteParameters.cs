using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfCarShopInstaller.Utility
{
    class FilterAutoCompleteParameters
    {

        public Action FilterComplete
        {
            get;
            private set;
        }

        public string FilterCriteria
        {
            get;
            private set;
        }



        public FilterAutoCompleteParameters(Action filterComplete, string filterCriteria)
        {
            FilterComplete = filterComplete;
            FilterCriteria = filterCriteria;
        }
    }
}
