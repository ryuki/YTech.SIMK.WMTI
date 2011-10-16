using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
    public class JsTreeModel
    {
        public string data;
        public JsTreeAttribute attributes;
        public JsTreeModel[] children;
    }

    public class JsTreeAttribute
    {
        public string id;
        public bool selected;
        public string link;
    }
}
