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
    public class RegisterClientController : ApiController
    {
        [HttpPost] 
        [Route("api/registerclient")]
        public IHttpActionResult Post([FromUri] string email, [FromUri] string name, [FromUri] string surname, [FromUri] string contactnumber, [FromUri] string altcontactnumber, [FromUri] string address, [FromUri] string password, [FromUri] string idnumber, [FromUri] string passportnumber)
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
                if (string.IsNullOrEmpty(address))
                    return BadRequest();
                if (string.IsNullOrEmpty(password))
                    return BadRequest();


                //Check if client is potential client to be deleted
                var potential = db.POTENTIALCLIENTs.FirstOrDefault(x => x.POTENTIALCLIENTEMAIL == email);
                if (potential != null)
                {
                    //Delete specified client
                    db.POTENTIALCLIENTs.Remove(potential);

                    //Save DB Changes
                    db.SaveChanges();
                }

                //Add a client 
                db.USERs.Add(new USER
                {
                    USEREMAIL = email.ToLower(),
                    USERPASSWORD = HomeController.HashPassword(password),
                    USERNAME = name,
                    USERSURNAME = surname,
                    USERCONTACTNUMBER = Utilities.Trimmer(contactnumber),
                    USERALTCONTACTNUMBER = Utilities.Trimmer(altcontactnumber),
                    USERIDNUMBER = Utilities.Trimmer(idnumber),
                    USERPASSPORTNUMBER = Utilities.Trimmer(passportnumber),
                    USERADDRESS = address,
                    USERGUID = HomeController.GUIDActions().USERGUID,
                    USERGUIDEXPIRY = HomeController.GUIDActions().USERGUIDEXPIRY
                });

                //Save DB changes
                db.SaveChanges();

                //Find the user id that was just registered
                int lastuserid = db.USERs.Max(item => item.USERID);

                //Link the user profile to client
                db.CLIENTs.Add(new CLIENT
                {
                    //Set user id to last registered id
                    USERID = lastuserid,

                    //Assign client as 'newly registered'
                    CLIENTTYPEID = 1
                });

                //Save DB changes
                db.SaveChanges();

                //Log user in
                var userToken = HomeController.Login(email, password);
                if (userToken == null)
                    return InternalServerError();

                //Return Ok
                return Ok(userToken);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
