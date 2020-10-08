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
using System.Net.Mail;

namespace BlackGoldProperties_API.Controllers._3._Agent
{
    public class AgentRentalAgreementsController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/agentrentalagreement")]
        public IHttpActionResult Get([FromUri] string token)
        { 
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in 
            if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all rental agreements   --- Needs document also
                    var rentalagreement = db.RENTALs.Where(z => z.RENTALSTATUSID == 1).Select(x => new {
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

                    if (rentalagreement == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(rentalagreement);
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
        [Route("api/agentrentalagreement")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        { 
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in
            if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get specified rental agreement
                    var rentalagreement = db.RENTALs.Where(z => z.RENTALID == id).Select(x => new {
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

                    if (rentalagreement == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(rentalagreement);
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
        [Route("api/agentrentalagreement")]
        public IHttpActionResult Delete([FromUri] string token, [FromUri] int id)
        { 
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in 
            if (TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;

                    //Find city
                    var rentalagreement = db.RENTALs.Include(y => y.CLIENT).Include(z => z.CLIENT.USER).FirstOrDefault(x => x.RENTALID == id);
                    var user = rentalagreement.CLIENT.USER;
                    var agent = db.EMPLOYEEPROPERTies.Where(x => x.PROPERTYID == rentalagreement.PROPERTYID).Select(y => new
                    {
                        y.EMPLOYEE.USER.USEREMAIL,
                        y.EMPLOYEE.USER.USERNAME,
                        y.EMPLOYEE.USER.USERSURNAME,
                        y.EMPLOYEE.USER.USERCONTACTNUMBER
                    }).FirstOrDefault();
                    if (rentalagreement == null)
                        return NotFound();

                    //Delete specified city
                    rentalagreement.RENTALSTATUSID = 1; // ---This sets the rental status to Available.. but maybe it should set it to something like Terminated as it is technically not available as yet??

                    string newSubject = "Rental application for property #" + rentalagreement.PROPERTYID + " concluded";
                    var userAddress = new MailAddress(user.USEREMAIL, user.USERNAME + " " + user.USERSURNAME);
                    string mailBody = "Dear " + user.USERNAME + " " + user.USERSURNAME + "<br/><br/>We hereby inform you that your rental agreement for property #" + rentalagreement.PROPERTYID + " has been concluded.<br/><br/>Thank you for your business with us, and please do not hesitate to contact us if you need another property.<br/><br/>Kind regards<br/>The Black Gold Properties Team<br/><br/>Your property agent: " + agent.USERNAME + " " + agent.USERSURNAME + "<br/>" + agent.USEREMAIL + "<br/>" + agent.USERCONTACTNUMBER;
                    bool mailSent = Utilities.SendMail(mailBody, newSubject, userAddress, null);

                    //Save DB Changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok(mailSent);
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
