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

namespace BlackGoldProperties_API.Controllers._6._Property_Administration
{
    public class PropertyTypeController : ApiController
    {
        //READ ALL DATA//
        [HttpGet]
        [Route("api/propertytype")]
        public IHttpActionResult Get()
        {
            
                try
                {
                    //DB context
                    var db = LinkToDBController.db;
                    db.Configuration.ProxyCreationEnabled = false;

                    //Get all property types
                    var propertytype = db.PROPERTYTYPEs.Select(x => new { 
                        x.PROPERTYTYPEID, 
                        x.PROPERTYTYPEDESCRIPTION 
                    }).ToList();

                    if (propertytype == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(propertytype);
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }
            
        }


        //READ DATA OF SPECIFIC ID//
        [HttpGet]
        [Route("api/propertytype")]
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

                    //Get specified property type
                    var propertytype = db.PROPERTYTYPEs.Where(z => z.PROPERTYTYPEID == id).Select(x => new { 
                        x.PROPERTYTYPEID, 
                        x.PROPERTYTYPEDESCRIPTION 
                    }).FirstOrDefault();

                    if (propertytype == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return Ok(propertytype);
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
        [Route("api/propertytype")]
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

                    //Add a property type
                    db.PROPERTYTYPEs.Add(new PROPERTYTYPE
                    {
                        PROPERTYTYPEDESCRIPTION = description
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
        [Route("api/propertytype")]
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
                    var propertytypes = db.PROPERTYTYPEs.FirstOrDefault(x => x.PROPERTYTYPEID == id);

                    //Null checks
                    if (string.IsNullOrEmpty(description))
                        return BadRequest();

                    //Update specified property type
                    propertytypes.PROPERTYTYPEDESCRIPTION = description;

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
        [Route("api/propertytype")]
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

                    //Find property type
                    var propertytype = db.PROPERTYTYPEs.Include(y => y.PROPERTies).FirstOrDefault(x => x.PROPERTYTYPEID == id);
                    if (propertytype == null)
                        return NotFound();
                    if (propertytype.PROPERTies.Count > 0)
                        return Conflict();

                    //Delete specified property type
                    db.PROPERTYTYPEs.Remove(propertytype);

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
