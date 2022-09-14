// CarShop.Utility.MakeIncomePayRollData
using CarShop.Models;
using System;

namespace CarShop.Utility
{

    public class MakeIncomePayRollData
    {
        public CarShopRestContext dbRest
        {
            get;
            set;
        }

        public CarShopIncomeContext dbIncome
        {
            get;
            set;
        }

        public CarShopArticleContext dbArticle
        {
            get;
            set;
        }

        public Guid IncomePayRollTDESGuid
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