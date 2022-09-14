// CarShop.Areas.HelpPage.TextSample
using CarShop.Areas.HelpPage;
using System;

namespace CarShop.Areas.HelpPage
{

    public class TextSample
    {
        public string Text
        {
            get;
            private set;
        }

        public TextSample(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            Text = text;
        }

        public override bool Equals(object obj)
        {
            TextSample textSample = obj as TextSample;
            if (textSample != null)
            {
                return Text == textSample.Text;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }

        public override string ToString()
        {
            return Text;
        }
    }
}