using System;
using System.Linq;
using System.Web.Http;
using System.IO;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;
using System.Web;
using Microsoft.Owin.Logging;
using System.Data;
using System.Data.Entity;
using System.Net.Mail;
using System.Collections.Generic;

namespace BlackGoldProperties_API.Controllers._2._Client
{
    public class RentalApplicationController : ApiController
    {
        //READ ALL DATA//  
        [HttpGet]
        [Route("api/rentalapplication")]
        public IHttpActionResult Get([FromUri] string token)
        {
            //Check valid token, logged in
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in

            try
            {
                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get client
                var email = TokenManager.ValidateToken(token);
                var user = db.USERs.FirstOrDefault(x => x.USEREMAIL == email);
                var uid = user.USERID;

                //Get all rental applications
                var rentalapplication = db.RENTALAPPLICATIONs.Where(z => z.CLIENT.USERID == uid).Select(x => new { 
                    x.RENTALAPPLICATIONID,
                    x.PROPERTY.PROPERTYID, 
                    x.PROPERTY.PROPERTYADDRESS, 
                    x.RENTALAPPLICATIONDATE, 
                    x.RENTALAPPLICATIONSTATU.RENTALAPPLICATIONSTATUSID, 
                    x.RENTALAPPLICATIONSTATU.RENTALAPPLICATIONSTATUSDESCRIPTION, 
                    x.RENTALAPPLICATIONDOCUMENT,
                    x.TERM.TERMID,
                    x.TERM.TERMDESCRIPTION,
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
        [Route("api/rentalapplication")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
        {
            //Check valid token, logged in
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in

            try
            {
                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Get specified rental application
                var rentalapplication = db.RENTALAPPLICATIONs.Where(z => z.RENTALAPPLICATIONID == id).Select(x => new {
                    x.RENTALAPPLICATIONID,
                    x.PROPERTY.PROPERTYID,
                    x.PROPERTY.PROPERTYADDRESS,
                    x.RENTALAPPLICATIONDATE,
                    x.RENTALAPPLICATIONSTATU.RENTALAPPLICATIONSTATUSID,
                    x.RENTALAPPLICATIONSTATU.RENTALAPPLICATIONSTATUSDESCRIPTION,
                    x.RENTALAPPLICATIONDOCUMENT,
                    x.TERM.TERMID,
                    x.TERM.TERMDESCRIPTION,
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
            catch (Exception)
            {
                return NotFound();
            }
        }


        //Apply To Rent//
        [HttpPost]
        [Route("api/rentalapplication")]
        public IHttpActionResult Post([FromUri] string token, [FromUri] int propertyid, [FromUri] int termid, [FromUri] DateTime start, /*[FromBody] DocumentController.UploadClass file*/ [FromBody] dynamic[] documents)  //-- client documents should contain the many documents that will be seperated here -- clientdocument and rentaldocument
        {
            //Check valid token, logged in
            if (TokenManager.Validate(token) != true)
                return BadRequest(); // Returns as user is invalid
            if (TokenManager.IsLoggedIn(token) != true)
                return BadRequest(); // Returns as user is not logged in    

            //Null checks
            /*if (string.IsNullOrEmpty(description))
                return BadRequest();*/

            try
            {
                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                var email = TokenManager.ValidateToken(token);
                var user = db.USERs.FirstOrDefault(x => x.USEREMAIL == email);
                var uid = user.USERID;
                var agent = db.EMPLOYEEPROPERTies.Where(x => x.PROPERTYID == propertyid).Select(y => new
                {
                    y.EMPLOYEE.USER.USERNAME,
                    y.EMPLOYEE.USER.USERSURNAME,
                    y.EMPLOYEE.USER.USEREMAIL
                }).FirstOrDefault();


                DocumentController.UploadClass application = new DocumentController.UploadClass();
                application.FileBase64 = documents[0].FileBase64;
                application.FileExtension = documents[0].FileExtension;

                DocumentController.UploadClass bankstatement = new DocumentController.UploadClass();
                bankstatement.FileBase64 = documents[1].FileBase64;
                bankstatement.FileExtension = documents[1].FileExtension;

                DocumentController.UploadClass copyofid = new DocumentController.UploadClass();
                copyofid.FileBase64 = documents[2].FileBase64;
                copyofid.FileExtension = documents[2].FileExtension;


                //Call upload file funtion
                var applicationUri = DocumentController.UploadFile(DocumentController.Containers.rentalApplicationDocumentsContainer, application);
                var bankstatementUri = DocumentController.UploadFile(DocumentController.Containers.clientDocumentsContainer, bankstatement);
                var copyofidUri = DocumentController.UploadFile(DocumentController.Containers.clientDocumentsContainer, copyofid);

                //Do not continue if upload document failed
                if (applicationUri == null)
                    return NotFound();
                if (bankstatementUri == null)
                    return NotFound();
                if (copyofidUri == null)
                    return NotFound();

                db.RENTALAPPLICATIONs.Add(new RENTALAPPLICATION
                {
                    RENTALAPPLICATIONDOCUMENT = applicationUri,
                    USERID = uid,  //---------fix this error.. keeps crashing when passing through old users??
                    PROPERTYID = propertyid,
                    RENTALAPPLICATIONSTATUSID = 1, //Sets status to 'Pending'
                    RENTALAPPLICATIONDATE = DateTime.Now,
                    TERMID = termid,
                    RENTALAPPLICATIONSTARTDATE = start
                });

                db.CLIENTDOCUMENTs.Add(new CLIENTDOCUMENT   
                {
                    USERID = uid ,
                    CLIENTDOCUMENTTYPEID = 5, //Sets to '3 Months Bank Statement'
                    CLIENTDOCUMENT1 = bankstatementUri,
                    CLIENTDOCUMENTUPLOADDATE = DateTime.Now,
                    CLIENTDOCUMENTUPLOADEXPIRY = DateTime.Now.AddMonths(3)
                });

                db.CLIENTDOCUMENTs.Add(new CLIENTDOCUMENT   //--- MAYBE DOCUMENT TYPE SHOULD BE ID/PASSPORT???
                {
                    USERID = uid,
                    CLIENTDOCUMENTTYPEID = 1, //Sets to 'Copy of ID'
                    CLIENTDOCUMENT1 = copyofidUri,
                    CLIENTDOCUMENTUPLOADDATE = DateTime.Now,
                    CLIENTDOCUMENTUPLOADEXPIRY = DateTime.Now.AddMonths(12)
                });

                //Save DB changes
                db.SaveChanges();

                string newSubject = user.USERNAME + " " + user.USERSURNAME + ": New rental application for property #" + propertyid;
                var agentAddress = new MailAddress(agent.USEREMAIL, agent.USERNAME + " " + agent.USERSURNAME);
                string mailBody = user.USERNAME + " " + user.USERSURNAME + " has made a new rental application for your property, #" + propertyid+ ".<br/><br/>" + user.USERCONTACTNUMBER + "<br/>" + user.USEREMAIL;
                bool mailSent = Utilities.SendMail(mailBody, newSubject, agentAddress, Utilities.bgpInfoAddress);

                //Return Ok
                return Ok(mailSent);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
