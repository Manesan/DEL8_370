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
using System.Dynamic;
using BlackGoldProperties_API.Controllers._3._Agent;

namespace BlackGoldProperties_API.Controllers._9._Reporting_Administration
{
    public class AgentReportController : ApiController
    {
        //READ ALL DATA//   -- DO A CHECK TO ENSURE THE EMPLOYEETYPE IS AGENT   -- also display: property, propertyowner of that property
        [HttpGet]
        [Route("api/agentreport")]
        public IHttpActionResult Get([FromUri] string token)
        {
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in
                if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).contains(2 /*Agent*/))
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all agents
                    var agents = db.EMPLOYEEROLEs.Include(x => x.EMPLOYEE).Where(x => x.EMPLOYEETYPEID == 2 && x.EMPLOYEE.EMPLOYEEPROPERTies.Count > 0).Select(x => new {
                        USERID = (int?)x.EMPLOYEE.USER.USERID,
                        x.EMPLOYEE.USER.USERNAME,
                        x.EMPLOYEE.USER.USERSURNAME,
                        Properties = x.EMPLOYEE.EMPLOYEEPROPERTies.Select(y => new { y.PROPERTY.PROPERTYADDRESS, y.PROPERTY.PROPERTYOWNER.PROPERTYOWNERNAME, y.PROPERTY.PROPERTYOWNER.PROPERTYOWNERSURNAME /*, y.PROPERTY.MARKETTYPE.MARKETTYPEDESCRIPTION */ })

                    }).ToList();

                    dynamic reportDetails = new ExpandoObject();
                    reportDetails.Agents = agents;
                    reportDetails.ReportDate = DateTime.Now;
                    var useremail = TokenManager.ValidateToken(token);
                    reportDetails.CurrentUser = db.USERs.Where(x => x.USEREMAIL == useremail).Select(y => new
                    {
                        y.USERNAME,
                        y.USERSURNAME
                    });

                    if (reportDetails == null)
                        return BadRequest();
                    else
                        return Ok(reportDetails);
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
