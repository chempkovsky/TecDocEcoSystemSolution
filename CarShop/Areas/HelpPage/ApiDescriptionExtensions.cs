// CarShop.Areas.HelpPage.ApiDescriptionExtensions
using System.Text;
using System.Web;
using System.Web.Http.Description;

namespace CarShop.Areas.HelpPage
{

    public static class ApiDescriptionExtensions
    {
        public static string GetFriendlyId(this ApiDescription description)
        {
            string relativePath = description.RelativePath;
            string[] array = relativePath.Split('?');
            string text = array[0];
            string text2 = null;
            if (array.Length > 1)
            {
                string query = array[1];
                string[] allKeys = HttpUtility.ParseQueryString(query).AllKeys;
                text2 = string.Join("_", allKeys);
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0}-{1}", description.HttpMethod.Method, text.Replace("/", "-").Replace("{", string.Empty).Replace("}", string.Empty));
            if (text2 != null)
            {
                stringBuilder.AppendFormat("_{0}", text2);
            }
            return stringBuilder.ToString();
        }
    }
}