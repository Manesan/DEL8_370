using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;

namespace BlackGoldProperties_API.Controllers._6._Property_Administration
{
    public class TermController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/term")]
        public IHttpActionResult Get(/*[FromUri] string token*/)
        {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all terms
                    var terms = db.TERMs.Select(x => new {
                        x.TERMID,
                        x.TERMDESCRIPTION
                    }).ToList();

                    if (terms == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(terms);
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }

        }
    }
}
