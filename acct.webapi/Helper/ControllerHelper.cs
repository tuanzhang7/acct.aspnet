using acct.common.POCO;
using acct.common.Repository;
using acct.service;
using AutoMapper;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
namespace acct.webapi.Helper
{
    public class ControllerHelper
    {
        private ExpenseSvc expenseSvc;

        public enum StatusFilter
        {
            Open = 1,
            Closed = 2,
        }

                
        public byte[] GetCompanyLogo()
        {
            string logoImage = "~/image/ThickPotentialLogo.jpg";
            return GetImage(logoImage);
        }
        public byte[] GetImage(string filepath)
        {
            filepath = System.Web.Hosting.HostingEnvironment.MapPath(filepath);
            //string FolderPath = Server.MapPath("Images");
            System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);

            byte[] image = br.ReadBytes((int)fs.Length);

            br.Close();

            fs.Close();

            return image;

        }

        public Byte[] GetOrderReport(Order _entity, string OrderType,
            string ReportFile="~/Content/reports/order/order.rdlc"){
            
            //=====DTO
            List<OrderDetail> dtoDataSet = new List<OrderDetail>();
            
            

            foreach (var item in _entity.OrderDetail)
            {
                OrderDetail _detail =
                    Mapper.Map<acct.common.POCO.OrderDetail, acct.common.POCO.OrderDetail>(item);
                _detail.Order = _entity;//dto;
                dtoDataSet.Add(_detail);
            }
            
            KeyValuePair<string,string> para=new KeyValuePair<string,string>("rp_orderType",OrderType);
            List<KeyValuePair<string, string>> ReportParameters=new List<KeyValuePair<string,string>>();
            ReportParameters.Add(para);
            Byte[] bytes = GetReport(ReportFile, "DataSet_ar_order", dtoDataSet, ReportParameters, true);
            
            return bytes;
        }

        public Byte[] GetReport(string ReportFile,string DataSourceName,object DataSource,
            List<KeyValuePair<string, string>> ReportParameters=null,bool LoadCompanyParameters=false)
        {
            ReportDataSource reportDataSource = new Microsoft.Reporting.WebForms.ReportDataSource();
            reportDataSource.Name = DataSourceName;
            reportDataSource.Value = DataSource;

            List<ReportParameter> reportList = new List<ReportParameter>();

            if (LoadCompanyParameters)
            {
                List<KeyValuePair<string, string>> companyParameters = GetCompanyReportInfo();
                foreach (var param in companyParameters)
                {
                    ReportParameter reportParam = new ReportParameter(param.Key, param.Value);
                    reportList.Add(reportParam);
                }
            }

            if (ReportParameters != null && ReportParameters.Count > 0)
            {
                foreach (var param in ReportParameters)
                {
                    ReportParameter reportParam = new ReportParameter(param.Key, param.Value);
                    reportList.Add(reportParam);
                }
            }
            ReportParameter[] reportParameters = reportList.ToArray();
            Byte[] bytes = PDFReport.InitialReport(ReportFile, reportDataSource, reportParameters);
            return bytes;
        }
        public List<KeyValuePair<string, string>> GetCompanyReportInfo()
        {
            OptionsSvc optionSvc = new OptionsSvc();
            string company_address = String.IsNullOrEmpty(optionSvc.GetOption("company_address"))?
                "Company Address":optionSvc.GetOption("company_address");
            string company_name = String.IsNullOrEmpty(optionSvc.GetOption("company_name")) ?
                "Company Name":optionSvc.GetOption("company_name");
            //string logoImage = new Uri(_controller.Request.MapPath("~/Images/ThickPotentialLogo.jpg")).AbsoluteUri;
            string logoImage = new Uri(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/image/ThickPotentialLogo.jpg")).AbsoluteUri;

            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("rp_company", company_name));
            list.Add(new KeyValuePair<string, string>("rp_company_address", company_address));
            list.Add(new KeyValuePair<string, string>("rp_logoImage", logoImage));

            return list;
        }
        public static decimal GetTotal(decimal qty, decimal unitprice, decimal discount)
        {
            decimal total = qty * unitprice;
            if (discount > 0 && discount < 100) {
                total = total * (100 - discount) / 100;
            }
            return decimal.Round(total,2);
        
        }
    }
}