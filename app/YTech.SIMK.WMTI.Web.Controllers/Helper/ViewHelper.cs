using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI;

namespace YTech.SIMK.WMTI.Web.Controllers.Helper
{
   public class ViewHelper
    {
       public static string RenderPartialToString(string controlName, object viewData, object model, System.Web.Routing.RequestContext viewContext)
       {
           ViewDataDictionary vd = new ViewDataDictionary(viewData);
           ViewPage vp = new ViewPage { ViewData = vd };

           vp.ViewData = vd;
           vp.ViewData.Model = model;
           vp.ViewContext = new ViewContext();
           vp.Url = new UrlHelper(viewContext);

           Control control = vp.LoadControl(controlName);

           vp.Controls.Add(control);

           StringBuilder sb = new StringBuilder();

           using (StringWriter sw = new StringWriter(sb))
           using (HtmlTextWriter tw = new HtmlTextWriter(sw))
           {
               vp.RenderControl(tw);
           }

           return sb.ToString();
       }

    }
       
}
