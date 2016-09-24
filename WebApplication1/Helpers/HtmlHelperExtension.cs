using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClayApplication.WebUI.Helpers
{
    public static class HtmlHelperExtension
    {
        public static string NotAuthorizedUsers(this HtmlHelper helper, int doorId)
        {
            return MvcHtmlString.Create("<ul>").ToHtmlString();
        }
    }
}