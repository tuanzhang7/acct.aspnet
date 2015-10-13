using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using System.Collections;
using System.Web.Mvc;
using System.Text;
using System.IO;
namespace acct.web.Helper
{
    public static class MyExtensions
    {
        public static IEnumerable Errors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()).Where(m => m.Value.Count() > 0);
            }
            return null;
        }
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = e.ToString() };

            return new SelectList(values, "Id", "Name", enumObj);
        }
        /// <summary>Renders a view to string.</summary>
        public static string RenderViewToString(this Controller controller,
                                                string viewName, object viewData)
        {
            //Create memory writer
            var sb = new StringBuilder();
            var memWriter = new StringWriter(sb);

            //Create fake http context to render the view
            var fakeResponse = new HttpResponse(memWriter);
            var fakeContext = new HttpContext(HttpContext.Current.Request, fakeResponse);
            var fakeControllerContext = new ControllerContext(
                new HttpContextWrapper(fakeContext),
                controller.ControllerContext.RouteData,
                controller.ControllerContext.Controller);

            var oldContext = HttpContext.Current;
            HttpContext.Current = fakeContext;

            //Use HtmlHelper to render partial view to fake context
            var html = new HtmlHelper(
                    new ViewContext(fakeControllerContext, new FakeView(),
             new ViewDataDictionary(), new TempDataDictionary(), memWriter),
                    new ViewPage());
            html.RenderPartial(viewName, viewData);

            //Restore context
            HttpContext.Current = oldContext;

            //Flush memory and return output
            memWriter.Flush();
            return sb.ToString();
        }

        /// <summary>Fake IView implementation used to instantiate an HtmlHelper.</summary>
        public class FakeView : IView
        {
            #region IView Members

            public void Render(ViewContext viewContext, System.IO.TextWriter writer)
            {
                throw new NotImplementedException();
            }

            #endregion
        }
    }
}