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
    public class PropertyDocumentTypeController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/propertydocumenttypes")]
        public IHttpActionResult Get()
        {
            try
            {
                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get all document types
                var documenttypes = db.PROPERTYDOCUMENTTYPEs.Select(x => new {
                    x.PROPERTYDOCUMENTTYPEID,
                    x.PROPERTYDOCUMENTTYPEDESCRIPTION
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

        //READ DATA OF SPECIFIC ID//
        [HttpGet]
        [Route("api/propertydocumenttypes")]
        public IHttpActionResult Get([FromUri] int id)
        {            

            try
            {
                //Null check
                if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                    return BadRequest();

                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get specified city
                var documenttypes = db.PROPERTYDOCUMENTTYPEs.Where(z => z.PROPERTYDOCUMENTTYPEID == id).Select(x => x.PROPERTYDOCUMENTTYPEDESCRIPTION ).FirstOrDefault();

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
