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

namespace BlackGoldProperties_API.Controllers._6._Space_
{
    public class SpaceController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/space")]
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

                    //Get all spaces
                    var space = db.SPACEs.Select(x => new {
                        x.SPACEID, 
                        x.SPACEDESCRIPTION,
                        SPACETYPEID = (int?)x.SPACETYPE.SPACETYPEID, 
                        x.SPACETYPE.SPACETYPEDESCRIPTION 
                    }).ToList();

                    if (space == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(space);
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
        [Route("api/space")]
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

                    //Get specified space
                    var space = db.SPACEs.Where(z => z.SPACEID == id).Select(x => new { 
                        x.SPACEID, 
                        x.SPACEDESCRIPTION,
                        SPACETYPEID = (int?)x.SPACETYPE.SPACETYPEID,
                        x.SPACETYPE.SPACETYPEDESCRIPTION 
                    }).FirstOrDefault();

                    if (space == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(space);
                    }
                }
                return Unauthorized();
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }

        //ADD// 
        [HttpPost]
        [Route("api/space")]
        public IHttpActionResult Post([FromUri] string token, [FromUri] string description, [FromUri] int spacetypeid)
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
                    if (spacetypeid < 1 || string.IsNullOrEmpty(spacetypeid.ToString()))
                        return BadRequest();
                    if (string.IsNullOrEmpty(description))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Null checks
                    if (string.IsNullOrEmpty(description))
                        return BadRequest();

                    //Add a space
                    db.SPACEs.Add(new SPACE
                    {
                        SPACEDESCRIPTION = description,
                        SPACETYPEID = spacetypeid 
                    });

                    //Save DB changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
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
        [Route("api/space")]
        public IHttpActionResult Patch([FromUri] string token, [FromUri] int id, [FromUri] string description, [FromUri] int spacetypeid)
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
                    if (string.IsNullOrEmpty(description))
                        return BadRequest();
                    if (spacetypeid < 1 || string.IsNullOrEmpty(spacetypeid.ToString()))
                        return BadRequest();

                    //DB context
                    var db = LinkToDBController.db;
                    var spaces = db.SPACEs.FirstOrDefault(x => x.SPACEID == id);

                    //Null checks
                    if (string.IsNullOrEmpty(description))
                        return BadRequest();

                    //Update specified space
                    spaces.SPACEDESCRIPTION = description;
                    spaces.SPACETYPEID = spacetypeid;   

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


        //DELETE//
        [HttpDelete]
        [Route("api/space")]
        public IHttpActionResult Delete([FromUri] string token, [FromUri] int id)
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

                    //DB context
                    var db = LinkToDBController.db;

                    //Find space
                    var space = db.SPACEs.Include(y => y.PROPERTYSPACEs).FirstOrDefault(x => x.SPACEID == id);
                    if (space == null)
                        return NotFound();
                    if (space.PROPERTYSPACEs.Count > 0)
                        return Conflict();

                    //Delete specified space
                    db.SPACEs.Remove(space);

                    //Save DB Changes
                    db.SaveChanges();

                    //Return Ok
                    return Ok();
                }
                return Unauthorized();
                }
                catch (Exception)
                {
                    return NotFound();
                }
        }
    }
}
