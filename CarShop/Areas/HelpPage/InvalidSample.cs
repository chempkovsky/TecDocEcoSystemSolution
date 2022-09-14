// CarShop.Areas.HelpPage.InvalidSample
using CarShop.Areas.HelpPage;
using System;

namespace CarShop.Areas.HelpPage
{

    public class InvalidSample
    {
        public string ErrorMessage
        {
            get;
            private set;
        }

        public InvalidSample(string errorMessage)
        {
            if (errorMessage == null)
            {
                throw new ArgumentNullException("errorMessage");
            }
            ErrorMessage = errorMessage;
        }

        public override bool Equals(object obj)
        {
            InvalidSample invalidSample = obj as InvalidSample;
            if (invalidSample != null)
            {
                return ErrorMessage == invalidSample.ErrorMessage;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ErrorMessage.GetHashCode();
        }

        public override string ToString()
        {
            return ErrorMessage;
        }
    }
}