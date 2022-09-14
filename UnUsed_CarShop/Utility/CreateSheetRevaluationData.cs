// CarShop.Utility.CreateSheetRevaluationData
using CarShop.Models;
using System;

namespace CarShop.Utility
{
    public class CreateSheetRevaluationData
    {
        public CarShopIncomeContext dbIncome
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