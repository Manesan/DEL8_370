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

namespace BlackGoldProperties_API.Controllers._2._Client
{
    public class RentalAgreementController : ApiController
    {
        //READ ALL DATA//    
        [HttpGet]
        [Route("api/rentalagreement")]
        public IHttpActionResult Get([FromUri] string token)
        {         

            try
            {
                //Check valid token, logged in
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in

                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get client
                var email = TokenManager.ValidateToken(token);
                var user = db.USERs.FirstOrDefault(x => x.USEREMAIL == email);
                var uid = user.USERID;

                //Get all rental agreements
                var rentalagreement = db.RENTALs.Where(z => z.CLIENT.USERID == uid).Select(x => new {
                    x.RENTALID,
                    x.RENTALDATESTART,
                    x.RENTALDATEEND,
                    x.RENTALSTATU.RENTALSTATUSID,
                    x.RENTALSTATU.RENTALSTATUSDESCRIPTION,
                    RentalAgreement = x.RENTALDOCUMENTs.Select(y => new {y.RENTALDOCUMENTID, y.RENTALAGREEMENTDOCUMENT }).OrderByDescending(y => y.RENTALDOCUMENTID).FirstOrDefault(),
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


        //READ DATA OF SPECIFIC ID//
        [HttpGet]
        [Route("api/rentalagreement")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        {            
            try
            {
                //Check valid token, logged in
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in

                //Null check
                if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                    return BadRequest();

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
                    RentalAgreement = x.RENTALDOCUMENTs.Select(y => new {y.RENTALDOCUMENTID, y.RENTALAGREEMENTDOCUMENT }).OrderByDescending(y => y.RENTALDOCUMENTID).FirstOrDefault(),
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

        //Accept/Reject Rental Agreement//  
        [HttpPatch]
        [Route("api/rentalagreement")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] bool accepted, [FromBody] DocumentController.UploadClass signedagreement) 
        {          

            try
            {
                //Check valid token, logged in
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in

                //Null checks
                if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                    return BadRequest();
                if (string.IsNullOrEmpty(accepted.ToString()))
                    return BadRequest();


                //DB context
                var db = LinkToDBController.db;
                var rental = db.RENTALs.FirstOrDefault(x => x.RENTALID == id);
                var rentalapplication = db.RENTALAPPLICATIONs.Include(x => x.PROPERTY).Include(y => y.CLIENT).Include(y => y.CLIENT.USER).FirstOrDefault(x => x.RENTALID == id);
                var user = rentalapplication.CLIENT.USER;
                var agent = db.EMPLOYEEPROPERTies.Where(x => x.PROPERTYID == rentalapplication.PROPERTYID).Select(y => new
                {
                    y.EMPLOYEE.USER.USEREMAIL,
                    y.EMPLOYEE.USER.USERNAME,
                    y.EMPLOYEE.USER.USERSURNAME
                }).FirstOrDefault();
                var agentAddress = new MailAddress(agent.USEREMAIL, agent.USERNAME + " " + agent.USERSURNAME);


                //Update specified rental
                if (accepted == true)   //---- LINK RENTALID TO RENTALAPPLICATION
                {
                    rentalapplication.PROPERTY.PROPERTYSTATUSID = 3; //Sets to 'In Progress'

                    //Upload rental agreement
                    var documenturi = DocumentController.UploadFile(DocumentController.Containers.rentalDocumentsContainer, signedagreement);

                    db.RENTALDOCUMENTs.Add(new RENTALDOCUMENT
                    {   
                        RENTALID = id,
                        RENTALAGREEMENTDOCUMENT = documenturi
                    });

                    if (rental.RENTALSTATUSID == 7) //'Pending Client Extension Acceptance'
                    {
                        rental.RENTALSTATUSID = 6; //Sets to 'Rented Extended'
                    }
                    else if(rental.RENTALSTATUSID == 8) //'Pending Client Renewal Acceptance'
                    {
                        rental.RENTALSTATUSID = 5; //Sets to 'Rented Renewed'
                    }
                    else //Brand new rental agreement
                    {
                        rental.RENTALSTATUSID = 2; //'Rented'                        
                        
                        //Assign an inspection to the property
                        db.INSPECTIONs.Add(new INSPECTION
                        {
                            PROPERTYID = rental.PROPERTY.PROPERTYID,
                            INSPECTIONTYPEID = 1, //Take-on inspection
                            IVSTATUSID = 1, //Outstanding
                            USERID = 19, //Default Inspector Set
                            INSPECTIONDATE = DateTime.Now
                        });
                    }

                    string newSubject = user.USERNAME + " " + user.USERSURNAME + ": Rental agreement acceptance";
                    string mailBody = user.USERNAME + " " + user.USERSURNAME + " has accepted their rental agreement for property #" + rentalapplication.PROPERTYID + ".";
                    bool mailSent = Utilities.SendMail(mailBody, newSubject, agentAddress, Utilities.bgpInfoAddress);

                    //Return Ok
                    return Ok(mailSent);
                }

                else if(accepted == false)
                {
                    db.RENTALs.Remove(rental);
                    rentalapplication.RENTALAPPLICATIONSTATUSID = 6; //Sets to 'Rejected By Client'
                    rentalapplication.PROPERTY.PROPERTYSTATUSID = 1; //Sets to 'Available'

                    string newSubject = user.USERNAME + " " + user.USERSURNAME + ": Rental agreement rejection";
                    string mailBody = user.USERNAME + " " + user.USERSURNAME + " has rejected their rental agreement for property #" + rentalapplication.PROPERTYID + ".";
                    bool mailSent = Utilities.SendMail(mailBody, newSubject, agentAddress, Utilities.bgpInfoAddress);

                    //Return Ok
                    return Ok(mailSent);
                }

                //Save DB changes
                db.SaveChanges();

                //Return Ok
                return Ok();
            }
            catch (System.Exception)
            {
                return NotFound();
            }
        }


        //Extend/Renew Rental Agreement//
        [HttpPost]
        [Route("api/rentalagreement")]

        public IHttpActionResult Post([FromUri] string token, [FromUri] int rentalid/*, [FromUri] int termid*/) 
        {
           
            try
            {
                //Check valid token, logged in
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in

                //Null check
                if (rentalid < 1 || string.IsNullOrEmpty(rentalid.ToString()))
                    return BadRequest();

                //DB context
                var db = LinkToDBController.db;
                var rental = db.RENTALs.Include(x => x.TERM).Include(y => y.PROPERTY).Include(z => z.CLIENT).Include(xx => xx.CLIENT.USER).FirstOrDefault(x => x.RENTALID == rentalid);
                var user = rental.CLIENT.USER;
                var agent = db.EMPLOYEEPROPERTies.Where(x => x.PROPERTYID == rental.PROPERTYID).Select(y => new
                {
                    y.EMPLOYEE.USER.USEREMAIL,
                    y.EMPLOYEE.USER.USERNAME,
                    y.EMPLOYEE.USER.USERSURNAME
                }).FirstOrDefault();
                var agentAddress = new MailAddress(agent.USEREMAIL, agent.USERNAME + " " + agent.USERSURNAME);

                if (rental == null)
                    return NotFound();


                db.RENTALAPPLICATIONs.Add(new RENTALAPPLICATION  
                {
                    RENTALAPPLICATIONDOCUMENT = "",
                    USERID = (int)rental.USERID,
                    PROPERTYID = rental.PROPERTYID,
                    RENTALAPPLICATIONSTATUSID = 4, //Sets status to 'Pending Agent Extension'
                    RENTALAPPLICATIONDATE = DateTime.Now,
                    TERMID = rental.TERMID,
                    RENTALAPPLICATIONSTARTDATE = (DateTime)rental.RENTALDATEEND, 
                    RENTALID = rentalid
                });

                string newSubject = user.USERNAME + " " + user.USERSURNAME + ": Rental agreement renewal/extension";
                string mailBody = user.USERNAME + " " + user.USERSURNAME + " has requested to renew/extend their rental agreement for property #" + rental.PROPERTYID + ".";
                bool mailSent = Utilities.SendMail(mailBody, newSubject, agentAddress, Utilities.bgpInfoAddress);

                //Save DB changes
                db.SaveChanges();

                //Return Ok
                return Ok(mailSent);
            }
            catch (Exception)
            {
                return NotFound();
    }
}


        //Terminate Rental Agreement//  
        [HttpDelete]
        [Route("api/rentalagreement")]

        public IHttpActionResult Delete([FromUri] string token, [FromUri] int rentalid, [FromUri] bool terminate)
        {
            try
            {
                //Check valid token, logged in
                if (TokenManager.Validate(token) != true)
                    return BadRequest(); // Returns as user is invalid
                if (TokenManager.IsLoggedIn(token) != true)
                    return BadRequest(); // Returns as user is not logged in

                //Null checks
                if (rentalid < 1 || string.IsNullOrEmpty(rentalid.ToString()))
                    return BadRequest();
                if (string.IsNullOrEmpty(terminate.ToString()))
                    return BadRequest();

                //DB context
                var db = LinkToDBController.db;
                var rental = db.RENTALs.Include(x => x.RENTALSTATU).Include(y => y.PROPERTY).Include(z => z.CLIENT).Include(xx => xx.CLIENT.USER).FirstOrDefault(x => x.RENTALID == rentalid);
                var user = rental.CLIENT.USER;
                var agent = db.EMPLOYEEPROPERTies.Where(x => x.PROPERTYID == rental.PROPERTYID).Select(y => new
                {
                    y.EMPLOYEE.USER.USEREMAIL,
                    y.EMPLOYEE.USER.USERNAME,
                    y.EMPLOYEE.USER.USERSURNAME
                }).FirstOrDefault();
                var agentAddress = new MailAddress(agent.USEREMAIL, agent.USERNAME + " " + agent.USERSURNAME);

                if (rental == null)
                    return NotFound();

                if (terminate == true)
                {
                    rental.RENTALSTATUSID = 3; //Sets to 'Pending Agent Termination'

                    string newSubject = user.USERNAME + " " + user.USERSURNAME + ": Rental agreement termination";
                    string mailBody = user.USERNAME + " " + user.USERSURNAME + " has requested to terminate their rental agreement for property #" + rental.PROPERTYID + ".";
                    bool mailSent = Utilities.SendMail(mailBody, newSubject, agentAddress, Utilities.bgpInfoAddress);

                    //Return Ok
                    return Ok(mailSent);
                }

                //Save DB changes
                db.SaveChanges();

                //Return Ok
                return Ok();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
