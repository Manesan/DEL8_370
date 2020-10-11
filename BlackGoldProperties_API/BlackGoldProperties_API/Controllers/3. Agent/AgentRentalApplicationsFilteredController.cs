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
    public class AgentRentalApplicationsFilteredController : ApiController
    {
        //READ ALL RENTAL APPLICATION TO EXTEND//    
        [HttpGet]
        [Route("api/rentalagreementextensions")]
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

                //Get all rental applications pending extension
                var rentalapplication = db.RENTALAPPLICATIONs.Where(z => z.RENTALAPPLICATIONSTATUSID == 4).Select(x => new
                {
                    x.RENTALAPPLICATIONID,
                    x.RENTALAPPLICATIONDATE,
                    x.RENTALAPPLICATIONSTATU.RENTALAPPLICATIONSTATUSID,
                    x.RENTALAPPLICATIONSTATU.RENTALAPPLICATIONSTATUSDESCRIPTION,
                    x.RENTALAPPLICATIONNOTE,
                    x.PROPERTY.PROPERTYID,
                    x.PROPERTY.PROPERTYADDRESS,
                    x.CLIENT.USER.USERID,
                    x.CLIENT.USER.USERNAME,
                    x.CLIENT.USER.USERSURNAME,
                    x.CLIENT.USER.USERCONTACTNUMBER,
                    x.CLIENT.USER.USEREMAIL
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



        //READ DATA OF SPECIFIC ID//
        [HttpGet]
        [Route("api/rentalagreementextensions")]
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
                    var rentalapplication = db.RENTALAPPLICATIONs.Where(z => z.RENTALAPPLICATIONID == id).Select(x => new {
                        x.RENTALAPPLICATIONID,
                        x.PROPERTY.PROPERTYID,
                        x.PROPERTY.PROPERTYADDRESS,
                        x.RENTALAPPLICATIONDATE,
                        x.RENTALAPPLICATIONSTATU.RENTALAPPLICATIONSTATUSID,
                        x.RENTALAPPLICATIONSTATU.RENTALAPPLICATIONSTATUSDESCRIPTION,
                        x.RENTALAPPLICATIONDOCUMENT,
                        x.RENTALAPPLICATIONNOTE,
                        x.CLIENT.USER.USERID,
                        x.CLIENT.USER.USERNAME,
                        x.CLIENT.USER.USERSURNAME,
                        x.CLIENT.USER.USERCONTACTNUMBER,
                        x.CLIENT.USER.USEREMAIL
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
                return Unauthorized();
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }


        //I DONT KNOW IF THE BELOW FUNCTION IS EVER ACTUALLY USED//

        //Accept/Reject Rental Application//
        [HttpPatch]
        [Route("api/rentalagreementextensions")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] bool accepted, [FromUri] string note/*, [FromBody] DocumentController.UploadClass rentalagreement*/) //--Store link to unsigned agreement
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
                    var rentalapplication = db.RENTALAPPLICATIONs.FirstOrDefault(x => x.RENTALAPPLICATIONID == id);

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

                            });

                            //---- Link rental application to rental

                            //db.RENTALDOCUMENTs.Add(new RENTALDOCUMENT
                            //{
                            //    RENTALID = rentalapplication.RENTAL.RENTALID,
                            //    RENTALAGREEMENTDOCUMENT = rentalagreement --add link to doc here   
                            //});
                        }
                        else //Called when the agent is accepting an extension/renewal     ----UPDATE RENTALTERM IN RENTAL ENTITY
                        {
                            rentalapplication.RENTALAPPLICATIONSTATUSID = 7; //Sets the application status to 'Approved Extension'      --- use if statement to check status if its renewal or extension
                            rentalapplication.RENTALAPPLICATIONNOTE = note;
                            rentalapplication.RENTAL.RENTALSTATUSID = 7; //Sets the rental status to 'Pending Client Extension Acceptance'
                            //db.RENTALDOCUMENTs.Add(new RENTALDOCUMENT
                            //{
                            //    RENTALID = rentalapplication.RENTAL.RENTALID,
                            //   // RENTALAGREEMENTDOCUMENT = rentalagreement ---add link to doc here  
                            //});
                        }
                    }
                    //Reject specified rental application
                    else if (accepted == false)
                    {
                        rentalapplication.RENTALAPPLICATIONSTATUSID = 3; //Sets the application status to 'Rejected'
                        rentalapplication.RENTALAPPLICATIONNOTE = note;  //---check if screen exists 
                    }

                    //Save DB changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
                }
                return Unauthorized();
                }
                catch (System.Exception)
                {
                    return NotFound();
                }
        }
    }
}
