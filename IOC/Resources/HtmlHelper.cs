using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;
using System.Web;

namespace IoC
{
    public static class HtmlHelper

    {
        public static string DecodeHtmlHelper(this IHtmlHelper helper, IHtmlContent text)
        {
            using (var writer = new System.IO.StringWriter())
            {
                text.WriteTo(writer, HtmlEncoder.Default);
                HtmlString msg = new HtmlString(writer.ToString());
                // return msg;
                var test = msg.Value.ToString();
                return HttpUtility.HtmlDecode(test);
            }

        }

        public static string IsSelected(this IHtmlHelper htmlHelper, string controllers, string actions, string id = null, string cssClass = "active", string? idTag = null)
        {
            var currentId = htmlHelper.ViewContext.RouteData.Values["id"] as string;
            if (idTag != null)
            {
                currentId = htmlHelper.ViewContext.HttpContext.Request.Query[idTag];
                
            }
            var currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
            var currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

            IEnumerable<string> acceptedId = id != null ? (id ?? currentId).Split(',') : null;
            IEnumerable<string> acceptedActions = (actions ?? currentAction).Split(',');
            IEnumerable<string> acceptedControllers = (controllers ?? currentController).Split(',');
            if (!string.IsNullOrWhiteSpace(id))
            {
                return acceptedId.Contains(currentId) && acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
              cssClass : string.Empty;
            }
            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
                    cssClass : string.Empty;
        }
        public static string? Language(this IHtmlHelper htmlHelper)
        {
            var language = htmlHelper.ViewContext.RouteData.Values["language"] as string ?? "";
            return language;
        }

        public static string? GetId(this IHtmlHelper htmlHelper, string id)
        {
            var idValue = htmlHelper.ViewContext.RouteData.Values[id] as string ?? null;
            return idValue;
        }

      
        public static string? Controller(this IHtmlHelper htmlHelper)
        {
            var controller = htmlHelper.ViewContext.RouteData.Values["controller"] as string ?? "";
            return controller;
        }
        public static string? Action(this IHtmlHelper htmlHelper)
        {
            var action = htmlHelper.ViewContext.RouteData.Values["action"] as string ??"";
            return action;
        }

    }


    public static class Globals
    {
   
        public static string Path = ""; // Modifiable
        //public static string Path = "https://localhost:7082"; // Modifiable

    }
}
