using acct.common.POCO;
using acct.common.Repository;
using acct.service;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace acct.webapi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/customer")]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Pagination")]
    public class CustomerController : ApiController
    {
        CustomerSvc svc;
        public CustomerController(ICustomerRepo iCustomerRepo)
        {
            svc = new CustomerSvc(iCustomerRepo);
        }
        // GET api/values
        public IHttpActionResult Get(int page = 1, int pageSize = 20)
        {
            var list = svc.GetAll();

            IPagedList<Customer> pagedList = list.OrderBy(x => x.Name)
                .ToPagedList(page, pageSize);

            var paginationHeader = new
            {
                TotalCount = pagedList.TotalItemCount,
                TotalPages = pagedList.PageCount
            };

            System.Web.HttpContext.Current.Response.Headers.Add("X-Pagination",
        Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));

            return Ok(pagedList.ToList());
        }

        // GET api/values/5
        public IHttpActionResult Get(int id)
        {
            var customer = svc.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        // GET api/values/5
        [Route("{id}/balance")]
        public IHttpActionResult GetBalance(int id)
        {
            decimal balance = svc.GetBalance(id);
            return Ok(new { balance = balance });
        }

        [Route("lookup")]
        [HttpGet]
        public IHttpActionResult Lookup(string q, int limit = 5)
        {
            IList<Customer> entity = null;
            if (!string.IsNullOrEmpty(q))// && q.Length >= 2)
            {
                entity = svc.GetAll()
                    .Where(o => o.Name.Contains(q))
                    .Take(limit)
                    .ToList();

                if (entity != null)
                {
                    var returnValue = from c in entity
                                      orderby c.Name
                                      select new
                                      {
                                          id = c.Id.ToString(),
                                          value = c.Name
                                      };
                    //var returnValue = entity.OrderBy(x => x.Name).Select(x => x.Name).ToArray();
                    return Ok(returnValue);
                }
            }
            return Ok();
        }

        // POST api/values
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (customer.Id == 0)
                    {
                        svc.Save(customer);
                    }
                    else
                    {
                        Customer _entity = svc.GetById(customer.Id);
                        _entity.Name = customer.Name;
                        _entity.Address = customer.Address;
                        _entity.Phone = customer.Phone;
                        _entity.Fax = customer.Fax;
                        _entity.Email = customer.Email;
                        _entity.ContactName = customer.ContactName;
                        _entity.idmas_GST = customer.idmas_GST;

                        svc.Update(customer);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, customer);
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
