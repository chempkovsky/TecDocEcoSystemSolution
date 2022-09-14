// CarShop.Utility.MakeSpellClosedData
using CarShop.Models;
using System;

namespace CarShop.Utility
{

    public class MakeSpellClosedData
    {
        public CarShopIncomeContext dbIncome
        {
            get;
            set;
        }

        public CarShopSalesContext dbSales
        {
            get;
            set;
        }

        public Guid SpellGuid
        {
            get;
            set;
        }

        public bool hasError
        {
            get;
            set;
        }

        public string ErrorText
        {
            get;
            set;
        }

        public bool isDone
        {
            get;
            set;
        }
    }
}