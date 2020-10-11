using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;
using System.Net.Mail;

namespace BlackGoldProperties_API.Controllers._2._Client
{
    public class ContactAgentController : ApiController
    {
        public int oldUserID = 0;
        //Contact agent//
        [HttpPost]
        [Route("api/contactagent")]
        public IHttpActionResult Post([FromUri] string email, [FromUri] string name, [FromUri] string surname, [FromUri] string subject, [FromUri] string message, [FromUri] string contactnumber)
        {
            try
            {
                //DB context
                var db = LinkToDBController.db;
                db.Configuration.ProxyCreationEnabled = false;

                //Null checks
                if (string.IsNullOrEmpty(email))
                    return BadRequest();
                if (string.IsNullOrEmpty(name))
                    return BadRequest();
                if (string.IsNullOrEmpty(surname))
                    return BadRequest();
                if (string.IsNullOrEmpty(subject))
                    return BadRequest();
                if (string.IsNullOrEmpty(message))
                    return BadRequest();
                if (string.IsNullOrEmpty(contactnumber))
                    return BadRequest();

                //Check if client already exists as a potential client
                var potential = db.POTENTIALCLIENTs.FirstOrDefault(x => x.POTENTIALCLIENTEMAIL == email);

                //Check if client already exists as a registered client
                var oldclient = db.USERs.FirstOrDefault(x => x.USEREMAIL == email);

                if (oldclient != null)
                {
                    oldUserID = oldclient.USERID;
                }

                if (potential != null)
                {
                    //Delete specified potential client
                    db.POTENTIALCLIENTs.Remove(potential);

                    //Save DB Changes
                    db.SaveChanges();
                }

                if (oldUserID != 0)
                {
                    //Add a potential client with existing user id
                    db.POTENTIALCLIENTs.Add(new POTENTIALCLIENT
                    {
                        POTENTIALCLIENTEMAIL = email,
                        POTENTIALCLIENTNAME = name,
                        POTENTIALCLIENTSURNAME = surname,
                        POTENTIALCLIENTSUBJECT = subject,
                        POTENTIALCLIENTMESSAGE = message,
                        POTENTIALCLIENTCONTACTNUMBER = contactnumber,
                        USERID = oldUserID
                    });
                }
                else
                {
                    //Add a potential client 
                    db.POTENTIALCLIENTs.Add(new POTENTIALCLIENT
                    {
                        POTENTIALCLIENTEMAIL = email,
                        POTENTIALCLIENTNAME = name,
                        POTENTIALCLIENTSURNAME = surname,
                        POTENTIALCLIENTSUBJECT = subject,
                        POTENTIALCLIENTMESSAGE = message,
                        POTENTIALCLIENTCONTACTNUMBER = contactnumber
                    });
                }

                //Save DB changes
                db.SaveChanges();

                string newSubject = name + " " + surname + ": " + subject;
                string mailBody = "You have recieved a message from " + name + " " + surname + ":" + "<br/><br/>" + message + "<br/><br/>" + contactnumber + "<br/>" + email;
                bool mailSent = Utilities.SendMail(mailBody, newSubject, Utilities.bgpInfoAddress, null);

                if (mailSent == true)
                    return Ok(mailSent);
                else
                    return BadRequest();
            }


            catch (Exception e)
            {
                return BadRequest();
            }

        }
    }
}
