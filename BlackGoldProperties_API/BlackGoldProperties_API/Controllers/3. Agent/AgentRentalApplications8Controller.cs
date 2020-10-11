using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;
using System.Dynamic;
using System.Data;
using System.Data.Entity;

namespace BlackGoldProperties_API.Controllers._3._Agent
{
    public class AgentRentalApplications8Controller : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/agentrentalapplication8")]
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

                //Get all rental applications
                var agentrentalapplication8 = db.RENTALAPPLICATIONs.Where(z => z.RENTALAPPLICATIONSTATUSID == 8).Select(x => new {
                    x.RENTALAPPLICATIONID,
                    x.PROPERTY.PROPERTYID,
                    x.PROPERTY.PROPERTYADDRESS,
                    x.RENTALAPPLICATIONDATE,
                    x.RENTALAPPLICATIONSTATU.RENTALAPPLICATIONSTATUSID,
                    x.RENTALAPPLICATIONSTATU.RENTALAPPLICATIONSTATUSDESCRIPTION,
                    x.RENTALAPPLICATIONDOCUMENT,
                    x.RENTALAPPLICATIONNOTE,
                    x.TERM.TERMID,
                    x.TERM.TERMDESCRIPTION,
                    x.CLIENT.USER.USERID,
                    x.CLIENT.USER.USERNAME,
                    x.CLIENT.USER.USERSURNAME,
                    x.CLIENT.USER.USERCONTACTNUMBER,
                    x.CLIENT.USER.USEREMAIL,
                    x.PROPERTY.SUBURB.SUBURBNAME
                }).ToList();


                if (agentrentalapplication8 == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(agentrentalapplication8);
                }


                }
                return Unauthorized();
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }


        //READ DATA OF SPECIFIC ID// 
        [HttpGet]
        [Route("api/agentrentalapplication8")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        {
                try
                {
                //  Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in
                if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get specified rental application
                    var rentalapplication2 = db.RENTALAPPLICATIONs.Where(z => z.RENTALAPPLICATIONID == id).Select(x => new
                    {
                        x.RENTALAPPLICATIONID,
                        x.PROPERTY.PROPERTYID,
                        x.PROPERTY.PROPERTYADDRESS,
                        x.RENTALAPPLICATIONDATE,
                        x.RENTALAPPLICATIONSTATU.RENTALAPPLICATIONSTATUSID,
                        x.RENTALAPPLICATIONSTATU.RENTALAPPLICATIONSTATUSDESCRIPTION,
                        x.RENTALAPPLICATIONDOCUMENT,
                        x.RENTALAPPLICATIONNOTE,
                        x.TERM.TERMID,
                        x.TERM.TERMDESCRIPTION,
                        x.CLIENT.USER.USERID,
                        x.CLIENT.USER.USERNAME,
                        x.CLIENT.USER.USERSURNAME,
                        x.CLIENT.USER.USERCONTACTNUMBER,
                        x.CLIENT.USER.USEREMAIL,
                        ClientDocuments = x.CLIENT.CLIENTDOCUMENTs.Select(y => new { y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEID, y.CLIENTDOCUMENTTYPE.CLIENTDOCUMENTTYPEDESCRIPTION, y.CLIENTDOCUMENTID, y.CLIENTDOCUMENT1, y.CLIENTDOCUMENTUPLOADDATE, y.CLIENTDOCUMENTUPLOADEXPIRY }).OrderByDescending(y => y.CLIENTDOCUMENTID).ToList(),
                        x.PROPERTY.SUBURB.SUBURBNAME
                    }).FirstOrDefault();

                    if (rentalapplication2 == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(rentalapplication2);
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
