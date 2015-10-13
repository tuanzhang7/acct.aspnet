using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using acct.service;
using acct.common.POCO;
namespace acct.test
{
    
    [TestClass]
    public class CustomerSvcTest
    {
        //[TestMethod]
        //public void CRUD()
        //{
        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        CustomerSvc svc = new CustomerSvc();

        //        Contact contact = new Contact()
        //        {
        //            Address="bedok reservoir",
        //             ContactNo="90400583",
        //              ContactName="RuiTing",
        //               Email="tuanzhang@gmail.com",
        //                 Name="ThickPotential"
        //        };
        //        GST gst = new GST()
        //        {
        //             Code="07",
        //              Rate=7
        //        };
        //        Customer entity = new Customer()
        //        {
        //              Contact = contact,
        //             GST = gst
        //        };
        //        object result = svc.Save(entity);
        //        int id = ((Customer)result).Id;

        //        //Get
        //        Customer newEntity = svc.GetById(id);
        //        Assert.AreEqual(newEntity.GST.Code, "07");

        //        //Update
        //        newEntity.Contact.ContactNo = "82981261";
        //        svc.Update(newEntity);
        //        Assert.AreEqual(newEntity.Contact.ContactNo, "82981261");

        //        //Delete
        //        svc.Delete(id);
        //        Assert.AreEqual(svc.GetById(id), null);
        //    }
        //}
    }
}
