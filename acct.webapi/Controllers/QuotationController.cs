using acct.common.POCO;
using acct.common.Repository;
using acct.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using acct.service.Helper;
using acct.common.Helper;
using PagedList;
using System.Web.Http.Cors;
using System.Net.Http.Headers;
using AutoMapper;
using acct.webapi.Helper;
namespace acct.webapi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Quotation")]
    //[EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Pagination")]
    public class QuotationController : ApiController
    {
        QuotationSvc svc;
        OrderDetailSvc odSvc;
        OptionsSvc optionsSvc;
        public QuotationController(IQuotationRepo iQuotationRepo,IOrderDetailRepo iOrderDetailRepo, IOptionsRepo ioptionsRepo)
        {
            svc = new QuotationSvc(iQuotationRepo);
            odSvc = new OrderDetailSvc(iOrderDetailRepo);
            optionsSvc = new OptionsSvc(ioptionsRepo);
        }
        /// <summary>
        /// filter inovice by data range and status
        /// </summary>
        /// <param name="dateRange">AnyTime|ThisYear|ThisMonth|Last3Month|Last7Days|Last365Days</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="status">All|Unpaid|Partial|Overdue|Paid</param>
        /// <returns></returns>
        //[Route("/{dateRange}")]
        public IHttpActionResult Get(string dateRange , int page = 1, int pageSize = 20)
        {
            DateRange.DateRangeFilter drFilter = 
                (DateRange.DateRangeFilter)Enum.Parse(typeof(DateRange.DateRangeFilter), dateRange);
            //DateRange.DateRangeFilter drFilter =(DateRange.DateRangeFilter)dateRange;
            
            IPagedList<Quotation> pagedList = svc.GetByFilter(null, drFilter)
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
        
        public IHttpActionResult Get(int id)
        {
            Quotation item = svc.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // GET api/Quotation/Customer/6
        [Route("Customer/{id}")]
        public IEnumerable<Quotation> GetByCustomer(int id)
        {
            IEnumerable<Quotation> list = svc.GetByCustomer(id).Take(20).ToList();
            
            return list;

        }

        [Route("GetMonthlyTotal/{startYear}")]
        public IHttpActionResult GetMonthlyTotal(int startYear)
        {
            DateTime year = new DateTime(startYear, 1, 1);
            var summary = from s in svc.GetAll().Where(i => i.OrderDate > year)
                          group s by new { month = s.OrderDate.Month, year = s.OrderDate.Year } into g
                          orderby g.Key.year, g.Key.month
                          select new
                          {
                              year = g.Key.year,
                              month = g.Key.month,
                              total = g.Sum(x => x.TotalAmount)
                          };

            var json = from s in summary.ToList()
                       select new
                       {
                           month = new DateTime(s.year, s.month, 1).ToString("MMM-yy", System.Globalization.CultureInfo.InvariantCulture),
                           total = s.total
                       };
            return Ok(json);
        }

        [Route("GetYearlyTopCustomer/{year}")]
        public IHttpActionResult GetYearlyTopCustomer(int year)
        {
            DateTime _yearStart = new DateTime(year, 1, 1);
            DateTime _yearEnd = new DateTime(year, 12, 31);

            var summary = from s in svc.GetAll().Where(i => i.OrderDate >= _yearStart && i.OrderDate <= _yearEnd)
                          group s by new { customer = s.Customer.Name } into g
                          orderby g.Key.customer
                          select new
                          {
                              customer = g.Key.customer,
                              total = g.Sum(x => x.TotalAmount)
                          };

            var json = from s in summary.OrderByDescending(s => s.total).Take(10).ToList()
                       select new
                       {
                           customer = s.customer.TruncateLongString(20),
                           total = Decimal.Round(s.total, 0)
                       };

            decimal others_total = 0;
            if (summary.OrderByDescending(s => s.total).Skip(10).Any())
            {
                others_total = summary.OrderByDescending(s => s.total).Skip(10).Sum(s => s.total);
            }

            var others = new[]
            {
                new 
                {
                    customer = "Others",
                    total = Decimal.Round(others_total,0)
                }
            };

            json = json.Concat(others);

            return Ok(json);
        }

        [Route("GetYearlyTopSalesman/{year}")]
        public IHttpActionResult GetYearlyTopSalesman(int year)
        {
            DateTime _yearStart = new DateTime(year, 1, 1);
            DateTime _yearEnd = new DateTime(year, 12, 31);

            var summary = from s in svc.GetAll().Where(i => i.OrderDate >= _yearStart && i.OrderDate <= _yearEnd)
                          group s by new { salesman = s.Salesman.Name } into g
                          orderby g.Key.salesman
                          select new
                          {
                              salesman = g.Key.salesman,
                              total = g.Sum(x => x.TotalAmount)
                          };

            var json = from s in summary.OrderByDescending(s => s.total).Take(10).ToList()
                       select new
                       {
                           salesman = s.salesman.TruncateLongString(20),
                           total = Decimal.Round(s.total, 0)
                       };

            decimal others_total = 0;
            if (summary.OrderByDescending(s => s.total).Skip(10).Any())
            {
                others_total = summary.OrderByDescending(s => s.total).Skip(10).Sum(s => s.total);
            }



            var others = new[]
            {
                new 
                {
                    salesman = "Others",
                    total = Decimal.Round(others_total,0)
                }
            };

            json = json.Concat(others);

            return Ok(json);
        }

        [HttpGet]
        [Route("print/{id}")]
        public HttpResponseMessage Print(int id)
        {
            Quotation _entity = svc.GetById(id);

            acct.report.DTO.Helper.Mapping();
            Mapper.AssertConfigurationIsValid();

            Quotation dto = Mapper.Map<acct.common.POCO.Quotation, Quotation>(_entity);
            ControllerHelper cHelper=new ControllerHelper();
            Byte[] bytes = cHelper.GetOrderReport((Order)dto, "Quotation");
            string fileName = "Quotation_" + _entity.OrderNumber + ".pdf";


            var res = new HttpResponseMessage(HttpStatusCode.OK);
            res.Content = new ByteArrayContent(bytes);
            res.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            res.Content.Headers.ContentDisposition.FileName = fileName;
            res.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return res;
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]Quotation entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //add
                    if (entity.Id == 0)
                    {
                        svc.Save(entity);
                        foreach (var detail in entity.OrderDetail)
                        {
                            if (detail.Id == 0)
                            {
                                detail.OrderId = entity.Id;
                                odSvc.Save(detail);
                            }
                        }
                    }
                    //edit
                    else
                    {
                        foreach (var detail in entity.OrderDetail)
                        {
                            if (detail.Id == 0)
                            {
                                detail.OrderId = entity.Id;
                                odSvc.Save(detail);
                            }
                            else
                            {
                                odSvc.Update(detail);
                            }
                        }

                        entity.Customer = null;
                        entity.OrderDetail = null;
                        svc.Update(entity);
                        
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [HttpGet]
        [Route("getNextQuotationNumber")]
        public IHttpActionResult GetNextQuotationNumber()
        {
            string nextNumber= optionsSvc.GetNextQuotationNumber();
            return Ok(new { nextNumber = nextNumber });
        }
    }
}
