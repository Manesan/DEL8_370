using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlackGoldProperties_API.Models;
using BlackGoldProperties_API.Controllers;

namespace BlackGoldProperties_API.Controllers._3._Agent
{
    public class PropertyOwnerController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/propertyowner")]
        public IHttpActionResult Get([FromUri] string token)
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
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all property owners
                    var propertyowner = db.PROPERTYOWNERs.Select(x => new { 
                        x.PROPERTYOWNERID, 
                        x.PROPERTYOWNERNAME, 
                        x.PROPERTYOWNERSURNAME, 
                        x.PROPERTYOWNEREMAIL
                    }).ToList();

                    if (propertyowner == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(propertyowner);
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
        [Route("api/propertyowner")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
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
                    //Null check
                    if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get specified property owner
                    var propertyowner = db.PROPERTYOWNERs.Where(z => z.PROPERTYOWNERID == id).Select(x => new {
                        x.PROPERTYOWNERID,
                        x.PROPERTYOWNERNAME,
                        x.PROPERTYOWNERSURNAME,
                        x.PROPERTYOWNEREMAIL,
                        PROPERTYOWNERIDNUMBER = x.PROPERTYOWNERIDNUMBER.Trim(),
                        PROPERTYOWNERPASSPORTNUMBER = x.PROPERTYOWNERPASSPORTNUMBER.Trim(),
                        x.PROPERTYOWNERADDRESS,
                        PROPERTYOWNERCONTACTNUMBER = x.PROPERTYOWNERCONTACTNUMBER.Trim(),
                        PROPERTYOWNERALTCONTACTNUMBER = x.PROPERTYOWNERALTCONTACTNUMBER.Trim()
                    }).FirstOrDefault();

                    if (propertyowner == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(propertyowner);
                    }
                }
                return Unauthorized();
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }

        //UPDATE//
        [HttpPatch]
        [Route("api/propertyowner")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] string name, [FromUri] string surname, [FromUri] string email, [FromUri] string owneridnumber, [FromUri] string ownerpassportnumber, [FromUri] string contactnumber, [FromUri] string altcontactnumber, [FromUri] string address)
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

                    //Null checks
                    if (id < 1 || string.IsNullOrEmpty(id.ToString()))
                        return BadRequest();
                    if (string.IsNullOrEmpty(name))
                        return BadRequest();
                    if (string.IsNullOrEmpty(surname))
                        return BadRequest();
                    if (string.IsNullOrEmpty(email))
                        return BadRequest();
                    if (string.IsNullOrEmpty(contactnumber))
                        return BadRequest();
                    if (string.IsNullOrEmpty(address))
                        return BadRequest();


                    //DB context
                    var db = LinkToDBController.db;
                    var propertyowner = db.PROPERTYOWNERs.FirstOrDefault(x => x.PROPERTYOWNERID == id);                  

                    //Update specified property owner
                    propertyowner.PROPERTYOWNERNAME = name;
                    propertyowner.PROPERTYOWNERSURNAME = surname;
                    propertyowner.PROPERTYOWNEREMAIL = email;
                    propertyowner.PROPERTYOWNERIDNUMBER = Utilities.Trimmer(owneridnumber);
                    propertyowner.PROPERTYOWNERPASSPORTNUMBER = Utilities.Trimmer(ownerpassportnumber);
                    propertyowner.PROPERTYOWNERCONTACTNUMBER = Utilities.Trimmer(contactnumber);
                    propertyowner.PROPERTYOWNERALTCONTACTNUMBER = Utilities.Trimmer(altcontactnumber);
                    propertyowner.PROPERTYOWNERADDRESS = address;

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
