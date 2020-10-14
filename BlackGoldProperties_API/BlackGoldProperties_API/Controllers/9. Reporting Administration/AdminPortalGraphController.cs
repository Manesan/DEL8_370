using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.Entity;
using BlackGoldProperties_API.Models;
using System.Dynamic;
using Microsoft.Ajax.Utilities;

namespace BlackGoldProperties_API.Controllers._9._Reporting_Administration
{
    public class AdminPortalGraphController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/adminportalgraph")]
        public IHttpActionResult Get([FromUri] string token)
        {
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in
                if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    dynamic graphsDetails = new ExpandoObject();

                    //Get sales count
                    dynamic salesCount = db.SALEs.Count();
                    graphsDetails.PropertySalesCount = salesCount;

                    //Get rental count
                    dynamic rentalCount = db.RENTALs.Count();
                    graphsDetails.PropertyRentalsCount = rentalCount;

                    //Get popular locations
                    var popularLocations = db.SUBURBs.Where(x => x.PROPERTies.Count > 0).OrderByDescending(y => y.PROPERTies.Count).Select(z => new
                    {
                        z.PROPERTies.Count,
                        z.SUBURBNAME
                    }).ToList();
                    graphsDetails.PopularLocations = popularLocations;

                    if (graphsDetails == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(graphsDetails);
                    }
                }
                return Unauthorized();
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }
    }
}
