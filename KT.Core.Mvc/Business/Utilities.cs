using System;
using System.Text.RegularExpressions;
using KT.Core.Mvc.Models;

namespace KT.Core.Mvc.Business
{
    public static class Utilities
    {
        public static string GetSettingAsString(string propertyName, Tenant setting)
        {
            var value = string.Empty;

            if (!string.IsNullOrEmpty(propertyName) && setting != null)
            {
                value = (string)(setting.GetType().GetProperty(propertyName).GetValue(setting, null));
            }

            return value;
        }

        public static string BuildColumnClass(dynamic value)
        {
            string result = string.Empty;
            int length = value.Count;

            if (length == 3)
            {
                result = "col-12 col-6 col-4";
            }
            else if (length == 2)
            {
                result = "col-12 col-6 col-6";
            }
            else
            {
                result = "col-12";
            }

            return result;
        }


        public static string StripHtml(string value)
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                var regex2 = "(?i)<(?!img|/img).*?>"; // with images
                result = Regex.Replace(value, regex2, String.Empty).Trim();
            }

            return result;
        }
        public static string TruncateHtml(string value, string link, string title)
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                result = StripHtml(value);

                if (result.Length > 300)
                {
                    result = result.Substring(0, 300) + " ...";
                }
            }

            if (!string.IsNullOrEmpty(link))
            {
                result += "<p><a title=\"" + title + "\"" + " href=" + link + " class=\"btn btn-block btn-outline-info\">Learn More</a></p>";
            }

            return result;
        }
    }
}
