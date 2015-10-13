using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
namespace acct.web.Helper
{
    public static class HTMLHelper
    {
        public static string ImageUrlFor(this HtmlHelper helper, string contentUrl)
        {
            // Put some caching logic here if you want it to perform better
            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            if (!File.Exists(helper.ViewContext.HttpContext.Server.MapPath(contentUrl)))
            {
                return urlHelper.Content("~/Content/images/none.jpg");
            }
            else
            {
                return urlHelper.Content(contentUrl);
            }
        }
        public static IHtmlString FormatBody(this HtmlHelper htmlHelper, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return MvcHtmlString.Empty;
            }
            var lines = value.Split('\n'); // Might need to adapt
            return htmlHelper.Raw(
                string.Join("<br/>", lines.Select(line => htmlHelper.Encode(line)))
            );
        }
        public static IHtmlString DisplaySummary(this HtmlHelper htmlHelper, string value, int length)
        {
            string summary = value;
            if (string.IsNullOrEmpty(value))
            {
                return MvcHtmlString.Empty;
            }
            else if (value.Length > length)
            {
                summary = value.Substring(0, length) + "...";
            }
            // Might need to adapt
            return htmlHelper.Raw(
                htmlHelper.Encode(summary)
            );
        }
    }
}