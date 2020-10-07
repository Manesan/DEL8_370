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

namespace BlackGoldProperties_API.Controllers._3._Agent
{
    public class AgentRentalAgreements4Controller : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/agentrentalagreement4")]
        public IHttpActionResult Get([FromUri] string token)
        { /*
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in */
            //if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all rental agreements   --- Needs document also
                    var agentrentalagreement4 = db.RENTALs.Where(z => z.RENTALSTATUSID == 4).Select(x => new {
                        x.RENTALID,
                        x.RENTALDATESTART,
                        x.RENTALDATEEND,
                        x.RENTALSTATU.RENTALSTATUSID,
                        x.RENTALSTATU.RENTALSTATUSDESCRIPTION,
                        RentalAgreementDocument = x.RENTALDOCUMENTs.Select(y => new { y.RENTALID, y.RENTALDOCUMENTID, y.RENTALAGREEMENTDOCUMENT }).OrderByDescending(y => y.RENTALDOCUMENTID).FirstOrDefault(),
                        x.PROPERTY.PROPERTYID,
                        x.PROPERTY.PROPERTYADDRESS,
                        x.CLIENT.USER.USERID,
                        x.CLIENT.USER.USERNAME,
                        x.CLIENT.USER.USERSURNAME,
                        x.CLIENT.USER.USERCONTACTNUMBER,
                        x.CLIENT.USER.USEREMAIL
                    }).ToList();

                    if (agentrentalagreement4 == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(agentrentalagreement4);
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }


        //READ DATA OF SPECIFIC ID//
        [HttpGet]
        [Route("api/agentrentalagreement4")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        { /*
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in */
            //if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get specified rental agreement
                    var agentrentalagreement4 = db.RENTALs.Where(z => z.RENTALID == id).Select(x => new {
                        x.RENTALID,
                        x.RENTALDATESTART,
                        x.RENTALDATEEND,
                        x.RENTALSTATU.RENTALSTATUSID,
                        x.RENTALSTATU.RENTALSTATUSDESCRIPTION,
                        RentalAgreementDocument = x.RENTALDOCUMENTs.Select(y => new { y.RENTALID, y.RENTALDOCUMENTID, y.RENTALAGREEMENTDOCUMENT }).OrderByDescending(y => y.RENTALDOCUMENTID).FirstOrDefault(),
                        x.PROPERTY.PROPERTYID,
                        x.PROPERTY.PROPERTYADDRESS,
                        x.CLIENT.USER.USERID,
                        x.CLIENT.USER.USERNAME,
                        x.CLIENT.USER.USERSURNAME,
                        x.CLIENT.USER.USERCONTACTNUMBER,
                        x.CLIENT.USER.USEREMAIL
                    }).FirstOrDefault();

                    if (agentrentalagreement4 == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(agentrentalagreement4);
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }


        //Cancel Rental Agreement//
        [HttpDelete]
        [Route("api/agentrentalagreement4")]
        public IHttpActionResult Delete([FromUri] string token, [FromUri] int id)
        { /*
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in */
            //if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;

                    //Find city
                    var agentrentalagreement4 = db.RENTALs.FirstOrDefault(x => x.RENTALID == id);
                    if (agentrentalagreement4 == null)
                        return NotFound();

                    //Delete specified city
                    agentrentalagreement4.RENTALSTATUSID = 1; // ---This sets the rental status to Available.. but maybe it should set it to something like Terminated as it is technically not available as yet??

                    //Save DB Changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }
    }
}
