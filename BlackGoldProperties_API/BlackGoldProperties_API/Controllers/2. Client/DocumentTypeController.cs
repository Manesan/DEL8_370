using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;

namespace BlackGoldProperties_API.Controllers._2._Client
{
    public class DocumentTypeController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/documenttypes")]
        public IHttpActionResult Get()
        {
            try
            {
                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get all document types
                var documenttypes = db.CLIENTDOCUMENTTYPEs.Select(x => new {
                    x.CLIENTDOCUMENTTYPEID,
                    x.CLIENTDOCUMENTTYPEDESCRIPTION
                }).ToList();

                if (documenttypes == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(documenttypes);
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
