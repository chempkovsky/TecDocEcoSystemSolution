// CarShop.Utility.MakeSheetRevaluationData
using CarShop.Models;
using System;

namespace CarShop.Utility
{

    public class MakeSheetRevaluationData
    {
        public CarShopIncomeContext dbIncome
        {
            get;
            set;
        }

        public CarShopRestContext dbRest
        {
            get;
            set;
        }

        public Guid SheetRevaluationTDESGuid
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