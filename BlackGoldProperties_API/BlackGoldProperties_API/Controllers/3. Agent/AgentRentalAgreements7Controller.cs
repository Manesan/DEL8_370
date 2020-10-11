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
    public class AgentRentalAgreements7Controller : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/agentrentalagreement7")]
        public IHttpActionResult Get([FromUri] string token)
        { 
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in 
                if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all rental agreements 
                    var agentrentalagreement7 = db.RENTALs.Where(z => z.RENTALSTATUSID == 7).Select(x => new {
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

                    if (agentrentalagreement7 == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(agentrentalagreement7);
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
        [Route("api/agentrentalagreement7")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        { 
                try
                {
                //Check valid token, logged in, role
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in 
                if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
                {
                    //Null check
                    if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get specified rental agreement
                    var agentrentalagreement7 = db.RENTALs.Where(z => z.RENTALID == id).Select(x => new {
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

                    if (agentrentalagreement7 == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(agentrentalagreement7);
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
