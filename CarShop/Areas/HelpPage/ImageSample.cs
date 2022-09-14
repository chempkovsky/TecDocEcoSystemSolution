// CarShop.Areas.HelpPage.ImageSample
using CarShop.Areas.HelpPage;
using System;

namespace CarShop.Areas.HelpPage
{

    public class ImageSample
    {
        public string Src
        {
            get;
            private set;
        }

        public ImageSample(string src)
        {
            if (src == null)
            {
                throw new ArgumentNullException("src");
            }
            Src = src;
        }

        public override bool Equals(object obj)
        {
            ImageSample imageSample = obj as ImageSample;
            if (imageSample != null)
            {
                return Src == imageSample.Src;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Src.GetHashCode();
        }

        public override string ToString()
        {
            return Src;
        }
    }
}