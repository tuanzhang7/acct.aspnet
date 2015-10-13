using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using acct.common.POCO;
using acct.service;
using acct.common.Repository;
using acct.web.Helper;
using AutoMapper;
using System.Configuration;
using PagedList;

namespace acct.web.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        //
        // GET: /Expense/

        ExpenseSvc svc;
        ExpenseCategorySvc expenseCategorySvc = new ExpenseCategorySvc();
        ControllerHelper cHelper;
        public ExpenseController(IExpenseRepo iExpenseRepo)
        {
            svc = new ExpenseSvc(iExpenseRepo);
            cHelper = new ControllerHelper(this, iExpenseRepo);
        }
        public ActionResult Index(int? page)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            int _page = page == null ? 1 : (int)page;

            IPagedList<Expense> list = svc.GetAll()
                .OrderBy(x => x.Date)
                .ToPagedList(_page, pageSize);

            return View(list);

        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(expenseCategorySvc.GetAll(), "Id", "Category");

            Expense _Expense = new Expense();
            _Expense.Date = DateTime.Today;
            return View(_Expense);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Expense Expense, FormCollection collection)
        {
            if (ModelState.IsValid)
            {

                this.UpdateModel(Expense);


                svc.Save(Expense);

                return RedirectToAction("Details", new { id = Expense.Id });
            }
            ViewBag.CategoryId = new SelectList(expenseCategorySvc.GetAll(), "Id", "Category");
            return View(Expense);
        }

        public ActionResult Edit(int id)
        {
            Expense _entity = svc.GetById(id);
            ViewBag.CategoryId = new SelectList(expenseCategorySvc.GetAll(), "Id", "Category", _entity.CategoryId);
            return View(_entity);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, Expense Expense, FormCollection collection)
        {
            Expense _entity = svc.GetById(id);
            if (ModelState.IsValid)
            {
                _entity.Date = Convert.ToDateTime(collection["Date"]);
                _entity.Amount = Convert.ToDecimal(collection["Amount"]);
                _entity.Remark = collection["Remark"];
                _entity.CategoryId = int.Parse(collection["CategoryId"]);
                try
                {
                    svc.Update(_entity);
                }
                catch (Exception e)
                {

                }


                return RedirectToAction("Details", new { id = _entity.Id });
            }
            ViewBag.CategoryId = new SelectList(expenseCategorySvc.GetAll(), "Id", "Category", _entity.CategoryId);
            return View(_entity);
        }

        public ActionResult Details(int id)
        {
            Expense _entity = svc.GetById(id);
            ViewBag.CategoryId = new SelectList(expenseCategorySvc.GetAll(), "Id", "Category", _entity.CategoryId);
            return View(_entity);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id)
        {
            try
            {
                string succ = "1 record Deleted";
                svc.Delete(id);
                return Json(new { status = true, message = succ });
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = "error when delete" });
            }
            //}

        }
        public ActionResult Print()
        {
            IList<Expense> list = svc.GetAll().Take(10).ToList();

            Mapper.AssertConfigurationIsValid();
            List<Expense> reportList = new List<Expense>();
            foreach (var item in list)
            {
                Expense cust =
                    Mapper.Map<Expense, Expense>(item);
                reportList.Add(cust);
            }

            string ReportFile = "~/Content/reports/order/Expenses.rdlc";
            string DataSourceName = "DataSet_Expenses";


            Byte[] bytes = cHelper.GetReport(ReportFile, DataSourceName, reportList);

            string fileName = "Expenses.pdf";
            return File(bytes, "application/pdf", fileName);
        }

    }
}
