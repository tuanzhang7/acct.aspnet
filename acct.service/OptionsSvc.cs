using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using acct.common.POCO;
using acct.common.Repository;
using acct.service.Base;
using acct.common.Helper.Settings;

namespace acct.service
{
    
    public partial class OptionsSvc : ServiceBase<Options, int>
    {
        string FORMATER = "{0:000000}";
        protected IOptionsRepo repo;
        // Dependency Injection enabled constructors
        public OptionsSvc(IOptionsRepo repository)
            : base(repository)
        {
            repo = repository;
        }

        public OptionsSvc()
            : this(new acct.repository.ef6.OptionsRepo())
        {
        }
        
        public string GetOption(string name)
        {
            Options options = this.GetAll().Where(o => o.Name == name).FirstOrDefault();
            if (options != null)
            {
                return options.Value;
            }
            return string.Empty;
            
        }
        public Options GetByName(string name)
        {
            Options options = this.GetAll().Where(o => o.Name == name).FirstOrDefault();
            return options;
        }
        private string GetNextNumber(string type)
        {
            string nextNum = GetOption(type);

            int result;
            if (int.TryParse(nextNum, out result))
            {
                return string.Format(FORMATER, result);
            }
            return nextNum;
        }
        public string GetNextInvoiceNumber()
        {
            return GetNextNumber("next_invoice_num");
        }
        public string GetNextQuotationNumber()
        {
            return GetNextNumber("next_quotation_num");
        }
        private void SetNextNumber(string type,string CurrentNumber)
        {
            Options nextNum = GetByName(type);
            if (nextNum == null)
            {
                Options nextInv = new Options();
                nextInv.Name = type;
                nextInv.Value = "1";
                this.Save(nextInv);
            }
            else
            {
                int result;
                int currnt;
                if (int.TryParse(nextNum.Value, out result))
                {
                    if (int.TryParse(CurrentNumber, out currnt))
                    {
                        if (result == currnt)
                        {
                            nextNum.Value = currnt + 1 + "";
                            this.Update(nextNum);
                        }
                    }

                }
            }
        }
        public void SetNextInvoiceNumber(string CurrentNumber)
        {
            SetNextNumber("next_invoice_num", CurrentNumber);
        }
        public void SetNextQuotationNumber(string CurrentNumber)
        {
            SetNextNumber("next_quotation_num", CurrentNumber);
        }
    }
}
