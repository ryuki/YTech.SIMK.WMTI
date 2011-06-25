using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;

namespace YTech.SIMK.WMTI.Web
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                string rpt = Request.QueryString["rpt"];

                rv.ProcessingMode = ProcessingMode.Local;
                rv.LocalReport.ReportPath = Server.MapPath(string.Format("~/Views/Transaction/Report/{0}.rdlc", rpt));

                rv.LocalReport.DataSources.Clear(); 

                ReportDataSource[] repCol = Session["ReportData"] as ReportDataSource[];
                if (repCol != null)
                {
                    foreach (ReportDataSource d in repCol)
                    {
                        rv.LocalReport.DataSources.Add(d);
                    }
                } 

                rv.LocalReport.Refresh();
            }
        }

        private object GetList(string datasource, string dsName)
        {
            Type t = Type.GetType(dsName); 
            {
                return JsonConvert.DeserializeObject(datasource, t);
            }
            return null;
        }
    }
}