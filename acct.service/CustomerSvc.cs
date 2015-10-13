using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using acct.common.POCO;
using acct.common.Repository;
using acct.service.Base;

namespace acct.service
{
    
    public partial class CustomerSvc : ServiceBase<Customer, int>
    {
        protected ICustomerRepo repo;
        // Dependency Injection enabled constructors
        public CustomerSvc(ICustomerRepo repository)
            : base(repository)
        {
            repo = repository;
        }
        // Service Locator pattern
        //public CustomerSvc()
        //    : this(new acct.repository.ef6.CustomerRepo())
        //{
        //}
        public bool IsCustomerNameExist(string CustomerName)
        {
            if (string.IsNullOrEmpty(CustomerName)) { throw new ArgumentException("Customer Name could not be Null Or Empty"); }
            var value = CustomerName.Trim();
            return repo.GetAll().Any(o => o.Name.Equals(CustomerName,StringComparison.OrdinalIgnoreCase));
        }
        public Customer GetByName(string CustomerName)
        {
            if (string.IsNullOrEmpty(CustomerName)) { throw new ArgumentException("Customer Name could not be Null Or Empty"); }
            var value = CustomerName.Trim();
            return repo.GetAll().Where
                (o => o.Name.Equals(CustomerName, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }
        public IQueryable<Customer> Search(string query)
        {
            return repo.GetAll().Where
                (o => o.Name.Contains(query));
        }
        public void Save(List<Customer> Customers)
        {
            repo.Save(Customers,false);
        }

        public decimal GetBalance(int customerId)
        {
            decimal balance=this.GetById(customerId).Order.Sum(c => c.AmountOutstanding);
            return balance;
        }
        
    }
}
