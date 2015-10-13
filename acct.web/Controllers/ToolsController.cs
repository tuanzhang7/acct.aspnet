using acct.common.Repository;
using acct.service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace acct.web.Controllers
{
    public class ToolsController : Controller
    {
        DataImportor dataImportor;
        public ToolsController(ICustomerRepo iCustomerRepo, IGSTRepo iGSTRepo,
            IInvoiceRepo iInvoiceRepo, IOrderDetailRepo iOrderDetailRepo,
            ISalesmanRepo iSalesmanRepo, IPaymentRepo iPaymentRepo, IPaymentDetailRepo iPaymentDetailRepo)
        {
            dataImportor = new DataImportor(iCustomerRepo, iGSTRepo, iInvoiceRepo,
                iOrderDetailRepo, iSalesmanRepo, iPaymentRepo, iPaymentDetailRepo);
        }
        // GET: Tools
        public ActionResult ImportData()
        {
            return View();
        }
         [HttpPost]
        public ActionResult ImportData(FormCollection collection)
        {
             int counter = 0;
             int errors = 0;
             foreach (string upload in Request.Files)
             {
                 HttpPostedFileBase file = Request.Files[upload];
                 if (file.ContentLength > 0)
                 {
                     try
                     {
                         if (upload == "FileUploadSalesman")
                         {
                             counter = dataImportor.ImportSalesman(file.InputStream);
                         }
                         if (upload == "FileUploadCustomer")
                         {
                             counter = dataImportor.ImportCustomer(file.InputStream);
                         }
                         else if (upload == "FileUploadInvoiceHeader")
                         {
                             counter = dataImportor.ImportInvioceHeader(file.InputStream);
                         }
                         else if (upload == "FileUploadInvoiceDetail")
                         {
                             counter = dataImportor.ImportInvioceDetail(file.InputStream);
                         }
                         else if (upload == "FileUploadInvoice")
                         {
                             counter = dataImportor.ImportInvioce(file.InputStream);
                         }
                         else if (upload == "FileUploadPaymentHeader")
                         {
                             counter = dataImportor.ImportPaymentHeader(file.InputStream);
                         }
                         else if (upload == "FileUploadPaymentDetail")
                         {
                             counter = dataImportor.ImportPaymentDetail(file.InputStream);
                         }
                     }
                     catch (Exception e)
                     {
                         errors++;
                     }
                     
                     
                 }
             }
            //byte[] uploadedFile = new byte[model.File.InputStream.Length];
            //model.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);
             ViewData["message"] = counter + " records imported!" + errors + " errors !";
            return View();
        }
    }
}