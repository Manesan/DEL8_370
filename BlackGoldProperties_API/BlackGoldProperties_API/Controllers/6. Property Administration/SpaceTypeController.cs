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

namespace BlackGoldProperties_API.Controllers._6._Space_Type
{
    public class SpaceTypeController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/spacetype")]
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

                    //Get all space types
                    var spacetype = db.SPACETYPEs.Select(x => new { 
                        x.SPACETYPEID, 
                        x.SPACETYPEDESCRIPTION 
                    }).ToList();

                    if (spacetype == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(spacetype);
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
        [Route("api/spacetype")]
        public IHttpActionResult Get([FromUri] string token, [FromUri] int id)
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

                    //Get specified space type
                    var spacetype = db.SPACETYPEs.Where(z => z.SPACETYPEID == id).Select(x => new { 
                        x.SPACETYPEID, 
                        x.SPACETYPEDESCRIPTION 
                    }).FirstOrDefault();

                    if (spacetype == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(spacetype);
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return Unauthorized();
        }


        //ADD//
        [HttpPost]
        [Route("api/spacetype")]
        public IHttpActionResult Post([FromUri] string token, [FromUri] string description)
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

                    //Null checks
                    if (string.IsNullOrEmpty(description))
                        return BadRequest();

                    //Add a space type
                    db.SPACETYPEs.Add(new SPACETYPE
                    {
                        SPACETYPEDESCRIPTION = description
                    });

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
            return Unauthorized();
        }


        //UPDATE//
        [HttpPatch]
        [Route("api/spacetype")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] string description)
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
                    var spacetypes = db.SPACETYPEs.FirstOrDefault(x => x.SPACETYPEID == id);

                    //Null checks
                    if (string.IsNullOrEmpty(description))
                        return BadRequest();

                    //Update specified space type
                    spacetypes.SPACETYPEDESCRIPTION = description;

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
            return Unauthorized();
        }


        //DELETE//
        [HttpDelete]
        [Route("api/spacetype")]
        public IHttpActionResult Delete([FromUri] string token, [FromUri] int id)
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

                    //Find space type
                    var spacetype = db.SPACETYPEs.Include(y => y.SPACEs).FirstOrDefault(x => x.SPACETYPEID == id);
                    if (spacetype == null)
                        return NotFound();
                    if (spacetype.SPACEs.Count > 0)
                        return Conflict();

                    //Delete specified space type
                    db.SPACETYPEs.Remove(spacetype);

                    //Save DB Changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
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

