using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;
using System.Data;
using System.Data.Entity;

namespace BlackGoldProperties_API.Controllers._6._Property_Administration
{
    public class PropertyAvailabilityCheckController : ApiController
    {
        //PROPERTY STATUS CHECK//
        [HttpGet]
        [Route("api/propertystatuscheck")]
        public IHttpActionResult Get()
        {
            try
            {
                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get all properties that arent available to the client
                var properties = db.PROPERTies.ToList();
                if (properties == null)
                {
                    return BadRequest();
                }
                else
                {

                    foreach (var property in properties)
                    {
                        if (property.PROPERTYAVAILABLEDATE <= DateTime.Now && (property.PROPERTYSTATUSID == 1 /*Available*/ || property.PROPERTYSTATUSID == 4 /*Not Available*/ ))
                        {
                            property.PROPERTYSTATUSID = 1; //'Available'
                        }
                    }

                    return Ok();
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
