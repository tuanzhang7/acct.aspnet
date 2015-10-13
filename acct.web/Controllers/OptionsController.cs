using acct.common.Helper.Settings;
using acct.common.POCO;
using acct.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace acct.web.Controllers
{
    [Authorize]
    public class OptionsController : Controller
    {
        //
        // GET: /Company/
        OptionsSvc svc = new OptionsSvc();

        public ActionResult Index()
        {
            List<Option> list = LoadOptions();
            UnitOfWork uow = new acct.common.Helper.Settings.UnitOfWork();

            //Settings settings2 = new Settings(uow);
            //ViewBag.CompanyName = settings2.General.company_name;
            return View(list);
        }
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(FormCollection collection)
        {
            foreach (var item in collection.AllKeys)
            {
                Options _entity = svc.GetByName(item);
                if (_entity == null)
                {
                    //new option,save
                    Options new_option = new Options();
                    new_option.Name = item;
                    new_option.Value = collection[item];
                    new_option.Type = "GeneralSettings";
                    svc.Save(new_option);
                }
                else
                {
                    _entity.Value = collection[item];
                    _entity.Type = "GeneralSettings";
                    svc.Update(_entity);
                }

            }

            return View(LoadOptions());
        }
        public List<Option> LoadOptions()
        {
            List<Option> list = new List<Option>
            {
                new Option { Name = "company_name",
                    DisplayName = "Company Name",DataType = ""},
                new Option { Name = "company_address", 
                    DisplayName = "Company Address",DataType = "TextArea"  },
                new Option { Name = "company_logo", 
                    DisplayName = "Logo",DataType = ""  },
                new Option { Name = "next_invoice_num", 
                    DisplayName = "Next Invoice#",DataType = ""  },
                    new Option { Name = "next_quotation_num", 
                    DisplayName = "Next Quotation#",DataType = ""  }
            };
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Value = svc.GetOption(list[i].Name);
            }
            return list;
        }
        public Settings GetSettings()
        {
            UnitOfWork uow = new acct.common.Helper.Settings.UnitOfWork();

            var settings2 = new Settings(uow);
            return settings2;
        }

    }
    public class Option
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string DisplayName { get; set; }
        public string DataType { get; set; }
    }
}
