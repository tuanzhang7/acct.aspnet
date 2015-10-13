using acct.common.Helper;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace acct.webapi.Helper
{
    public class PagedListActionFilterAttribute : ActionFilterAttribute
    {
        //public override void OnActionExecuted(
        //    HttpActionExecutedContext actionExecutedContext)
        //{
        //    base.OnActionExecuted(actionExecutedContext);

        //    var objectContent = actionExecutedContext.Response.Content
        //        as ObjectContent;
        //    if (objectContent == null)
        //    {
        //        return;
        //    }

        //    var pagedList = objectContent.Value as IPagedList;
        //    if (pagedList == null)
        //    {
        //        return;
        //    }

        //    actionExecutedContext.Response.Headers.Add(
        //        "X-Page-Index",
        //        pagedList.PageNumber.ToString(CultureInfo.InvariantCulture));
        //    actionExecutedContext.Response.Headers.Add(
        //        "X-Page-Size",
        //        pagedList.PageSize.ToString(CultureInfo.InvariantCulture));
        //    actionExecutedContext.Response.Headers.Add(
        //        "X-Total-Count",
        //        pagedList.TotalItemCount.ToString(CultureInfo.InvariantCulture));

        //    var listType = pagedList.List.GetType();
        //    actionExecutedContext.Response.Content = new ObjectContent(
        //        listType,
        //        pagedList.List,
        //        objectContent.Formatter);
        //}
    }
}