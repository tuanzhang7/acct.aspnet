using acct.common.POCO;
using acct.common.Repository;
using acct.service;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace acct.webapi.Controllers
{
    //[Authorize]
    public class GSTController : ApiController
    {
        GSTSvc svc = new GSTSvc();
        public GSTController(IGSTRepo iGSTRepo)
        {
            svc = new GSTSvc(iGSTRepo);
        }
        // GET api/values
        public IEnumerable<GST> Get()
        {
            List<GST> list = svc.GetAll()
                .ToList();
            return list;
        }

        // GET api/values/5
        public IHttpActionResult Get(int id)
        {
            var GST = svc.GetById(id);
            if (GST == null)
            {
                return NotFound();
            }
            return Ok(GST);
        }

        // POST api/values
        [HttpPost]
        public HttpResponseMessage Post([FromBody]GST GST)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (GST.Id == 0)
                    {
                        svc.Save(GST);
                    }
                    else
                    {
                        GST _entity = svc.GetById(GST.Id);
                        _entity.Code = GST.Code;
                        _entity.Rate = GST.Rate;

                        svc.Update(GST);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK);
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
