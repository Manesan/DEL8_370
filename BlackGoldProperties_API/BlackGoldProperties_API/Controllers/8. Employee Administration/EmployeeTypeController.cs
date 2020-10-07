using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;

namespace BlackGoldProperties_API.Controllers._8._Employee_Administration
{
    public class EmployeeTypeController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/employeetype")]
        public IHttpActionResult Get()
        {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all employee types
                    var employeetype = db.EMPLOYEETYPEs.Select(x => new {
                        x.EMPLOYEETYPEID,
                        x.EMPLOYEETYPEDESCRIPTION
                    }).ToList();

                    if (employeetype == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(employeetype);
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }
    }
}
