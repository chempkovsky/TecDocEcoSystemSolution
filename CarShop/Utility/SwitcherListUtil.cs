// CarShop.Utility.SwitcherListUtil
using CarShop.Properties;
using System.Web.Mvc;

namespace CarShop.Utility
{

    public static class SwitcherListUtil
    {
        public const int pageSize = 20;

        public static SelectList SelectListHelper(int showIs)
        {
            return new SelectList(new SelectListItem[3]
            {
            new SelectListItem
            {
                Value = "1",
                Text = Resources.SHOWALL,
                Selected = (showIs == 1)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.SHOWACTIVE,
                Selected = (showIs == 2)
            },
            new SelectListItem
            {
                Value = "3",
                Text = Resources.SHOWNOTACTIVE,
                Selected = (showIs == 3)
            }
            }, "Value", "Text", showIs);
        }

        public static SelectList SelectColumnListHelper(int showIs)
        {
            return new SelectList(new SelectListItem[2]
            {
            new SelectListItem
            {
                Value = "1",
                Text = Resources.ByEntSupplierId,
                Selected = (showIs == 1)
            },
            new SelectListItem
            {
                Value = "2",
                Text = Resources.ByEntSupplierDescription,
                Selected = (showIs == 2)
            }
            }, "Value", "Text", showIs);
        }
    }
}