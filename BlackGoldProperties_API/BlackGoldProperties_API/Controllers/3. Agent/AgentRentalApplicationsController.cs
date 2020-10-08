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
using System.Net.Mail;

namespace BlackGoldProperties_API.Controllers._3._Agent
{
    public class AgentRentalApplicationsController : ApiController
    {
        //READ ALL DATA//   -- Should this only return ones that are pending? or should we have sections on the front end categorizing them according to their status
        [HttpGet]
        [Route("api/agentrentalapplication")]
        public IHttpActionResult Get([FromUri] string token)
        {
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in
            if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all rental applications
                    var rentalapplication = db.RENTALAPPLICATIONs.Where(z => z.RENTALAPPLICATIONSTATUSID == 1).Select(x => new {
                        x.RENTALAPPLICATIONID,
                        PROPERTYID = (int?)x.PROPERTY.PROPERTYID,
                        x.PROPERTY.PROPERTYADDRESS,
                        x.RENTALAPPLICATIONDATE,
                        RENTALAPPLICATIONSTATUSID = (int?)x.RENTALAPPLICATIONSTATU.RENTALAPPLICATIONSTATUSID,
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
                        x.PROPERTY.SUBURB.SUBURBNAME,
                        RENTALID = (int?)x.RENTAL.RENTALID
                    }).ToList();


                    if (rentalapplication == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(rentalapplication);
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
        [Route("api/agentrentalapplication")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        {
            //  Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in
            if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get specified rental application
                    var rentalapplication = db.RENTALAPPLICATIONs.Where(z => z.RENTALAPPLICATIONID == id).Select(x => new
                    {
                        x.RENTALAPPLICATIONID,
                        PROPERTYID = (int?)x.PROPERTY.PROPERTYID,
                        x.PROPERTY.PROPERTYADDRESS,
                        x.RENTALAPPLICATIONDATE,
                        RENTALAPPLICATIONSTATUSID = (int?)x.RENTALAPPLICATIONSTATU.RENTALAPPLICATIONSTATUSID,
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
                        x.PROPERTY.SUBURB.SUBURBNAME,
                        RENTALID = (int?)x.RENTAL.RENTALID
                    }).FirstOrDefault();

                    if (rentalapplication == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(rentalapplication);
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }



        //Accept/Reject Rental Application//    --- THis is also used for accepting an extension to a rental agreement also as it accepts the application and generates a new rental agreement for the client to sign
        [HttpPatch]
        [Route("api/agentrentalapplication")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] bool accepted, [FromUri] string note, [FromBody] DocumentController.UploadClass rentalagreement) //--Store link to unsigned agreement
        {
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in
            if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            {
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;
                    var rentalapplication = db.RENTALAPPLICATIONs.Include(x => x.TERM).Include(x => x.RENTAL).FirstOrDefault(x => x.RENTALAPPLICATIONID == id);
                    var rent = db.RENTALAPPLICATIONs.Where(x => x.RENTALAPPLICATIONID == id);
                    var email = TokenManager.ValidateToken(token);
                    var user = db.USERs.FirstOrDefault(x => x.USEREMAIL == email);
                    var PROPERTYID = db.RENTALAPPLICATIONs.Where(x => x.RENTALAPPLICATIONID == id).Select(y => new
                    {
                       y.PROPERTYID
                    }).FirstOrDefault();
                    var propertyid = PROPERTYID.PROPERTYID;
                    var agent = db.EMPLOYEEPROPERTies.Where(x => x.PROPERTYID == propertyid).Select(y => new
                    {
                        y.EMPLOYEE.USER.USERNAME,
                        y.EMPLOYEE.USER.USERSURNAME,
                        y.EMPLOYEE.USER.USEREMAIL,
                        y.EMPLOYEE.USER.USERCONTACTNUMBER
                    }).FirstOrDefault();
                    //Null checks
                    //if (string.IsNullOrEmpty(description))
                    //return BadRequest();

                    //Accept specified rental application
                    if (accepted == true)  //--- Can only accept if available and not in progress/occupied    //---- CHECK TO SEE IF THEY ARE ABLE TO ACCEPT IN CASE THERES ALREADY ANOTHER ACCEPTED APPLICATION
                    {
                        if (rentalapplication.RENTALID == null) //Only creates a rental record if this is a brand new rental application
                        {
                            rentalapplication.RENTALAPPLICATIONSTATUSID = 2; //Sets the application status to 'Approved'
                            rentalapplication.RENTALAPPLICATIONNOTE = note;

                            var property = rentalapplication.PROPERTYID;
                            var properties = db.PROPERTies.FirstOrDefault(x => x.PROPERTYID == property);
                            properties.PROPERTYSTATUSID = 3;//Sets to 'In Progress'

                            //Create rental record
                            db.RENTALs.Add(new RENTAL
                            {
                                PROPERTYID = rentalapplication.PROPERTY.PROPERTYID,
                                RENTALSTATUSID = 4,//Sets to 'Pending client acceptance'
                                USERID = rentalapplication.USERID,
                                RENTALDATESTART = rentalapplication.RENTALAPPLICATIONSTARTDATE,
                                RENTALDATEEND = rentalapplication.RENTALAPPLICATIONSTARTDATE.AddMonths(rentalapplication.TERM.TERMDESCRIPTION),
                                TERMID = rentalapplication.TERMID
                            });

                            //Save DB Changes
                            db.SaveChanges();

                            //Find the user id that was just registered
                            int lastrentalid = db.RENTALs.Max(item => item.RENTALID);

                            //Upload rental agreement
                            var documenturi = DocumentController.UploadFile(DocumentController.Containers.rentalDocumentsContainer, rentalagreement);

                            db.RENTALDOCUMENTs.Add(new RENTALDOCUMENT
                            {
                                RENTALID = lastrentalid,
                                RENTALAGREEMENTDOCUMENT = documenturi
                            });

                            //Link rental application to rental
                            rentalapplication.RENTALID = lastrentalid;

                            string newSubject = "Rental application for property #" + propertyid + " accepted. Please accept your rental agreement";
                            var userAddress = new MailAddress(user.USEREMAIL, user.USERNAME + " " + user.USERSURNAME);
                            string mailBody = "Dear " + user.USERNAME + " " + user.USERSURNAME + "<br/><br/>We are pleased to inform you that your rental application for property #" + propertyid + " has been accepted. Please login to the Black Gold Properties website to accept your rental agreement.<br/><br/>Kind regards<br/>The Black Gold Properties Team<br/><br/>Your property agent: " + agent.USERNAME + " " + agent.USERSURNAME + "<br/>" + agent.USEREMAIL +"<br/>" + agent.USERCONTACTNUMBER;
                            bool mailSent = Utilities.SendMail(mailBody, newSubject, userAddress, null);

                            return Ok(mailSent);

                        }
                        else //Called when the agent is accepting an extension/renewal     ----UPDATE RENTALTERM IN RENTAL ENTITY
                        {
                            rentalapplication.RENTALAPPLICATIONSTATUSID = 7; //Sets the application status to 'Approved Extension'      --- use if statement to check status if its renewal or extension
                            rentalapplication.RENTALAPPLICATIONNOTE = note;
                            rentalapplication.RENTAL.RENTALSTATUSID = 7; //Sets the rental status to 'Pending Client Extension Acceptance'     -----THis doesnt work

                            //Upload rental agreement
                            var documenturi = DocumentController.UploadFile(DocumentController.Containers.rentalDocumentsContainer, rentalagreement);

                            db.RENTALDOCUMENTs.Add(new RENTALDOCUMENT
                            {
                                RENTALID = rentalapplication.RENTALID,
                                RENTALAGREEMENTDOCUMENT = documenturi
                            });

                            string newSubject = "Rental renewal/extension for property #" + propertyid + " accepted. Please accept your updated rental agreement";
                            var infoAddress = new MailAddress("u18320997@tuks.co.za", "Black Gold Properties");
                            var userAddress = new MailAddress(user.USEREMAIL, user.USERNAME + " " + user.USERSURNAME);
                            string mailBody = "Dear " + user.USERNAME + " " + user.USERSURNAME + "<br/><br/>We are pleased to inform you that your rental renewal/extension for property #" + propertyid + " has been accepted. Please login to the Black Gold Properties website to accept your updated rental agreement.<br/><br/>Kind regards<br/>The Black Gold Properties Team<br/><br/>Your property agent: " + agent.USERNAME + " " + agent.USERSURNAME + "<br/>" + agent.USEREMAIL + "<br/>" + agent.USERCONTACTNUMBER;
                            bool mailSent = Utilities.SendMail(mailBody, newSubject, userAddress, null);

                            return Ok(mailSent);
                        }
                    }
                    //Reject specified rental application
                    else if (accepted == false)
                    {
                        rentalapplication.RENTALAPPLICATIONSTATUSID = 3; //Sets the application status to 'Rejected'
                        rentalapplication.RENTALAPPLICATIONNOTE = note;  //---check if screen exists 

                        string newSubject = "Rental application for property #" + propertyid + " rejected";
                        var userAddress = new MailAddress(user.USEREMAIL, user.USERNAME + " " + user.USERSURNAME);
                        string mailBody = "Dear " + user.USERNAME + " " + user.USERSURNAME + "<br/><br/>We regret to inform you that your rental application for property #" + propertyid + " has been rejected. Please login to the Black Gold Properties website to view further details. For queries, please contact your property agent.<br/><br/>Kind regards<br/>The Black Gold Properties Team<br/><br/>Your property agent: " + agent.USERNAME + " " + agent.USERSURNAME + "<br/>" + agent.USEREMAIL + "<br/>" + agent.USERCONTACTNUMBER;
                        bool mailSent = Utilities.SendMail(mailBody, newSubject, userAddress, null);

                        return Ok(mailSent);
                    }

                    //Save DB changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
                }
                catch (System.Exception e)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }
    }
}
