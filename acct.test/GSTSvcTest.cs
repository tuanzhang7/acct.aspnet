using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using acct.service;
using acct.common.POCO;
namespace acct.test
{
    
    [TestClass]
    public class GSTSvcTest
    {
        [TestMethod]
        public void CRUD()
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
                GSTSvc svc = new GSTSvc();

                //Create
                GST entity = new GST()
                {
                   Code="07",
                    Rate=7
                };
                object result = svc.Save(entity);
                int id = ((GST)result).Id;

                //Get
                GST newEntity = svc.GetById(id);
                Assert.AreEqual(newEntity.Code, "07");

                //Update
                newEntity.Rate = 5;
                svc.Update(newEntity);
                Assert.AreEqual(newEntity.Rate, 5);

                //Delete
                svc.Delete(id);
                Assert.AreEqual(svc.GetById(id), null);
            //}
        }
    }
}
