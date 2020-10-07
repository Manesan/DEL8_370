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
    public class AgentRentalApplications6Controller : ApiController
    {
        //READ ALL DATA//   -- Should this only return ones that are pending? or should we have sections on the front end categorizing them according to their status
        [HttpGet]
        [Route("api/agentrentalapplication6")]
        public IHttpActionResult Get([FromUri] string token)
        {
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in
            if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            {
                //try
                //{
                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get all rental applications
                var agentrentalapplication6 = db.RENTALAPPLICATIONs.Where(z => z.RENTALAPPLICATIONSTATUSID == 6).Select(x => new {
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


                if (agentrentalapplication6 == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(agentrentalapplication6);
                }


                //}
                //catch (Exception)
                //{
                //    return NotFound();
                //}
            }
            return Unauthorized();
        }


        //READ DATA OF SPECIFIC ID// 
        [HttpGet]
        [Route("api/agentrentalapplication6")]
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
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }



        //Accept/Reject Rental Application//    --- THis is also used for accepting an extension to a rental agreement also as it accepts the application and generates a new rental agreement for the client to sign
        [HttpPatch]
        [Route("api/agentrentalapplication6")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] bool accepted, [FromUri] string note, [FromBody] DocumentController.UploadClass rentalagreement) //--Store link to unsigned agreement
        {
            //Check valid token, logged in, role
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in
            if (TokenManager.GetRoles(token).Contains(5 /*Administrator*/) || TokenManager.GetRoles(token).Contains(1 /*Director*/) || TokenManager.GetRoles(token).Contains(2 /*Agent*/))
            {
                //try
                //{
                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;
                var agentrentalapplication6 = db.RENTALAPPLICATIONs.Include(x => x.TERM).Include(x => x.RENTAL).FirstOrDefault(x => x.RENTALAPPLICATIONID == id);
                var rent = db.RENTALAPPLICATIONs.Where(x => x.RENTALAPPLICATIONID == id);
                //Null checks
                //if (string.IsNullOrEmpty(description))
                //return BadRequest();

                //Accept specified rental application
                if (accepted == true)  //--- Can only accept if available and not in progress/occupied    //---- CHECK TO SEE IF THEY ARE ABLE TO ACCEPT IN CASE THERES ALREADY ANOTHER ACCEPTED APPLICATION
                {
                    if (agentrentalapplication6.RENTALID == null) //Only creates a rental record if this is a brand new rental application
                    {
                        agentrentalapplication6.RENTALAPPLICATIONSTATUSID = 2; //Sets the application status to 'Approved'
                        agentrentalapplication6.RENTALAPPLICATIONNOTE = note;

                        var property = agentrentalapplication6.PROPERTYID;
                        var properties = db.PROPERTies.FirstOrDefault(x => x.PROPERTYID == property);
                        properties.PROPERTYSTATUSID = 3;//Sets to 'In Progress'

                        //Create rental record
                        db.RENTALs.Add(new RENTAL
                        {
                            PROPERTYID = agentrentalapplication6.PROPERTY.PROPERTYID,
                            RENTALSTATUSID = 4,//Sets to 'Pending client acceptance'
                            USERID = agentrentalapplication6.USERID,
                            RENTALDATESTART = agentrentalapplication6.RENTALAPPLICATIONSTARTDATE,
                            RENTALDATEEND = agentrentalapplication6.RENTALAPPLICATIONSTARTDATE.AddMonths(agentrentalapplication6.TERM.TERMDESCRIPTION),
                            TERMID = agentrentalapplication6.TERMID
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
                        agentrentalapplication6.RENTALID = lastrentalid;

                    }
                    else //Called when the agent is accepting an extension/renewal     ----UPDATE RENTALTERM IN RENTAL ENTITY
                    {
                        agentrentalapplication6.RENTALAPPLICATIONSTATUSID = 7; //Sets the application status to 'Approved Extension'      --- use if statement to check status if its renewal or extension
                        agentrentalapplication6.RENTALAPPLICATIONNOTE = note;
                        agentrentalapplication6.RENTAL.RENTALSTATUSID = 7; //Sets the rental status to 'Pending Client Extension Acceptance'     -----THis doesnt work

                        //Upload rental agreement
                        var documenturi = DocumentController.UploadFile(DocumentController.Containers.rentalDocumentsContainer, rentalagreement);

                        db.RENTALDOCUMENTs.Add(new RENTALDOCUMENT
                        {
                            RENTALID = agentrentalapplication6.RENTALID,
                            RENTALAGREEMENTDOCUMENT = documenturi
                        });
                    }
                }
                //Reject specified rental application
                else if (accepted == false)
                {
                    agentrentalapplication6.RENTALAPPLICATIONSTATUSID = 3; //Sets the application status to 'Rejected'
                    agentrentalapplication6.RENTALAPPLICATIONNOTE = note;  //---check if screen exists 
                }

                //Save DB changes
                db.SaveChanges();

                //Return Ok
                return Ok();
                //}
                //catch (System.Exception)
                //{
                //    return NotFound();
                //}
            }
            return Unauthorized();
        }
    }
}
