using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
namespace acct.web.Helper
{
    public class PDFReport
    {
        public PDFReport()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static Byte[] InitialReport(string reportFile,
            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource,
            Microsoft.Reporting.WebForms.ReportParameter[] ReportParameters,
            Microsoft.Reporting.WebForms.ReportViewer ReportViewer1)
        {
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(reportDataSource);
            ReportViewer1.LocalReport.ReportPath = System.Web.HttpContext.Current.Server.MapPath(reportFile);
            //ReportViewer1.LocalReport.AddTrustedCodeModuleInCurrentAppDomain
            //("SportsCenterMvc.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

            if (ReportParameters != null)
            {
                ReportViewer1.LocalReport.SetParameters(ReportParameters);
            }
            Microsoft.Reporting.WebForms.Warning[] warnings = null;
            string[] streamids = null;
            String mimeType = null;
            String encoding = null;
            String extension = null;
            Byte[] bytes = null;
            string ExportReportFormat = ConfigurationManager.AppSettings["ExportReportFormat"];
            bytes = ReportViewer1.LocalReport.Render(ExportReportFormat, "", out mimeType,
                out encoding, out extension, out streamids, out warnings);
            //this.ReportViewer1.LocalReport.Refresh();
            return bytes;
        }
        
        public static Byte[] InitialReport(string reportFile,
            Microsoft.Reporting.WebForms.ReportDataSource reportDataSource,
            Microsoft.Reporting.WebForms.ReportParameter[] ReportParameters)
        {
            Microsoft.Reporting.WebForms.ReportViewer ReportViewer1 = new Microsoft.Reporting.WebForms.ReportViewer();

            
            //this.ReportViewer1.Reset();
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(reportDataSource);
            ReportViewer1.LocalReport.ReportPath = System.Web.HttpContext.Current.Server.MapPath(reportFile);
            //ReportViewer1.LocalReport.AddTrustedCodeModuleInCurrentAppDomain
            //("SportsCenterMvc.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            ReportViewer1.LocalReport.EnableExternalImages = true;
            if (ReportParameters != null)
            {
                ReportViewer1.LocalReport.SetParameters(ReportParameters);
            }
            Microsoft.Reporting.WebForms.Warning[] warnings = null;
            string[] streamids = null;
            String mimeType = null;
            String encoding = null;
            String extension = null;
            Byte[] bytes = null;
            string ExportReportFormat = "pdf";// ConfigurationManager.AppSettings["ExportReportFormat"];
            bytes = ReportViewer1.LocalReport.Render(ExportReportFormat, "", out mimeType,
                out encoding, out extension, out streamids, out warnings);
            //this.ReportViewer1.LocalReport.Refresh();
            return bytes;
        }
        public static Byte[] InitialReport(string reportFile,
            Microsoft.Reporting.WebForms.ReportDataSource[] reportDataSource,
            Microsoft.Reporting.WebForms.ReportParameter[] ReportParameters)
        {
            Microsoft.Reporting.WebForms.ReportViewer ReportViewer1 = new Microsoft.Reporting.WebForms.ReportViewer();

            //this.ReportViewer1.Reset();
            ReportViewer1.LocalReport.DataSources.Clear();
            foreach (var ds in reportDataSource)
            {
                ReportViewer1.LocalReport.DataSources.Add(ds);
            }
            ReportViewer1.LocalReport.ReportPath = System.Web.HttpContext.Current.Server.MapPath(reportFile);
            ReportViewer1.LocalReport.AddTrustedCodeModuleInCurrentAppDomain
            ("SportsCenterMvc.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

            if (ReportParameters != null)
            {
                ReportViewer1.LocalReport.SetParameters(ReportParameters);
            }
            Microsoft.Reporting.WebForms.Warning[] warnings = null;
            string[] streamids = null;
            String mimeType = null;
            String encoding = null;
            String extension = null;
            Byte[] bytes = null;
            string ExportReportFormat = ConfigurationManager.AppSettings["ExportReportFormat"];
            bytes = ReportViewer1.LocalReport.Render(ExportReportFormat, "", out mimeType,
                out encoding, out extension, out streamids, out warnings);
            //this.ReportViewer1.LocalReport.Refresh();
            return bytes;
        }
        public static void RenderPDF(HttpResponse Response, string fileName, Byte[] bytes)
        {
            string ExportReportFormat = ConfigurationManager.AppSettings["ExportReportFormat"];
            string extention = "PDF";
            if (ExportReportFormat == "Excel")
            {
                extention = "xls";
            }
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "." + extention);
            Response.Buffer = true;
            Response.ContentType = "application/" + extention;
            Response.BinaryWrite(bytes);
            //Response.OutputStream.Write(bytes,0,bytes.Length);
            Response.End();
        }
    }
}
