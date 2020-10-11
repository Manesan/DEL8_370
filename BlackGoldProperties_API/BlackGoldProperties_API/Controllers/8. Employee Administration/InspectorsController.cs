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
    public class InspectorsController : ApiController
    {
        //READ DATA OF SPECIFIC ID//
        [HttpGet]
        [Route("api/inspectors")]
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

                    //Get all inspectors
                    var inspectors = db.EMPLOYEEROLEs.Where(z => z.EMPLOYEETYPEID == 3).Select(x => new { x.EMPLOYEE.USER.USERID, x.EMPLOYEE.USER.USERNAME, x.EMPLOYEE.USER.USERSURNAME, x.EMPLOYEE.USER.USEREMAIL}).ToList();

                    if (inspectors == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(inspectors);
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
