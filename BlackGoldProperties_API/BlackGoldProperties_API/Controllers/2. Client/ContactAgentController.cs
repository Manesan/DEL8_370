using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;

namespace BlackGoldProperties_API.Controllers._2._Client
{
    public class ContactAgentController : ApiController
    {
        public int oldUserID = 0;
        //Contact agent// - [Potential client]   ---TEST ALL THIS
        [HttpPost]
        [Route("api/contactagent")]
        public IHttpActionResult Post([FromUri] string email, [FromUri] string name, [FromUri] string surname, [FromUri] string subject, [FromUri] string message, [FromUri] string contactnumber)
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

                if(oldUserID != 0)
                {
                    //Add a potential client with existing user id
                    db.POTENTIALCLIENTs.Add(new POTENTIALCLIENT
                    {
                        POTENTIALCLIENTEMAIL = email,
                        POTENTIALCLIENTNAME = name,
                        POTENTIALCLIENTSURNAME = surname,
                        POTENTIALCLIENTSUBJECT = subject,
                        POTENTIALCLIENTMESSAGE = message,
                        POTENTIALCLIENTCONTACTNUMBER = Utilities.Trimmer(contactnumber),
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
                        POTENTIALCLIENTCONTACTNUMBER = Utilities.Trimmer(contactnumber)
                    });
                }

                try
                {
                    //Save DB changes
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return BadRequest();
                }



            //Return Ok
            return Ok();
            
            
        }
    }
}
